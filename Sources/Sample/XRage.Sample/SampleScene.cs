using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AISTek.XRage.SceneManagement;
using AISTek.XRage.Entities;
using Microsoft.Xna.Framework;
using AISTek.XRage.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace AISTek.XRage.Sample
{
    public class SampleScene : Scene
    {
        public SampleScene(XGame game, string name)
            : base(game, name)
        {
            GlobalLight = new LightEmitter(game, "light_emitter", Color.SkyBlue, LightType.Directional)
            {
                Direction = -Vector3.UnitY,
                Intensity = 0.5f
            };

            PointLight1 = new LightEmitter(game, "light_emitter", Color.LightGreen, LightType.Point)
            {
                Position = new Vector3(25f, 5000f + 50f, 0f),
                Radius = 100f
            };

            PointLight2 = new LightEmitter(game, "light_emitter", Color.LightPink, LightType.Point)
            {
                Position = new Vector3(25f, 5000f - 50f, 0f),
                Radius = 100f
            };

            StaticModel1 = new SampleModelEntity(Game, ModelPath, MaterialPath)
            {
                Position = new Vector3(0f, 5000f, 0f)
            };
            
            //Terrain = new Terrain(Game, "textures/terrain/gcanyon_height", "materials/terrain") { ScaleFactor = 75, ElevationFactor = 10.0f };
            //AddEntity(Terrain);
            
            var staticModels = new List<SampleModelEntity>();

            for (var x = 0; x < 25; x++)
            {
                for (var z = 0; z < 25; z++)
                {
                    if (x == 0 && z == 0)
                        continue;

                    staticModels.Add(new SampleModelEntity(Game, ModelPath, MaterialPath)
                    {
                        Position = new Vector3(x * 200f, 5000f, z * 200f)
                    });
                }
            }

            AddEntities(staticModels.ToArray<BaseEntity>());

            //var pointLights = new List<LightEmitter>();
            //for (var x = 0; x < 5; x++)
            //{
            //    for (var z = 0; z < 5; z++)
            //    {
            //        pointLights.Add(new LightEmitter(Game, Color.Red, LightType.Point)
            //        {
            //            position = new Vector3((x + 0.5f) * 500f, 0, (z + 0.5f) * 500f),
            //            Radius = 1000f
            //        });
            //    }
            //}

            //PointLights = pointLights.ToArray();
            //AddEntities(pointLights.ToArray<BaseEntity>());
            AddEntities(GlobalLight, PointLight1, PointLight2);
        }

        //private const string ModelPath = "models/ground";
        //private const string MaterialPath = "materials/ground";

        private const string ModelPath = "models/ship1";
        private const string MaterialPath = "materials/ship1";

        public LightEmitter GlobalLight { get; private set; }

        public LightEmitter PointLight1 { get; private set; }

        public LightEmitter PointLight2 { get; private set; }

        public LightEmitter[] PointLights { get; private set; }

        public SampleModelEntity StaticModel1 { get; private set; }
        
        public Terrain Terrain { get; private set; }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            var movement = Vector3.Zero;
            var rotation = Matrix.Identity;
            
            if (Keyboard.GetState().IsKeyDown(Keys.F))
                rotation *= Matrix.CreateRotationX(MathHelper.ToRadians(1.0f));
            if (Keyboard.GetState().IsKeyDown(Keys.R))
                rotation *= Matrix.CreateRotationX(MathHelper.ToRadians(-1.0f));

            StaticModel1.Position += movement;
            StaticModel1.Rotation *= rotation;

            if (Keyboard.GetState().IsKeyDown(Keys.OemPlus))
            {
                foreach (var light in PointLights)
                {
                    light.Radius *= 1.01f;                    
                }

                PointLight1.Radius *= 1.01f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.OemMinus))
            {
                foreach (var light in PointLights)
                {
                    light.Radius *= 1.01f;
                }

                PointLight1.Radius *= 0.99f;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.F1))
            {
                foreach (var light in PointLights)
                {
                    light.Falloff = FalloffType.Linear;
                }

                PointLight1.Falloff = FalloffType.Linear;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.F2))
            {
                foreach (var light in PointLights)
                {
                    light.Falloff = FalloffType.Linear;
                }

                PointLight1.Falloff = FalloffType.InverseLinear;
            }
        }
    }
}
