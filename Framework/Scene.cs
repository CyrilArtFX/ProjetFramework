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
        public UI_Group ui;
        private IDictionary<string, SpritePickable> pickables = new Dictionary<string, SpritePickable>();

        private Texture2D backgroundTex;
        private Texture2D groundTex;
        private int screenSizeX;
        private int screenSizeY;
        private GraphicsDevice graphicsDevice;

        private Vector2 mousePosition;
        private bool mouseLeftClick = false;
        private bool mouseLeftPress = false;

        public IDictionary<string, SpritePickable> inventory = new Dictionary<string, SpritePickable>();
        private Action<IDictionary<string, SpritePickable>> ChangeInventory;
        private Action<string> RemoveFromInventories;

        private List<Message> messages = new List<Message>();
        private List<SpriteStateful> observers = new List<SpriteStateful>();

        private string[] datas;


        public Scene(GraphicsDevice graphicsDevice, string datasFilePath, IDictionary<string, SpritePickable> inventory, Action<IDictionary<string, SpritePickable>> ChangeInventory, Action<string> RemoveFromInventories)
        {
            screenSizeX = graphicsDevice.Viewport.Width;
            screenSizeY = graphicsDevice.Viewport.Height;
            this.graphicsDevice = graphicsDevice;

            this.inventory = inventory;
            this.ChangeInventory = ChangeInventory;
            this.RemoveFromInventories = RemoveFromInventories;
            ui = new UI_Group();

            datas = File.ReadAllLines(datasFilePath);
        }

        public void Update(Action<Scene, int> ChangeScene)
        {
            if(mouseLeftClick)
                if(!ui.isOneElementHover)
                    player.MoveTo(mousePosition.X);

            player.Update();
            if (ui != null) ui.Update();

            foreach (var warp in warps)
                if (player.Intersects(warp.Value))
                    ChangeScene(warp.Value.sceneToGo, warp.Value.xPlayerArrive);

            foreach (var pickable in pickables)
                if (player.Intersects(pickable.Value))
                    AddToInventory(pickable.Value, pickable.Key);



            foreach(Message message in messages)
            {
                Notify(message);
                foreach(SpriteStateful observer in observers)
                    observer.Notify(message);
            }
            messages.Clear();


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
            foreach (var pickable in pickables)
                pickable.Value.Draw(spriteBatch);
            player.Draw(spriteBatch);
            if(ui != null) ui.Draw(spriteBatch);
        }

        public void SendMessage(Message message)
        {
            messages.Add(message);
        }

        public void Notify(Message message)
        {
            if (message.type == Message.MessageType.inventoryElementClicked)
                RemoveFromInventory(message.content);
        }

        public void AddToInventory(SpritePickable pickable, string pickableName)
        {
            pickables.Remove(pickableName);
            inventory.Add(pickableName, pickable);
            UpdateInventoryUI();
        }

        public void RemoveFromInventory(string pickableName)
        {
            RemoveFromInventories(pickableName);
            inventory.Remove(pickableName);
            UpdateInventoryUI();
        }

        public void UpdateInventoryUI()
        {
            foreach (var element in inventory)
                ui.RemoveElement(element.Key + "Button");
            int i = 0;
            foreach (var element in inventory)
            {
                List<Message> messagesToSend = element.Value.messagesToSendWhenInInventory;
                messagesToSend.Add(new Message(Message.MessageType.inventoryElementClicked, element.Key));
                ui.AddElement(element.Key + "Button", new UI_Button(new Vector2((i * 95) + 10, 10), new Vector2(90, 90), element.Value.texture, element.Value.texture, element.Value.texture, SendMessage, messagesToSend));
                i++;
            }
            ChangeInventory(inventory);
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
                    if (cells[7] == "ColliderBox")
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

                else if (cells[0] == "panel")
                {
                    ui.AddElement(cells[5], new UI_Panel(new Vector2(float.Parse(cells[1]), float.Parse(cells[2])), new Vector2(float.Parse(cells[3]), float.Parse(cells[4])), graphicsDevice));
                }

                else if (cells[0] == "button")
                {
                    int numberOfMessages = int.Parse(cells[9]);
                    List<Message> messagesToSent = new List<Message>();
                    for (int i = 0; i < numberOfMessages; i++)
                        messagesToSent.Add(new Message((Message.MessageType)Enum.Parse(typeof(Message.MessageType), cells[10 + i * 2]), cells[11 + i * 2]));
                    ui.AddElement(cells[5], new UI_Button(new Vector2(float.Parse(cells[1]), float.Parse(cells[2])), new Vector2(float.Parse(cells[3]), float.Parse(cells[4])), GetContent(cells[6]), GetContent(cells[7]), GetContent(cells[8]), SendMessage, messagesToSent));
                }

                else if (cells[0] == "spriteStateful")
                {
                    if (cells[6] == "ColliderBox")
                    {
                        int numberOfStates = int.Parse(cells[11]);
                        List<Texture2D> textures = new List<Texture2D>();
                        IDictionary<string, int> states = new Dictionary<string, int>();
                        for (int i = 0; i < numberOfStates; i++)
                        {
                            textures.Add(GetContent(cells[12 + i]));
                            states.Add(cells[12 + numberOfStates + i], i);
                        }
                        SpriteStateful spriteStateful = new SpriteStateful(new Vector2(int.Parse(cells[1]), int.Parse(cells[2])), new Vector2(int.Parse(cells[3]), int.Parse(cells[4])), bool.Parse(cells[5]), new Rectangle(int.Parse(cells[7]), int.Parse(cells[8]), int.Parse(cells[9]), int.Parse(cells[10])), textures, states, cells[12 + 2 * numberOfStates]);
                        sprites.Add(cells[13 + 2 * numberOfStates], spriteStateful);
                        observers.Add(spriteStateful);
                    }
                    else
                    {
                        int numberOfStates = int.Parse(cells[6]);
                        List<Texture2D> textures = new List<Texture2D>();
                        IDictionary<string, int> states = new Dictionary<string, int>();
                        for (int i = 0; i < numberOfStates; i++)
                        {
                            textures.Add(GetContent(cells[7 + i]));
                            states.Add(cells[7 + numberOfStates + i], i);
                        }
                        SpriteStateful spriteStateful = new SpriteStateful(new Vector2(int.Parse(cells[1]), int.Parse(cells[2])), new Vector2(int.Parse(cells[3]), int.Parse(cells[4])), bool.Parse(cells[5]), textures, states, cells[7 + 2 * numberOfStates]);
                        sprites.Add(cells[8 + 2 * numberOfStates], spriteStateful);
                        observers.Add(spriteStateful);
                    }
                }

                else if (cells[0] == "spritePickable")
                {
                    if (cells[7] == "ColliderBox")
                    {
                        int numberOfMessages = int.Parse(cells[13]);
                        List<Message> messagesToSentWhenInInventory = new List<Message>();
                        for(int i = 0; i < numberOfMessages; i++)
                            messagesToSentWhenInInventory.Add(new Message((Message.MessageType)Enum.Parse(typeof(Message.MessageType), cells[14 + i * 2]), cells[15 + i * 2]));
                        pickables.Add(cells[12], new SpritePickable(GetContent(cells[1]), new Vector2(float.Parse(cells[2]), float.Parse(cells[3])), new Vector2(float.Parse(cells[4]), float.Parse(cells[5])), bool.Parse(cells[6]), new Rectangle(int.Parse(cells[8]), int.Parse(cells[9]), int.Parse(cells[10]), int.Parse(cells[11])), messagesToSentWhenInInventory));
                    }
                    else
                    {
                        int numberOfMessages = int.Parse(cells[8]);
                        List<Message> messagesToSentWhenInInventory = new List<Message>();
                        for (int i = 0; i < numberOfMessages; i++)
                            messagesToSentWhenInInventory.Add(new Message((Message.MessageType)Enum.Parse(typeof(Message.MessageType), cells[9 + i * 2]), cells[10 + i * 2]));
                        pickables.Add(cells[7], new SpritePickable(GetContent(cells[1]), new Vector2(float.Parse(cells[2]), float.Parse(cells[3])), new Vector2(float.Parse(cells[4]), float.Parse(cells[5])), bool.Parse(cells[6]), messagesToSentWhenInInventory));
                    }
                }
            }
        }
    }
}
