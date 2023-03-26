using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

// プレイヤー
public class Player : MonoBehaviour
{
    // グリッド
    public GridSystem grid;

    // ライングリッド
    public LineGridSystem line;

    // 速度
    public float speed = 1.0f;

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

        // 内側、外側
        Vector3Int innerCell = GetSideCell(newPos, turnRight);
        TileBase innerTile = grid.baseTilemap.GetTile(innerCell);
        Vector3Int outerCell = GetSideCell(newPos, -turnRight);
        TileBase outerTile = grid.baseTilemap.GetTile(outerCell);

        // 内側の角に到達したら
        if (currentTile != innerTile)
        {
            // 90度向きを変える
            direction = Rotate90(direction, turnRight);
            // 向いていない方向をマスにスナップする
            newPos = grid.SnapToGrid(newPos);
        }
        // 外側の角に到達したら
        else if (currentTile == outerTile)
        {
            // 90度向きを変える
            direction = Rotate90(direction, -turnRight);
            // 向いていない方向をマスにスナップする
            newPos = grid.SnapToGrid(newPos);
        }
        else
        {
            // 内側に線を追加する
            line.DrawLine((Vector2Int)innerCell, Rotate90(direction, -turnRight));
        }

        // スペースキー押したら
        if (Input.GetButtonDown("Jump"))
        {
            if (outerTile != null)
            {
                // 回転を反転
                turnRight *= -1;
                // 現在のタイルを選択
                SelectCurrentCell(outerCell);
            }
        }

        // 新しい位置を反映
        transform.position = newPos;
    }
}
