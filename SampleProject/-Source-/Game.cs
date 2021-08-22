﻿using System;
using N8Engine;
using N8Engine.SceneManagement;

namespace SampleProject
{
    internal sealed class Game : Launcher
    {
        public override Scene[] Scenes => new Scene[]
        {
            new Level1Scene()
        };
        public override string PathToDebugLogsFile => "../../../.logs";
        public override short FontSize => 6;

        private static void Main() => Application.Start(new Game());

        public override void OnFirstFrame() => Console.Title = "sample n8engine game";

        public override void OnEveryDebugFrame() => Console.Title = Application.FramesPerSecond.ToString();
    }
}