using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Framework
{
    public class UI_Button : UI_Element
    {
        private Texture2D currentSprite;
        private Texture2D spriteIdle;
        private Texture2D spriteHover;
        private Texture2D spriteClicked;

        private bool mouseLeftClick = false;
        private bool mouseLeftPress = false;

        public bool isClick = false;

        private Action<Message> sendMessage;

        public UI_Button(Vector2 position, Vector2 size, GraphicsDevice graphicsDevice, Texture2D spriteIdle, Texture2D spriteHover, Texture2D spriteClicked, Action<Message> sendMessage) : base(position, size, graphicsDevice)
        {
            this.sendMessage = sendMessage;

            this.spriteIdle = spriteIdle;
            this.spriteHover = spriteHover;
            this.spriteClicked = spriteClicked;
            currentSprite = spriteIdle;

            events.Add("click", OnClick);
            events.Add("release", OnRelease);
        }

        public override void Update()
        {
            base.Update();
            if(isHover)
            {
                if(!isClick && mouseLeftClick)
                {
                    isClick = true;
                    events["click"]();
                }
                if(isClick && !mouseLeftClick)
                {
                    isClick = false;
                    events["release"]();
                }
            }

            //MousePart
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

        public override void Draw(SpriteBatch spriteBatch)
        {
            if(isVisible) spriteBatch.Draw(currentSprite, position, Color.White);
        }

        public override void OnHoverIn()
        {
            currentSprite = spriteHover;
        }

        public override void OnHoverOut()
        {
            currentSprite = spriteIdle;
        }

        public void OnClick()
        {
            currentSprite = spriteClicked;
            sendMessage(new Message(Message.MessageType.changeState, "pause"));
        }

        public void OnRelease()
        {
            if (isHover) currentSprite = spriteHover;
            else currentSprite = spriteIdle;
        }
    }
}
