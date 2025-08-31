using Asteroids.Levels;
using System;
using VContainer.Unity;

namespace Asteroids
{
    /// <summary>
    /// Component handling main game logic.
    /// </summary>
    public class GameController : IGameController, IStartable
    {
        private readonly ILevelInitializer levelInitializer;

        private Level level;

        public GameController(ILevelInitializer levelInitializer)
        {
            this.levelInitializer = levelInitializer
                ?? throw new ArgumentNullException(nameof(levelInitializer), $"{nameof(GameController)} requires reference to {nameof(ILevelInitializer)}.");
        }

        public void Start()
        {
            CreateLevel();
        }

        private void CreateLevel()
        {
            level = levelInitializer.CreateLevel();
        }
    }
}
