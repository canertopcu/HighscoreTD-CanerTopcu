using Assets.Scripts.Enemy;
using Zenject;

namespace Assets.Scripts.Pool
{
    public class GiantEnemyMemoryPool : MonoMemoryPool<EnemyController>
    {
        protected override void Reinitialize(EnemyController item)
        {
            item.OnSpawned(this);
        }
    }
}
