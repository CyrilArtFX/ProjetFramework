using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Framework
{
    public class Scene
    {
        private IDictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();
        private IDictionary<string, SpriteControlled> spritesControlled = new Dictionary<string, SpriteControlled>();

        private Texture2D backgroundTex;
        private Texture2D groundTex;
        private int screenSizeX;
        private int screenSizeY;

        public Vector2 mousePosition;
        public bool mouseLeftClick = false;
        public bool mouseLeftPress = false;

        public Scene(Texture2D backgroundTexture, Texture2D groundTexture, GraphicsDevice graphicsDevice)
        {
            backgroundTex = backgroundTexture;
            groundTex = groundTexture;
            screenSizeX = graphicsDevice.Viewport.Width;
            screenSizeY = graphicsDevice.Viewport.Height;
        }

        public void Update()
        {
            if(mouseLeftClick)
                foreach (var player in spritesControlled)
                    player.Value.MoveTo(mousePosition.X);

            foreach (var player in spritesControlled)
                player.Value.Update();


            //Mouse Part
            mousePosition = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
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

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(backgroundTex, new Rectangle(0, 0, screenSizeX, screenSizeY), Color.White);
            spriteBatch.Draw(groundTex, new Rectangle(0, 5 * screenSizeY / 6, screenSizeX, screenSizeY / 6), Color.White);
            foreach (var sprite in sprites)
                sprite.Value.Draw(spriteBatch);
            foreach (var player in spritesControlled)
                player.Value.Draw(spriteBatch);
        }

        public void AddSprite(string name, Sprite sprite)
        {
            sprites.Add(name, sprite);
        }

        public void AddSprite(string name, SpriteControlled spriteControlled)
        {
            spritesControlled.Add(name, spriteControlled);
        }
    }
}
