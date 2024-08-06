namespace Assets.Scripts.Core.Signals
{
    public class LevelSignal
    {
        public int Level { get; private set; }

        public LevelSignal(int level)
        {
            Level = level;
        }
    }
}
