using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerScoreManager : MonoBehaviour
{
    private int playerCount = 0;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // プレイヤー数が変わったときソート
        if (playerCount != transform.childCount)
        {
            SortScores();
        }
    }

    // スコアを配置する
    public void SortScores()
    {
        // ソートする
        var scores = GetComponentsInChildren<PlayerScore>();
        foreach (var (score, index) in scores.OrderByDescending(x => x.score).Select((v, i) => (v, i)))
        {
            score.transform.SetSiblingIndex(index);
        }

        // プレイヤー数を更新
        playerCount = transform.childCount;
    }
}
