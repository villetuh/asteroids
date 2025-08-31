using Asteroids.Entities;
using Asteroids.Factories;
using Asteroids.Utilities;
using System;
using UnityEngine;

namespace Asteroids.Levels
{
    /// <summary>
    /// Component handling level creation.
    /// </summary>
    public class LevelInitializer : ILevelInitializer
    {
        private readonly IPlayerFactory playerFactory;
        private readonly IAsteroidFactory asteroidFactory;
        private readonly ScreenEdgeSpawner screenEdgeSpawner;

        private float levelStartTime = 0.0f;
        private float timeBetweenAsteroidSpawns = 5.0f;
        private float lastAsteroidSpawnTime = -5.0f;

        public LevelInitializer(IPlayerFactory playerFactory, IAsteroidFactory asteroidFactory, ScreenEdgeSpawner screenEdgeSpawner)
        {
            this.playerFactory = playerFactory
                ?? throw new ArgumentNullException(nameof(playerFactory), $"{nameof(LevelInitializer)} requires reference to {nameof(IPlayerFactory)}.");

            this.asteroidFactory = asteroidFactory
                ?? throw new ArgumentNullException(nameof(asteroidFactory), $"{nameof(LevelInitializer)} requires reference to {nameof(IAsteroidFactory)}.");

            this.screenEdgeSpawner = screenEdgeSpawner
                ?? throw new ArgumentNullException(nameof(screenEdgeSpawner), $"{nameof(LevelInitializer)} requires reference to {nameof(ScreenEdgeSpawner)}.");
        }

        /// <inheritdoc cref="ILevelInitializer.CreateLevel" />
        public Level CreateLevel()
        {
            var player = playerFactory.CreatePlayer();
            return new Level(player);
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

        private Asteroid CreateAsteroid(AsteroidSize asteroidSize)
        {
            var (position, direction) = screenEdgeSpawner.GetScreenEdgeSpawnPointAndDirection(0.5f);
            return CreateAsteroid(asteroidSize, position, direction);
        }

        private Asteroid CreateAsteroid(AsteroidSize asteroidSize, Vector2 position, Vector2 direction)
        {
            return asteroidFactory.CreateAsteroid(asteroidSize, position, direction);
        }
    }
}
