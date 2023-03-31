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

    // ���[�U�[�̃y�C���g�Z�b�g
    public PaintTileSet paint;

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

    // ���݂̃Z����I��
    private void SelectCurrentCell(Vector3Int cell)
    {
        // �p�ɓ��B�����^�C����ۑ�
        currentTile = grid.baseTilemap.GetTile(cell);
        // ����Ă���^�C���̉��I������
        grid.SelectCluster(cell, currentTile, paint);
    }

    // Start is called before the first frame update
    void Start()
    {
        // ���݂̃^�C����I��
        Vector2Int nowInnerSideDirection = grid.Rotate90(moveDirection, turnRight);
        Vector3Int nowInnerCell = grid.GetSideCell(transform.position, nowInnerSideDirection);
        SelectCurrentCell(nowInnerCell);
    }

    // Update is called once per frame
    void Update()
    {
        // ���݂̃t���[��
        var nowPos = transform.position;
        Vector2Int nowInnerSideDirection = grid.Rotate90(moveDirection, turnRight);
        Vector3Int nowInnerCell = grid.GetSideCell(nowPos, nowInnerSideDirection);

        // ����̃t���[��
        var pos = nowPos + new Vector3(moveDirection.x, moveDirection.y, 0) * (Time.deltaTime * speed);
        Vector2Int innerSideDirection = grid.Rotate90(moveDirection, turnRight);
        Vector2Int outerSideDirection = grid.Rotate90(moveDirection, -turnRight);
        Vector3Int innerCell = grid.GetSideCell(pos, innerSideDirection);
        Vector3Int outerCell = grid.GetSideCell(pos, outerSideDirection);
        TileBase innerTile = grid.baseTilemap.GetTile(innerCell);
        TileBase outerTile = grid.baseTilemap.GetTile(outerCell);

        // �V�����}�X�Ɉړ����Ă��邩
        if (!isTurn && grid.baseGrid.WorldToCell(nowPos) != grid.baseGrid.WorldToCell(pos))
        {
            // �c�葱�������ǉ�
            line.DrawLine(paint, (Vector2Int)nowInnerCell, moveDirection, outerSideDirection);

            // ��_��ݒ�
            basePosition = grid.SnapToGrid(pos);
        }
        else
        {
            // ���������ɕϊ�
            float cellSize = moveDirection.x != 0 ? grid.baseGrid.cellSize.x : grid.baseGrid.cellSize.y;
            float partialPercent = Vector3.Distance(pos, basePosition) / cellSize;

            // �����I�ɐ���ǉ�
            line.DrawLinePartial(paint, (Vector2Int)innerCell, moveDirection, outerSideDirection, partialPercent);
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
