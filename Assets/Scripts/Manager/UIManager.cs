using Assets.Scripts.UI;
using UnityEngine;

namespace Assets.Scripts.Manager
{
    public class UIManager : MonoBehaviour
    {
        public GameObject gameOverPanel;
        public GameObject gamePanel;

        private void Start()
        {
            gameOverPanel.SetActive(false);
            gamePanel.SetActive(true);
        }

        public void ShowGameOverPanel(int score, int highScore)
        {
            gameOverPanel.SetActive(true);
            gameOverPanel.GetComponent<EndingUIContoller>().ShowPanel(score,  highScore);
            gamePanel.SetActive(false);
        }
    }
}
