using Asteroids.Entities;
using UnityEngine;

namespace Asteroids.Factories
{
    /// <summary>
    /// Interface for components creating asteroids.
    /// </summary>
    public interface IAsteroidFactory
    {
        /// <summary>
        /// Creates an asteroid.
        /// </summary>
        /// <param name="size"></param>
        /// <param name="position"></param>
        /// <param name="speed"></param>
        /// <returns></returns>
        public Asteroid CreateAsteroid(AsteroidSize size, Vector2 position, Vector2 speed);

        /// <summary>
        /// Destroys an asteroid created by the factory.
        /// </summary>
        /// <param name="asteroid"></param>
        public void DestroyAsteroid(Asteroid asteroid);
    }
}