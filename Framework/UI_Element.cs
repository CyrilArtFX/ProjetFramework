using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Framework
{
    public class UI_Element
    {
        public Vector2 position;
        public Vector2 size;
        public Rectangle rectangleToDraw;

        public Vector2 mousePosition;

        public bool isHover;

        public bool isVisible = true;

        public IDictionary<string, Action> events = new Dictionary<string, Action>();

        public UI_Element(Vector2 position, Vector2 size)
        {
            this.position = position;
            this.size = size;
            rectangleToDraw = new Rectangle((int)(position.X), (int)(position.Y), (int)(size.X), (int)(size.Y));

            events.Add("hoverIn", OnHoverIn);
            events.Add("hoverOut", OnHoverOut);
        }

        public virtual void Update()
        {
            mousePosition = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            if (!isHover && (mousePosition.X > position.X && mousePosition.X < position.X + size.X && mousePosition.Y > position.Y && mousePosition.Y < position.Y + size.Y))
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

        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }

        public virtual void OnHoverIn()
        {

        }

        public virtual void OnHoverOut()
        {

        }

        public void ChangeVisibility(bool newVisibility)
        {
            isVisible = newVisibility;
        }
    }
}
