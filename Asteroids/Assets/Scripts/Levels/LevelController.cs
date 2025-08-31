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
        private readonly ScreenEdgeSpawner screenEdgeSpawner;

        private float levelStartTime = 0.0f;
        private float timeBetweenAsteroidSpawns = 5.0f;
        private float lastAsteroidSpawnTime = -5.0f;

        private Level level;

        public LevelController(IPlayerFactory playerFactory, IAsteroidFactory asteroidFactory,
            IBulletFactory bulletFactory,
            ScreenEdgeSpawner screenEdgeSpawner)
        {
            this.playerFactory = playerFactory
                ?? throw new ArgumentNullException(nameof(playerFactory), $"{nameof(LevelController)} requires reference to {nameof(IPlayerFactory)}.");

            this.asteroidFactory = asteroidFactory
                ?? throw new ArgumentNullException(nameof(asteroidFactory), $"{nameof(LevelController)} requires reference to {nameof(IAsteroidFactory)}.");

            this.bulletFactory = bulletFactory
                ?? throw new ArgumentNullException(nameof(bulletFactory), $"{nameof(LevelController)} requires reference to {nameof(IBulletFactory)}.");

            this.screenEdgeSpawner = screenEdgeSpawner
                ?? throw new ArgumentNullException(nameof(screenEdgeSpawner), $"{nameof(LevelController)} requires reference to {nameof(ScreenEdgeSpawner)}.");
        }

        /// <inheritdoc cref="ILevelController.CreateLevel" />
        public void CreateLevel()
        {
            var player = playerFactory.CreatePlayer();
            level = new Level(player);
        }

        public void UpdateLevel(float time)
        {
            if (levelStartTime == 0.0f)
            {
                levelStartTime = time;
            }

            if ((time - lastAsteroidSpawnTime) > timeBetweenAsteroidSpawns)
            {
                CreateAsteroid(AsteroidSize.Large);
                lastAsteroidSpawnTime = time;
            }
        }

        public void HandleAsteroidHit(Bullet bullet, Asteroid asteroid)
        {
            HandleBulletExpired(bullet);

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

        public void HandleBulletFired(Vector2 position, Vector2 direction)
        {
            var bullet = bulletFactory.CreateBullet(position, direction);

            level.AddBullet(bullet);
        }

        public void HandleBulletExpired(Bullet bullet)
        {
            level.RemoveBullet(bullet);

            bulletFactory.DestroyBullet(bullet);
        }

        private Asteroid CreateAsteroid(AsteroidSize asteroidSize)
        {
            var (position, direction) = screenEdgeSpawner.GetScreenEdgeSpawnPointAndDirection(0.5f);
            return CreateAsteroid(asteroidSize, position, direction);
        }

        private Asteroid CreateAsteroid(AsteroidSize asteroidSize, Vector2 position, Vector2 direction)
        {
            var asteroid = asteroidFactory.CreateAsteroid(asteroidSize, position, direction);
            level.AddAsteroid(asteroid);
            return asteroid;
        }
    }
}
