using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Framework
{
    public class UI_Panel : UI_Element
    {
        private Texture2D texture;
        private Color color;

        public UI_Panel(Vector2 position, Vector2 size, GraphicsDevice graphicsDevice) : base(position, size, graphicsDevice)
        {
            color = Color.White;

            texture = new Texture2D(graphicsDevice, (int)(size.X), (int)(size.Y));
            Color[] panelColorData = new Color[(int)(size.X) * (int)(size.Y)];
            for (int i = 0; i < (int)(size.X) * (int)(size.Y); i++)
                panelColorData[i] = Color.White;
            texture.SetData<Color>(panelColorData);
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Draw(SpriteBatch spriteBash)
        {
            if(isVisible) spriteBash.Draw(texture, position, color);
        }

        public override void OnHoverIn()
        {
            color = Color.Green;
        }

        public override void OnHoverOut()
        {
            color = Color.White;
        }
    }
}
