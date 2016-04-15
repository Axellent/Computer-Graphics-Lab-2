﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using GameEngine;
using Microsoft.Xna.Framework.Input;

namespace NAJ_Lab2.Init
{
    class InitGame
    {
        private SystemManager sm = SystemManager.Instance;

        public InitGame(ECSEngine engine)
        {

            sm.RegisterSystem("Game", new TransformSystem());
            sm.RegisterSystem("Game", new ModelRenderSystem());
            sm.RegisterSystem("Game", new TerrainRenderSystem());
            sm.RegisterSystem("Game", new KeyBoardSystem());
            sm.RegisterSystem("Game", new CameraSystem());
            sm.RegisterSystem("Game", new ChopperControlSystem(engine));

            Entity keyboardControl = EntityFactory.Instance.NewEntityWithTag("keyboard");
            ComponentManager.Instance.AddComponentToEntity(keyboardControl, new KeyBoardComponent());
            KeyBoardComponent k = ComponentManager.Instance.GetEntityComponent<KeyBoardComponent>(keyboardControl);

            k.AddKeyToAction("forward", Keys.Up);
            k.AddKeyToAction("back", Keys.Down);
            k.AddKeyToAction("left", Keys.Left);
            k.AddKeyToAction("right", Keys.Right);
            k.AddKeyToAction("down", Keys.Z);
            k.AddKeyToAction("up", Keys.A);

            Entity chopper = EntityFactory.Instance.NewEntityWithTag("Chopper");
            ModelComponent cm = new ModelComponent(engine.LoadContent<Model>("Chopper"), true);
            cm.AddMeshTransform(1, Matrix.CreateRotationY(0.2f));
            cm.AddMeshTransform(3, Matrix.CreateRotationY(0.5f));
            ComponentManager.Instance.AddComponentToEntity(chopper, cm);
            TransformComponent chopperTransform = new TransformComponent();
            chopperTransform.position = new Vector3(0.0f, 0.0f, 0.0f);
            chopperTransform.vRotation = new Vector3(0,0,0);
            chopperTransform.scale = new Vector3(0.3f, 0.3f, 0.3f);
            ComponentManager.Instance.AddComponentToEntity(chopper, chopperTransform);

            Entity Camera = EntityFactory.Instance.NewEntityWithTag("3DCamera");
            CameraComponent cc = new CameraComponent(engine.GetGraphicsDeviceManager());
            cc.position = new Vector3(0, 20, 60);

            //Use this line instead to see the back rotor rotate at a different speed! :)
            //cc.SetChaseCameraPosition(new Vector3(10f, 20f, 40f));
            cc.SetChaseCameraPosition(new Vector3(0f, 10f, 20f));
                        
            ComponentManager.Instance.AddComponentToEntity(Camera, cc);
            ComponentManager.Instance.AddComponentToEntity(Camera, new TransformComponent());
            cc.SetTargetEntity("Chopper");

            Entity Terrain = EntityFactory.Instance.NewEntityWithTag("Terrain");
            TerrainComponent t = new TerrainComponent(engine.GetGraphicsDevice(), engine.LoadContent<Texture2D>("Canyon"), engine.LoadContent<Texture2D>("Grass"));
            TransformComponent tf = new TransformComponent();
            tf.world = Matrix.CreateTranslation(0, 0, 0);
            tf.position = Vector3.Zero;
            ComponentManager.Instance.AddComponentToEntity(Terrain, t);
            ComponentManager.Instance.AddComponentToEntity(Terrain, tf);

            SceneManager.Instance.AddEntityToSceneOnLayer("Game",6, Camera);
            SceneManager.Instance.AddEntityToSceneOnLayer("Game",3, chopper);
            SceneManager.Instance.AddEntityToSceneOnLayer("Game",2, Terrain);
            SceneManager.Instance.SetActiveScene("Game");
            SceneManager.Instance.AddEntityToSceneOnLayer("Game", 0, keyboardControl);
            SystemManager.Instance.Category = "Game";
        }
    }
}
