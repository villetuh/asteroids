using Asteroids.Entities;
using System;

namespace Asteroids.Scores
{
    /// <summary>
    /// Interface for components implementing scoring related logic.
    /// </summary>
    public interface IScoreController
    {
        /// <summary>
        /// Event invoked when score changes.
        /// </summary>
        public Action<int> OnScoreChanged { get; set; }
        
        /// <summary>
        /// Current score.
        /// </summary>
        public int Score { get; }

        /// <summary>
        /// Grant points for destroying an asteroid.
        /// </summary>
        /// <param name="asteroidSize"></param>
        public void ScoreDestroyedAsteroid(AsteroidSize asteroidSize);

        /// <summary>
        /// Reset current score.
        /// </summary>
        public void ResetScore();
    }
}