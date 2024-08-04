using Assets.Scripts.Core.Enums;
using Assets.Scripts.ScriptableObjects;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{

    public int width = 10;
    public int height = 10;
    public float tileSize = 1f;

    private TileType[,] map;
    private TileElement[,] tileObjects;

    public GameObject tileElement;
    public TileSO tileSO;

    void InitializeMap()
    {
        map = new TileType[width, height];
        tileObjects = new TileElement[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                map[x, y] = TileType.Passive;

                if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                {
                    map[x, y] = TileType.Edge;
                }

                if ((x == 0 && y == 0) || (x == 0 && y == height - 1) || (x == width - 1 && y == 0) || (x == width - 1 && y == height - 1))
                {
                    map[x, y] = TileType.Corner;
                }

            }
        }
    }

    [ContextMenu("Create Visual Map")]
    void CreateVisualMap()
    {
        DeleteAllGridElements();
        InitializeMap();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                CreateTileVisual(x, y);
            }
        }
    }

    [ContextMenu("Delete All Grid Elements")]
    public void DeleteAllGridElements()
    { 
        while (transform.childCount > 0)
        {
            Transform child = transform.GetChild(0);
            child.SetParent(null);
            DestroyImmediate(child.gameObject);
        }
    }

    void CreateTileVisual(int x, int y)
    {
        TileType tileType = map[x, y];

        Vector3 position = new Vector3(x * tileSize, 0, y * tileSize);
        Quaternion localRotation = Quaternion.Euler(0, 0, 0);
        if (y == 0)
        {
            if (x == 0)
            {
                localRotation = Quaternion.Euler(0, 180, 0);
            }
            if (x == width - 1)
            {
                localRotation = Quaternion.Euler(0, 90, 0);
            }
            else
            {
                localRotation = Quaternion.Euler(0, 180, 0);
            }
        }
        else if (y < height - 1)
        {

            if (x == 0)
            {
                localRotation = Quaternion.Euler(0, 270, 0);
            }
            else if (x == width-1)
            {
                localRotation = Quaternion.Euler(0, 90, 0);
            }
            else
            {
                localRotation = Quaternion.Euler(0, 0, 0);
            }
        }
        else
        {
            if (x == 0)
            {
                localRotation = Quaternion.Euler(0, 270, 0);
            }
            else
            {
                localRotation = Quaternion.Euler(0, 0, 0);
            }
        }


        GameObject tileObject = Instantiate(tileElement, position, Quaternion.identity, transform);
        tileObject.transform.localRotation = localRotation;
        tileObject.name = $"Tile_{x}_{y}";

        if (tileObjects[x, y] != null)
        {
            Destroy(tileObjects[x, y]);
        }
        tileObjects[x, y] = tileObject.GetComponent<TileElement>();
        tileObjects[x, y].Set(tileSO, tileType, x, y);

    }



}


