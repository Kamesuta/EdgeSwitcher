using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridSystem : MonoBehaviour
{
    // �O���b�h
    public Grid baseGrid;

    // �^�C���}�b�v
    public Tilemap baseTilemap;

    // �I�[�o�[���C�^�C���}�b�v
    public Tilemap overlayTilemap;

    // �}�X�ɃX�i�b�v����
    public Vector3 SnapToGrid(Vector3 position)
    {
        position.x = Mathf.Round(position.x / baseGrid.cellSize.x) * baseGrid.cellSize.x;
        position.y = Mathf.Round(position.y / baseGrid.cellSize.y) * baseGrid.cellSize.y;
        return position;
    }

    // �אڂ���^�C���̉���擾����
    private HashSet<Vector3Int> GetTileCluster(TileBase tile, Vector3Int basePosition)
    {
        // �T���ς݂̃^�C��
        var searched = new HashSet<Vector3Int>();

        // �x�[�X�ꏊ�̃^�C������v���Ă��邩
        if (baseTilemap.GetTile(basePosition) != tile)
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
                if (baseTilemap.GetTile(nextPos) == tile)
                {
                    search.Enqueue(nextPos);
                }
            }
        }

        // �T���ς݂̃^�C����Ԃ�
        return searched;
    }

    // �^�C���̉��I������
    public void SelectCluster(TileBase tile, Vector3Int basePosition)
    {
        // �^�C���̉���擾
        var cluster = GetTileCluster(tile, basePosition);
        // �I�[�o�[���C��ݒ�
        overlayTilemap.ClearAllTiles();
        foreach (var pos in cluster)
        {
            overlayTilemap.SetTile(pos, tile);
        }
    }
}
