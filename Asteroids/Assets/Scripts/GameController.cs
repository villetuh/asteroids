using Asteroids.Entities;
using Asteroids.Levels;
using Asteroids.Scores;
using Asteroids.Utilities;
using System;
using UnityEngine;
using VContainer.Unity;

namespace Asteroids
{
    /// <summary>
    /// Current state of the game.
    /// </summary>
    public enum GameState
    {
        Undefined = -1,
        MainMenu = 0,
        Playing = 1,
        GameOver = 2,
    }

    /// <summary>
    /// Component handling main game logic.
    /// </summary>
    public class GameController : IGameController, IStartable, ITickable
    {
        private readonly ILevelController levelController;
        private readonly IScoreController scoreController;
        private readonly ITimeProvider timeProvider;

        private GameState currentState = GameState.Undefined;

        /// <inheritdoc cref="IGameController.OnGameStateChanged" />
        public Action<GameState> OnGameStateChanged { get; set; }

        public GameController(ILevelController levelController, IScoreController scoreController,
                              ITimeProvider timeProvider)
        {
            this.levelController = levelController
                ?? throw new ArgumentNullException(nameof(levelController), $"{nameof(GameController)} requires reference to {nameof(ILevelController)}.");

            this.scoreController = scoreController
                ?? throw new ArgumentNullException(nameof(scoreController), $"{nameof(GameController)} requires reference to {nameof(IScoreController)}.");

            this.timeProvider = timeProvider
                ?? throw new ArgumentNullException(nameof(timeProvider), $"{nameof(GameController)} requires reference to {nameof(ITimeProvider)}.");
        }

        /// <inheritdoc cref="IStartable.Start" />
        public void Start()
        {
            ChangeGameState(GameState.MainMenu);
        }

        /// <inheritdoc cref="ITickable.Tick" />
        public void Tick()
        {
            if (currentState != GameState.Playing)
            {
                return;
            }

            levelController.UpdateLevel(timeProvider.Time);
        }

        /// <inheritdoc cref="IGameController.StartGame" />
        public void StartGame()
        {
            scoreController.ResetScore();
            levelController.CreateLevel();

            ChangeGameState(GameState.Playing);
        }

        /// <inheritdoc cref="IGameController.EndGame" />
        public void EndGame()
        {
            levelController.ClearLevel();

            ChangeGameState(GameState.MainMenu);
        }

        /// <inheritdoc cref="IGameController.OnPlayerFire(Vector2, Vector2)" />
        public void OnPlayerFire(Vector2 position, Vector2 direction)
        {
            levelController.HandleBulletFired(position, direction);
        }

        /// <inheritdoc cref="IGameController.OnBulletExpired(IBullet)" />
        public void OnBulletExpired(IBullet bullet)
        {
            levelController.HandleBulletExpired(bullet);
        }

        /// <inheritdoc cref="IGameController.OnAsteroidHit(IBullet, IAsteroid)" />
        public void OnAsteroidHit(IBullet bullet, IAsteroid asteroid)
        {
            scoreController.ScoreDestroyedAsteroid(asteroid.Size);
            levelController.HandleAsteroidHit(bullet, asteroid);
        }

        /// <inheritdoc cref="IGameController.OnPlayerHit(IAsteroid)" />
        public void OnPlayerHit(IAsteroid asteroid)
        {
            levelController.HandlePlayerHit(asteroid);

            ChangeGameState(GameState.GameOver);
        }

        private void ChangeGameState(GameState newState)
        {
            if (currentState == newState)
            {
                return;
            }

            currentState = newState;
            OnGameStateChanged?.Invoke(currentState);
        }
    }
}
