using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class EndingUIContoller : MonoBehaviour
    {
        public TextMeshProUGUI scoreText;
        public TextMeshProUGUI highScoreText;
        public Button replayButton;
        public Button quitButton;

        public void ShowPanel(int score, int highScore)
        {
            scoreText.text = "Score: " + score;
            highScoreText.text = "High Score: " + highScore;
        }

        private void Start()
        {
            replayButton.onClick.AddListener(OnReplayButtonClicked);
            quitButton.onClick.AddListener(OnQuitButtonClicked);
        }
        private void OnDisable()
        {
            replayButton.onClick.RemoveListener(OnReplayButtonClicked);
            quitButton.onClick.RemoveListener(OnQuitButtonClicked);
        }

        private void OnReplayButtonClicked()
        { 
            SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
        }

        private void OnQuitButtonClicked()
        { 
            Application.Quit(); 
        }
    }
}
