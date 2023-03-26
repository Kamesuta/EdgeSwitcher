using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// プレイヤー
public class Player : MonoBehaviour
{
    // グリッド
    [SerializeField, InspectorName("グリッド")]
    private GridSystem grid;

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

    // サイドのタイルを取得する
    private Vector3Int GetSideCell(Vector3 position, int turnRight)
    {
        var offset = ((Vector3)(Vector3Int)Rotate90(direction, turnRight)) * (grid.baseGrid.cellSize.magnitude / 2f);
        var pos = grid.baseGrid.WorldToCell(position + offset);
        return pos;
    }

    // 現在のセルを選択
    private void SelectCurrentCell(Vector3Int cell)
    {
        // 角に到達したタイルを保存
        currentTile = grid.baseTilemap.GetTile(cell);
        // 乗っているタイルの塊を選択する
        grid.SelectCluster(currentTile, cell);
    }

    // Start is called before the first frame update
    void Start()
    {
        // 現在のタイルを選択
        Vector3Int currentCell = GetSideCell(transform.position, turnRight);
        SelectCurrentCell(currentCell);
    }

    // Update is called once per frame
    void Update()
    {
        // 新しい位置
        var oldPos = transform.position;
        var newPos = oldPos + new Vector3(direction.x, direction.y, 0) * (Time.deltaTime * speed);

        // 外側の角に到達したら
        TileBase outerTile = grid.baseTilemap.GetTile(GetSideCell(newPos, turnRight));
        if (currentTile != outerTile)
        {
            // 90度向きを変える
            direction = Rotate90(direction, turnRight);
            // 向いていない方向をマスにスナップする
            newPos = grid.SnapToGrid(newPos);
        }
        // 内側の角に到達したら
        Vector3Int innerCell = GetSideCell(newPos, -turnRight);
        TileBase innerTile = grid.baseTilemap.GetTile(innerCell);
        if (currentTile == innerTile)
        {
            // 90度向きを変える
            direction = Rotate90(direction, -turnRight);
            // 向いていない方向をマスにスナップする
            newPos = grid.SnapToGrid(newPos);
        }

        // スペースキー押したら
        if (Input.GetButtonDown("Jump"))
        {
            if (innerTile != null)
            {
                // 回転を反転
                turnRight *= -1;
                // 現在のタイルを選択
                SelectCurrentCell(innerCell);
            }
        }

        // 新しい位置を反映
        transform.position = newPos;
    }
}
