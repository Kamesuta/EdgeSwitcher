using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// 線関係
public class LineGridSystem : MonoBehaviour
{
    // グリッド
    public Grid baseGrid;

    // 横タイルマップ
    public Tilemap horizontalTilemap;

    // 縦タイルマップ
    public Tilemap verticalTilemap;

    // 途中タイルマップ
    public Tilemap partialTilemap;

    // 途中タイルマップマスク
    public Transform partialMask;

    // 線を描画する
    public void DrawLine(PaintTileSet paint, Vector2Int centerCell, Vector2Int moveDirection, Vector2Int sideDirection)
    {
        // 横線
        if (moveDirection.y == 0)
        {
            int sideOffset = (sideDirection.y - 1) / 2;
            var pos = new Vector3Int(centerCell.x, centerCell.y + sideOffset, 0);
            horizontalTilemap.SetTile(pos, paint.lineHorizontal);
        }
        // 縦線
        if (moveDirection.x == 0)
        {
            int sideOffset = (sideDirection.x - 1) / 2;
            var pos = new Vector3Int(centerCell.x + sideOffset, centerCell.y, 0);
            verticalTilemap.SetTile(pos, paint.lineVertical);
        }
    }

    // 途中の線を描画する
    public void DrawLinePartial(PaintTileSet paint, Vector2Int centerCell, Vector2Int moveDirection, Vector2Int sideDirection, float partialPercent)
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
            partialTilemap.SetTile(Vector3Int.zero, paint.lineHorizontal);
            partialTilemap.transform.position = centerPos + new Vector3(0, sideOffset + half.y, 0);
            partialMask.transform.localPosition = new Vector3(partialPercent * moveDirection.x * half.x - moveOffset, half.y);
            partialMask.transform.localScale = new Vector3(partialPercent, 1, 1);
        }
        // 縦線
        if (moveDirection.x == 0)
        {
            int sideOffset = (sideDirection.x - 1) / 2;
            int moveOffset = (moveDirection.y - 1) / 2;
            partialTilemap.SetTile(Vector3Int.zero, paint.lineVertical);
            partialTilemap.transform.position = centerPos + new Vector3(sideOffset + half.x, 0, 0);
            partialMask.transform.localPosition = new Vector3(half.x, partialPercent * moveDirection.y * half.y - moveOffset);
            partialMask.transform.localScale = new Vector3(1, partialPercent, 1);
        }
    }
}
