using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OceloController_mono : MonoBehaviour
{
    [SerializeField]OceloController _myoceloCtrl;
    [SerializeField] OceloBanDisplayer _disp;

    [SerializeField] GameObject whiteTurn;
    [SerializeField] GameObject blackTurn;

    [SerializeField]OceloPlayer_input _mypl;

    void Start()
    {
        _myoceloCtrl = new OceloController();
        _myoceloCtrl._callback_display = () =>
        {
            _disp.SyncKoma(_myoceloCtrl._MyBan);
        };
        _myoceloCtrl._callback_plChenge = SetTurnGuid;
    }

    [ContextMenu("setpl")]
    void RandomSetPl()
    {
        var rand=Random.Range(0, 2);
        if (rand == 0) _myoceloCtrl.SetMyPl(Koma_ocelo.Type.Black);
        else _myoceloCtrl.SetMyPl(Koma_ocelo.Type.White);
    }

    [ContextMenu("gamestart")]
    void Onclick_gameStart()
    {
        _disp.Init();
        _disp._callback_masuclick = Onclick_putKoma;
        _mypl = _myoceloCtrl.myPl;
        _myoceloCtrl._MyBan.SetMasu(new Koma_ocelo(Koma_ocelo.Type.White), new Vector2Int(3, 3));
        _myoceloCtrl._MyBan.SetMasu(new Koma_ocelo(Koma_ocelo.Type.Black), new Vector2Int(3, 4));
        _myoceloCtrl._MyBan.SetMasu(new Koma_ocelo(Koma_ocelo.Type.Black), new Vector2Int(4, 3));
        _myoceloCtrl._MyBan.SetMasu(new Koma_ocelo(Koma_ocelo.Type.White), new Vector2Int(4, 4));
        _myoceloCtrl.Action();
        _myoceloCtrl.Action();
        _myoceloCtrl.Action();
    }

    public void Onclick_putKoma(Vector2Int pos)
    {
        _myoceloCtrl.SetKoma(pos,_mypl);
        _myoceloCtrl.Action();
        _myoceloCtrl.Action();
        _myoceloCtrl.Action();
    }
    
    void SetTurnGuid()
    {
        if(_myoceloCtrl._NowPlType== Koma_ocelo.Type.Black)
        {
            blackTurn.SetActive(true);
            whiteTurn.SetActive(false);
        }
        else
        {
            blackTurn.SetActive(false);
            whiteTurn.SetActive(true);
        }
    }
}
