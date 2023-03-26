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

    // 横タイル
    public TileBase horizontalTilebase;

    // 縦タイル
    public TileBase verticalTilebase;

    // 線を描画する
    public void DrawLine(Vector2Int centerCell, Vector2Int sideDirection)
    {
        // 横線
        if (sideDirection.x == 0)
        {
            int offset = (sideDirection.y - 1) / 2;
            var pos = new Vector3Int(centerCell.x, centerCell.y + offset, 0);
            horizontalTilemap.SetTile(pos, horizontalTilebase);
        }
        // 縦線
        if (sideDirection.y == 0)
        {
            int offset = (sideDirection.x - 1) / 2;
            var pos = new Vector3Int(centerCell.x + offset, centerCell.y, 0);
            verticalTilemap.SetTile(pos, verticalTilebase);
        }
    }
}
