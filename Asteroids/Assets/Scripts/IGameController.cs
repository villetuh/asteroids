using UnityEngine;

namespace Asteroids
{
    /// <summary>
    /// Interface for game controller handling main game logic.
    /// </summary>
    public interface IGameController
    {
        /// <summary>
        /// Event invoked when player performs a fire action.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="direction"></param>
        public void OnPlayerFire(Vector2 position, Vector2 direction);
    }
}
