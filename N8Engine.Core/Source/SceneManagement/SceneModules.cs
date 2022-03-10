﻿using System.Linq;
using N8Engine.Utilities;

namespace N8Engine.SceneManagement;

public sealed class SceneModules : ServiceLocator<SceneModule, SceneModuleNotFoundException>
{
    string _sceneName;
    
    internal bool IsInitialized { get; private set; }
    SceneModule[] Modules => Services.Values.ToArray();
    
    internal SceneModules() { }
    
    protected override SceneModuleNotFoundException ServiceNotFoundException<T>() =>  new($"Module of type {typeof(T)} not found in Scene {_sceneName}!");
    
    internal void Initialize(string sceneName)
    {
        _sceneName = sceneName;
        IsInitialized = true;
    }

    internal void OnSceneLoad(Scene scene)
    {
        foreach (var module in Modules) 
            module.OnSceneLoad(scene);
    }
}