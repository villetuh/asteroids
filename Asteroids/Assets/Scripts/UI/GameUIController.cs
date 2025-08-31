using Asteroids.Scores;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Asteroids.UI
{
    /// <summary>
    /// Component handling UI logic of the game.
    /// </summary>
    public class GameUIController : MonoBehaviour
    {
        [Header("MainMenu")]
        [SerializeField] private Transform mainMenu;
        [SerializeField] private Button startGameButton;

        [Header("GameScreen")]
        [SerializeField] private Transform gameScreen;
        [SerializeField] private TMPro.TextMeshProUGUI scoreText;

        [Header("GameOverScreen")]
        [SerializeField] private Transform gameOverScreen;
        [SerializeField] private Button backToMainMenuButton;

        private IGameController gameController;
        private IScoreController scoreController;

        [Inject]
        private void Construct(IGameController gameController, IScoreController scoreController)
        {
            this.gameController = gameController
                ?? throw new System.ArgumentNullException(nameof(gameController), $"{nameof(GameUIController)} requires reference to {nameof(IGameController)}.");

            gameController.OnGameStateChanged += OnGameStateChanged;

            this.scoreController = scoreController
                ?? throw new System.ArgumentNullException(nameof(scoreController), $"{nameof(GameUIController)} requires reference to {nameof(IScoreController)}.");

            scoreController.OnScoreChanged += OnScoreChanged;

            startGameButton.onClick.AddListener(OnStartGameClicked);
            backToMainMenuButton.onClick.AddListener(OnBackToMainMenuClicked);
        }

        private void OnDestroy()
        {
            if (gameController != null)
            {
                gameController.OnGameStateChanged -= OnGameStateChanged;
            }

            if (scoreController != null)
            {
                scoreController.OnScoreChanged -= OnScoreChanged;
            }

            if (startGameButton != null)
            {
                startGameButton.onClick.RemoveListener(OnStartGameClicked);
            }

            if (backToMainMenuButton != null)
            {
                backToMainMenuButton.onClick.RemoveListener(OnBackToMainMenuClicked);
            }
        }

        private void OnStartGameClicked()
        {
            gameController.StartGame();
        }

        private void OnBackToMainMenuClicked()
        {
            gameController.EndGame();
        }

        private void OnGameStateChanged(GameState newState)
        {
            switch (newState)
            {
                case GameState.MainMenu:
                    ShowMainMenu();
                    break;
                case GameState.Playing:
                    ShowGameScreen();
                    break;
                case GameState.GameOver:
                    ShowGameOverScreen();
                    break;
                default:
                    Debug.LogWarning($"Unhandled game state: {newState}");
                    break;
            }
        }

        private void OnScoreChanged(int newScore)
        {
            scoreText.SetText($"{newScore}");
        }

        private void ShowMainMenu()
        {
            mainMenu.gameObject.SetActive(true);
            gameScreen.gameObject.SetActive(false);
            gameOverScreen.gameObject.SetActive(false);
        }

        private void ShowGameScreen()
        {
            mainMenu.gameObject.SetActive(false);
            gameScreen.gameObject.SetActive(true);
            gameOverScreen.gameObject.SetActive(false);
        }

        private void ShowGameOverScreen()
        {
            mainMenu.gameObject.SetActive(false);
            gameScreen.gameObject.SetActive(true);
            gameOverScreen.gameObject.SetActive(true);
        }
    }
}
