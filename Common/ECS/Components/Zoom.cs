using System;
using System.Numerics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Common.ECS.Components
{
    public struct Zoom
    {
        public float Speed;
        public float Min;
        public float Max;

        public float Value { get; private set; }

        public Zoom(float value, float speed)
        {
            Value = value;
            Speed = speed;
            Min = 0;
            Max = 20;

            ClampValue();
        }

        public Zoom(float value, float min, float max, float speed)
        {
            Value = value;
            Speed = speed;
            Min = min;
            Max = max;

            ClampValue();
        }

        public void SetValue(float value)
        {
            Value = value;

            ClampValue();
        }

        public void AddValue(float value)
        {
            Value += value;

            ClampValue();
        }

        public void ClampValue()
        {
            Value = MathHelper.Clamp(Value, Min, Max);
        }
    }
}