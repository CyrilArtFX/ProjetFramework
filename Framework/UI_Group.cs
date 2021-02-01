using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Framework
{
    public class UI_Group
    {
        public bool isOneElementHover;

        private bool isVisible = true;
        public IDictionary<string, UI_Element> elements = new Dictionary<string, UI_Element>();

        public UI_Group()
        {

        }

        public void Update()
        {
            isOneElementHover = false;
            foreach(var element in elements)
            {
                element.Value.Update();
                if (element.Value.isHover) isOneElementHover = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if(isVisible)
            {
                foreach(var element in elements)
                {
                    element.Value.Draw(spriteBatch);
                }
            }   
        }

        public void AddElement(string name, UI_Element element)
        {
            elements.Add(name, element);
        }

        public void RemoveElement(string name)
        {
            elements.Remove(name);
        }

        public void ChangeVisibility(bool newVisibility)
        {
            isVisible = newVisibility;
        }
    }
}
