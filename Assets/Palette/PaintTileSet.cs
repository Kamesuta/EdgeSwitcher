using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "PaintTileSet")]
public class PaintTileSet : ScriptableObject
{
    // 横タイル
    public Tile lineHorizontal;

    // 縦タイル
    public Tile lineVertical;

    // オーバーレイタイル
    public Tile overlay;

    // 色付き
    public PaintTileSet CreateColored(Color color)
    {
        var clone = CreateInstance<PaintTileSet>();
        clone.lineHorizontal = Instantiate(lineHorizontal);
        clone.lineVertical = Instantiate(lineVertical);
        clone.overlay = Instantiate(overlay);
        clone.lineHorizontal.color = color;
        clone.lineVertical.color = color;
        clone.overlay.color = color;
        return clone;
    }
}
