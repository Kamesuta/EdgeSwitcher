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

    // �r���^�C���}�b�v
    public Tilemap partialTilemap;

    // ���^�C��
    public TileBase horizontalTilebase;

    // �c�^�C��
    public TileBase verticalTilebase;

    // ����`�悷��
    public void DrawLine(Vector2Int centerCell, Vector2Int moveDirection, Vector2Int sideDirection)
    {
        // ����
        if (moveDirection.y == 0)
        {
            int sideOffset = (sideDirection.y - 1) / 2;
            var pos = new Vector3Int(centerCell.x, centerCell.y + sideOffset, 0);
            horizontalTilemap.SetTile(pos, horizontalTilebase);
        }
        // �c��
        if (moveDirection.x == 0)
        {
            int sideOffset = (sideDirection.x - 1) / 2;
            var pos = new Vector3Int(centerCell.x + sideOffset, centerCell.y, 0);
            verticalTilemap.SetTile(pos, verticalTilebase);
        }
    }

    // �r���̐���`�悷��
    public void DrawLinePartial(Vector2Int centerCell, Vector2Int moveDirection, Vector2Int sideDirection, float partialPercent)
    {
        // �r���^�C���̈ʒu
        Vector3 centerPos = baseGrid.CellToWorld((Vector3Int)centerCell);
        // �r���^�C���}�b�v���N���A
        partialTilemap.ClearAllTiles();
        // ����
        if (moveDirection.y == 0)
        {
            int sideOffset = (sideDirection.y - 1) / 2;
            int moveOffset = (moveDirection.x - 1) / 2;
            partialTilemap.SetTile(new Vector3Int(moveOffset, 0), horizontalTilebase);
            partialTilemap.transform.position = centerPos + new Vector3(-moveOffset, sideOffset + 0.5f, 0);
            partialTilemap.transform.localScale = new Vector3(partialPercent, 1, 1);
        }
        // �c��
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
