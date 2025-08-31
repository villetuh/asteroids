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

        [Header("GameOverScreen")]
        [SerializeField] private Transform gameOverScreen;
        [SerializeField] private Button backToMainMenuButton;

        private IGameController gameController;

        [Inject]
        private void Construct(IGameController gameController)
        {
            this.gameController = gameController
                ?? throw new System.ArgumentNullException(nameof(gameController), $"{nameof(GameUIController)} requires reference to {nameof(IGameController)}.");

            gameController.OnGameStateChanged += OnGameStateChanged;

            startGameButton.onClick.AddListener(OnStartGameClicked);
            backToMainMenuButton.onClick.AddListener(OnBackToMainMenuClicked);
        }

        private void OnDestroy()
        {
            if (gameController != null)
            {
                gameController.OnGameStateChanged -= OnGameStateChanged;
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
            gameScreen.gameObject.SetActive(false);
            gameOverScreen.gameObject.SetActive(true);
        }
    }
}
