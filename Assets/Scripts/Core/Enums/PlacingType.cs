namespace Assets.Scripts.Core.Enums
{
    [System.Flags]
    public enum PlacingType
    {
        OnPath = 1 << 0,
        OnDock = 1 << 1,
    }
}
