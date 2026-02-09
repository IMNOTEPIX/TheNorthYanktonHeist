using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheNorthYanktonHeist.Interfaces;

namespace TheNorthYanktonHeist.Scenes
{
    public static class SceneManager
    {
        private static Dictionary<string, IScene> _scenes =
            new Dictionary<string, IScene>()
        {
        { "BombPlant", new BombPlantScene() }
        };

        private static IScene _currentScene;

        public static void StartScene(string name)
        {
            if (_currentScene != null)
                return;

            if (_scenes.TryGetValue(name, out var scene))
            {
                _currentScene = scene;
                scene.Start();
            }
        }

        public static void Update()
        {
            if (_currentScene == null)
                return;

            _currentScene.Update();

            if (_currentScene.IsFinished)
            {
                _currentScene = null;
            }
        }

        public static void StopCurrentScene()
        {
            _currentScene?.Stop();
            _currentScene = null;
        }

        public static bool IsSceneRunning =>
            _currentScene != null;
    }

}
