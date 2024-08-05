using Zenject;

namespace Assets.Scripts.Pool
{
    public class TinyEnemyMemoryPool : MonoMemoryPool<EnemyController>
    {
        protected override void Reinitialize(EnemyController item)
        {
            item.OnSpawned(this);
        }
    }
}
