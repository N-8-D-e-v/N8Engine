﻿using N8Engine;
using N8Engine.Mathematics;
using N8Engine.Physics;

namespace SampleProject
{
    public sealed class Player : GameObject
    {
        private const int SPEED = 2600;
        private const int JUMP_FORCE = -200;
        
        private readonly PlayerWalkAnimation _walkAnimation = new();
        private readonly FlippedPlayerWalkAnimation _flippedWalkAnimation = new();
        private readonly PlayerIdleAnimation _idleAnimation = new();
        private readonly FlippedPlayerIdleAnimation _flippedIdleAnimation = new();
        private readonly PlayerJumpAnimation _jumpAnimation = new();
        private readonly FlippedPlayerJumpAnimation _flippedJumpAnimation = new();

        private Direction _currentDirection = Direction.Right;
        private GroundCheck<Ground> _groundCheck;
        private PlayerInputs _inputs;
        
        private bool CanJump => _groundCheck.IsGrounded && _inputs.JustPressedJump;

        protected override void OnStart()
        {
            _groundCheck = GameObject.Create<GroundCheck<Ground>>("player ground check");
            _groundCheck.OnLandedOnTheGround += Land;
            _groundCheck.Collider.Size = new Vector(10f, 2f);
            _groundCheck.Collider.Offset = Vector.Up * 4f;

            _inputs = GameObject.Create<PlayerInputs>("player inputs");
            
            Collider.Size = new Vector(7f, 7f);
            Collider.Offset = Vector.Right;
            Collider.IsDebugModeEnabled = true;
            SpriteRenderer.SortingOrder = 1;
            PhysicsBody.UseGravity = true;
            AnimationPlayer.Animation = _idleAnimation;
            AnimationPlayer.Play();
        }

        protected override void OnDestroy() => _groundCheck.OnLandedOnTheGround -= Land;

        protected override void OnUpdate(float deltaTime)
        {
            UpdateDirection();
            HandleAnimations();
            Move(deltaTime);
            if (CanJump) Jump();
        }

        protected override void OnLateUpdate(float deltaTime) => _groundCheck.Transform.Position = Transform.Position;

        private void UpdateDirection()
        {
            _currentDirection = _inputs.Axis.X switch
            {
                > 0 => Direction.Right,
                < 0 => Direction.Left,
                var _ => _currentDirection
            };
        }

        private void HandleAnimations()
        {
            if (_groundCheck.IsGrounded)
                AnimationPlayer.Animation = _inputs.Axis.X switch
                {
                    > 0 => _walkAnimation,
                    < 0 => _flippedWalkAnimation,
                    0 when _currentDirection == Direction.Right => _idleAnimation,
                    0 when _currentDirection == Direction.Left => _flippedIdleAnimation,
                    var _ => AnimationPlayer.Animation
                };
            else
                AnimationPlayer.Animation = _inputs.Axis.X switch
                {
                    > 0 => _jumpAnimation,
                    < 0 => _flippedJumpAnimation,
                    0 when _currentDirection == Direction.Right => _jumpAnimation,
                    0 when _currentDirection == Direction.Left => _flippedJumpAnimation,
                    var _ => AnimationPlayer.Animation
                };
        }
        
        private void Move(float deltaTime) => PhysicsBody.Velocity = new Vector(_inputs.Axis.X * SPEED * deltaTime, PhysicsBody.Velocity.Y);

        private void Jump() => PhysicsBody.Velocity = new Vector(PhysicsBody.Velocity.X, JUMP_FORCE);
        
        private void Land()
        {
            AnimationPlayer.Animation = _currentDirection switch
            {
                Direction.Left => _flippedIdleAnimation,
                Direction.Right => _idleAnimation,
                var _ => _idleAnimation
            };
        }
    }
}