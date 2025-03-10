﻿using N8Engine.SceneManagement;
using NUnit.Framework;
using static NUnit.Framework.Assert;

namespace N8Engine.Tests;

sealed class GameObjectTests
{
    sealed class C1 : Component { }
    sealed class C2 : Component { }
    sealed class S : Scene { protected override void Load() { } }

    GameObject _gameObject = null!;

    [SetUp]
    public void Setup() => _gameObject = new(new S(), "foo");
    
    [Test]
    public void TestGetNonExistentComponent() => IsNull(_gameObject.GetComponent<C1>());

    [Test]
    public void TestGetExistentComponent()
    {
        _gameObject.AddComponent<C1>();
        IsNotNull(_gameObject.GetComponent<C1>());
    }

    [Test]
    public void TestAddComponent()
    {
        _gameObject.AddComponent<C1>();
        IsNotNull(_gameObject.GetComponent<C1>());
    }

    [Test]
    public void TestAddSpecificComponent()
    {
        _gameObject.AddComponent(new C1(), out var c1);
        IsNotNull(c1);
    }
    
    [Test]
    public void TestAddComponentForEquality()
    {
        _gameObject.AddComponent(new C1(), out var c1);
        AreEqual(c1, _gameObject.GetComponent<C1>());
    }

    [Test]
    public void TestAddDuplicateComponent()
    {
        _gameObject.AddComponent<C1>();
        Catch<ComponentAlreadyAttachedException>(() => _gameObject.AddComponent<C1>());
    }
    
    [Test]
    public void TestRemoveComponent()
    {
        _gameObject.AddComponent<C1>();
        _gameObject.RemoveComponent<C1>();
        IsNull(_gameObject.GetComponent<C1>());
    }

    [Test]
    public void TestRemoveSpecificComponent()
    {
        _gameObject.AddComponent<C1>(out var c1);
        _gameObject.RemoveComponent(c1);
        IsNull(_gameObject.GetComponent<C1>());
    }

    [Test]
    public void TestRemoveNonExistentComponent() => Catch<ComponentNotAttachedException>(() => _gameObject.RemoveComponent(new C2()));

    [Test]
    public void TestGameObjectIsNotDestroyedInitially() => IsFalse(_gameObject.IsDestroyed);

    [Test]
    public void TestDestroyGameObject()
    {
        _gameObject.Destroy();
        IsTrue(_gameObject.IsDestroyed);
    }

    [Test]
    public void TestGetComponentOnDestroyedGameObject()
    {
        _gameObject.AddComponent<C1>().Destroy();
        Catch<GameObjectIsDestroyedException>(() => _gameObject.GetComponent<C1>());
    }
    
    [Test]
    public void TestAddComponentOnDestroyedGameObject()
    {
        _gameObject.Destroy();
        Catch<GameObjectIsDestroyedException>(() => _gameObject.AddComponent<C1>());
    }
    
    [Test]
    public void TestRemoveComponentOnDestroyedGameObject()
    {
        _gameObject.AddComponent<C1>(out var c1).Destroy();
        Catch<GameObjectIsDestroyedException>(() => _gameObject.RemoveComponent(c1));
    }
}