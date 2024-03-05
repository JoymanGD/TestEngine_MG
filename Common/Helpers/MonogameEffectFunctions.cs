using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Common.Helpers;

public class MonogameEffectFunctions
{
    public static void SetParameterSafe(Effect effect, string paramName, BufferResource value)
    {
        var parameter = effect.Parameters[paramName];

        if (parameter != null)
        {
            parameter.SetValue(value);
        }
    }
    
    public static void SetParameterSafe(Effect effect, string paramName, Matrix value)
    {
        var parameter = effect.Parameters[paramName];

        if (parameter != null)
        {
            parameter.SetValue(value);
        }
    }
    
    public static void SetParameterSafe(Effect effect, string paramName, Vector3 value)
    {
        var parameter = effect.Parameters[paramName];

        if (parameter != null)
        {
            parameter.SetValue(value);
        }
    }
    
    public static void SetParameterSafe(Effect effect, string paramName, Texture2D value)
    {
        var parameter = effect.Parameters[paramName];

        if (parameter != null)
        {
            parameter.SetValue(value);
        }
    }
    
    public static void SetParameterSafe(Effect effect, string paramName, int value)
    {
        var parameter = effect.Parameters[paramName];

        if (parameter != null)
        {
            parameter.SetValue(value);
        }
    }
    
    public static void SetParameterSafe(Effect effect, string paramName, float value)
    {
        var parameter = effect.Parameters[paramName];

        if (parameter != null)
        {
            parameter.SetValue(value);
        }
    }
}
