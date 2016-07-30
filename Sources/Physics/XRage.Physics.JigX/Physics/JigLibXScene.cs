using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using JigLibX.Physics;
using Microsoft.Xna.Framework;
using JigLibX.Collision;

namespace AISTek.XRage.Physics
{
    internal class JigLibXScene : IPhysicsScene
    {
        internal JigLibXScene()
        {
            // TODO:  Expose some of these settings in a generic way.
            jigLibXPhysics = new PhysicsSystem();
            jigLibXPhysics.CollisionSystem = new CollisionSystemGrid(32, 32, 4, 13, 13, 13);
            jigLibXPhysics.EnableFreezing = true;
            jigLibXPhysics.SolverType = PhysicsSystem.Solver.Normal;
            jigLibXPhysics.CollisionSystem.UseSweepTests = true;
            jigLibXPhysics.AllowedPenetration = 0.0001f;

            deletionList = new List<JigLibXActor>();

            // Create synchronization handles
            startFrame = new ManualResetEvent(false);
            endFrame = new ManualResetEvent(false);
            endThread = new ManualResetEvent(false);

            // Create and start the physics processing thread.
            processingThread = new Thread(SceneProcessing);
            processingThread.Priority = ThreadPriority.Normal;
            processingThread.IsBackground = false;
            processingThread.Start();
        }

        public Vector3 Gravity
        {
            get { return jigLibXPhysics.Gravity; }
            set { jigLibXPhysics.Gravity = value; }
        }

        public void SetPhysicsTimeStep(int stepsPerSecond)
        {
            fixedStepFrequency = 1.0f / stepsPerSecond;
        }

        public IPhysicsActor CreateActor(ActorDesc desc)
        {
            if (bFixedTimeMet)
            {
                // This should pause the physics frame ever so slightly so we can add the
                // actor to the physics scene.
                startFrame.Reset();
            }

            var actor = new JigLibXActor(desc);

            if (bFixedTimeMet)
            {
                // Unpause physics now that new actor is in the physics scene
                startFrame.Set();
            }

            return actor;
        }

        public void ScheduleForDeletion(IPhysicsActor actor)
        {
            if (actor is JigLibXActor)
            {
                deletionList.Add((actor as JigLibXActor));
            }
        }

        public void BeginFrame(GameTime gameTime)
        {
            for (int i = deletionList.Count - 1; i > 0; --i)
            {
                RemoveActor(deletionList[i]);
                deletionList.RemoveAt(i);
            }

            // Save the time delta and signal the physics thread to start processing.
            totalDelta += (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f;

            // We want a steady 30hz physics simulation, don't run physics this frame if it has been less than 1/30th of a second
            if (totalDelta >= fixedStepFrequency)
            {
                bFixedTimeMet = true;

                startFrame.Set();
            }
            else
            {
                bFixedTimeMet = false;
            }
        }

        public void EndFrame()
        {
            if (bFixedTimeMet)
            {
                // Block until the physics frame is finished processing.
                endFrame.WaitOne(Timeout.Infinite, false);
                endFrame.Reset();
            }
        }

        /// <summary>
        /// Releases all unmanaged resources for the scene.
        /// </summary>
        public void Dispose()
        { }

        public void KillProcessingThread()
        {
            endThread.Set();
        }

        private void SceneProcessing()
        {
            try
            {
                // Execute until we are exiting.
                while (!endThread.WaitOne(0, false))
                {
                    // Wait for frame start event.
                    if (!startFrame.WaitOne(10, false))
                    {
                        continue;
                    }

                    // Reset the event.
                    startFrame.Reset();

                    int steps = 0;

                    // We only run the physics simulation if there is still enough time left to
                    // simulation, and we do not allow more than a few steps per frame, otherwise
                    // a low framerate could lead to many physics steps, which would lower framerate
                    // even further.
                    while (steps < 3 &&
                           (totalDelta >= fixedStepFrequency))
                    {
                        // Process the frame.
                        jigLibXPhysics.Integrate(fixedStepFrequency);

                        totalDelta -= fixedStepFrequency;
                        ++steps;
                    }

                    // Signal frame completion.
                    endFrame.Set();
                }
            }
            catch (ThreadAbortException)
            { }
        }

        private void RemoveActor(JigLibXActor actor)
        {
            actor.RemoveFromSimulation();
        }

        private float fixedStepFrequency = 1 / 30.0f;

        private PhysicsSystem jigLibXPhysics;
        private float totalDelta = 0.0f;
        private bool bFixedTimeMet = false;
        private Thread processingThread;

        private ManualResetEvent startFrame;
        private ManualResetEvent endFrame;
        private ManualResetEvent endThread;

        private List<JigLibXActor> deletionList;
    }
}
