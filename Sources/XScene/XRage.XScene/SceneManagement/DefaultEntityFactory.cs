using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AISTek.XRage.Content.SceneManagement;
using AISTek.XRage.Entities;
using AISTek.XRage.Graphics;

namespace AISTek.XRage.SceneManagement
{
    internal class DefaultEntityFactory : XComponent, IEntityFactory
    {
        public DefaultEntityFactory(XGame game)
            : base(game)
        {
            RegisterEntityConstuctor<LightEmitterConstuctor>();
            RegisterEntityConstuctor<PropStaticConstructor>();
            RegisterEntityConstuctor<ControllerRotateConstructor>();
            RegisterEntityConstuctor<PlayerControlledModelConstructor>();
            RegisterEntityConstuctor<AnimateColorControllerContructor>();
        }

        public void RegisterEntityConstuctor<T>()
            where T : IEntityConstuctor, new()
        {
            RegisterEntityConstuctor(new T());
        }

        public void RegisterEntityConstuctor(IEntityConstuctor entityConstuctor)
        {
            entityConstuctors.Add(entityConstuctor.Class, entityConstuctor);
        }

        public BaseEntity CreateEntity(CompiledEntity entity)
        {
            if (entityConstuctors.ContainsKey(entity.ClassName))
                return entityConstuctors[entity.ClassName].CreateEntity(Game, entity);

            return null;
        }

        private Dictionary<string, IEntityConstuctor> entityConstuctors = new Dictionary<string, IEntityConstuctor>();
    }
}
