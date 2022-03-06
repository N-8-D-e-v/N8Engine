﻿using System;
using N8Engine.InputSystem;
using Silk.NET.Input;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using static Silk.NET.Windowing.Window;
using GLWindow = Silk.NET.Windowing.IWindow;

namespace N8Engine.Windowing;

sealed class Window : WindowSize
{
    public static GL Graphics { get; private set; }

    public event Action OnLoad;
    public event Action OnClose;
    public event Action<Frame> OnUpdate;
    public event Action OnRender;
    public event Action<InputSystem.Key> OnKeyDown;
    public event Action<InputSystem.Key> OnKeyUp;

    readonly GLWindow _window;    
    
    int WindowSize.Width => _window.Size.X;
    int WindowSize.Height => _window.Size.Y;

    public Window(WindowOptions options)
    {
        _window = Create(options);
        _window.Load += () =>
        {
            SetupInput();
            OnLoad?.Invoke();
        };
        _window.Closing += () => OnClose?.Invoke();
        _window.Update += fps => OnUpdate?.Invoke(new((float) fps));
        _window.Render += _ => OnRender?.Invoke();
        Graphics = _window.CreateOpenGL();
    }

    public void Run() => _window.Run();

    public void Close() => _window.Close();

    void SetupInput()
    {
        var input = _window.CreateInput();
        foreach (var keyboard in input.Keyboards)
        {
            keyboard.KeyDown += (_, glKey, _) => OnKeyDown?.Invoke(glKey.AsKey());
            keyboard.KeyUp += (_, glKey, _) => OnKeyUp?.Invoke(glKey.AsKey());
        }
    }
}