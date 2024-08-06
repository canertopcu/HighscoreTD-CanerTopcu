using Assets.Scripts.Core.Enums;
using Assets.Scripts.ScriptableObjects;
using UnityEditor;
using UnityEngine;
namespace Assets.Scripts.Map
{
    public class TileElement : MonoBehaviour
    {
        [SerializeField] private TileType _tileType;
        private TileSO _tileSO;
        GameObject tileVisualObject;
        public int x, y;

        public TileType TileType => _tileType;
        public bool isFilled = false;
        public void Set(TileSO tileSO, TileType tileType, int x, int y)
        {
#if UNITY_EDITOR
            this.x = x;
            this.y = y;
            _tileSO = tileSO;
            _tileType = tileType;

            tileVisualObject = null;

            while (transform.childCount > 0)
            {
                Transform child = transform.GetChild(0);
                DestroyImmediate(child.gameObject);
            }

            tileVisualObject = PrefabUtility.InstantiatePrefab(_tileSO.tilePrefabs[tileType]) as GameObject;
            tileVisualObject.transform.SetParent(transform);

            PrefabUtility.ApplyPrefabInstance(tileVisualObject, InteractionMode.AutomatedAction);

            tileVisualObject.transform.localPosition = new Vector3(0, 0, 0);
            tileVisualObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
#endif
        }

        [ContextMenu("Reset Tile")]
        private void ResetTile()
        {
            if (_tileSO == null)
            {
                _tileSO = Resources.Load<TileSO>("Tiles/TileSystem");
            }
            Set(_tileSO, _tileType, x, y);
        }

    }
}