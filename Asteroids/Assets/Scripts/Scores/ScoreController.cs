using Asteroids.Configurations;
using Asteroids.Entities;
using System;

namespace Asteroids.Scores
{
    /// <summary>
    /// Component handling scoring related logic.
    /// </summary>
    public class ScoreController : IScoreController
    {
        private readonly IGameSettings gameSettings;

        /// <inheritdoc cref="IScoreController.OnScoreChanged" />
        public Action<int> OnScoreChanged { get; set; }

        /// <inheritdoc cref="IScoreController.Score" />
        public int Score { get; private set; }

        public ScoreController(IGameSettings gameSettings)
        {
            this.gameSettings = gameSettings
                ?? throw new ArgumentNullException(nameof(gameSettings), $"{nameof(ScoreController)} requires reference to {nameof(IGameSettings)}.");
        }

        /// <inheritdoc cref="IScoreController.ScoreDestroyedAsteroid(AsteroidSize)" />
        public void ScoreDestroyedAsteroid(AsteroidSize asteroidSize)
        {
            Score += asteroidSize switch
            {
                AsteroidSize.Large => gameSettings.ScoreSettings.LargeAsteroidScore,
                AsteroidSize.Medium => gameSettings.ScoreSettings.MediumAsteroidScore,
                AsteroidSize.Small => gameSettings.ScoreSettings.SmallAsteroidScore,
                _ => throw new ArgumentOutOfRangeException(),
            };

            OnScoreChanged?.Invoke(Score);
        }

        /// <inheritdoc cref="IScoreController.ResetScore" />
        public void ResetScore()
        {
            Score = 0;
            OnScoreChanged?.Invoke(Score);
        }
    }
}
