using System;
using Common.Settings;
using Microsoft.Xna.Framework;

namespace Common.ECS.Components
{
    public class Transform
    {
        public Vector3 Position {
                                    get => position;
                                    set => SetPosition(value);
                                }

        public Quaternion Rotation {
                                    get => rotation;
                                    set => SetRotation(value);
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
                                    get => WorldMatrix.Forward;
                                }
        public Vector3 DeltaPosition { get; private set; }
        public Matrix WorldMatrix { get; private set; }
        public float RotationSpeed;
        private Vector3 position, scale;
        private Quaternion rotation;

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
#region Constructors

        public Transform(Vector3 _position){
            WorldMatrix = Matrix.Identity;
            position = _position;
            rotation = Quaternion.Identity;
            scale = Vector3.One;
            RotationSpeed = 1;
            UpdateWorldMatrix();
        }

        public Transform(Vector3 _position, float rotationSpeed){
            WorldMatrix = Matrix.Identity;
            position = _position;
            rotation = Quaternion.Identity;
            scale = Vector3.One;
            RotationSpeed = rotationSpeed;
            UpdateWorldMatrix();
        }

        public Transform(Vector3 _position, Vector3 _forward){
            WorldMatrix = Matrix.Identity;
            position = _position;
            rotation = Quaternion.Identity;
            scale = Vector3.One;
            RotationSpeed = 1;
            LookAt(_forward + _position);
        }

        public Transform(Vector3 _position, Vector3 _forward, Vector3 _scale){
            WorldMatrix = Matrix.Identity;
            position = _position;
            rotation = Quaternion.Identity;
            scale = _scale;
            RotationSpeed = 1;
            LookAt(_forward + _position);
        }

        public Transform(Vector3 _position, Vector3 _forward, float _scale){
            WorldMatrix = Matrix.Identity;
            position = _position;
            rotation = Quaternion.Identity;
            scale = new Vector3(_scale, _scale, _scale);
            RotationSpeed = 1;
            LookAt(_forward + _position);
        }

#endregion
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        void UpdateWorldMatrix(){ //ISRT            
            WorldMatrix = Matrix.Identity;
            WorldMatrix *= Matrix.CreateScale(Scale);
            WorldMatrix *= Matrix.CreateFromQuaternion(Rotation);
            WorldMatrix *= Matrix.CreateTranslation(Position);
        }

        public void Translate(Vector3 _translation, bool _updateMatrix = true){
            var oldPosition = position;
            position += _translation;
            DeltaPosition = position - oldPosition;
            
            if(_updateMatrix)
                UpdateWorldMatrix();
        }

        public void Translate(float _x, float _y, float _z){
            Translate(new Vector3(_x, _y, _z));
        }

        public void SetPosition(Vector3 _newPosition, bool _updateMatrix = true){
            var oldPosition = position;
            position = _newPosition;
            DeltaPosition = position - oldPosition;
            
            if(_updateMatrix)
                UpdateWorldMatrix();
        }

        public void Rotate(Quaternion _rotation, bool _updateMatrix = true){
            rotation *= _rotation;
            
            if(_updateMatrix)
                UpdateWorldMatrix();
        }

        public void SetRotation(Quaternion rotation, bool updateMatrix = true)
        {
            this.rotation = rotation;

            if(updateMatrix)
            {
                UpdateWorldMatrix();
            }
        }

        public void Rotate(Vector3 _rotation){
            Rotate(Quaternion.CreateFromYawPitchRoll(_rotation.X, _rotation.Y, _rotation.Z));
        }

        public void RotateSmooth(Vector3 _rotation, float _smoothness){
            var newRotation = Quaternion.CreateFromYawPitchRoll(_rotation.X, _rotation.Y, _rotation.Z);
            var newSmoothedRotation = Quaternion.Lerp(rotation, newRotation, _smoothness);
            Rotate(newSmoothedRotation);
        }

        public void Rotate(Vector3 _axis, float _angle, bool _updateMatrix = true){
            Rotate(Quaternion.CreateFromAxisAngle(_axis, _angle), _updateMatrix);
        }

        public void RotateAroundPoint(Vector3 _point, Vector3 _axis, float _angle){
            var direction = _point - Position;
            var distance = direction.Length();
            
            SetPosition(Vector3.Zero);
            Rotate(_axis, _angle);
            SetPosition(_point);
            Translate(WorldMatrix.Backward * distance);
        }

        public void RotateAroundPoint(Vector3 _point, Quaternion _rotation){
            var direction = _point - Position;
            var directionNormalized = Vector3.Normalize(direction);
            var distance = direction.Length();
            
            Translate(directionNormalized * distance);
            Rotate(_rotation);
            Translate(WorldMatrix.Backward * distance);
        }

        public void SetScale(Vector3 _newScale, bool _updateMatrix = true){
            scale = _newScale;
            
            if(_updateMatrix)
                UpdateWorldMatrix();
        }

        public void SetScale(float _x, float _y, float _z){
            SetScale(new Vector3(_x, _y, _z));
        }

        public void LookAt(Vector3 _targetPosition, bool _updateMatrix = true){
            var direction = Vector3.Normalize(_targetPosition - Position);
            var up = Vector3.Up;

            var dot = Vector3.Dot(direction, up);
            if(dot > -1 && dot < 1){
                var rotMatrix = Matrix.CreateWorld(Position, direction, up);
                Quaternion rot;
                rotMatrix.Decompose(out _, out rot, out _);
                rotation = rot;
                
                if(_updateMatrix)
                    UpdateWorldMatrix();
            }
        }

        public void LookAtSmooth(Vector3 _targetPosition, float rotationSpeed, bool _updateMatrix = true){
            var direction = Vector3.Normalize(_targetPosition - Position);
            var up = Vector3.Up;

            var dot = Vector3.Dot(direction, up);
            if(dot > -1 && dot < 1){
                var rotMatrix = Matrix.CreateWorld(Position, direction, up);
                Quaternion rot;
                rotMatrix.Decompose(out _, out rot, out _);
                rotation = Quaternion.Lerp(rotation, rot, rotationSpeed);
                
                if(_updateMatrix)
                    UpdateWorldMatrix();
            }
        }
    }
}