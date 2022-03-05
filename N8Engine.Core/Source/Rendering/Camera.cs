﻿using System.Numerics;
using N8Engine.Windowing;

namespace N8Engine.Rendering;

public sealed class Camera
{
    readonly WindowSize _windowSize;

    public Vector2 Position { get; set; }
    public float Zoom { get; set; }
    
    internal Camera(WindowSize windowSize) => _windowSize = windowSize;

    internal Matrix4x4 ProjectionMatrix()
    {
        var left = Position.X - _windowSize.Width / 2f;
        var right = Position.X + _windowSize.Width / 2f;
        var top = Position.Y - _windowSize.Height / 2f;
        var bottom = Position.Y + _windowSize.Height / 2f;

        var orthographicMatrix = Matrix4x4.CreateOrthographicOffCenter(left, right, bottom, top, 0.01f, 100f);
        var zoomMatrix = Matrix4x4.CreateScale(Zoom);
        return orthographicMatrix * zoomMatrix;
    }
}