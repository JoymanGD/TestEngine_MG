using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Common.Settings;
using MonoGame.Extended.Input;

namespace Common.ECS.Components
{
    public struct Controller
    {
        public Dictionary<string, bool> Holdings { get; private set; }
        public Dictionary<string, bool> Pressings { get; private set; }
        public Dictionary<string, bool> Unpressings { get; private set; }

        public void Init(Bindings bindings)
        {
            Holdings = new Dictionary<string, bool>();
            Pressings = new Dictionary<string, bool>();
            Unpressings = new Dictionary<string, bool>();
            InitializeFlags(Holdings, bindings);
            InitializeFlags(Pressings, bindings);
            InitializeFlags(Unpressings, bindings);
        }

        void InitializeFlags(Dictionary<string, bool> dictionary, Bindings bindings)
        {
            var pairs = bindings.Pairs;

            foreach (var pair in pairs)
            {
                dictionary.Add(pair.Key, false);
            }
        }

        public void SetHolding(string key, bool value)
        {
            Holdings[key] = value;
        }

        public void SetPressing(string key, bool value)
        {
            Pressings[key] = value;
        }

        public void SetUnpressing(string key, bool value)
        {
            Unpressings[key] = value;
        }

        public bool IsHolding(string key)
        {
            return Holdings[key];
        }

        public bool WasPressed(string key)
        {
            return Pressings[key];
        }

        public bool WasUnpressed(string key)
        {
            return Unpressings[key];
        }

        public Vector2 GetInputVector(string left, string right, string up, string down)
        {
            var result = Vector2.Zero;

            if(IsHolding(left))
            {
                result.X -= 1;
            }
            if(IsHolding(right))
            {
                result.X += 1;
            }
            if(IsHolding(down))
            {
                result.Y -= 1;
            }
            if(IsHolding(up))
            {
                result.Y += 1;
            }

            return result;
        }
    }
}