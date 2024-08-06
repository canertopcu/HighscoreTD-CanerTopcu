namespace Assets.Scripts.Core.Signals
{
    public class BoosterSignal
    {
        public bool IsBoosterActive { get; private set; }

        public BoosterSignal(bool isBoosterActive)
        {
            IsBoosterActive = isBoosterActive;
        }
    } 
}
