using System;
using Common.Settings;
using Microsoft.Xna.Framework;

namespace Common.ECS.Components
{
    public struct Transform
    {
        public Vector3 Position {
                                    get => position;
                                    set => Translate(value); 
                                }

        public Quaternion Rotation {
                                    get => rotation;
                                    set => Rotate(value);
                                }
                                
        public Vector3 Scale    {
                                    get => scale;
                                    set => SetScale(value);
                                }
        public float OneScale   {
                                    get => scale.X;
                                    set => SetScale(new Vector3(value, value, value));
                                }

        public Vector3 Forward  {
                                    get => forward;
                                }
        public Matrix WorldMatrix { get; private set; }
        private Vector3 position, scale, forward;
        private Quaternion rotation;

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
#region Constructors

        public Transform(Vector3 _position){
            WorldMatrix = Matrix.Identity;
            position = _position;
            rotation = Quaternion.Identity;
            scale = Vector3.One;
            forward = Vector3.Backward;
            UpdateWorldMatrix();
        }

        public Transform(Vector3 _position, Quaternion _rotation){
            WorldMatrix = Matrix.Identity;
            position = _position;
            rotation = _rotation;
            scale = Vector3.One;
            forward = Vector3.Backward;
            UpdateWorldMatrix();
        }

        public Transform(Vector3 _position, Vector3 _forward, Vector3 _up){
            WorldMatrix = Matrix.Identity;
            position = _position;
            rotation = Quaternion.Identity;
            scale = Vector3.One;
            forward = Vector3.Backward;
            SetRotationByDirection(_forward, _up);
            UpdateWorldMatrix();
        }

        public Transform(Vector3 _position, Quaternion _rotation, Vector3 _scale){
            WorldMatrix = Matrix.Identity;
            position = _position;
            rotation = _rotation;
            scale = _scale;
            forward = Vector3.Backward;
            UpdateWorldMatrix();
        }

        public Transform(Vector3 _position, Vector3 _forward, Vector3 _up, Vector3 _scale){
            WorldMatrix = Matrix.Identity;
            position = _position;
            rotation = Quaternion.Identity;
            scale = _scale;
            forward = Vector3.Backward;
            SetRotationByDirection(_forward, _up);
            UpdateWorldMatrix();
        }

        public Transform(Vector3 _position, Quaternion _rotation, float _scale){
            WorldMatrix = Matrix.Identity;
            position = _position;
            rotation = _rotation;
            scale = new Vector3(_scale, _scale, _scale);
            forward = Vector3.Backward;
            UpdateWorldMatrix();
        }

        public Transform(Vector3 _position, Vector3 _forward, Vector3 _up, float _scale){
            WorldMatrix = Matrix.Identity;
            position = _position;
            rotation = Quaternion.Identity;
            scale = new Vector3(_scale, _scale, _scale);
            forward = Vector3.Backward;
            SetRotationByDirection(_forward, _up);
            UpdateWorldMatrix();
        }

#endregion
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        void UpdateWorldMatrix(){ //ISRT
            WorldMatrix = Matrix.Identity;
            WorldMatrix *= Matrix.CreateScale(Scale);
            WorldMatrix *= Matrix.CreateFromQuaternion(Rotation);
            WorldMatrix *= Matrix.CreateTranslation(Position);
        }

        public void Translate(Vector3 _translation){
            position += _translation;
            UpdateWorldMatrix();
        }

        public void Translate(float _x, float _y, float _z){
            Translate(new Vector3(_x, _y, _z));
        }

        public void Rotate(Quaternion _rotation){
            rotation *= _rotation;
            UpdateWorldMatrix();
        }

        public void Rotate(Vector3 _rotation){
            Rotate(Quaternion.CreateFromYawPitchRoll(_rotation.X, _rotation.Y, _rotation.Z));
        }

        public void SetScale(Vector3 _newScale){
            scale = _newScale;
            UpdateWorldMatrix();
        }

        public void SetScale(float _x, float _y, float _z){
            SetScale(new Vector3(_x, _y, _z));
        }

        public void LookAt(Vector3 _direction, Vector3 _up){
            SetRotationByDirection(_direction, _up);
            UpdateWorldMatrix();
        }

        void SetRotationByDirection(Vector3 _direction, Vector3 _up){
            if(_direction.Length() > 0)
                _direction.Normalize();
            
            var matrix = Matrix.CreateWorld(Position, _direction, _up);
            matrix.Decompose(out _, out rotation, out _);
            forward = _direction;
        }
    }
}