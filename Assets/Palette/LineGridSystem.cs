using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// ���֌W
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

    // �r���^�C���}�b�v�}�X�N
    public Transform partialMask;

    // ����`�悷��
    public void DrawLine(PaintTileSet paint, Vector2Int centerCell, Vector2Int moveDirection, Vector2Int sideDirection)
    {
        // ����
        if (moveDirection.y == 0)
        {
            int sideOffset = (sideDirection.y - 1) / 2;
            var pos = new Vector3Int(centerCell.x, centerCell.y + sideOffset, 0);
            horizontalTilemap.SetTile(pos, paint.lineHorizontal);
        }
        // �c��
        if (moveDirection.x == 0)
        {
            int sideOffset = (sideDirection.x - 1) / 2;
            var pos = new Vector3Int(centerCell.x + sideOffset, centerCell.y, 0);
            verticalTilemap.SetTile(pos, paint.lineVertical);
        }
    }

    // �r���̐���`�悷��
    public void DrawLinePartial(PaintTileSet paint, Vector2Int centerCell, Vector2Int moveDirection, Vector2Int sideDirection, float partialPercent)
    {
        // �r���^�C���̈ʒu
        Vector3 centerPos = baseGrid.CellToWorld((Vector3Int)centerCell);
        // �����O���b�h�T�C�Y
        var half = (Vector2)baseGrid.cellSize / 2f;

        // ����
        if (moveDirection.y == 0)
        {
            int sideOffset = (sideDirection.y - 1) / 2;
            int moveOffset = (moveDirection.x - 1) / 2;
            partialTilemap.SetTile(Vector3Int.zero, paint.lineHorizontal);
            partialTilemap.transform.position = centerPos + new Vector3(0, sideOffset + half.y, 0);
            partialMask.transform.localPosition = new Vector3(partialPercent * moveDirection.x * half.x - moveOffset, half.y);
            partialMask.transform.localScale = new Vector3(partialPercent, 1, 1);
        }
        // �c��
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
