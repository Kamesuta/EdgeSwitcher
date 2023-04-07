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
    private Dictionary<KeyCode, Player> players = new();
    private bool isPlayerAdd = true;
    public GameObject playerJoinEnabled;

    // プレイヤーの色候補
    private readonly Color[] colors =
    {
        new Color32(0x41, 0x53, 0xA1, 0xFF), // Dark Blue
        new Color32(0xCD, 0x2E, 0x55, 0xFF), // Wine Red
        new Color32(0x13, 0x95, 0x5F, 0xFF), // Forest Green
        new Color32(0xC5, 0x29, 0x9B, 0xFF), // Purple
    };

    // Start is called before the first frame update
    void Start()
    {
        grid = GetComponentInParent<GridSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        // プレイヤー追加モード切り替え
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPlayerAdd = !isPlayerAdd;
            playerJoinEnabled.SetActive(isPlayerAdd);
        }

        // 全プレイヤーを操作
        foreach (var (keycode, player) in players)
        {
            // キー入力があったら反転
            if (Input.GetKeyDown(keycode))
            {
                player.Flip();
            }
        }

        // プレイヤー追加モード
        if (isPlayerAdd)
        {
            // 何かのキーが押下されたら
            if (Input.anyKeyDown)
            {
                foreach (KeyCode keycode in System.Enum.GetValues(typeof(KeyCode)))
                {
                    // Escapeキーは除外
                    if (keycode == KeyCode.Escape) continue;

                    if (Input.GetKeyDown(keycode) && !players.ContainsKey(keycode))
                    {
                        // プレイヤーを作成/登録
                        players.Add(keycode, AddPlayer());
                        // プレイヤー追加モードを終了
                        isPlayerAdd = false;
                        playerJoinEnabled.SetActive(isPlayerAdd);
                    }
                }
            }
        }
    }

    public Player AddPlayer()
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

        return player;
    }
}
