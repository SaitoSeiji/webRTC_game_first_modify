using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class OceloBanDisplayer : MonoBehaviour
{
    [SerializeField] RectTransform _trParent;
    OceloMasuDisplayer[][] _masus;
    public Action<Vector2Int> _callback_masuclick;
    
    void Onclick(Vector2Int pos)
    {
        Debug.Log($"click:{pos}");
        _callback_masuclick?.Invoke(pos);
    }

    [ContextMenu("init")]
    public void Init()
    {
        _masus = new OceloMasuDisplayer[8][];
        for (int x = 0; x < 8; x++)
        {
            _masus[x] = new OceloMasuDisplayer[8];
        }

        int count = 0;
        foreach(Transform tr in _trParent)
        {
            var bt = tr.GetComponent<Button>();
            var pos = GetGrid(count);
            bt.onClick.AddListener(()=>Onclick(new Vector2Int(pos.x,pos.y)));
            var masu = tr.GetComponent<OceloMasuDisplayer>();
            _masus[pos.x][pos.y] = masu;

            count++;
        }
    }

    public void SyncKoma(Ban<Koma_ocelo> ban)
    {
        for(int x = 0; x < 8; x++)
        {
            for(int y = 0; y < 8; y++)
            {
                _masus[x][y].SetSprite(ban.GetKoma(new Vector2Int(x, y)));
            }
        }
    }

    (int x,int y)GetGrid(int count)
    {
        var x = count % 8;
        var y=count/8;
        return (x, y);
    }
}
