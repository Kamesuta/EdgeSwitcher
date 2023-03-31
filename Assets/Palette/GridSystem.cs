using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// �O���b�h�A�^�C���֌W����
public class GridSystem : MonoBehaviour
{
    // �O���b�h
    public Grid baseGrid;

    // �^�C���}�b�v
    public Tilemap baseTilemap;

    // �I�[�o�[���C�^�C���}�b�v
    public Tilemap overlayTilemap;

    // direction����]����
    public Vector2Int Rotate90(Vector2Int direction, int turnRight)
    {
        // 90�x��]������
        return new Vector2Int(direction.y, -direction.x) * turnRight;
    }

    // �T�C�h�̃^�C�����擾����
    public Vector3Int GetSideCell(Vector3 position, Vector2Int sideDirection)
    {
        var offset = ((Vector3)(Vector3Int)sideDirection) * (baseGrid.cellSize.magnitude / 2f);
        var pos = baseGrid.WorldToCell(position + offset);
        return pos;
    }

    // �}�X�ɃX�i�b�v����
    public Vector3 SnapToGrid(Vector3 position)
    {
        position.x = Mathf.Round(position.x / baseGrid.cellSize.x) * baseGrid.cellSize.x;
        position.y = Mathf.Round(position.y / baseGrid.cellSize.y) * baseGrid.cellSize.y;
        return position;
    }

    // �אڂ���^�C���̉���擾����
    private HashSet<Vector3Int> GetTileCluster(Vector3Int basePosition, TileBase selectTile)
    {
        // �T���ς݂̃^�C��
        var searched = new HashSet<Vector3Int>();

        // �x�[�X�ꏊ�̃^�C������v���Ă��邩
        if (baseTilemap.GetTile(basePosition) != selectTile)
        {
            return searched;
        }

        // �T������^�C��
        var search = new Queue<Vector3Int>();
        search.Enqueue(basePosition);

        // �T������I�t�Z�b�g�W
        Vector3Int[] offsets = new Vector3Int[] { Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right };

        // �T������
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

        // �T���ς݂̃^�C����Ԃ�
        return searched;
    }

    // �^�C���̉��I������
    public void SelectCluster(Vector3Int basePosition, TileBase selectTile, PaintTileSet paint)
    {
        // �^�C���̉���擾
        var cluster = GetTileCluster(basePosition, selectTile);
        // �I�[�o�[���C��ݒ�
        overlayTilemap.ClearAllTiles();
        foreach (var pos in cluster)
        {
            overlayTilemap.SetTile(pos, paint.overlay);
        }
    }
}
