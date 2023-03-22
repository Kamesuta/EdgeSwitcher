using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{
    // グリッド
    [SerializeField, InspectorName("グリッド")]
    private Grid baseGrid;

    // タイルマップ
    [SerializeField, InspectorName("タイルマップ")]
    private Tilemap baseTilemap;

    // 速度
    [SerializeField, InspectorName("速度")]
    private float speed = 1.0f;

    // 現在のタイル
    private TileBase currentTile;
    // 現在の向き
    private Vector2Int direction = Vector2Int.right;
    // 回転方向
    private int turnRight = 1;

    // directionを回転する
    private Vector2Int Rotate90(Vector2Int direction, int turnRight)
    {
        // 90度回転させる
        return new Vector2Int(direction.y, -direction.x) * turnRight;
    }

    // 向いていない向きをマスにスナップする
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

    // サイドのタイルを取得する
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
        // 現在のタイルを保存
        currentTile = GetSideTile(transform.position, turnRight);
    }

    // Update is called once per frame
    void Update()
    {
        // 新しい位置
        var oldPos = transform.position;
        var newPos = oldPos + new Vector3(direction.x, direction.y, 0) * (Time.deltaTime * speed);

        // 内側の角に到達したら
        TileBase innerTile = GetSideTile(newPos, turnRight);
        if (currentTile != innerTile)
        {
            // 90度向きを変える
            direction = Rotate90(direction, turnRight);
            // 向いていない方向をマスにスナップする
            newPos = SnapToGrid(newPos, direction);
        }
        // 外側の角に到達したら
        TileBase outerTile = GetSideTile(newPos, -turnRight);
        if (currentTile == outerTile)
        {
            // 90度向きを変える
            direction = Rotate90(direction, -turnRight);
            // 向いていない方向をマスにスナップする
            newPos = SnapToGrid(newPos, direction);
        }

        // スペースキー押したら
        if (Input.GetButtonDown("Jump"))
        {
            if (outerTile != null)
            {
                // 回転を反転
                turnRight *= -1;
                // 角に到達したタイルを保存
                currentTile = outerTile;
            }
        }

        // 新しい位置を反映
        transform.position = newPos;
    }
}
