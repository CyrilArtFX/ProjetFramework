﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Framework
{
    public class SpriteControlled : Sprite
    {
        private float speed;
        private float goalPositionX;
        public bool isMoving;

        public SpriteControlled(Texture2D baseTexture, Vector2 basePosition, Vector2 baseSize, bool isCentered, Rectangle colliderBoxAdjustments, float baseSpeed, Animation anim) : base(baseTexture, basePosition, baseSize, isCentered, colliderBoxAdjustments, anim)
        {
            isMoving = false;
            speed = baseSpeed;
        }

        public SpriteControlled(Texture2D baseTexture, Vector2 basePosition, Vector2 baseSize, bool isCentered, float baseSpeed, Animation anim) : base(baseTexture, basePosition, baseSize, isCentered, anim)
        {
            isMoving = false;
            speed = baseSpeed;
        }

        public void MoveTo(float goalPositionX)
        {
            this.goalPositionX = goalPositionX;
            isMoving = true;
        }

        public override void Update()
        {
            base.Update();
            if (isMoving)
            {
                if (goalPositionX > position.X)
                {
                    if (position.X + speed <= goalPositionX) position.X += speed;
                    else position.X = goalPositionX;
                }
                else
                {
                    if (position.X - speed >= goalPositionX) position.X -= speed;
                    else position.X = goalPositionX;
                }
                if (position.X == goalPositionX) isMoving = false;
            }
        }
    }
}
