using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// プレイヤーのスコア
public class PlayerScore : MonoBehaviour
{
    // スコアテキスト
    public TextMeshProUGUI scoreText;
    // プレイヤー名テキスト
    public TextMeshProUGUI playerNameText;
    // 背景イメージ
    public Image backImage;
    // 現在のスコア
    public int score;
    // プレイヤーナンバー
    public int playerNumber;

    // Start is called before the first frame update
    void Start()
    {
        SetScore(score);
    }

    // 得点をセット
    public void SetScore(int value)
    {
        score = value;
        scoreText.text = $"+{score}";
    }

    // 得点を加算
    public void AddScore(int value)
    {
        score += value;
        scoreText.text = $"+{score}";
    }

    // プレイヤー色を設定
    public void SetPlayerColor(Color color)
    {
        backImage.color = color;
        scoreText.color = color;
        playerNameText.color = color;
    }

    // プレイヤーの番号を設定
    public void SetPlayerNumber(int number)
    {
        playerNumber = number;
        playerNameText.text = $"{playerNumber}P";
    }
}
