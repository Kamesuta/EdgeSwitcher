using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridSystem : MonoBehaviour
{
    // グリッド
    public Grid baseGrid;

    // タイルマップ
    public Tilemap baseTilemap;

    // オーバーレイタイルマップ
    public Tilemap overlayTilemap;

    // マスにスナップする
    public Vector3 SnapToGrid(Vector3 position)
    {
        position.x = Mathf.Round(position.x / baseGrid.cellSize.x) * baseGrid.cellSize.x;
        position.y = Mathf.Round(position.y / baseGrid.cellSize.y) * baseGrid.cellSize.y;
        return position;
    }

    // 隣接するタイルの塊を取得する
    private HashSet<Vector3Int> GetTileCluster(TileBase tile, Vector3Int basePosition)
    {
        // 探索済みのタイル
        var searched = new HashSet<Vector3Int>();

        // ベース場所のタイルが一致しているか
        if (baseTilemap.GetTile(basePosition) != tile)
        {
            return searched;
        }

        // 探索するタイル
        var search = new Queue<Vector3Int>();
        search.Enqueue(basePosition);

        // 探索するオフセット集
        Vector3Int[] offsets = new Vector3Int[] { Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right };

        // 探索する
        while (search.Count > 0)
        {
            var pos = search.Dequeue();
            searched.Add(pos);
            foreach (var offset in offsets)
            {
                var nextPos = pos + offset;
                if (searched.Contains(nextPos))
                {
                    continue;
                }
                if (baseTilemap.GetTile(nextPos) == tile)
                {
                    search.Enqueue(nextPos);
                }
            }
        }

        // 探索済みのタイルを返す
        return searched;
    }

    // タイルの塊を選択する
    public void SelectCluster(TileBase tile, Vector3Int basePosition)
    {
        // タイルの塊を取得
        var cluster = GetTileCluster(tile, basePosition);
        // オーバーレイを設定
        overlayTilemap.ClearAllTiles();
        foreach (var pos in cluster)
        {
            overlayTilemap.SetTile(pos, tile);
        }
    }
}
