﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.InputDefs;
using GameEngine;
using Microsoft.Xna.Framework;

namespace NAJ_Lab2
{
    class ChopperControlSystem : IUpdateSystem
    {
        ECSEngine engine;

        public ChopperControlSystem(ECSEngine engine)
        {
            this.engine = engine;
        }
        public void Update(GameTime gameTime)
        {
            List<Entity> sceneEntities = SceneManager.Instance.GetActiveScene().GetAllEntities();
            Entity chopper = ComponentManager.Instance.GetEntityWithTag("Chopper",sceneEntities);
            TransformComponent t = ComponentManager.Instance.GetEntityComponent<TransformComponent>(chopper);
            ModelComponent chopModel = ComponentManager.Instance.GetEntityComponent<ModelComponent>(chopper);

            Entity terrain = ComponentManager.Instance.GetEntityWithTag("Terrain", sceneEntities);
            TerrainComponent tcomp = ComponentManager.Instance.GetEntityComponent<TerrainComponent>(terrain);

            engine.SetWindowTitle("Chopper x:" + t.position.X + "Chopper y:" + t.position.Y + "Chopper z:" +t.position.Z + "Map height:" + tcomp.GetTerrainHeight(t.position.X, Math.Abs(t.position.Z)));
            t.position = new Vector3(t.position.X, 0.2f+tcomp.GetTerrainHeight(t.position.X, Math.Abs(t.position.Z)), t.position.Z);

            //set the mesh transforms to zero
            chopModel.SetMeshTransform(1, Matrix.CreateRotationY(0.0f));
            chopModel.SetMeshTransform(3, Matrix.CreateRotationY(0.0f));

            Entity kb = ComponentManager.Instance.GetEntityWithTag("keyboard", sceneEntities);
            if (kb != null)
            {
                KeyBoardComponent k = ComponentManager.Instance.GetEntityComponent<KeyBoardComponent>(kb);
                if (k != null)
                {
                    Vector3 newRot = Vector3.Zero;
                    bool moving = false;

                    if (Utilities.CheckKeyboardAction("right", BUTTON_STATE.HELD, k))
                    {
                        newRot = new Vector3(-0.4f, 0f, 0f) * (float)gameTime.ElapsedGameTime.TotalSeconds;
                        t.vRotation = newRot;
                        moving = true;
                    }
                    else if (Utilities.CheckKeyboardAction("left", BUTTON_STATE.HELD, k))
                    {
                        newRot = new Vector3(0.4f, 0f, 0f) * (float)gameTime.ElapsedGameTime.TotalSeconds; 
                        t.vRotation = newRot;
                        moving = true;
                    }
                    else
                    {
                        t.vRotation = Vector3.Zero;
                    }

                    if (Utilities.CheckKeyboardAction("up", BUTTON_STATE.HELD, k))
                    {
                        t.position += new Vector3(0f, 4f, 0f) * (float)gameTime.ElapsedGameTime.TotalSeconds;
                        moving = true;
                    }
                    if (Utilities.CheckKeyboardAction("down", BUTTON_STATE.HELD, k))
                    {
                        t.position += new Vector3(0f, -4f, 0f) * (float)gameTime.ElapsedGameTime.TotalSeconds;
                        moving = true;
                    }
                    if (Utilities.CheckKeyboardAction("forward", BUTTON_STATE.HELD, k))
                    {
                        t.position += t.forward * 6f * (float)gameTime.ElapsedGameTime.TotalSeconds;
                        moving = true;
                    }
                    if (Utilities.CheckKeyboardAction("back", BUTTON_STATE.HELD, k))
                    {
                        t.position += t.forward * -6f * (float)gameTime.ElapsedGameTime.TotalSeconds;
                        moving = true;
                    }

                    if(moving == true)
                    {
                        chopModel.SetMeshTransform(1, Matrix.CreateRotationY(0.02f));
                        chopModel.SetMeshTransform(3, Matrix.CreateRotationY(0.05f));
                    }
                }
            }
        }
    }
}
