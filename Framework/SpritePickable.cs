﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Framework
{
    public class SpritePickable : Sprite
    {
        public List<Message> messagesToSendWhenInInventory = new List<Message>();

        public SpritePickable(Texture2D baseTexture, Vector2 basePosition, Vector2 baseSize, bool isCentered, Rectangle colliderBoxAdjustments, List<Message> messagesToSendWhenInInventory) : base(baseTexture, basePosition, baseSize, isCentered, colliderBoxAdjustments)
        {
            this.messagesToSendWhenInInventory = messagesToSendWhenInInventory;
        }

        public SpritePickable(Texture2D baseTexture, Vector2 basePosition, Vector2 baseSize, bool isCentered, List<Message> messagesToSendWhenInInventory) : base(baseTexture, basePosition, baseSize, isCentered)
        {
            this.messagesToSendWhenInInventory = messagesToSendWhenInInventory;
        }
    }
}
