using UnityEngine;

namespace Asteroids.Entities
{
    /// <summary>
    /// Interface for game entities.
    /// </summary>
    public interface IGameEntity
    {
        /// <summary>
        /// Type of the entity.
        /// </summary>
        public GameEntityType EntityType { get; }

        /// <summary>
        /// Position of the entity.
        /// </summary>
        public Vector2 Position { get; }

        /// <summary>
        /// Rotation of the entity.
        /// </summary>
        public float Rotation { get; }

        /// <summary>
        /// Current speed and direction of the entity.
        /// </summary>
        public Vector2 Speed { get; }
    }
}
