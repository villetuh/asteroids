using Asteroids.Entities;
using Asteroids.Factories;
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
        private readonly IBulletFactory bulletFactory;
        private readonly ITimeProvider timeProvider;

        private Level level;

        public GameController(ILevelController levelController, IBulletFactory bulletFactory,
            ITimeProvider timeProvider)
        {
            this.levelController = levelController
                ?? throw new ArgumentNullException(nameof(levelController), $"{nameof(GameController)} requires reference to {nameof(ILevelController)}.");

            this.bulletFactory = bulletFactory
                ?? throw new ArgumentNullException(nameof(bulletFactory), $"{nameof(GameController)} requires reference to {nameof(IBulletFactory)}.");

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
            level = levelController.CreateLevel();
        }

        /// <inheritdoc cref="IGameController.OnPlayerFire(Vector2, Vector2)" />
        public void OnPlayerFire(Vector2 position, Vector2 direction)
        {
            var bullet = bulletFactory.CreateBullet(position, direction);

            level.AddBullet(bullet);
        }

        /// <inheritdoc cref="IGameController.OnBulletExpired(Bullet)" />
        public void OnBulletExpired(Bullet bullet)
        {
            level.RemoveBullet(bullet);

            bulletFactory.DestroyBullet(bullet);
        }
    }
}
