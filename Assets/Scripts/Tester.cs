using Assets.Scripts.Core.Interfaces;
using UnityEngine;
using Zenject;

public class Tester : MonoBehaviour
{
    [Inject]
    private IGameManager _gameManager; 
    void Start()
    {
        _gameManager.StartGame();
    }
     
    void Update()
    {

    }
}
