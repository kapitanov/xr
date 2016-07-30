using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AISTek.XRage.Graphics;
using AISTek.XRage.Content.SceneManagement;
using AISTek.XRage.Entities;

namespace AISTek.XRage.SceneManagement
{
    public class XWorld : XComponent
    {
        public XWorld(XGame game)
            : base(game)
        {
            Brushes = new List<Brush>();
        }

        public IList<Brush> Brushes { get; private set; }

        //public BaseSky Sky { get; private set; }

        public Terrain Terrain { get; private set; }

        internal void LoadContent(CompiledXWorld world, LoadingProgress progress)
        {
            progress.Stage = Stage.LoadingBrushes;

            var brushIndex = 0;
            foreach (var xBrush in world.Brushes)
            {
                progress.Update(brushIndex++, world.Brushes.Count);

                var brush = new Brush(Game);
                brush.LoadContent(xBrush);
                Brushes.Add(brush);
            }

            if (world.HasTerrain)
            {
                progress.Update(Stage.LoadingTerrain);
                Terrain = new Terrain(
                    Game, 
                    PropertyConvertor.ScenePath(world.Terrain.HeightMapPath),
                    PropertyConvertor.MaterialPath(world.Terrain.MaterialPath));
                Terrain.LoadContent();
            }
        }

        public void QueryForChunks(ref RenderPassDescriptor pass)
        {
            foreach (var brush in Brushes)
            {
                brush.QueryForChunks(ref pass);
            }

            if (Terrain != null)
            {
                Terrain.QueryForChunks(ref pass);
            }
        }

        public void UnloadContent()
        {
            if (Terrain != null)
            {
                Terrain.UnloadContent();            
            }
        }
    }
}
