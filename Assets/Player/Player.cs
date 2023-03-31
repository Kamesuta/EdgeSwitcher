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
    private Vector2Int moveDirection = Vector2Int.right;
    // ��]����
    private int turnRight = 1;
    // �������ς������
    private bool isTurn = false;
    // ��_
    private Vector3 basePosition;

    // direction����]����
    private Vector2Int Rotate90(Vector2Int direction, int turnRight)
    {
        // 90�x��]������
        return new Vector2Int(direction.y, -direction.x) * turnRight;
    }

    // �T�C�h�̃^�C�����擾����
    private Vector3Int GetSideCell(Vector3 position, int turnRight)
    {
        var offset = ((Vector3)(Vector3Int)Rotate90(moveDirection, turnRight)) * (grid.baseGrid.cellSize.magnitude / 2f);
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
        // ���݂̃t���[��
        var nowPos = transform.position;
        Vector3Int nowInnerCell = GetSideCell(nowPos, turnRight);

        // ����̃t���[��
        var pos = nowPos + new Vector3(moveDirection.x, moveDirection.y, 0) * (Time.deltaTime * speed);
        Vector3Int innerCell = GetSideCell(pos, turnRight);
        Vector3Int outerCell = GetSideCell(pos, -turnRight);
        TileBase innerTile = grid.baseTilemap.GetTile(innerCell);
        TileBase outerTile = grid.baseTilemap.GetTile(outerCell);
        Vector2Int innerSideDirection = Rotate90(moveDirection, turnRight);
        Vector2Int outerSideDirection = Rotate90(moveDirection, -turnRight);

        // �V�����}�X�Ɉړ����Ă��邩
        if (!isTurn && grid.baseGrid.WorldToCell(nowPos) != grid.baseGrid.WorldToCell(pos))
        {
            // �c�葱�������ǉ�
            line.DrawLine((Vector2Int)nowInnerCell, moveDirection, outerSideDirection);

            // ��_��ݒ�
            basePosition = grid.SnapToGrid(pos);
        }
        else
        {
            // ���������ɕϊ�
            float cellSize = moveDirection.x != 0 ? grid.baseGrid.cellSize.x : grid.baseGrid.cellSize.y;
            float partialPercent = Vector3.Distance(pos, basePosition) / cellSize;

            // �����I�ɐ���ǉ�
            line.DrawLinePartial((Vector2Int)innerCell, moveDirection, outerSideDirection, partialPercent);
        }

        // �����̊p�ɓ��B������
        isTurn = false;
        if (currentTile != innerTile)
        {
            // 90�x������ς���
            moveDirection = innerSideDirection;
            // �����Ă��Ȃ��������}�X�ɃX�i�b�v����
            pos = grid.SnapToGrid(pos);
            // �������ς����
            isTurn = true;
        }
        // �O���̊p�ɓ��B������
        else if (currentTile == outerTile)
        {
            // 90�x������ς���
            moveDirection = outerSideDirection;
            // �����Ă��Ȃ��������}�X�ɃX�i�b�v����
            pos = grid.SnapToGrid(pos);
            // �������ς����
            isTurn = true;
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
        transform.position = pos;
    }
}
