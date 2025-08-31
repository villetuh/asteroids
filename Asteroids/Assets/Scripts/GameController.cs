using Asteroids.Entities;
using Asteroids.Levels;
using Asteroids.Utilities;
using System;
using UnityEngine;
using VContainer.Unity;

namespace Asteroids
{
    /// <summary>
    /// Component handling main game logic.
    /// </summary>
    public class GameController : IGameController, IStartable, ITickable
    {
        private readonly ILevelController levelController;
        private readonly ITimeProvider timeProvider;

        public GameController(ILevelController levelController, ITimeProvider timeProvider)
        {
            this.levelController = levelController
                ?? throw new ArgumentNullException(nameof(levelController), $"{nameof(GameController)} requires reference to {nameof(ILevelController)}.");

            this.timeProvider = timeProvider
                ?? throw new ArgumentNullException(nameof(timeProvider), $"{nameof(GameController)} requires reference to {nameof(ITimeProvider)}.");
        }

        public void Start()
        {
            CreateLevel();
        }

        public void Tick()
        {
            levelController.UpdateLevel(timeProvider.Time);
        }

        private void CreateLevel()
        {
            levelController.CreateLevel();
        }

        /// <inheritdoc cref="IGameController.OnPlayerFire(Vector2, Vector2)" />
        public void OnPlayerFire(Vector2 position, Vector2 direction)
        {
            levelController.HandleBulletFired(position, direction);
        }

        /// <inheritdoc cref="IGameController.OnBulletExpired(Bullet)" />
        public void OnBulletExpired(Bullet bullet)
        {
            levelController.HandleBulletExpired(bullet);
        }

        public void OnAsteroidHit(Bullet bullet, Asteroid asteroid)
        {
            levelController.HandleAsteroidHit(bullet, asteroid);
        }
    }
}
