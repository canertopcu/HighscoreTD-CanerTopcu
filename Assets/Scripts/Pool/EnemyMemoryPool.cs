using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
using Zenject;
using static UnityEngine.EventSystems.EventTrigger;

namespace Assets.Scripts.Pool
{
    public class EnemyMemoryPool : MonoMemoryPool<EnemyController>
    {
        protected override void Reinitialize(EnemyController item)
        {
            item.OnSpawned(this);
        }
    }
}
