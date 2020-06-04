using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OceloMasuDisplayer : MonoBehaviour
{
    [SerializeField] Sprite _black;
    [SerializeField] Sprite _white;
    [SerializeField] Sprite _clear;
    Image _myrenderer;

    private void Awake()
    {
        _myrenderer = GetComponent<Image>();
    }

    public void SetSprite(Koma_ocelo koma)
    {
        if (koma == null) _myrenderer.sprite = _clear;
        else if (koma._type == Koma_ocelo.Type.Black) _myrenderer.sprite = _black;
        else if (koma._type == Koma_ocelo.Type.White) _myrenderer.sprite = _white;
    }

    [ContextMenu("test_black")]
    public void Test_SetBlack()
    {
        SetSprite(new Koma_ocelo(Koma_ocelo.Type.Black));
    }
    [ContextMenu("test_white")]
    void Test_SetWhite()
    {
        SetSprite(new Koma_ocelo(Koma_ocelo.Type.White));
    }
    [ContextMenu("test_clear")]
    void Test_SetClear()
    {
        SetSprite(null);
    }
}
