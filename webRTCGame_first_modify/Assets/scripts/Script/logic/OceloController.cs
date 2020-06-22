﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using NCMB;

[Serializable]
public class GameControllData
{
    [Serializable]
    public class PlayerData
    {
        [SerializeField] int plNumber;
        public int _PlNumber { get { return plNumber; } }
        [SerializeField] PlayerColor _plColor;
        public PlayerColor _PlColor { get { return _plColor; } }

        public PlayerData(int num,PlayerColor color)
        {
            plNumber = num;
            _plColor = color;
        }
    }

    public enum PlayerColor
    {
        NONE=0,
        BLACK=1,
        WHITE=2
    }
    [SerializeField] PlayerColor _activePlayerColor;
    public PlayerColor _ActivePlayerColor { get { return _activePlayerColor; } }

    [SerializeField] List<PlayerData> _playerData;
    public IReadOnlyList<PlayerData> _PlayerData { get { return _playerData; } }

    public GameControllData(PlayerColor _firstTurn)
    {
        _activePlayerColor = _firstTurn;
        SetColor(PlayerColor.BLACK, PlayerColor.WHITE);
    }

    public void SwichActivePlayer()
    {
        _activePlayerColor=GetOtherColor(_activePlayerColor);
    }

    public void SetColor(PlayerColor pl1c,PlayerColor pl2c)
    {
        _playerData = new List<PlayerData>();
        _playerData.Add(new PlayerData(1,pl1c));
        _playerData.Add(new PlayerData(2,pl2c));
    }
    #region static
    public static PlayerColor GetOtherColor(PlayerColor color)
    {
        return (color == PlayerColor.BLACK) ? PlayerColor.WHITE : PlayerColor.BLACK;
    }
    public static int GetOtherColor(int color)
    {
        return (color ==(int) PlayerColor.BLACK) ? (int)PlayerColor.WHITE : (int)PlayerColor.BLACK;
    }

    public static PlayerColor ConvertNum2Color(int num)
    {
        return (PlayerColor)Enum.ToObject(typeof(PlayerColor), num);
    }
    #endregion
}
#region player
public abstract class OceloPlayer
{
    public GameControllData.PlayerColor _myColor { get; private set; }
    public OceloPlayer(GameControllData.PlayerColor myColor)
    {
        _myColor = myColor;
    }

    public abstract void TurnAction(OceloController oc);
}

public class HandOceloPlayer:OceloPlayer
{
    public HandOceloPlayer(GameControllData.PlayerColor myColor) :base(myColor)
    {

    }

    public override void TurnAction(OceloController oc)
    {
    }
}

public class AutoOceloPlayer : OceloPlayer
{

    public AutoOceloPlayer(GameControllData.PlayerColor myColor) : base(myColor)
    {

    }

    public override void TurnAction(OceloController oc)
    {
        WaitAction.Instance.CoalWaitAction(() =>
        {
            var putlist = GameLogic_ocelo.GetPutEnable(oc._BanData, (int)_myColor);
            var target = putlist[0];
            oc.SetKoma(target, _myColor);
        },1);
    }
}
#endregion
public abstract class OceloController
{
    public GameControllData _processData { get;protected set; }
    public GameControllData.PlayerColor _NowPlType { get { return _processData._ActivePlayerColor; } }
    public Ban _ban { get;protected set; }
    public int[,] _BanData { get { return _ban.GetBanData(); } }

    public List<OceloPlayer> _oceloPlayer=new List<OceloPlayer>();

    #region callback
    public Action _callback_gameStart;
    public Action _callback_syncBanData;
    public Action _callback_syncGameProcess;
    public Action _callback_waitInput;
    public Action _callback_skipTurn;
    public Action<GameControllData.PlayerColor> _callback_endGame;
    #endregion

    public OceloController()
    {
        _ban = new Ban(8);
        _processData = new GameControllData(GameControllData.PlayerColor.BLACK);
        //_gamestate = GameState.Display;
    }

    #region turnAction
    
    protected void TurnAction_waitInput()
    {


        _callback_waitInput?.Invoke();
        _oceloPlayer.ForEach(x => {
            if (x._myColor == _NowPlType)
            {
                x.TurnAction(this);
            }
        });
    }
    protected void TurnAction_display()
    {
        Display();
        _callback_syncBanData?.Invoke();
    }
    protected void TurnAction_swichPl()
    {
        PlChenge();
        _callback_syncGameProcess?.Invoke();
    }

    protected abstract void TurnAction();
    protected abstract void SetKomaAction();

    void Display()
    {
        //Debug.Log( OutLogBan());
    }

    void PlChenge()
    {
        _processData.SwichActivePlayer();
    }
    #endregion
    public bool SetKoma(Vector2Int putPos,GameControllData.PlayerColor putColor)
    {
        if (putColor != _NowPlType)
        {
            return false;
        }
        if (GameLogic_ocelo.IsPutEnable(_ban.GetBanData(), (int)_NowPlType, putPos))
        {
            _ban[putPos.x, putPos.y] = (int)_NowPlType;
            var newBanData=GameLogic_ocelo.Reverse(_ban.GetBanData(), putPos);
            _ban.SetBan(newBanData);
            //_gamestate = GameState.Display;
            SetKomaAction();
            return true;
        }
        else
        {
            //Debug.LogWarning($"putpos is not put enable:{putPos}");
            return false;
        }
    }

    public string OutLogBan()
    {
        string result = "";
        for(int x = 0; x < 8; x++)
        {
            for(int y = 0; y < 8; y++)
            {
                var masu = _ban[x, y];
                if (masu == 0) result += "x";
                else if (masu == (int)GameControllData.PlayerColor.BLACK) result += "●";
                else if (masu == (int)GameControllData.PlayerColor.WHITE) result += "○";
            }
            result += "\n";
        }
        return result;
    }


    public void PrepareGame()
    {
        _processData.SetColor(GameControllData.PlayerColor.BLACK, GameControllData.PlayerColor.WHITE);//後でランダムにする
    }

    public void StartGame()
    {
        var input = new int[,] {
            // 1 2 3 4 5 6 7 8
             { 0,0,0,0,0,0,0,0}//1
            ,{ 0,0,0,0,0,0,0,0}//2
            ,{ 0,0,0,0,0,0,0,0}//3
            ,{ 0,0,0,1,2,0,0,0}//4
            ,{ 0,0,0,2,1,0,0,0}//5
            ,{ 0,0,0,0,0,0,0,0}//6
            ,{ 0,0,0,0,0,0,0,0}//7
            ,{ 0,0,0,0,0,0,0,0}//8
        };
        _ban.SetBan(input);
        _callback_gameStart?.Invoke();
        TurnAction_display();
    }

    public void EndGame()
    {
        var check = GameControllData.PlayerColor.BLACK;
        var winBlack=GameLogic_ocelo.CheckWinner(_ban.GetBanData(), (int)check);
        var winner = (winBlack) ?check:GameControllData.PlayerColor.WHITE;
        _callback_endGame?.Invoke(winner);
    }

    #region 勝敗判定
    protected bool CheckPutEnable()
    {
        return GameLogic_ocelo.CheckAnyPutEnable(_ban.GetBanData(), (int)_NowPlType);
    }

    protected bool CheckEndGame()
    {
        return GameLogic_ocelo.CheckEndGame(_ban.GetBanData());
    }
    
    #endregion
}

public class LocalOceloController : OceloController
{
    protected override void TurnAction()
    {
        TurnAction_display();
        TurnAction_swichPl();
        if (CheckEndGame())
        {
            EndGame();
        }else if (!CheckPutEnable())
        {
            _callback_skipTurn?.Invoke();
            WaitAction.Instance.CoalWaitAction(()=> TurnAction(),1);
        }
        else
        {
            TurnAction_waitInput();
        }
        //勝敗判定
    }
    
    protected override void SetKomaAction()
    {
        TurnAction();
    }
}

public class RemoteOceloController : OceloController
{
    private OceloDataChannelReciever _dataReciever { get; set; }
    private NCMBSendData _ncmbSendData;//本体をこっちに持ってきたい

    public RemoteOceloController(OceloDataChannelReciever reciever) : base()
    {
        _dataReciever = reciever;
    }
    protected override void TurnAction()
    {
        TurnAction_display();
        TurnAction_swichPl();
        //勝敗判定
        if (CheckEndGame())
        {
            EndGame();
        }else if (!CheckPutEnable())
        {
            _callback_skipTurn?.Invoke();
            TurnAction_swichPl();
            UpdateNCMBObjectData(_ncmbSendData._myNCMBObject);
            _ncmbSendData.UpdateObject((obj) => {
                _dataReciever.SendOceloMessage(OceloDataChannelReciever.messageCode_playUser);
            });
        }
        else
        {
            TurnAction_waitInput();
        }
    }

    protected override void SetKomaAction()
    {
        TurnAction();
        //情報送信
        UpdateNCMBObjectData(_ncmbSendData._myNCMBObject);
        _ncmbSendData.UpdateObject((obj) => {
            _dataReciever.SendOceloMessage(OceloDataChannelReciever.messageCode_playUser);
        });
    }

    //情報受信
    public void FetchGameState()
    {
        _ncmbSendData.FetchObject((obj)=> {
            FetchNCMBObjectData(_ncmbSendData._myNCMBObject);
            _callback_syncBanData?.Invoke();
            _callback_syncGameProcess?.Invoke();
            //勝敗判定
            //勝敗判定
            if (CheckEndGame())
            {
                EndGame();
                _ncmbSendData.DeleteObject();
            }
            else if (!CheckPutEnable())
            {
                _callback_skipTurn?.Invoke();
                TurnAction_swichPl();
                UpdateNCMBObjectData(_ncmbSendData._myNCMBObject);
                _ncmbSendData.UpdateObject((obj2) => {
                    _dataReciever.SendOceloMessage(OceloDataChannelReciever.messageCode_playUser);
                });
            }
            else
            {
                TurnAction_waitInput();
            }
        });
    }
    #region ncmb
    public void CreateNCMB()
    {
        _ncmbSendData = new NCMBSendData();
        _ncmbSendData.CreateObject();
        UpdateNCMBObjectData(_ncmbSendData._myNCMBObject);
        _ncmbSendData.SaveObject((id)=> {
            _dataReciever.SendOceloMessage(OceloDataChannelReciever.messageCode_startGame, new string[] { id });
        });
    }

    public void CreateNCMB(string id)
    {
        _ncmbSendData = new NCMBSendData();
        _ncmbSendData.CreateObject(id);
    }

    void UpdateNCMBObjectData(NCMBObject obj)
    {
        obj[NCMBSendData._objKey_gameData] = JsonConverter.ToJson(_processData);
        obj[NCMBSendData._objKey_banData] = JsonConverter.ToJson_full(_ban);
    }

    void FetchNCMBObjectData(NCMBObject obj)
    {
        var json_game = obj[NCMBSendData._objKey_gameData].ToString();
        var controllData = JsonConverter.FromJson<GameControllData>(json_game);
        var json_ban = obj[NCMBSendData._objKey_banData].ToString();
        var banData = JsonConverter.FromJson_full<Ban>(json_ban);
        _processData = controllData;
        _ban = banData;
    }
    #endregion
}