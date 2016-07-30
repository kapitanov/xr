using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.ComponentModel;
using AISTek.XRage.Components;
using AISTek.XRage.Graphics;
using AISTek.XRage.Messaging.EntityMessages;
using System.Diagnostics.Contracts;
using AISTek.XRage.Messaging;
using AISTek.XRage.SceneManagement;

namespace AISTek.XRage.Entities
{
    public abstract class BaseEntity : XComponent, INotifyPropertyChanged
    {
        #region Constructors

        protected BaseEntity(XGame game, string name, BaseEntity parentEntity)
            : base(game)
        {
            Name = name;
            ParentEntity = parentEntity;
            Scale = 1.0f;
            Position = Vector3.Zero;
            Rotation = Matrix.Identity;

            UniqueId = EntityIds.RequestUniqueId();
            Components = new List<BaseComponent>();
            ActiveComponents = new List<BaseComponent>();
            Children = new Dictionary<long, BaseEntity>();

            AttachToMessagePoll();
        }

        protected BaseEntity(XGame game, string name)
            : this(game, name, null)
        { }

        protected BaseEntity(XGame game)
            : this(game, "<unnamed_entity>", null)
        { } 

        #endregion

        #region Public properties
        
        public long UniqueId { get; private set; }

        public Scene Scene { get; internal set; }

        public string Name { get; set; }

        public BaseEntity ParentEntity { get; private set; }

        public bool HasParent { get { return ParentEntity != null; } }

        public virtual Vector3 Position
        {
            get { return position; }
            set
            {
                position = value;
                OnPropertyChanged(XProperty.Position);
            }
        }

        public virtual Matrix Rotation
        {
            get { return rotation; }
            set
            {
                rotation = value;
                OnPropertyChanged(XProperty.Rotation);
            }
        }

        public virtual float Scale { get; set; }

        #endregion

        #region Protected properties

        protected IList<BaseComponent> Components { get; private set; }

        protected IList<BaseComponent> ActiveComponents { get; private set; }

        protected IDictionary<long, BaseEntity> Children { get; private set; }
        
        #endregion

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region Component management

        public void ActivateComponent(BaseComponent component)
        {
            if (Components.Contains(component))
            {
                if (!ActiveComponents.Contains(component))
                {
                    ActiveComponents.Add(component);
                }
            }
        }

        public void DeactivateComponent(BaseComponent component)
        {
            if (ActiveComponents.Contains(component))
            {
                ActiveComponents.Remove(component);
            }
        }

        public void AddComponent(BaseComponent component)
        {
            if (!Components.Contains(component))
            {
                Components.Add(component);
            }
        }

        public void RemoveComponent(BaseComponent component)
        {
            if (Components.Contains(component))
            {
                component.Shutdown();

                Components.Remove(component);
                ActiveComponents.Remove(component);
            }
        }

        #endregion

        #region Entity update and rendering

        public virtual void Initialize()
        {
            foreach (var component in ActiveComponents)
            {
                component.Initialize();
            }
        }

        public virtual void LoadContent()
        {
            foreach (var component in ActiveComponents)
            {
                component.LoadContent();
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            foreach (var component in ActiveComponents)
            {
                component.Update(gameTime);
            }
        }

        public virtual void QueryForChunks(ref RenderPassDescriptor pass)
        {
            foreach (var component in ActiveComponents)
            {
                component.QueryForChunks(ref pass);
            }
        }

        public virtual void UnloadContent()
        {
            foreach (var component in ActiveComponents)
            {
                component.UnloadContent();
            }
        }

        public void Shutdown()
        {
            SetParent(null);
            RemoveAllChildren();

            foreach (var component in Components)
            {
                component.Shutdown();
            }

            Components.Clear();
            DetachFromMessagePoll();
        }

        #endregion

        #region Entity graph management

        public void AddChild(BaseEntity child)
        {
            var message = XObjectPool.Acquire<MessageParentAdded>();
            message.UniqueTarget = child.UniqueId;
            Game.MessagingPoll.SendMessage(message);

            Children.Add(child.UniqueId, child);
        }

        public void RemoveAllChildren()
        {
            foreach (var childId in Children.Keys)
            {
                var message = XObjectPool.Acquire<MessageParentRemoved>();
                message.UniqueTarget = childId;
                Game.MessagingPoll.SendMessage(message);
            }

            Children.Clear();
        }

        public void RemoveChild(BaseEntity child)
        {
            var message = XObjectPool.Acquire<MessageParentRemoved>();
            message.UniqueTarget = child.UniqueId;
            Game.MessagingPoll.SendMessage(message);

            Children.Remove(child.UniqueId);
        }

        public void SetParent(BaseEntity parent)
        {
            if (HasParent)
            {
                var message = XObjectPool.Acquire<MessageChildRemoved>();
                message.UniqueTarget = ParentEntity.UniqueId;
                Game.MessagingPoll.SendMessage(message);
            }

            if (parent == null)
            {
                parent = Scene.SceneRoot;
            }

            var msgChildAdd = XObjectPool.Acquire<MessageChildAdded>();
            msgChildAdd.UniqueTarget = parent.UniqueId;
            msgChildAdd.Child = this;
            Game.MessagingPoll.SendMessage(msgChildAdd);

            ParentEntity = parent;
        }

        #endregion

        #region Messaging

        private void AttachToMessagePoll()
        {
            Game.MessagingPoll.OnMessage += OnMessage;
        }

        private void DetachFromMessagePoll()
        {
            Game.MessagingPoll.OnMessage -= OnMessage;
        }

        protected virtual void OnMessage(IMessage message)
        {
            ExecuteMessage(message);
        }

        public virtual bool ExecuteMessage(IMessage message)
        {
            switch (message.Type)
            {
                case (int)MessageType.Entity.GetName:
                    message.TypeCheck<MessageGetName>().Name = Name;
                    return true;

                //case (int)MessageType.Entity.GetParentId:
                //    if (HasParent)
                //    {
                //        message.TypeCheck<MessageGetParentId>().ParentId = ParentEntity.UniqueId;
                //    }
                //    return true;

                case (int)MessageType.Entity.SetPosition:
                    Position = message.TypeCheck<MessageSetPosition>().Position;
                    SendMessageThroughComponents(message);
                    return true;

                case (int)MessageType.Entity.ModifyPosition:
                    Position += message.TypeCheck<MessageModifyPosition>().Position;
                    SendMessageThroughComponents(message);
                    return true;

                case (int)MessageType.Entity.GetPosition:
                    message.TypeCheck<MessageGetPosition>().Position = Position;
                    return true;

                case (int)MessageType.Entity.SetRotation:
                    Rotation = message.TypeCheck<MessageSetRotation>().Rotation;
                    SendMessageThroughComponents(message);
                    return true;

                case (int)MessageType.Entity.ModifyRotation:
                    rotation *= message.TypeCheck<MessageModifyRotation>().Rotation;
                    SendMessageThroughComponents(message);
                    return true;

                case (int)MessageType.Entity.GetRotation:
                    message.TypeCheck<MessageGetRotation>().Rotation = rotation;
                    return true;

                case (int)MessageType.Entity.SetParent:
                    SetParent(message.TypeCheck<MessageSetParent>().ParentEntity);
                    return true;

                case (int)MessageType.Entity.RemoveChild:
                    RemoveChild(message.TypeCheck<MessageRemoveChild>().Child);
                    return true;

                case (int)MessageType.Entity.ParentRemoved:
                    if (ParentEntity == message.TypeCheck<MessageParentRemoved>().Parent)
                    {
                        SetParent(null);
                    }
                    return true;

                case (int)MessageType.Entity.ParentAdded:
                    SetParent(message.TypeCheck<MessageParentAdded>().Parent);
                    return true;

                case (int)MessageType.Entity.ChildRemoved:
                    var childRemovedMessage = message.TypeCheck<MessageChildRemoved>();

                    if (childRemovedMessage.Child != null)
                    {
                        Children.Remove(childRemovedMessage.Child.UniqueId);
                    }
                    return true;

                case (int)MessageType.Entity.ChildAdded:
                    var childAddedMessage = message.TypeCheck<MessageChildAdded>();

                    if (childAddedMessage.Child != null &&
                        childAddedMessage.Child != this)
                    {
                        Children.Add(childAddedMessage.Child.UniqueId, childAddedMessage.Child);
                    }
                    return true;

                default:
                    return SendMessageThroughComponents(message);
            }
        }

        protected bool SendMessageThroughComponents(IMessage message)
        {
            var handled = false;

            foreach (var component in ActiveComponents)
            {
                if (component.ExecuteMessage(message))
                {
                    handled = true;
                    break;
                }
            }

            return handled;
        }

        #endregion

        #region Private fields

        private Vector3 position;
        private Matrix rotation;

        #endregion
    }
}
