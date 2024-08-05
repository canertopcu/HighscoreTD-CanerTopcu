namespace Assets.Scripts.Core.Signals
{
    public class GameStateChangedSignal
    {
        public bool State { get; private set; }
        public GameStateChangedSignal(bool state)
        {
            State = state;
        }
    }
}
