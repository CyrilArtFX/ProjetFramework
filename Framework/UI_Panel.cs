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

        private Vector2 mousePosition;
        public bool isHover = false;

        public UI_Panel(Vector2 position, Vector2 size, GraphicsDevice graphicsDevice) : base(position, size, graphicsDevice)
        {
            this.position = position;
            this.size = size;
            color = Color.White;

            texture = new Texture2D(graphicsDevice, (int)(size.X), (int)(size.Y));
            Color[] panelColorData = new Color[(int)(size.X) * (int)(size.Y)];
            for (int i = 0; i < (int)(size.X) * (int)(size.Y); i++)
                panelColorData[i] = Color.White;
            texture.SetData<Color>(panelColorData);

            events.Add("hoverIn", OnHoverIn);
            events.Add("hoverOut", OnHoverOut);
        }

        public override void Update()
        {
            mousePosition = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            if(!isHover && (mousePosition.X > position.X && mousePosition.X < position.X + size.X && mousePosition.Y > position.Y && mousePosition.Y < position.Y + size.Y))
            {
                isHover = true;
                events["hoverIn"]();
            }
            if (isHover && !(mousePosition.X > position.X && mousePosition.X < position.X + size.X && mousePosition.Y > position.Y && mousePosition.Y < position.Y + size.Y))
            {
                isHover = false;
                events["hoverOut"]();
            }
        }

        public override void Draw(SpriteBatch spriteBash)
        {
            if(isVisible) spriteBash.Draw(texture, position, color);
        }

        public void OnHoverIn()
        {
            color = Color.Green;
        }

        public void OnHoverOut()
        {
            color = Color.White;
        }
    }
}
