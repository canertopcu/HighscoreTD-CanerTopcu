using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Map
{
    public class MapController : MonoBehaviour
    {
        public List<TileElement> PathElements;
        public List<TileElement> TowerElements;
        public TileElement Spawner;
        public TileElement MainTower;
    }
}
