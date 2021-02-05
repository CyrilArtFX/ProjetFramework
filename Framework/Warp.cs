using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Framework
{
    public class Warp : Sprite
    {
        public Scene sceneToGo;
        public int xPlayerArrive;

        public Warp(Texture2D baseTexture, Vector2 basePosition, Vector2 baseSize, bool isCentered, Rectangle colliderBoxAdjustments, Scene sceneToGo, int xPlayerArrive, Animation anim) : base(baseTexture, basePosition, baseSize, isCentered, colliderBoxAdjustments, anim)
        {
            this.sceneToGo = sceneToGo;
            this.xPlayerArrive = xPlayerArrive;
        }

        public Warp(Texture2D baseTexture, Vector2 basePosition, Vector2 baseSize, bool isCentered, Scene sceneToGo, int xPlayerArrive, Animation anim) : base(baseTexture, basePosition, baseSize, isCentered, anim)
        {
            this.sceneToGo = sceneToGo;
            this.xPlayerArrive = xPlayerArrive;
        }

        public override void Update()
        {
            base.Update();
        }
    }
}
