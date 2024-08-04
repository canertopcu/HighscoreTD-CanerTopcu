﻿using System;
using UnityEngine;
using Zenject;

namespace Assets.Scripts
{
    public class Bullet : MonoBehaviour, IPoolable<IMemoryPool>, IDisposable
    {
        IMemoryPool _pool;

        public void OnSpawned(IMemoryPool pool)
        {
            _pool = pool; 
        }

        public void OnDespawned()
        { 
        }

        public void Dispose()
        {
            _pool.Despawn(this);
        }

        private void OnTriggerEnter(Collider other)
        { 
            Dispose();
        }
    }
}
