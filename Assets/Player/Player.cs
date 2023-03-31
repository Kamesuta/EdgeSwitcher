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
    private Vector2Int moveDirection = Vector2Int.right;
    // 回転方向
    private int turnRight = 1;
    // 向きが変わったか
    private bool isTurn = false;
    // 基準点
    private Vector3 basePosition;

    // directionを回転する
    private Vector2Int Rotate90(Vector2Int direction, int turnRight)
    {
        // 90度回転させる
        return new Vector2Int(direction.y, -direction.x) * turnRight;
    }

    // サイドのタイルを取得する
    private Vector3Int GetSideCell(Vector3 position, int turnRight)
    {
        var offset = ((Vector3)(Vector3Int)Rotate90(moveDirection, turnRight)) * (grid.baseGrid.cellSize.magnitude / 2f);
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
        // 現在のフレーム
        var nowPos = transform.position;
        Vector3Int nowInnerCell = GetSideCell(nowPos, turnRight);

        // 次回のフレーム
        var pos = nowPos + new Vector3(moveDirection.x, moveDirection.y, 0) * (Time.deltaTime * speed);
        Vector3Int innerCell = GetSideCell(pos, turnRight);
        Vector3Int outerCell = GetSideCell(pos, -turnRight);
        TileBase innerTile = grid.baseTilemap.GetTile(innerCell);
        TileBase outerTile = grid.baseTilemap.GetTile(outerCell);
        Vector2Int innerSideDirection = Rotate90(moveDirection, turnRight);
        Vector2Int outerSideDirection = Rotate90(moveDirection, -turnRight);

        // 新しいマスに移動しているか
        if (!isTurn && grid.baseGrid.WorldToCell(nowPos) != grid.baseGrid.WorldToCell(pos))
        {
            // 残り続ける線を追加
            line.DrawLine((Vector2Int)nowInnerCell, moveDirection, outerSideDirection);

            // 基準点を設定
            basePosition = grid.SnapToGrid(pos);
        }
        else
        {
            // 距離を％に変換
            float cellSize = moveDirection.x != 0 ? grid.baseGrid.cellSize.x : grid.baseGrid.cellSize.y;
            float partialPercent = Vector3.Distance(pos, basePosition) / cellSize;

            // 部分的に線を追加
            line.DrawLinePartial((Vector2Int)innerCell, moveDirection, outerSideDirection, partialPercent);
        }

        // 内側の角に到達したら
        isTurn = false;
        if (currentTile != innerTile)
        {
            // 90度向きを変える
            moveDirection = innerSideDirection;
            // 向いていない方向をマスにスナップする
            pos = grid.SnapToGrid(pos);
            // 向きが変わった
            isTurn = true;
        }
        // 外側の角に到達したら
        else if (currentTile == outerTile)
        {
            // 90度向きを変える
            moveDirection = outerSideDirection;
            // 向いていない方向をマスにスナップする
            pos = grid.SnapToGrid(pos);
            // 向きが変わった
            isTurn = true;
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
        transform.position = pos;
    }
}
