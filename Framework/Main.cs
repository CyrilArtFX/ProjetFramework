using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
            screenSizeX = graphicsDevice.Viewport.Width;
            screenSizeY = graphicsDevice.Viewport.Height;


            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            scenes.Add("level00" ,new Scene(Content.Load<Texture2D>("Sprites/background"), Content.Load<Texture2D>("Sprites/ground"), graphicsDevice));
            scenes["level00"].AddSprite("player", new SpriteControlled(Content.Load<Texture2D>("Sprites/player"), new Vector2(300, 405), new Vector2(64, 64), true, new Rectangle(20, 15, 23, 47), 5));
            scenes["level00"].AddSprite("friend", new Sprite(Content.Load<Texture2D>("Sprites/friend"), new Vector2(400, 405), new Vector2(64, 64), true, new Rectangle(21, 15, 23, 47)));
            currentScene = scenes["level00"];

            pixeled10 = Content.Load<SpriteFont>("Fonts/Pixeled10");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (IsActive)
            {
                currentScene.Update();
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

    }
}