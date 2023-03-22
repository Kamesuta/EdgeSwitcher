using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{
    // �O���b�h
    [SerializeField, InspectorName("�O���b�h")]
    private Grid baseGrid;

    // �^�C���}�b�v
    [SerializeField, InspectorName("�^�C���}�b�v")]
    private Tilemap baseTilemap;

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

    // �����Ă��Ȃ��������}�X�ɃX�i�b�v����
    private Vector3 SnapToGrid(Vector3 position, Vector2Int direction)
    {
        if (direction.x == 0)
        {
            position.x = Mathf.Round(position.x / baseGrid.cellSize.x) * baseGrid.cellSize.x;
        }
        if (direction.y == 0)
        {
            position.y = Mathf.Round(position.y / baseGrid.cellSize.y) * baseGrid.cellSize.y;
        }
        return position;
    }

    // �T�C�h�̃^�C�����擾����
    private TileBase GetSideTile(Vector3 position, int turnRight)
    {
        var offset = ((Vector3)(Vector3Int)Rotate90(direction, turnRight)) * (baseGrid.cellSize.magnitude / 2f);
        var pos = baseGrid.WorldToCell(position + offset);
        var tile = baseTilemap.GetTile(pos);
        return tile;
    }

    // Start is called before the first frame update
    void Start()
    {
        // ���݂̃^�C����ۑ�
        currentTile = GetSideTile(transform.position, turnRight);
    }

    // Update is called once per frame
    void Update()
    {
        // �V�����ʒu
        var oldPos = transform.position;
        var newPos = oldPos + new Vector3(direction.x, direction.y, 0) * (Time.deltaTime * speed);

        // �����̊p�ɓ��B������
        TileBase innerTile = GetSideTile(newPos, turnRight);
        if (currentTile != innerTile)
        {
            // 90�x������ς���
            direction = Rotate90(direction, turnRight);
            // �����Ă��Ȃ��������}�X�ɃX�i�b�v����
            newPos = SnapToGrid(newPos, direction);
        }
        // �O���̊p�ɓ��B������
        TileBase outerTile = GetSideTile(newPos, -turnRight);
        if (currentTile == outerTile)
        {
            // 90�x������ς���
            direction = Rotate90(direction, -turnRight);
            // �����Ă��Ȃ��������}�X�ɃX�i�b�v����
            newPos = SnapToGrid(newPos, direction);
        }

        // �X�y�[�X�L�[��������
        if (Input.GetButtonDown("Jump"))
        {
            if (outerTile != null)
            {
                // ��]�𔽓]
                turnRight *= -1;
                // �p�ɓ��B�����^�C����ۑ�
                currentTile = outerTile;
            }
        }

        // �V�����ʒu�𔽉f
        transform.position = newPos;
    }
}
