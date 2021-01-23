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

        public bool isVisible = true;

        public IDictionary<string, Action> events = new Dictionary<string, Action>();

        public UI_Element(Vector2 position, Vector2 size, GraphicsDevice graphicsDevice)
        {
            this.position = position;
            this.size = size;
        }

        public virtual void Update()
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }

        public void ChangeVisibility(bool newVisibility)
        {
            isVisible = newVisibility;
        }
    }
}
