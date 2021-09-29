using System.Collections.Generic;
using System;

namespace Common.ECS.Components
{
    public struct Controller
    {
        public Dictionary<string, bool> Holdings { get; private set; }
        public Dictionary<string, bool> Pressings { get; private set; }
        public Dictionary<string, bool> Unpressings { get; private set; }

        public Controller(Bindings _bindings){
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
    }
}