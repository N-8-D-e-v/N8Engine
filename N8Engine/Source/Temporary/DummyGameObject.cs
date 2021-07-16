﻿using System;
using N8Engine.Rendering;
using N8Engine.Inputs;
using N8Engine.Mathematics;
using N8Engine.Physics;

namespace N8Engine
{
    internal sealed class DummyGameObject : GameObject
    {
        protected override void OnStart()
        {
            Sprite = new Sprite(@"C:\Users\NateDawg\RiderProjects\N8Engine\N8Engine\Source\Temporary\sus.n8sprite");
            BoxCollider __boxCollider = Collider.Create<BoxCollider>(this);
            __boxCollider.Size = new Vector(50, 50);
        }

        protected override void OnUpdate(in float deltaTime)
        {
            Console.Title = GameLoop.FramesPerSecond.ToString();
            Position += Input.MovementAxis * 30 * deltaTime;
        }
    }
}