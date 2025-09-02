using UnityEngine;

namespace Asteroids.Entities
{
    /// <summary>
    /// Interface for components handling asteroid related logic.
    /// </summary>
    public interface IAsteroid : IGameEntity
    {
        /// <summary>
        /// Size of the asteroid.
        /// </summary>
        public AsteroidSize Size { get; set; }

        /// <summary>
        /// Set key properties of the asteroid.
        /// </summary>
        /// <param name="size"></param>
        /// <param name="position"></param>
        /// <param name="direction"></param>
        public void SetSizePositionAndDirection(AsteroidSize size, Vector2 position, Vector2 direction);
    }
}
