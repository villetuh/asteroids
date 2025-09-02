using Asteroids.Entities;
using Asteroids.Levels;
using Asteroids.Scores;
using Asteroids.Utilities;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace Asteroids.Tests
{
    public class GameControllerTests
    {
        private GameController CreateGameController(out Mock<ILevelController> levelControllerMock,
                                                    out Mock<IScoreController> scoreControllerMock,
                                                    out Mock<ITimeProvider> timeProviderMock)
        {
            levelControllerMock = new Mock<ILevelController>();
            scoreControllerMock = new Mock<IScoreController>();
            timeProviderMock = new Mock<ITimeProvider>();

            return new GameController(levelControllerMock.Object, scoreControllerMock.Object, timeProviderMock.Object);
        }

        [Test]
        public void Initializing_GameController_Sets_GameState_To_MainMenu_And_Invokes_GameStateChanged_Event()
        {
            // Arrange
            var gameController = CreateGameController(out _, out _, out _);

            var latestGameState = GameState.Undefined;
            gameController.OnGameStateChanged += (newState) =>
            {
                latestGameState = newState;
            };

            // Act
            gameController.Start();

            // Assert
            Assert.AreEqual(GameState.MainMenu, latestGameState, $"Game state not updated as expected.");
        }

        [Test]
        public void Starting_Game_Sets_GameState_To_Playing_And_Invokes_GameStateChanged_Event()
        {
            // Arrange
            var gameController = CreateGameController(out _, out _, out _);

            var latestGameState = GameState.Undefined;
            gameController.OnGameStateChanged += (newState) =>
            {
                latestGameState = newState;
            };

            // Act
            gameController.StartGame();

            // Assert
            Assert.AreEqual(GameState.Playing, latestGameState, $"Game state not updated as expected.");
        }

        [Test]
        public void Ending_Game_Sets_GameState_To_MainMenu_And_Invokes_GameStateChanged_Event()
        {
            // Arrange
            var gameController = CreateGameController(out _, out _, out _);

            var latestGameState = GameState.Undefined;
            gameController.OnGameStateChanged += (newState) =>
            {
                latestGameState = newState;
            };

            // Act
            gameController.EndGame();

            // Assert
            Assert.AreEqual(GameState.MainMenu, latestGameState, $"Game state not updated as expected.");
        }

        [Test]
        public void Player_Getting_Hit_Sets_GameState_To_GameOver_And_Invokes_GameStateChanged_Event()
        {
            // Arrange
            var gameController = CreateGameController(out var levelControllerMock, out _, out _);
            var asteroidMock = new Mock<IAsteroid>();

            var latestGameState = GameState.Undefined;
            gameController.OnGameStateChanged += (newState) =>
            {
                latestGameState = newState;
            };

            // Act
            gameController.OnPlayerHit(asteroidMock.Object);

            // Assert
            Assert.AreEqual(GameState.GameOver, latestGameState, $"Game state not updated to GameOver as expected.");
        }

        [Test]
        public void Ticking_GameController_Invokes_LevelController_UpdateLevel_With_Current_Time()
        {
            // Arrange
            var gameController = CreateGameController(out var levelControllerMock, out _, out var timeProviderMock);
            var testTime = 123.45f;
            timeProviderMock.Setup(tp => tp.Time).Returns(testTime);

            gameController.StartGame(); // Set state to Playing

            // Act
            gameController.Tick();

            // Assert
            levelControllerMock.Verify(mock => mock.UpdateLevel(testTime), Times.Once,
                "UpdateLevel should be called with current time.");
        }

        [Test]
        public void Starting_Game_Resets_Players_Score()
        {
            // Arrange
            var gameController = CreateGameController(out _, out var scoreControllerMock, out _);

            // Act
            gameController.StartGame();

            // Assert
            scoreControllerMock.Verify(mock => mock.ResetScore(), Times.Once,
                "Starting a new game resets player's score.");
        }

        [Test]
        public void Starting_Game_Creates_Level()
        {
            // Arrange
            var gameController = CreateGameController(out var levelControllerMock, out _, out _);

            // Act
            gameController.StartGame();

            // Assert
            levelControllerMock.Verify(mock => mock.CreateLevel(), Times.Once,
                "Starting a new game should create the game level.");
        }

        [Test]
        public void Ending_Game_Clears_The_Current_Level()
        {
            // Arrange
            var gameController = CreateGameController(out var levelControllerMock, out _, out _);

            // Act
            gameController.EndGame();

            // Assert
            levelControllerMock.Verify(mock => mock.ClearLevel(), Times.Once,
                "Ending the game should clear the current level.");
        }

        [Test]
        public void GameController_Passes_OnPlayerFire_Event_To_LevelController()
        {
            // Arrange
            var gameController = CreateGameController(out var levelControllerMock, out _, out _);

            var firePosition = new Vector2(1, 2);
            var fireDirection = new Vector2(0, 1);

            // Act
            gameController.OnPlayerFire(firePosition, fireDirection);

            // Assert
            levelControllerMock.Verify(mock => mock.HandleBulletFired(firePosition, fireDirection),
                                                Times.Once, 
                                                $"OnPlayerFire event not passed to {nameof(ILevelController)} as expected.");
        }

        [Test]
        public void GameController_Passes_OnBulletExpired_Event_To_LevelController()
        {
            // Arrange
            var gameController = CreateGameController(out var levelControllerMock, out _, out _);
            var bulletMock = new Mock<IBullet>();

            // Act
            gameController.OnBulletExpired(bulletMock.Object);

            // Assert
            levelControllerMock.Verify(mock => mock.HandleBulletExpired(bulletMock.Object),
                                                Times.Once,
                                                $"OnBulletExpired event not passed to {nameof(ILevelController)} as expected.");
        }

        [Test]
        public void GameController_Passes_OnAsteroidHit_Event_To_ScoreController()
        {
            // Arrange
            var gameController = CreateGameController(out _, out var scoreControllerMock, out _);
            var bulletMock = new Mock<IBullet>();
            var asteroidMock = new Mock<IAsteroid>();
            asteroidMock.Setup(a => a.Size).Returns(AsteroidSize.Large);

            // Act
            gameController.OnAsteroidHit(bulletMock.Object, asteroidMock.Object);

            // Assert
            scoreControllerMock.Verify(mock => mock.ScoreDestroyedAsteroid(AsteroidSize.Large),
                                                Times.Once,
                                                $"OnBulletExpired event not passed to {nameof(IScoreController)} as expected.");
        }

        [Test]
        public void GameController_Passes_OnAsteroidHit_Event_To_LevelController()
        {
            // Arrange
            var gameController = CreateGameController(out var levelControllerMock, out _, out _);
            var bulletMock = new Mock<IBullet>();
            var asteroidMock = new Mock<IAsteroid>();

            // Act
            gameController.OnAsteroidHit(bulletMock.Object, asteroidMock.Object);

            // Assert
            levelControllerMock.Verify(mock => mock.HandleAsteroidHit(bulletMock.Object, asteroidMock.Object),
                                                Times.Once,
                                                $"OnAsteroidHit event not passed to {nameof(ILevelController)} as expected.");
        }

        [Test]
        public void GameController_Passes_OnPlayerHit_Event_To_LevelController()
        {
            // Arrange
            var gameController = CreateGameController(out var levelControllerMock, out _, out _);
            var asteroidMock = new Mock<IAsteroid>();

            // Act
            gameController.OnPlayerHit(asteroidMock.Object);

            // Assert
            levelControllerMock.Verify(mock => mock.HandlePlayerHit(asteroidMock.Object),
                                                Times.Once,
                                                $"OnPlayerHit event not passed to {nameof(ILevelController)} as expected.");
        }

        [Test]
        public void Starting_Game_Then_Getting_Player_Hit_Event_Ends_Game()
        {
            // Arrange
            var gameController = CreateGameController(out var levelControllerMock, out _, out _);
            var asteroidMock = new Mock<IAsteroid>();

            var stateTransitions = new List<GameState>();
            gameController.OnGameStateChanged += (newState) =>
            {
                stateTransitions.Add(newState);
            };

            // Act
            gameController.StartGame();
            gameController.OnPlayerHit(asteroidMock.Object);

            // Assert
            Assert.AreEqual(2, stateTransitions.Count, "Should have two state transitions.");
            Assert.AreEqual(GameState.Playing, stateTransitions[0], "First transition should be to Playing.");
            Assert.AreEqual(GameState.GameOver, stateTransitions[1], "Second transition should be to GameOver.");
        }
    }
}
