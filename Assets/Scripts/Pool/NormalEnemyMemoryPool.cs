using Zenject;

namespace Assets.Scripts.Pool
{
    public class NormalEnemyMemoryPool : MonoMemoryPool<EnemyController>
    {
        protected override void Reinitialize(EnemyController item)
        {
            item.OnSpawned(this);
        }
    }
}
