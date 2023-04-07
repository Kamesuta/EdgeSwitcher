using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(GridSystem))]
public class GoalDetector : MonoBehaviour
{
    private GridSystem grid;
    public TextMeshProUGUI winner;
    public PlayerManager playerManager;

    // Start is called before the first frame update
    void Start()
    {
        grid = GetComponent<GridSystem>();        
    }

    // Update is called once per frame
    void Update()
    {
        // タイルがすべて取得されたら
        if (grid.foregroundTilemap.GetUsedTilesCount() == 0)
        {
            // 勝者を計算
            var player = playerManager.players.Values.MaxBy(e => e.score.score);

            // 勝者を表示
            winner.text = $"Winner is {player.score.playerNameText.text}";
            winner.transform.parent.gameObject.SetActive(true);
        }
    }
}
