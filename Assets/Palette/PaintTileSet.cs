using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "PaintTileSet")]
public class PaintTileSet : ScriptableObject
{
    // ���^�C��
    public TileBase lineHorizontal;

    // �c�^�C��
    public TileBase lineVertical;

    // �I�[�o�[���C�^�C��
    public TileBase overlay;
}
