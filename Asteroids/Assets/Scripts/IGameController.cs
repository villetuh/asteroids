using Asteroids.Entities;
using System;
using UnityEngine;

namespace Asteroids
{
    /// <summary>
    /// Interface for game controller handling main game logic.
    /// </summary>
    public interface IGameController
    {
        /// <summary>
        /// Event invoked when game state changes.
        /// </summary>
        public Action<GameState> OnGameStateChanged { get; set; }

        /// <summary>
        /// Start a new game
        /// </summary>
        public void StartGame();

        /// <summary>
        /// End current game.
        /// </summary>
        public void EndGame();

        /// <summary>
        /// Event invoked when player performs a fire action.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="direction"></param>
        public void OnPlayerFire(Vector2 position, Vector2 direction);

        /// <summary>
        /// Event invoked when bullet reaches its lifetime without hitting anything.
        /// </summary>
        /// <param name="bullet"></param>
        public void OnBulletExpired(IBullet bullet);

        /// <summary>
        /// Event invoked when bullet hits an asteroid.
        /// </summary>
        /// <param name="bullet"></param>
        /// <param name="asteroid"></param>
        public void OnAsteroidHit(IBullet bullet, IAsteroid asteroid);

        /// <summary>
        /// Event invoked when asteroid hits the player.
        /// </summary>
        public void OnPlayerHit(IAsteroid asteroid);
    }
}
