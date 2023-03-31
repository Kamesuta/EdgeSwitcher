using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "PaintTileSet")]
public class PaintTileSet : ScriptableObject
{
    // ���^�C��
    public Tile lineHorizontal;

    // �c�^�C��
    public Tile lineVertical;

    // �I�[�o�[���C�^�C��
    public Tile overlay;

    // �F�t��
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
