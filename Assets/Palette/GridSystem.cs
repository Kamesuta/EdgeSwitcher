using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// グリッド、タイル関係操作
public class GridSystem : MonoBehaviour
{
    // グリッド
    public Grid baseGrid;

    // タイルマップ
    public Tilemap baseTilemap;

    // オーバーレイタイルマップ
    public Tilemap overlayTilemap;

    // directionを回転する
    public Vector2Int Rotate90(Vector2Int direction, int turnRight)
    {
        // 90度回転させる
        return new Vector2Int(direction.y, -direction.x) * turnRight;
    }

    // サイドのタイルを取得する
    public Vector3Int GetSideCell(Vector3 position, Vector2Int sideDirection)
    {
        var offset = ((Vector3)(Vector3Int)sideDirection) * (baseGrid.cellSize.magnitude / 2f);
        var pos = baseGrid.WorldToCell(position + offset);
        return pos;
    }

    // マスにスナップする
    public Vector3 SnapToGrid(Vector3 position)
    {
        position.x = Mathf.Round(position.x / baseGrid.cellSize.x) * baseGrid.cellSize.x;
        position.y = Mathf.Round(position.y / baseGrid.cellSize.y) * baseGrid.cellSize.y;
        return position;
    }

    // 隣接するタイルの塊を取得する
    private HashSet<Vector3Int> GetTileCluster(Vector3Int basePosition, TileBase selectTile)
    {
        // 探索済みのタイル
        var searched = new HashSet<Vector3Int>();

        // ベース場所のタイルが一致しているか
        if (baseTilemap.GetTile(basePosition) != selectTile)
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
                if (baseTilemap.GetTile(nextPos) == selectTile)
                {
                    search.Enqueue(nextPos);
                }
            }
        }

        // 探索済みのタイルを返す
        return searched;
    }

    // タイルの塊を選択する
    public void SelectCluster(Vector3Int basePosition, TileBase selectTile, PaintTileSet paint)
    {
        // タイルの塊を取得
        var cluster = GetTileCluster(basePosition, selectTile);
        // オーバーレイを設定
        overlayTilemap.ClearAllTiles();
        foreach (var pos in cluster)
        {
            overlayTilemap.SetTile(pos, paint.overlay);
        }
    }
}
