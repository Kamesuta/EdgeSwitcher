using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CollectingTile : MonoBehaviour
{
    // 開始位置
    public Vector3 start;
    // 終了位置
    public Vector3 end;
    // Lerp補間する値
    public float value = 0.0f;

    // UI
    public PlayerScore score;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(start, end, value);
    }

    // アニメーション終了時にアニメーションから呼ばれる
    public void OnFinishAnimation()
    {
        // 破棄
        Destroy(gameObject);

        // 得点を加算
        score.AddScore(1);
    }
}
