using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OceloController_mono : MonoBehaviour
{
    [SerializeField]public OceloController _myoceloCtrl;
    [SerializeField] OceloBanDisplayer _disp;
    [SerializeField] OceloDataChannelReciever _dataReciever;

    [SerializeField] GameObject whiteTurn;
    [SerializeField] GameObject blackTurn;
    [SerializeField] Text _userTypeDisplay;
    [SerializeField] Text _winText;

    [SerializeField]OceloPlayer _mypl;

    void Start()
    {
        _myoceloCtrl = new OceloController(_dataReciever);
        _myoceloCtrl._callback_display = () =>
        {
            _disp.SyncKoma(_myoceloCtrl._MyBan);
        };
        _myoceloCtrl._callback_plChenge = SetTurnGuid;
        _myoceloCtrl._callback_skipTurn = SetTurnGuid;
        _myoceloCtrl._callback_gameStart = GameStart;
        _myoceloCtrl._callback_endGame = EndGame;
        _disp.Init();
        _disp._callback_masuclick = Onclick_putKoma;
    }
    #region callback
    void GameStart()
    {
        _mypl = _myoceloCtrl.myPl;
        var plTypeText = (_mypl._MyPlType == Koma_ocelo.KomaType.Black) ? "黒" : "白";
        _userTypeDisplay.text = $"あなたは{plTypeText}です";
    }

    public void Onclick_putKoma(Vector2Int pos)
    {
        if (!(_mypl is IKomaPut)) return;
        var inputer = _mypl as IKomaPut;
        inputer.SetKoma(pos);
    }
    
    void SetTurnGuid()
    {
        if(_myoceloCtrl._NowPlType== Koma_ocelo.KomaType.Black)
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

    void EndGame(Koma_ocelo.KomaType winType)
    {
        _winText.gameObject.SetActive(true);
        var plTypeText = (winType == Koma_ocelo.KomaType.Black) ? "黒" : "白";
        _winText.text = $"{plTypeText}の勝利です";
    }
    #endregion
}
