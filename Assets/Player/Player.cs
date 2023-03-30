using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

// �v���C���[
public class Player : MonoBehaviour
{
    // �O���b�h
    public GridSystem grid;

    // ���C���O���b�h
    public LineGridSystem line;

    // ���x
    public float speed = 1.0f;

    // ���݂̃^�C��
    private TileBase currentTile;
    // ���݂̌���
    private Vector2Int direction = Vector2Int.right;
    // ��]����
    private int turnRight = 1;

    // direction����]����
    private Vector2Int Rotate90(Vector2Int direction, int turnRight)
    {
        // 90�x��]������
        return new Vector2Int(direction.y, -direction.x) * turnRight;
    }

    // �T�C�h�̃^�C�����擾����
    private Vector3Int GetSideCell(Vector3 position, int turnRight)
    {
        var offset = ((Vector3)(Vector3Int)Rotate90(direction, turnRight)) * (grid.baseGrid.cellSize.magnitude / 2f);
        var pos = grid.baseGrid.WorldToCell(position + offset);
        return pos;
    }

    // ���݂̃Z����I��
    private void SelectCurrentCell(Vector3Int cell)
    {
        // �p�ɓ��B�����^�C����ۑ�
        currentTile = grid.baseTilemap.GetTile(cell);
        // ����Ă���^�C���̉��I������
        grid.SelectCluster(currentTile, cell);
    }

    // Start is called before the first frame update
    void Start()
    {
        // ���݂̃^�C����I��
        Vector3Int currentCell = GetSideCell(transform.position, turnRight);
        SelectCurrentCell(currentCell);
    }

    // Update is called once per frame
    void Update()
    {
        // �V�����ʒu
        var oldPos = transform.position;
        var newPos = oldPos + new Vector3(direction.x, direction.y, 0) * (Time.deltaTime * speed);

        // �����A�O��
        Vector3Int innerCell = GetSideCell(newPos, turnRight);
        TileBase innerTile = grid.baseTilemap.GetTile(innerCell);
        Vector3Int outerCell = GetSideCell(newPos, -turnRight);
        TileBase outerTile = grid.baseTilemap.GetTile(outerCell);

        // �����̊p�ɓ��B������
        if (currentTile != innerTile)
        {
            // 90�x������ς���
            direction = Rotate90(direction, turnRight);
            // �����Ă��Ȃ��������}�X�ɃX�i�b�v����
            newPos = grid.SnapToGrid(newPos);
        }
        // �O���̊p�ɓ��B������
        else if (currentTile == outerTile)
        {
            // 90�x������ς���
            direction = Rotate90(direction, -turnRight);
            // �����Ă��Ȃ��������}�X�ɃX�i�b�v����
            newPos = grid.SnapToGrid(newPos);
        }
        else
        {
            // �����ɐ���ǉ�����
            line.DrawLine((Vector2Int)innerCell, Rotate90(direction, -turnRight));
        }

        // �X�y�[�X�L�[��������
        if (Input.GetButtonDown("Jump"))
        {
            if (outerTile != null)
            {
                // ��]�𔽓]
                turnRight *= -1;
                // ���݂̃^�C����I��
                SelectCurrentCell(outerCell);
            }
        }

        // �V�����ʒu�𔽉f
        transform.position = newPos;
    }
}
