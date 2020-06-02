using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OceloController_mono : MonoBehaviour
{
    [SerializeField]public OceloController _myoceloCtrl;
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
        _myoceloCtrl._callback_gameStart = GameStart;
        _disp.Init();
        _disp._callback_masuclick = Onclick_putKoma;
    }
    #region callback
    void GameStart()
    {
        _mypl = _myoceloCtrl.myPl;
    }

    public void Onclick_putKoma(Vector2Int pos)
    {
        _mypl.SetKoma(pos);
        //_myoceloCtrl.SetKoma(pos,_mypl);
        //_myoceloCtrl.Action();
        //_myoceloCtrl.Action();
        //_myoceloCtrl.Action();
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
    #endregion
}
