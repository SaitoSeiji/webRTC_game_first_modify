using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OceloController_mono : MonoBehaviour
{
    public OceloController _myoceloCtrl { get; private set; }
    [SerializeField] GameSetting _gameSetting;
    [SerializeField] public int _myPlNum;
    [SerializeField, NonEditable] GameControllData.PlayerColor _myColor;
    [SerializeField,Space(10)] OceloBanDisplayer _disp;
    [SerializeField] OceloDataChannelReciever _dataReciever;

    [SerializeField] PlayerSetter _myPl;
    [SerializeField] PlayerSetter _enemyPl;

    [SerializeField] GameObject whiteTurn;
    [SerializeField] GameObject blackTurn;
    [SerializeField] Text _userTypeDisplay;
    [SerializeField] Text _winText;
    [SerializeField] Text _skipText;

    //[SerializeField]OceloPlayer _mypl;

    void Start()
    {
        if(_gameSetting._gameStartType== GameSetting.GameStartType.Remote)
        {
            _myoceloCtrl = new RemoteOceloController(_dataReciever);
        }
        else
        {
            _myoceloCtrl = new LocalOceloController();
        }

        _myoceloCtrl._callback_syncBanData = () =>
        {
            _disp.SyncKoma(_myoceloCtrl._BanData);
        };
        _myoceloCtrl._callback_gameStart = GameStart;
        _myoceloCtrl._callback_syncGameProcess = SetTurnGuid;
        _myoceloCtrl._callback_skipTurn = SkipGuid;
        _myoceloCtrl._callback_endGame = EndGame;
        _disp.Init();
        _disp._callback_masuclick = Onclick_putKoma;

        if(_gameSetting._gameStartType== GameSetting.GameStartType.Awake)
        {
            _myoceloCtrl.PrepareGame();
            _myoceloCtrl.StartGame();
        }
    }
    #region callback
    void GameStart()
    {
        _myColor = (_myoceloCtrl._processData._PlayerData[0]._PlNumber == _myPlNum) ? _myoceloCtrl._processData._PlayerData[0]._PlColor : _myoceloCtrl._processData._PlayerData[1]._PlColor;
        //プレイヤーの設定
        _myPl._MyColor = _myColor;
        if (_gameSetting._pltype_1p != PlayerSetter.PlType.None) _myPl._myPlType = _gameSetting._pltype_1p;
        _enemyPl._MyColor = GameControllData.GetOtherColor(_myColor);
        if (_gameSetting._pltype_2p != PlayerSetter.PlType.None) _enemyPl._myPlType = _gameSetting._pltype_2p;
        _myoceloCtrl._oceloPlayer.Add(_myPl.CreatePlayer());
        _myoceloCtrl._oceloPlayer.Add(_enemyPl.CreatePlayer());
        
        var plTypeText = (_myColor ==  GameControllData.PlayerColor.BLACK) ? "黒" : "白";
        _userTypeDisplay.text = $"あなたは{plTypeText}です";
        
        SetTurnGuid();
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

    void SkipGuid()
    {
        _skipText.gameObject.SetActive(true);
        var plTypeText = (_myoceloCtrl._NowPlType == GameControllData.PlayerColor.BLACK) ? "黒" : "白";
        _skipText.text = $"{plTypeText}は置く場所がないのでスキップします";
        WaitAction.Instance.CoalWaitAction(() => _skipText.gameObject.SetActive(false), 1);
    }

    void EndGame(GameControllData.PlayerColor winType)
    {
        _winText.gameObject.SetActive(true);
        var plTypeText = (winType == GameControllData.PlayerColor.BLACK) ? "黒" : "白";
        _winText.text = $"{plTypeText}の勝利です";
    }
    #endregion
    
}
