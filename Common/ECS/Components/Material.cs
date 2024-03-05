using Common.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Common.ECS.Components
{
    public struct Material
    {
        public Texture2D Texture;
        public Effect Shader;
        public Color Diffuse;
        public float Ambient;
        public float Specular;

        private readonly Color DIFFUSE = Color.White;
        private const float AMBIENT = .05f;
        private const float SPECULAR = .4f;

        public Material(Texture2D texture = null, float ambient = AMBIENT, float specular = SPECULAR)
        {
            Texture = texture;
            Shader = GameSettings.Instance.ForwardRenderingShader;
            Diffuse = DIFFUSE;
            Ambient = ambient;
            Specular = specular;
        }

        public Material(Effect shader, Texture2D texture = null, float ambient = AMBIENT, float specular = SPECULAR)
        {
            Texture = texture;
            Shader = shader;
            Diffuse = DIFFUSE;
            Ambient = ambient;
            Specular = specular;
        }

        public Material(Color diffuse, Texture2D texture = null, float ambient = AMBIENT, float specular = SPECULAR)
        {
            Texture = texture;
            Shader = GameSettings.Instance.ForwardRenderingShader;
            Diffuse = diffuse;
            Ambient = ambient;
            Specular = specular;
        }

        public Material(Effect shader, Color diffuse, Texture2D texture = null, float ambient = AMBIENT, float specular = SPECULAR)
        {
            Texture = texture;
            Shader = shader;
            Diffuse = diffuse;
            Ambient = ambient;
            Specular = specular;
        }
    }
}