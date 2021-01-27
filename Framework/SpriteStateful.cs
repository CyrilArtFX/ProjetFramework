using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Framework
{
    public class SpriteStateful : Sprite
    {
        private List<Texture2D> textures = new List<Texture2D>();
        private IDictionary<string, int> states = new Dictionary<string, int>();
        public string currentState;

        public SpriteStateful(Vector2 basePosition, Vector2 baseSize, bool isCentered, Rectangle colliderBoxAdjustments, List<Texture2D> textures, IDictionary<string, int> states, string baseState) : base(textures[0], basePosition, baseSize, isCentered, colliderBoxAdjustments)
        {
            this.textures = textures;
            this.states = states;
            ChangeState(baseState);
        }

        public SpriteStateful(Vector2 basePosition, Vector2 baseSize, bool isCentered, List<Texture2D> textures, IDictionary<string, int> states, string baseState) : base(textures[0], basePosition, baseSize, isCentered)
        {
            this.textures = textures;
            this.states = states;
            ChangeState(baseState);
        }

        public void ChangeState(string newState)
        {
            int index = states[newState];
            texture = textures[index];
            currentState = newState;
        }

        public void Notify(Message message)
        {
            if (message.type == Message.MessageType.changeState)
                if(states.ContainsKey(message.content))
                    ChangeState(message.content);
        }
    }
}
