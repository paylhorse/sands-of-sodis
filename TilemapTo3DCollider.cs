using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapTo3DCollider : MonoBehaviour
{
    public Tilemap tilemap;
    public GameObject colliderPrefab;

    void Start()
    {
        BoundsInt bounds = tilemap.cellBounds;
        TileBase[] allTiles = tilemap.GetTilesBlock(bounds);

        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                TileBase tile = allTiles[x + y * bounds.size.x];
                if (tile != null)
                {
                    // Vector3 pos = new Vector3(x, 0, y) + bounds.min;
                    Vector3 pos = new Vector3(x, 0, y);
                    Instantiate(colliderPrefab, pos, Quaternion.identity, transform);
                }
            }
        }
    }
}
