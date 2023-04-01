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
    public interface IPartialTilemap
    {
        // �r���^�C���}�b�v
        public Tilemap PartialTilemap { get; }

        // �r���^�C���}�b�v�}�X�N
        public Transform PartialMask { get; }
    }

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
    public void DrawLinePartial(IPartialTilemap partial, PaintTileSet paint, Vector2Int centerCell, Vector2Int moveDirection, Vector2Int sideDirection, float partialPercent)
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
            partial.PartialTilemap.SetTile(Vector3Int.zero, paint.lineHorizontal);
            partial.PartialTilemap.transform.position = centerPos + new Vector3(0, sideOffset + half.y, 0);
            partial.PartialMask.transform.localPosition = new Vector3(partialPercent * moveDirection.x * half.x - moveOffset, half.y);
            partial.PartialMask.transform.localScale = new Vector3(partialPercent, 1, 1);
        }
        // �c��
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
}
