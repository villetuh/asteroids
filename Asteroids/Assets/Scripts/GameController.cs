using Asteroids.Factories;
using Asteroids.Levels;
using System;
using UnityEngine;
using VContainer.Unity;

namespace Asteroids
{
    /// <summary>
    /// Component handling main game logic.
    /// </summary>
    public class GameController : IGameController, IStartable
    {
        private readonly ILevelInitializer levelInitializer;
        private readonly IBulletFactory bulletFactory;

        private Level level;

        public GameController(ILevelInitializer levelInitializer, IBulletFactory bulletFactory)
        {
            this.levelInitializer = levelInitializer
                ?? throw new ArgumentNullException(nameof(levelInitializer), $"{nameof(GameController)} requires reference to {nameof(ILevelInitializer)}.");

            this.bulletFactory = bulletFactory
                ?? throw new ArgumentNullException(nameof(bulletFactory), $"{nameof(GameController)} requires reference to {nameof(IBulletFactory)}.");
        }

        public void Start()
        {
            CreateLevel();
        }

        private void CreateLevel()
        {
            level = levelInitializer.CreateLevel();
        }

        public void OnPlayerFire(Vector2 position, Vector2 direction)
        {
            var bullet = bulletFactory.CreateBullet(position, direction);

            level.AddBullet(bullet);
        }
    }
}
