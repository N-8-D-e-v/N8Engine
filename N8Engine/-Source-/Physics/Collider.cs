﻿using N8Engine.Mathematics;
using N8Engine.SceneManagement;

namespace N8Engine.Physics
{
    public sealed class Collider
    {
        public readonly GameObject GameObject;
        internal readonly DebugRectangle DebugRectangle;
        private readonly Transform _transform;
        private Vector _size;

        public Vector Velocity { get; set; }
        public bool IsDebugModeEnabled { get; set; }
        public Vector Offset { get; set; }
        public Vector Size
        {
            get => _size;
            set
            {
                if (_size == value) return;
                DebugRectangle.Size = value;
                _size = value;
            }
        }
        public Vector Position => _transform.Position + Offset;
        private BoundingBox BoundingBoxCurrentFrame { get; set; }
        private BoundingBox BoundingBoxNextFrame { get; set; }

        internal Collider(GameObject gameObject)
        {
            GameObject = gameObject;
            _transform = gameObject.Transform;
            DebugRectangle = new DebugRectangle(Size, Position);
            GameLoop.OnPostUpdate += PostUpdate;
            GameLoop.OnPhysicsUpdate += OnPhysicsUpdate;
        }

        internal void Destroy()
        {
            GameLoop.OnPostUpdate -= PostUpdate;
            GameLoop.OnPhysicsUpdate -= OnPhysicsUpdate;
        }

        private void PostUpdate(float deltaTime)
        {
            BoundingBoxCurrentFrame = new BoundingBox(Size, Position);
            BoundingBoxNextFrame = new BoundingBox(Size, Position + Velocity * deltaTime);
        }

        private void OnPhysicsUpdate(float deltaTime)
        {
            foreach (var otherGameObject in SceneManager.CurrentScene.GameObjects)
            {
                var otherCollider = otherGameObject.Collider;
                if (otherCollider == this) continue;
                if (BoundingBoxNextFrame.IsOverlapping(otherCollider.BoundingBoxNextFrame))
                {
                    var directionOfCollision = BoundingBoxCurrentFrame.DirectionRelativeTo(otherCollider.BoundingBoxCurrentFrame);
                    Velocity = new Vector
                    (
                        directionOfCollision is Direction.Left or Direction.Right ? 0f : Velocity.X,
                        directionOfCollision is Direction.Top or Direction.Down ? 0f : Velocity.Y
                    );
                    if (directionOfCollision != Direction.None) GameObject.CollidedWith(otherCollider);
                    break;
                }
            }
            _transform.Position += Velocity * deltaTime;
            DebugRectangle.Position = Position;
        }
    }
}