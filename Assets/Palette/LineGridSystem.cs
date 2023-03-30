using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LineGridSystem : MonoBehaviour
{
    // �O���b�h
    public Grid baseGrid;

    // ���^�C���}�b�v
    public Tilemap horizontalTilemap;

    // �c�^�C���}�b�v
    public Tilemap verticalTilemap;

    // ���^�C��
    public TileBase horizontalTilebase;

    // �c�^�C��
    public TileBase verticalTilebase;

    // ����`�悷��
    public void DrawLine(Vector2Int centerCell, Vector2Int sideDirection)
    {
        // ����
        if (sideDirection.x == 0)
        {
            int offset = (sideDirection.y - 1) / 2;
            var pos = new Vector3Int(centerCell.x, centerCell.y + offset, 0);
            horizontalTilemap.SetTile(pos, horizontalTilebase);
        }
        // �c��
        if (sideDirection.y == 0)
        {
            int offset = (sideDirection.x - 1) / 2;
            var pos = new Vector3Int(centerCell.x + offset, centerCell.y, 0);
            verticalTilemap.SetTile(pos, verticalTilebase);
        }
    }
}
