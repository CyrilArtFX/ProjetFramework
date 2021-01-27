using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Framework
{
    public class SpritePickable : Sprite
    {
        public SpritePickable(Texture2D baseTexture, Vector2 basePosition, Vector2 baseSize, bool isCentered, Rectangle colliderBoxAdjustments) : base(baseTexture, basePosition, baseSize, isCentered, colliderBoxAdjustments)
        {
        }
    }
}
