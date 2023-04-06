using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class PlayerManager : MonoBehaviour
{
    public GameObject playerPrefab;
    private GridSystem grid;
    public PlayerScoreManager scoreManager;
    public GameObject scorePrefab;

    // 開始時にスポーンするプレイヤー数
    public int playerNum;

    // プレイヤーの色候補
    private readonly Color[] colors =
    {
        new Color32(0xCD, 0x2E, 0x55, 0xFF), // Wine Red
        new Color32(0xFC, 0xC0, 0x10, 0xFF), // Sun Yellow
        new Color32(0x41, 0x53, 0xA1, 0xFF), // Dark Blue
        new Color32(0x13, 0x95, 0x5F, 0xFF), // Forest Green
    };

    // Start is called before the first frame update
    void Start()
    {
        grid = GetComponentInParent<GridSystem>();

        // プレイヤーを生成
        for (var i = 0; i < playerNum; i++)
        {
            AddPlayer();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddPlayer()
    {
        // プレイヤーNo
        var index = transform.childCount;
        // プレイヤーカラー
        var color = index < colors.Length
            ? colors[index]
            : Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);

        // UIを追加
        var scoreObj = Instantiate(scorePrefab, scoreManager.transform);
        var score = scoreObj.GetComponent<PlayerScore>();
        score.SetPlayerColor(color);
        score.playerNumber = index + 1;
        scoreManager.SortScores();

        // プレイヤーを配置する場所候補
        var bounds = new BoundsInt(grid.backgroundTilemap.origin, grid.backgroundTilemap.size);
        var availablePositions = new List<Vector3Int>();
        foreach (var pos in bounds.allPositionsWithin)
        {
            if (grid.backgroundTilemap.GetTile(pos) != null)
            {
                availablePositions.Add(pos);
            }
        }

        // プレイヤーを配置する場所をランダムに決定
        var cell = availablePositions[Random.Range(0, availablePositions.Count)];
        var position = grid.baseGrid.CellToWorld(cell);

        // プレイヤーを生成
        var playerObj = Instantiate(playerPrefab, transform);
        var player = playerObj.GetComponentInChildren<Player>();
        player.color = color;
        player.transform.position = position;
        player.score = score;
    }
}
