using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

// プレイヤー
public class Player : MonoBehaviour, LineGridSystem.IPartialTilemap
{
    // グリッド
    private GridSystem grid;

    // 速度
    public float speed = 1.0f;

    // ユーザーのペイントセット
    public PaintTileSet paint;
    // 色
    public Color color;
    // 色付きペイントセット
    private PaintTileSet coloredPaint;

    // 途中タイルマップ
    [field: SerializeField]
    public Tilemap PartialTilemap { get; set; }

    // 途中タイルマップマスク
    [field: SerializeField]
    public Transform PartialMask { get; set; }

    // オーバーレイタイルマップ
    public Tilemap overlayTilemap;

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

    // 現在のセルを選択
    private void SelectCurrentCell(Vector3Int cell)
    {
        // 角に到達したタイルを保存
        currentTile = grid.baseTilemap.GetTile(cell);
        // 乗っているタイルの塊を選択する
        grid.SelectCluster(overlayTilemap, cell, currentTile, coloredPaint);
    }

    // Start is called before the first frame update
    void Start()
    {
        // グリッド取得
        grid = GetComponentInParent<GridSystem>();

        // 色付きペイントセットを作成
        coloredPaint = paint.CreateColored(color);

        // 現在のタイルを選択
        Vector2Int nowInnerSideDirection = grid.Rotate90(moveDirection, turnRight);
        Vector3Int nowInnerCell = grid.GetSideCell(transform.position, nowInnerSideDirection);
        SelectCurrentCell(nowInnerCell);
    }

    // Update is called once per frame
    void Update()
    {
        // 現在のフレーム
        var nowPos = transform.position;
        Vector2Int nowInnerSideDirection = grid.Rotate90(moveDirection, turnRight);
        Vector3Int nowInnerCell = grid.GetSideCell(nowPos, nowInnerSideDirection);

        // 次回のフレーム
        var pos = nowPos + new Vector3(moveDirection.x, moveDirection.y, 0) * (Time.deltaTime * speed);
        Vector2Int innerSideDirection = grid.Rotate90(moveDirection, turnRight);
        Vector2Int outerSideDirection = grid.Rotate90(moveDirection, -turnRight);
        Vector3Int innerCell = grid.GetSideCell(pos, innerSideDirection);
        Vector3Int outerCell = grid.GetSideCell(pos, outerSideDirection);
        TileBase innerTile = grid.baseTilemap.GetTile(innerCell);
        TileBase outerTile = grid.baseTilemap.GetTile(outerCell);

        // 新しいマスに移動しているか
        if (!isTurn && grid.baseGrid.WorldToCell(nowPos) != grid.baseGrid.WorldToCell(pos))
        {
            // 残り続ける線を追加
            grid.line.DrawLine(coloredPaint, (Vector2Int)nowInnerCell, moveDirection, outerSideDirection);

            // 基準点を設定
            basePosition = grid.SnapToGrid(pos);
        }
        else
        {
            // 距離を％に変換
            float cellSize = moveDirection.x != 0 ? grid.baseGrid.cellSize.x : grid.baseGrid.cellSize.y;
            float partialPercent = Vector3.Distance(pos, basePosition) / cellSize;

            // 部分的に線を追加
            grid.line.DrawLinePartial(this, coloredPaint, (Vector2Int)innerCell, moveDirection, outerSideDirection, partialPercent);
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
