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

        public Vector2 mousePosition;
        public bool mouseLeftClick = false;
        public bool mouseLeftPress = false;

        public Texture2D backgroundTexture;
        public Texture2D groundTexture;

        public SpriteControlled player;
        public Sprite friend;

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

            backgroundTexture = Content.Load<Texture2D>("Sprites/background");
            groundTexture = Content.Load<Texture2D>("Sprites/ground");
            player = new SpriteControlled(Content.Load<Texture2D>("Sprites/player"), new Vector2(300, 405), new Vector2(64, 64), true, new Rectangle(20, 15, 23, 47), 5);
            friend = new Sprite(Content.Load<Texture2D>("Sprites/friend"), new Vector2(400, 405), new Vector2(64, 64), true, new Rectangle(21, 15, 23, 47));

            pixeled10 = Content.Load<SpriteFont>("Fonts/Pixeled10");

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (IsActive)
            {
                mousePosition = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
                if (mouseLeftClick)
                {
                    player.MoveTo(mousePosition.X);
                }

                player.Update();

                //Mouse Part
                ButtonState mouseLeftButtonState = Mouse.GetState().LeftButton;
                if (mouseLeftPress)
                {
                    mouseLeftClick = false;
                }
                if (mouseLeftButtonState == ButtonState.Pressed && !mouseLeftPress)
                {
                    mouseLeftClick = true;
                    mouseLeftPress = true;
                }
                if (mouseLeftButtonState == ButtonState.Released) mouseLeftPress = false;
            }


            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();

            _spriteBatch.Draw(backgroundTexture, new Rectangle(0, 0, screenSizeX, screenSizeY), Color.White);
            _spriteBatch.Draw(groundTexture, new Rectangle(0, 5 * screenSizeY / 6, screenSizeX, screenSizeY / 6), Color.White);
            player.Draw(_spriteBatch);
            if (player.Intersects(friend))
            {
                _spriteBatch.DrawString(pixeled10, "Oops, sorry.", new Vector2(player.position.X - player.centerOffsetX - 30, player.position.Y - player.centerOffsetY - 25), Color.Red);
            }
            friend.Draw(_spriteBatch);


            // TODO: Add your drawing code here
            _spriteBatch.End();

            base.Draw(gameTime);
        }

    }
}