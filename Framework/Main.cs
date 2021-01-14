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

        public Texture2D backgroundTexture;

        public List<SpriteDatas> sprites = new List<SpriteDatas>();

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

            backgroundTexture = Content.Load<Texture2D>("jpp");
            sprites.Add(AddSprite("templier1", Content.Load<Texture2D>("templier"), new Vector2(20, 10), new Vector2(50, 50)));
            sprites.Add(AddSprite("templier2", Content.Load<Texture2D>("templier"), new Vector2(400, 100), new Vector2(70, 70)));

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();

            _spriteBatch.Draw(backgroundTexture, new Rectangle(0, 0, screenSizeX, screenSizeY), Color.White);
            foreach(SpriteDatas sprite in sprites)
            {
                _spriteBatch.Draw(sprite.texture, new Rectangle((int)(sprite.position.X), (int)(sprite.position.Y), (int)(sprite.size.X), (int)(sprite.size.Y)), Color.White);
            }

            // TODO: Add your drawing code here
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        public SpriteDatas AddSprite(string name, Texture2D texture, Vector2 position, Vector2 size)
        {
            SpriteDatas sprite = new SpriteDatas();
            sprite.name = name;
            sprite.texture = texture;
            sprite.position = position;
            sprite.size = size;

            return sprite;
        }
    }
}

[System.Serializable]
public struct SpriteDatas
{
    public string name;
    public Texture2D texture;
    public Vector2 position;
    public Vector2 size;
}