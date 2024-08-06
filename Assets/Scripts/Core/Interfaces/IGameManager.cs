namespace Assets.Scripts.Core.Interfaces
{
    internal interface IGameManager
    {
        bool IsGameStarted();
        void StartGame();
        void EndGame();
    }
}
