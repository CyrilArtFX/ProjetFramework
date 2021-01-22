using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;

namespace Framework
{
    public class Scene
    {
        private IDictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();
        private IDictionary<string, Warp> warps = new Dictionary<string, Warp>();
        public SpriteControlled player;

        private Texture2D backgroundTex;
        private Texture2D groundTex;
        private int screenSizeX;
        private int screenSizeY;

        public Vector2 mousePosition;
        public bool mouseLeftClick = false;
        public bool mouseLeftPress = false;

        private string[] datas;


        public Scene(GraphicsDevice graphicsDevice, string datasFilePath, Func<string, Texture2D> GetContent)
        {
            screenSizeX = graphicsDevice.Viewport.Width;
            screenSizeY = graphicsDevice.Viewport.Height;

            datas = File.ReadAllLines(datasFilePath);
        }

        public void Update(Action<Scene, int> ChangeScene)
        {
            if(mouseLeftClick)
                player.MoveTo(mousePosition.X);

            player.Update();

            foreach (var warp in warps)
                if (player.Intersects(warp.Value))
                    ChangeScene(warp.Value.sceneToGo, warp.Value.xPlayerArrive);


            //Mouse Part
            mousePosition = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            ButtonState mouseLeftButtonState = Mouse.GetState().LeftButton;
            if (mouseLeftPress)
            {
                mouseLeftClick = false;
            }
            if (mouseLeftButtonState == ButtonState.Pressed && !mouseLeftPress)
            {
                mouseLeftClick = true;
                mouseLeftPress = true;
            }
            if (mouseLeftButtonState == ButtonState.Released) mouseLeftPress = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(backgroundTex, new Rectangle(0, 0, screenSizeX, screenSizeY), Color.White);
            spriteBatch.Draw(groundTex, new Rectangle(0, 5 * screenSizeY / 6, screenSizeX, screenSizeY / 6), Color.White);
            foreach (var sprite in sprites)
                sprite.Value.Draw(spriteBatch);
            foreach (var warp in warps)
                warp.Value.Draw(spriteBatch);
            player.Draw(spriteBatch);
        }

        public void InitializeDatas(Func<string, Texture2D> GetContent, IDictionary<string, Scene> listOfScenes)
        {
            foreach(string line in datas)
            {
                string[] cells = line.Split(';');

                if (cells[0] == "background")
                {
                    backgroundTex = GetContent(cells[1]);
                }

                else if (cells[0] == "ground")
                {
                    groundTex = GetContent(cells[1]);
                }

                else if (cells[0] == "player")
                {
                    if(cells[7] == "ColliderBox")
                        player = new SpriteControlled(GetContent(cells[1]), new Vector2(float.Parse(cells[2]), float.Parse(cells[3])), new Vector2(float.Parse(cells[4]), float.Parse(cells[5])), bool.Parse(cells[6]), new Rectangle(int.Parse(cells[8]), int.Parse(cells[9]), int.Parse(cells[10]), int.Parse(cells[11])), float.Parse(cells[12]));
                    else
                        player = new SpriteControlled(GetContent(cells[1]), new Vector2(float.Parse(cells[2]), float.Parse(cells[3])), new Vector2(float.Parse(cells[4]), float.Parse(cells[5])), bool.Parse(cells[6]), float.Parse(cells[7]));
                }

                else if (cells[0] == "sprite")
                {
                    if (cells[7] == "ColliderBox")
                        sprites.Add(cells[12], new Sprite(GetContent(cells[1]), new Vector2(float.Parse(cells[2]), float.Parse(cells[3])), new Vector2(float.Parse(cells[4]), float.Parse(cells[5])), bool.Parse(cells[6]), new Rectangle(int.Parse(cells[8]), int.Parse(cells[9]), int.Parse(cells[10]), int.Parse(cells[11]))));
                    else
                        sprites.Add(cells[7], new Sprite(GetContent(cells[1]), new Vector2(float.Parse(cells[2]), float.Parse(cells[3])), new Vector2(float.Parse(cells[4]), float.Parse(cells[5])), bool.Parse(cells[6])));
                }

                else if (cells[0] == "warp")
                {
                    if (cells[7] == "ColliderBox")
                        warps.Add(cells[14], new Warp(GetContent(cells[1]), new Vector2(float.Parse(cells[2]), float.Parse(cells[3])), new Vector2(float.Parse(cells[4]), float.Parse(cells[5])), bool.Parse(cells[6]), new Rectangle(int.Parse(cells[8]), int.Parse(cells[9]), int.Parse(cells[10]), int.Parse(cells[11])), listOfScenes[cells[12]], int.Parse(cells[13])));
                    else
                        warps.Add(cells[9], new Warp(GetContent(cells[1]), new Vector2(float.Parse(cells[2]), float.Parse(cells[3])), new Vector2(float.Parse(cells[4]), float.Parse(cells[5])), bool.Parse(cells[6]), listOfScenes[cells[7]], int.Parse(cells[8])));
                }
            }
        }
    }
}
