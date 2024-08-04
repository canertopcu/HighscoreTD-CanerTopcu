using Assets.Scripts.ScriptableObjects;
using TMPro;
using UnityEngine;
using Zenject;

public class TopUIController : MonoBehaviour
{
    [Inject]
    GameDataSO gameData;

    [SerializeField] TextMeshProUGUI goldAmount;
    [SerializeField] TextMeshProUGUI score;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        goldAmount.text = gameData.playerGold.ToString();
        score.text = gameData.playerScore.ToString();
    }
}
