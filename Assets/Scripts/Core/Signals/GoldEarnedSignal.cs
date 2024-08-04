namespace Assets.Scripts.Core.Signals
{
    public class GoldEarnedSignal
    {
        public int Amount { get; private set; }

        public GoldEarnedSignal(int amount)
        {
            Amount = amount;
        }
    }
}
