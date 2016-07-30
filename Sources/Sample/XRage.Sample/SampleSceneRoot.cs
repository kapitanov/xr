using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AISTek.XRage.Graphics;
using AISTek.XRage.Entities;
using Microsoft.Xna.Framework;

namespace AISTek.XRage.Sample
{
    public class SampleSceneRoot : XComponent, IRenderChunkProvider
    {
        public SampleSceneRoot (XGame game)
            : base(game)
        {
            Entities = new List<BaseEntity>();
        }

        public IList<BaseEntity> Entities { get; private set; }

        public void LoadContent()
        {
            foreach (var entity in Entities)
            {
                entity.LoadContent();
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (var entity in Entities)
            {
                entity.Update(gameTime);
            }
        }

        public void QueryForChunks(ref RenderPassDescriptor pass)
        {
            foreach (var entity in Entities)
            {
                entity.QueryForChunks(ref pass);
            }
        }
    }
}
