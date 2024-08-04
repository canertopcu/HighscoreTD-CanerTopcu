using Assets.Scripts.Core.Enums;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewTileSystem", menuName = "Tiles/New Tile System")]

    public class TileSO : ScriptableObject
    {
        public SerializedDictionary<TileType, GameObject> tilePrefabs = new();
    }
}
