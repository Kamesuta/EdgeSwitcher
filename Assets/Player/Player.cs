using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// �v���C���[
public class Player : MonoBehaviour
{
    // �O���b�h
    [SerializeField, InspectorName("�O���b�h")]
    private GridSystem grid;

    // ���x
    [SerializeField, InspectorName("���x")]
    private float speed = 1.0f;

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

        // �O���̊p�ɓ��B������
        TileBase outerTile = grid.baseTilemap.GetTile(GetSideCell(newPos, turnRight));
        if (currentTile != outerTile)
        {
            // 90�x������ς���
            direction = Rotate90(direction, turnRight);
            // �����Ă��Ȃ��������}�X�ɃX�i�b�v����
            newPos = grid.SnapToGrid(newPos);
        }
        // �����̊p�ɓ��B������
        Vector3Int innerCell = GetSideCell(newPos, -turnRight);
        TileBase innerTile = grid.baseTilemap.GetTile(innerCell);
        if (currentTile == innerTile)
        {
            // 90�x������ς���
            direction = Rotate90(direction, -turnRight);
            // �����Ă��Ȃ��������}�X�ɃX�i�b�v����
            newPos = grid.SnapToGrid(newPos);
        }

        // �X�y�[�X�L�[��������
        if (Input.GetButtonDown("Jump"))
        {
            if (innerTile != null)
            {
                // ��]�𔽓]
                turnRight *= -1;
                // ���݂̃^�C����I��
                SelectCurrentCell(innerCell);
            }
        }

        // �V�����ʒu�𔽉f
        transform.position = newPos;
    }
}
