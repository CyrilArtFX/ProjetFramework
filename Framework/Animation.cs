using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Framework
{
    public class Animation
    {
        private List<Texture2D> sprites = new List<Texture2D>();
        private int numberOfFramesBySprite;
        private int frameCounter = 0;
        private int currentSpriteIndex = 0;
        private int numberOfSprites;
        public Texture2D currentSprite;

        public Animation(Texture2D completeTexture, int spritesSizeX, int numberOfSprites, int numberOfFramesBySprite, GraphicsDevice graphicsDevice)
        {
            for (int i = 0; i < numberOfSprites; i++)
                sprites.Add(SliceTexture(completeTexture, graphicsDevice, new Rectangle(spritesSizeX * i, 0, spritesSizeX, completeTexture.Height)));
            this.numberOfFramesBySprite = numberOfFramesBySprite;
            this.numberOfSprites = numberOfSprites;
            currentSprite = sprites[0];
        }

        public void Update()
        {
            if(frameCounter >= numberOfFramesBySprite)
            {
                currentSpriteIndex++;
                if (currentSpriteIndex == numberOfSprites) currentSpriteIndex = 0;
                currentSprite = sprites[currentSpriteIndex];
            }
            frameCounter++;
        }

        public Texture2D SliceTexture(Texture2D completeTexture, GraphicsDevice graphicsDevice, Rectangle rect)
        {
            Texture2D slicedTexture = new Texture2D(graphicsDevice, rect.Width, rect.Height);
            int size = rect.Width * rect.Height;
            Color[] colorDatas = new Color[size];
            completeTexture.GetData(0, rect, colorDatas, 0, size);
            slicedTexture.SetData(colorDatas);
            return slicedTexture;
        }
    }
}
