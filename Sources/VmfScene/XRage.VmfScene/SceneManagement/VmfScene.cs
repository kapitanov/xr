using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AISTek.XRage.Entities;

namespace AISTek.XRage.SceneManagement
{
    public class VmfScene : Scene
    {
        /// <remarks>
        /// This is only temporary solution
        /// </remarks>
        public static XGame StaticGame { get; set; }

        public VmfScene(string name)
            : base(StaticGame, name)
        {
            Brushes = new List<VmfBrush>();
        }

        public IList<VmfBrush> Brushes { get; internal set; }

        public override void LoadContent()
        {
            var c = 0;
            foreach (var brush in Brushes)
            {
                c++;
                var p = (int)(c * 10.0f / Brushes.Count);
                var pb = "[" + new string('*', p) + new string(' ', 10 - p) + "]";

                Game.SceneManager.UpdateLoadingState(
                    new LoadingState(
                        Name,
                        string.Format("Loading brushes ({0}/{1}) ...\n       {2}", c, Brushes.Count, pb),
                        (int)(c * 100.0f / Brushes.Count)));

                brush.LoadContent();
            }
        }

        public override void UnloadContent()
        { }

        public override void QueryForChunks(ref Graphics.RenderPassDescriptor pass)
        {
            foreach (var brush in Brushes)
            {
                brush.QueryForChunks(ref pass);
            }
        }
    }
}
