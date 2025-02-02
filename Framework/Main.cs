﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Framework
{
    public class Main : Game
    {
        private GraphicsDeviceManager _graphics;
        private GraphicsDevice graphicsDevice;
        private SpriteBatch _spriteBatch;

        public int screenSizeX;
        public int screenSizeY;

        public IDictionary<string, Scene> scenes = new Dictionary<string, Scene>();
        public Scene currentScene;
        public IDictionary<string, SpritePickable> inventory = new Dictionary<string, SpritePickable>();

        private SpriteFont pixeled10;


        public Main()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            graphicsDevice = this.GraphicsDevice;
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 500;
            _graphics.ApplyChanges();
            screenSizeX = graphicsDevice.Viewport.Width;
            screenSizeY = graphicsDevice.Viewport.Height;


            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            scenes.Add("level00", new Scene(graphicsDevice, "Content/ScenesDatas/scene00.lvl", inventory, ChangeMainInventory, RemoveFromUIInventories));
            scenes.Add("level01", new Scene(graphicsDevice, "Content/ScenesDatas/scene01.lvl", inventory, ChangeMainInventory, RemoveFromUIInventories));

            foreach(var scene in scenes)
                scene.Value.InitializeDatas(GetContent, scenes);

            currentScene = scenes["level00"];

            pixeled10 = Content.Load<SpriteFont>("Fonts/Pixeled10");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (IsActive)
            {
                currentScene.Update(ChangeScene);
            }


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            currentScene.Draw(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }


        public void ChangeScene(Scene newScene, int xPositionToTeleportPlayer)
        {
            inventory = currentScene.inventory;
            currentScene = newScene;
            currentScene.player.position.X = xPositionToTeleportPlayer;
            currentScene.player.isMoving = false;
            currentScene.inventory = inventory;
            currentScene.UpdateUIInventory();
        }

        public void RemoveFromUIInventories(string elementButtonName)
        {
            foreach(var scene in scenes)
                scene.Value.ui.RemoveElement(elementButtonName);
        }

        public void ChangeMainInventory(IDictionary<string, SpritePickable> newInventory)
        {
            inventory = newInventory;
        }

        public Texture2D GetContent(string path)
        {
            return Content.Load<Texture2D>(path);
        }
    }
}