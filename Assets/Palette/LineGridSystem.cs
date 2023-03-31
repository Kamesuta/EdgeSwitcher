using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

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

    // 横タイル
    public TileBase horizontalTilebase;

    // 縦タイル
    public TileBase verticalTilebase;

    // 線を描画する
    public void DrawLine(Vector2Int centerCell, Vector2Int moveDirection, Vector2Int sideDirection)
    {
        // 横線
        if (moveDirection.y == 0)
        {
            int sideOffset = (sideDirection.y - 1) / 2;
            var pos = new Vector3Int(centerCell.x, centerCell.y + sideOffset, 0);
            horizontalTilemap.SetTile(pos, horizontalTilebase);
        }
        // 縦線
        if (moveDirection.x == 0)
        {
            int sideOffset = (sideDirection.x - 1) / 2;
            var pos = new Vector3Int(centerCell.x + sideOffset, centerCell.y, 0);
            verticalTilemap.SetTile(pos, verticalTilebase);
        }
    }

    // 途中の線を描画する
    public void DrawLinePartial(Vector2Int centerCell, Vector2Int moveDirection, Vector2Int sideDirection, float partialPercent)
    {
        // 途中タイルの位置
        Vector3 centerPos = baseGrid.CellToWorld((Vector3Int)centerCell);
        // 途中タイルマップをクリア
        partialTilemap.ClearAllTiles();
        // 横線
        if (moveDirection.y == 0)
        {
            int sideOffset = (sideDirection.y - 1) / 2;
            int moveOffset = (moveDirection.x - 1) / 2;
            partialTilemap.SetTile(new Vector3Int(moveOffset, 0), horizontalTilebase);
            partialTilemap.transform.position = centerPos + new Vector3(-moveOffset, sideOffset + 0.5f, 0);
            partialTilemap.transform.localScale = new Vector3(partialPercent, 1, 1);
        }
        // 縦線
        if (moveDirection.x == 0)
        {
            int sideOffset = (sideDirection.x - 1) / 2;
            int moveOffset = (moveDirection.y - 1) / 2;
            partialTilemap.SetTile(new Vector3Int(0, moveOffset), verticalTilebase);
            partialTilemap.transform.position = centerPos + new Vector3(sideOffset + 0.5f, -moveOffset, 0);
            partialTilemap.transform.localScale = new Vector3(1, partialPercent, 1);
        }
    }
}
