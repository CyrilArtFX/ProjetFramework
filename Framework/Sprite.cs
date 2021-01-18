using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Framework
{
    public class Sprite
    {
        public Texture2D texture;
        public Vector2 position;
        public Vector2 size;
        public float centerOffsetX = 0;
        public float centerOffsetY = 0;

        private Rectangle colliderBoxAdjustments;

        public Sprite(Texture2D baseTexture, Vector2 basePosition, Vector2 baseSize, bool isCentered, Rectangle colliderBoxAdjustments)
        {
            texture = baseTexture;
            position = basePosition;
            size = baseSize;
            if(isCentered)
            {
                centerOffsetX = size.X / 2;
                centerOffsetY = size.Y;
            }

            this.colliderBoxAdjustments = colliderBoxAdjustments;
        }


        public bool Intersects(Sprite otherSprite)
        {
            (float pX1, float pY1) = (position.X - centerOffsetX + colliderBoxAdjustments.X, position.Y - centerOffsetY + colliderBoxAdjustments.Y);
            (float sX1, float sY1) = (colliderBoxAdjustments.Width, colliderBoxAdjustments.Height);
            (float pX2, float pY2) = (otherSprite.position.X - otherSprite.centerOffsetX + otherSprite.colliderBoxAdjustments.X, otherSprite.position.Y - otherSprite.centerOffsetY + otherSprite.colliderBoxAdjustments.Y);
            (float sX2, float sY2) = (otherSprite.colliderBoxAdjustments.Width, otherSprite.colliderBoxAdjustments.Height);
            if (pX1 + sX1 < pX2 || pX2 + sX2 < pX1 || pY1 + sY1 < pY2 || pY2 + sY2 < pY1) return false;
            else return true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Rectangle((int)(position.X - centerOffsetX), (int)(position.Y - centerOffsetY), (int)(size.X), (int)(size.Y)), Color.White);
        }
    }
}
