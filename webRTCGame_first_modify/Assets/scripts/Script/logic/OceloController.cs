using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
        _playerData = new List<PlayerData>();
        _playerData.Add(new PlayerData(1, PlayerColor.BLACK));
        _playerData.Add(new PlayerData(2, PlayerColor.WHITE));
    }

    public void SwichActivePlayer()
    {
        _activePlayerColor=GetOtherColor(_activePlayerColor);
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
        var putlist=GameLogic_ocelo.GetPutEnable(oc._BanData,(int) _myColor);
        var target = putlist[0];
        oc.SetKoma(target, _myColor);
        oc.TurnAction();
        oc.TurnAction();
        oc.TurnAction();
    }
}
#endregion
public class OceloController
{
    public enum GameState
    {
        WaitInput,
        Display,
        PlChenge
    }
    public GameState _gamestate { get; private set; }
    public GameControllData _processData { get; private set; }
    public GameControllData.PlayerColor _NowPlType { get { return _processData._ActivePlayerColor; } }
    Ban_new _ban { get; set; }
    public int[,] _BanData { get { return _ban.GetBanData(); } }

    public List<OceloPlayer> _oceloPlayer=new List<OceloPlayer>();

    #region callback
    public Action _callback_gameStart;
    public Action _callback_display;
    public Action _callback_plChenge;
    public Action _callback_waitInput;
    public Action _callback_skipTurn;
    public Action<GameControllData.PlayerColor> _callback_endGame;
    #endregion

    public OceloController()
    {
        _ban = new Ban_new(8);
        _processData = new GameControllData(GameControllData.PlayerColor.BLACK);
        _gamestate = GameState.Display;
    }
    
    #region turnAction
    public void TurnAction()
    {

        switch (_gamestate)
        {
            case GameState.WaitInput:
                _callback_waitInput?.Invoke();
                _oceloPlayer.ForEach(x => {
                    if(x._myColor == _NowPlType)
                    {
                        x.TurnAction(this);
                    }
                });
                break;
            case GameState.Display:
                Display();
                _gamestate = GameState.PlChenge;
                _callback_display?.Invoke();

                break;
            case GameState.PlChenge:
                PlChenge();
                if (PutEnable())
                {
                    _gamestate = GameState.WaitInput;
                    _callback_plChenge?.Invoke();
                }
                else
                {
                    PlChenge();
                    if (PutEnable())
                    {
                        _gamestate = GameState.WaitInput;
                        _callback_skipTurn?.Invoke();
                        //_callback_plChenge?.Invoke();
                    }
                    else
                    {
                        var win = GameLogic_ocelo.CheckWin(_ban.GetBanData(), (int)GameControllData.PlayerColor.BLACK);
                        _callback_endGame?.Invoke((win)? GameControllData.PlayerColor.BLACK: GameControllData.PlayerColor.WHITE);
                    }
                }
                break;
        }
    }

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
        if (putColor!=_NowPlType)return false;
        if (GameLogic_ocelo.IsPutEnable(_ban.GetBanData(), (int)_NowPlType, putPos))
        {
            _ban[putPos.x, putPos.y] = (int)_NowPlType;
            var newBanData=GameLogic_ocelo.Reverse(_ban.GetBanData(), putPos);
            _ban.SetBan(newBanData);
            _gamestate = GameState.Display;
            return true;
        }
        else
        {
            Debug.LogWarning($"putpos is not put enable:{putPos}");
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

    public void Init()
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
    }
    
    bool PutEnable()
    {
        var list= GameLogic_ocelo.GetPutEnable(_ban.GetBanData(), (int)_NowPlType);
        return list.Count > 0;
    }
}
