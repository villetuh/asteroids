using Asteroids.Entities;
using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Asteroids.Factories
{
    /// <summary>
    /// Factory creating asteroid instances.
    /// </summary>
    public class AsteroidFactory : IAsteroidFactory
    {
        private readonly IObjectResolver container;
        private readonly Asteroid asteroidLargePrefab;
        private readonly Asteroid asteroidMediumPrefab;
        private readonly Asteroid asteroidSmallPrefab;

        public AsteroidFactory(IObjectResolver container,
            [Key("AsteroidLargePrefab")] Asteroid asteroidLargePrefab,
            [Key("AsteroidMediumPrefab")] Asteroid asteroidMediumPrefab,
            [Key("AsteroidSmallPrefab")] Asteroid asteroidSmallPrefab)
        {
            this.container = container
                ?? throw new ArgumentNullException(nameof(container), $"{nameof(AsteroidFactory)} requires reference to {nameof(IObjectResolver)}.");

            this.asteroidLargePrefab = asteroidLargePrefab;
            if (asteroidLargePrefab == null)
            {
                throw new ArgumentNullException(nameof(asteroidLargePrefab), $"{nameof(AsteroidFactory)} requires reference to {nameof(Asteroid)} large prefab.");
            }

            this.asteroidMediumPrefab = asteroidMediumPrefab;
            if (asteroidMediumPrefab == null)
            {
                throw new ArgumentNullException(nameof(asteroidMediumPrefab), $"{nameof(AsteroidFactory)} requires reference to {nameof(Asteroid)} large prefab.");
            }

            this.asteroidSmallPrefab = asteroidSmallPrefab;
            if (asteroidSmallPrefab == null)
            {
                throw new ArgumentNullException(nameof(asteroidSmallPrefab), $"{nameof(AsteroidFactory)} requires reference to {nameof(Asteroid)} large prefab.");
            }
        }

        /// <inheritdoc cref="IAsteroidFactory.CreateAsteroid" />
        public Asteroid CreateAsteroid(AsteroidSize size, Vector2 position, Vector2 speed)
        {
            Asteroid asteroid;
            switch (size)
            {
                case AsteroidSize.Large:
                    asteroid = container.Instantiate(asteroidLargePrefab);
                    break;
                case AsteroidSize.Medium:
                    asteroid = container.Instantiate(asteroidMediumPrefab);
                    break;
                case AsteroidSize.Small:
                    asteroid = container.Instantiate(asteroidSmallPrefab);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(size), size, $"{nameof(AsteroidFactory)} cannot create asteroid of size {size}.");
            }

            asteroid.SetSizePositionAndDirection(size, position, speed);

            return asteroid;
        }

        /// <inheritdoc cref="IAsteroidFactory.DestroyAsteroid" />
        public void DestroyAsteroid(Asteroid asteroid)
        {
            if (asteroid != null)
            {
                GameObject.Destroy(asteroid.gameObject);
            }
        }
    }
}
