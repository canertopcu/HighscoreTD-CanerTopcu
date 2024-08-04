using Assets.Scripts.Core.Interfaces;
using UnityEngine;
namespace Assets.Scripts.Manager
{
    public class GameManager : MonoBehaviour, IGameManager
    {
        public void StartGame()
        {
            Debug.Log("Game Started");
        }

        private void Update()
        {
        }
    }
}
