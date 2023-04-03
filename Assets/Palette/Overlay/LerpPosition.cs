using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LerpPosition : MonoBehaviour
{
    // 開始位置
    public Vector3 start;
    // 終了位置
    public Vector3 end;
    // Lerp補間する値
    public float value = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(start, end, value);
    }

    // 破棄
    public void Destroy()
    {
        Destroy(gameObject);
    }
}
