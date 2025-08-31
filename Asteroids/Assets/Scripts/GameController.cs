using Asteroids.Entities;
using Asteroids.Levels;
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
        private readonly ITimeProvider timeProvider;

        private GameState currentState = GameState.Undefined;

        /// <inheritdoc cref="IGameController.OnGameStateChanged" />
        public Action<GameState> OnGameStateChanged { get; set; }

        public GameController(ILevelController levelController, ITimeProvider timeProvider)
        {
            this.levelController = levelController
                ?? throw new ArgumentNullException(nameof(levelController), $"{nameof(GameController)} requires reference to {nameof(ILevelController)}.");

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

        /// <inheritdoc cref="IGameController.OnBulletExpired(Bullet)" />
        public void OnBulletExpired(Bullet bullet)
        {
            levelController.HandleBulletExpired(bullet);
        }

        /// <inheritdoc cref="IGameController.OnAsteroidHit(Bullet, Asteroid)" />
        public void OnAsteroidHit(Bullet bullet, Asteroid asteroid)
        {
            levelController.HandleAsteroidHit(bullet, asteroid);
        }

        /// <inheritdoc cref="IGameController.OnPlayerHit(Asteroid)" />
        public void OnPlayerHit(Asteroid asteroid)
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
