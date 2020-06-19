using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OceloController_mono : MonoBehaviour
{
    public OceloController _myoceloCtrl { get; private set; }
    [SerializeField] int _myPlNum;
    [SerializeField, NonEditable] GameControllData.PlayerColor _myColor;
    [SerializeField,Space(10)] OceloBanDisplayer _disp;
    [SerializeField] OceloDataChannelReciever _dataReciever;

    [SerializeField] PlayerSetter _myPl;
    [SerializeField] PlayerSetter _enemyPl;

    [SerializeField] GameObject whiteTurn;
    [SerializeField] GameObject blackTurn;
    [SerializeField] Text _userTypeDisplay;
    [SerializeField] Text _winText;

    //[SerializeField]OceloPlayer _mypl;

    void Start()
    {
        _myoceloCtrl = new OceloController();

        _myoceloCtrl._callback_display = () =>
        {
            _disp.SyncKoma(_myoceloCtrl._BanData);
        };
        _myoceloCtrl._callback_gameStart = GameStart;
        _myoceloCtrl._callback_plChenge = SetTurnGuid;
        _myoceloCtrl._callback_skipTurn = SetTurnGuid;
        _myoceloCtrl._callback_endGame = EndGame;
        _disp.Init();
        _disp._callback_masuclick = Onclick_putKoma;
    }
    #region callback
    void GameStart()
    {
        _myColor = (_myoceloCtrl._processData._PlayerData[0]._PlNumber == _myPlNum) ? _myoceloCtrl._processData._PlayerData[0]._PlColor : _myoceloCtrl._processData._PlayerData[1]._PlColor;
        //プレイヤーの設定
        _myPl._MyColor = _myColor;
        _enemyPl._MyColor = GameControllData.GetOtherColor(_myColor);
        _myoceloCtrl._oceloPlayer.Add(_myPl.CreatePlayer());
        _myoceloCtrl._oceloPlayer.Add(_enemyPl.CreatePlayer());
        
        var plTypeText = (_myColor ==  GameControllData.PlayerColor.BLACK) ? "黒" : "白";
        _userTypeDisplay.text = $"あなたは{plTypeText}です";
    }

     void Onclick_putKoma(Vector2Int pos)
    {
        _myPl.Onclick_putKoma(_myoceloCtrl, pos);
        _enemyPl.Onclick_putKoma(_myoceloCtrl, pos);
    }

    void SetTurnGuid()
    {
        if(_myoceloCtrl._NowPlType==  GameControllData.PlayerColor.BLACK)
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

    void EndGame(GameControllData.PlayerColor winType)
    {
        _winText.gameObject.SetActive(true);
        var plTypeText = (winType == GameControllData.PlayerColor.BLACK) ? "黒" : "白";
        _winText.text = $"{plTypeText}の勝利です";
    }
    #endregion

    [ContextMenu("startGame")]
    void StartGame()
    {
        _myoceloCtrl.Init();
        _myoceloCtrl.TurnAction();
        _myoceloCtrl.TurnAction();
        _myoceloCtrl.TurnAction();
    }
}
