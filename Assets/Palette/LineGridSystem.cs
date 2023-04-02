using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

// 線関係
public class LineGridSystem : MonoBehaviour
{
    // グリッド
    public Grid baseGrid;

    // 線用ハーフサイズタイルマップ
    public Tilemap lineTilemap;

    // コーナー用ハーフサイズタイルマップ
    public Tilemap cornerTilemap;

    // 途中タイルマップ
    public interface IPartialTilemap
    {
        // 途中タイルマップ
        public Tilemap PartialTilemap { get; }

        // 途中タイルマップマスク
        public Transform PartialMask { get; }
    }

    // 線を描画する
    public void DrawLine(PaintTileSet paint, Vector2Int centerCell, Vector2Int moveDirection, Vector2Int sideDirection)
    {
        // 横線
        if (moveDirection.y == 0)
        {
            var pos = new Vector3Int(centerCell.x * 2, centerCell.y * 2 + sideDirection.y, 0);
            lineTilemap.SetTile(pos, paint.lineHorizontal);
        }
        // 縦線
        if (moveDirection.x == 0)
        {
            var pos = new Vector3Int(centerCell.x * 2 + sideDirection.x, centerCell.y * 2, 0);
            lineTilemap.SetTile(pos, paint.lineVertical);
        }
    }

    // コーナーを描画する
    public void DrawCorner(PaintTileSet paint, Vector2Int centerCell, Vector2Int moveDirection, Vector2Int sideDirection)
    {
        // 横線
        if (moveDirection.y == 0)
        {
            var pos = new Vector3Int(centerCell.x * 2 + moveDirection.x, centerCell.y * 2 + sideDirection.y, 0);
            cornerTilemap.SetTile(pos, paint.lineCorner);
        }
        // 縦線
        if (moveDirection.x == 0)
        {
            var pos = new Vector3Int(centerCell.x * 2 + sideDirection.x, centerCell.y * 2 + moveDirection.y, 0);
            cornerTilemap.SetTile(pos, paint.lineCorner);
        }
    }

    // 途中の線を描画する
    public void DrawLinePartial(IPartialTilemap partial, PaintTileSet paint, Vector2Int centerCell, Vector2Int moveDirection, Vector2Int sideDirection, float partialPercent)
    {
        // 途中タイルの位置
        Vector3 centerPos = baseGrid.CellToWorld((Vector3Int)centerCell);
        // 半分グリッドサイズ
        var half = (Vector2)baseGrid.cellSize / 2f;

        // 横線
        if (moveDirection.y == 0)
        {
            int sideOffset = (sideDirection.y - 1) / 2;
            int moveOffset = (moveDirection.x - 1) / 2;
            partial.PartialTilemap.SetTile(Vector3Int.zero, paint.lineHorizontal);
            partial.PartialTilemap.transform.position = centerPos + new Vector3(0, sideOffset + half.y, 0);
            partial.PartialMask.transform.localPosition = new Vector3(partialPercent * moveDirection.x * half.x - moveOffset, half.y);
            partial.PartialMask.transform.localScale = new Vector3(partialPercent, 1, 1);
        }
        // 縦線
        if (moveDirection.x == 0)
        {
            int sideOffset = (sideDirection.x - 1) / 2;
            int moveOffset = (moveDirection.y - 1) / 2;
            partial.PartialTilemap.SetTile(Vector3Int.zero, paint.lineVertical);
            partial.PartialTilemap.transform.position = centerPos + new Vector3(sideOffset + half.x, 0, 0);
            partial.PartialMask.transform.localPosition = new Vector3(half.x, partialPercent * moveDirection.y * half.y - moveOffset);
            partial.PartialMask.transform.localScale = new Vector3(1, partialPercent, 1);
        }
    }

    // 4方向
    private readonly Vector3Int[] directions = { Vector3Int.left, Vector3Int.up, Vector3Int.right, Vector3Int.down };

    // タイルの塊が線で囲まれているか
    public bool IsClusterEnclosed(HashSet<Vector3Int> cluster, PaintTileSet paint)
    {
        // すべてのタイル
        foreach (var cell in cluster)
        {
            // 方向ごと
            foreach (var direction in directions)
            {
                // タイルの塊が隣にあるか
                if (cluster.Contains(cell + direction)) continue;

                // 線が隣りにあるか
                TileBase tile = lineTilemap.GetTile(cell * 2 + direction);
                if (tile == paint.lineVertical || tile == paint.lineHorizontal) continue;

                // 穴がある
                return false;
            }
        }

        // 穴がない
        return true;
    }
}
