using Asteroids.Configurations;
using Asteroids.Entities;
using Asteroids.Factories;
using Asteroids.Utilities;
using System;
using UnityEngine;

namespace Asteroids.Levels
{
    /// <summary>
    /// Component handling logic related to the current level.
    /// </summary>
    public class LevelController : ILevelController
    {
        private readonly IPlayerFactory playerFactory;
        private readonly IAsteroidFactory asteroidFactory;
        private readonly IBulletFactory bulletFactory;
        private readonly IGameSettings gameSettings;
        private readonly ScreenEdgeSpawner screenEdgeSpawner;

        private float levelStartTime = 0.0f;
        private float lastAsteroidSpawnTime = -5.0f;

        private Level level;

        public LevelController(IPlayerFactory playerFactory, IAsteroidFactory asteroidFactory,
            IBulletFactory bulletFactory, IGameSettings gameSettings,
            ScreenEdgeSpawner screenEdgeSpawner)
        {
            this.playerFactory = playerFactory
                ?? throw new ArgumentNullException(nameof(playerFactory), $"{nameof(LevelController)} requires reference to {nameof(IPlayerFactory)}.");

            this.asteroidFactory = asteroidFactory
                ?? throw new ArgumentNullException(nameof(asteroidFactory), $"{nameof(LevelController)} requires reference to {nameof(IAsteroidFactory)}.");

            this.bulletFactory = bulletFactory
                ?? throw new ArgumentNullException(nameof(bulletFactory), $"{nameof(LevelController)} requires reference to {nameof(IBulletFactory)}.");

            this.gameSettings = gameSettings;
            if (gameSettings == null)
            {
                throw new ArgumentNullException(nameof(gameSettings), $"{nameof(LevelController)} requires reference to {nameof(IGameSettings)}.");
            }

            this.screenEdgeSpawner = screenEdgeSpawner
                ?? throw new ArgumentNullException(nameof(screenEdgeSpawner), $"{nameof(LevelController)} requires reference to {nameof(ScreenEdgeSpawner)}.");
        }

        /// <inheritdoc cref="ILevelController.CreateLevel" />
        public void CreateLevel()
        {
            var player = playerFactory.CreatePlayer();
            level = new Level()
            {
                Player = player
            };
        }

        /// <inheritdoc cref="ILevelController.ClearLevel" />
        public void ClearLevel()
        {
            if (level == null)
            {
                return;
            }

            foreach (var asteroid in level.Asteroids)
            {
                asteroidFactory.DestroyAsteroid(asteroid);
            }
            level.Asteroids.Clear();

            foreach (var bullet in level.Bullets)
            {
                bulletFactory.DestroyBullet(bullet);
            }
            level.Bullets.Clear();

            if (level.Player != null)
            {
                playerFactory.DestroyPlayer(level.Player);
                level.Player = null;
            }
            level = null;
        }

        /// <inheritdoc cref="ILevelController.UpdateLevel(float)" />
        public void UpdateLevel(float time)
        {
            if (levelStartTime == 0.0f)
            {
                levelStartTime = time;
            }

            if ((time - lastAsteroidSpawnTime) > gameSettings.LevelSettings.TimeBetweenAsteroidSpawns)
            {
                CreateAsteroid(AsteroidSize.Large);
                lastAsteroidSpawnTime = time;
            }
        }

        /// <inheritdoc cref="ILevelController.HandleAsteroidHit(Bullet, Asteroid)" />
        public void HandleAsteroidHit(Bullet bullet, Asteroid asteroid)
        {
            HandleBulletExpired(bullet);

            HandleAsteroidHit(asteroid);
        }

        /// <inheritdoc cref="ILevelController.HandleBulletFired(Vector2, Vector2)" />
        public void HandleBulletFired(Vector2 position, Vector2 direction)
        {
            var bullet = bulletFactory.CreateBullet(position, direction);

            level.AddBullet(bullet);
        }

        /// <inheritdoc cref="ILevelController.HandleBulletExpired(Bullet)" />
        public void HandleBulletExpired(Bullet bullet)
        {
            level.RemoveBullet(bullet);

            bulletFactory.DestroyBullet(bullet);
        }

        /// <inheritdoc cref="ILevelController.HandlePlayerHit(Asteroid)" />
        public void HandlePlayerHit(Asteroid asteroid)
        {
            playerFactory.DestroyPlayer(level.Player);
            level.Player = null;

            HandleAsteroidHit(asteroid);
        }

        private Asteroid CreateAsteroid(AsteroidSize asteroidSize)
        {
            var (position, direction) = screenEdgeSpawner.GetScreenEdgeSpawnPointAndDirection(0.0f);
            return CreateAsteroid(asteroidSize, position, direction);
        }

        private Asteroid CreateAsteroid(AsteroidSize asteroidSize, Vector2 position, Vector2 direction)
        {
            var asteroid = asteroidFactory.CreateAsteroid(asteroidSize, position, direction);
            level.AddAsteroid(asteroid);
            return asteroid;
        }

        private void HandleAsteroidHit(Asteroid asteroid)
        {
            var asteroidPosition = asteroid.Position;

            level.RemoveAsteroid(asteroid);
            asteroidFactory.DestroyAsteroid(asteroid);

            switch (asteroid.Size)
            {
                case AsteroidSize.Large:
                    CreateAsteroid(AsteroidSize.Medium, asteroidPosition, UnityEngine.Random.insideUnitCircle.normalized);
                    CreateAsteroid(AsteroidSize.Medium, asteroidPosition, UnityEngine.Random.insideUnitCircle.normalized);
                    break;
                case AsteroidSize.Medium:
                    CreateAsteroid(AsteroidSize.Small, asteroidPosition, UnityEngine.Random.insideUnitCircle.normalized);
                    CreateAsteroid(AsteroidSize.Small, asteroidPosition, UnityEngine.Random.insideUnitCircle.normalized);
                    break;
                case AsteroidSize.Small:
                    // Small asteroids do not spawn new asteroids when hit.
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(asteroid.Size), $"Unhandled asteroid size: {asteroid.Size}");
            }
        }
    }
}
