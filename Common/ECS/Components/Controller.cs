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

        public void Init(Bindings _bindings)
        {
            Holdings = new Dictionary<string, bool>();
            Pressings = new Dictionary<string, bool>();
            Unpressings = new Dictionary<string, bool>();
            InitializeFlags(Holdings, _bindings);
            InitializeFlags(Pressings, _bindings);
            InitializeFlags(Unpressings, _bindings);
        }

        void InitializeFlags(Dictionary<string, bool> _dictionary, Bindings _bindings){
            var pairs = _bindings.Pairs;

            foreach (var pair in pairs)
            {
                _dictionary.Add(pair.Key, false);
            }
        }

        public void SetHolding(string _key, bool _value){
            Holdings[_key] = _value;
        }

        public void SetPressing(string _key, bool _value){
            Pressings[_key] = _value;
        }

        public void SetUnpressing(string _key, bool _value){
            Unpressings[_key] = _value;
        }

        public bool IsHolding(string _key){
            return Holdings[_key];
        }

        public bool WasPressed(string _key){
            return Pressings[_key];
        }

        public bool WasUnpressed(string _key){
            return Unpressings[_key];
        }

        public Vector2 GetInputVector(string _left, string _right, string _up, string _down)
        {
            var result = Vector2.Zero;

            if(IsHolding(_left))
            {
                result.X -= 1;
            }
            if(IsHolding(_right))
            {
                result.X += 1;
            }
            if(IsHolding(_down))
            {
                result.Y -= 1;
            }
            if(IsHolding(_up))
            {
                result.Y += 1;
            }

            return result;
        }
    }
}