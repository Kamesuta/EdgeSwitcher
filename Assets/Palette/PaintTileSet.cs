using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "PaintTileSet")]
public class PaintTileSet : ScriptableObject
{
    // 横タイル
    public TileBase lineHorizontal;

    // 縦タイル
    public TileBase lineVertical;

    // オーバーレイタイル
    public TileBase overlay;
}
