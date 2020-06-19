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

    public void SetSprite(GameControllData.PlayerColor komaType)
    {
        if (komaType == GameControllData.PlayerColor.NONE) _myrenderer.sprite = _clear;
        else if (komaType == GameControllData.PlayerColor.BLACK) _myrenderer.sprite = _black;
        else if (komaType == GameControllData.PlayerColor.WHITE) _myrenderer.sprite = _white;
    }

    //[ContextMenu("test_black")]
    //public void Test_SetBlack()
    //{
    //    SetSprite(new Koma_ocelo(Koma_ocelo.KomaType.Black));
    //}
    //[ContextMenu("test_white")]
    //void Test_SetWhite()
    //{
    //    SetSprite(new Koma_ocelo(Koma_ocelo.KomaType.White));
    //}
    //[ContextMenu("test_clear")]
    //void Test_SetClear()
    //{
    //    SetSprite(null);
    //}
}
