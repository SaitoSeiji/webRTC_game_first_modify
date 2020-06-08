using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#region Player

interface IKomaPut
{
    void SetKoma(Vector2Int pos);
}

[System.Serializable]
public class OceloPlayer
{
    public enum PlTYPE
    {
        INPUT,AUTO,RTC
    }

    [SerializeField] Koma_ocelo.KomaType _myPlType;
    public Koma_ocelo.KomaType _MyPlType { get { return _myPlType; } }

    protected OceloController _myOctrl;
    protected GameControlOrder _myOrder;
    protected OceloDataChannelReciever Dcr { get { return _myOctrl._dcr; } }
    public OceloPlayer(OceloController octrl,GameControlOrder order,Koma_ocelo.KomaType myType)
    {
        _myOctrl = octrl;
        _myOrder = order;
        _myPlType = myType;
    }
    
    public bool IsMoveAble()
    {
        return _myPlType == _myOctrl._NowPlType;
    }

    public virtual void TurnAction() { }

    public static OceloPlayer CreatePlayer(PlTYPE pltype, OceloController octrl, GameControlOrder order, Koma_ocelo.KomaType myType)
    {
        switch (pltype)
        {
            case PlTYPE.INPUT:
                return new OceloPlayer_input(octrl, order, myType);
            case PlTYPE.AUTO:
                return new OceloPlayer_auto(octrl, order, myType);
            case PlTYPE.RTC:
                return new OceloPlayer_rtc(octrl, order, myType);
            default:
                return null;
        }
    }
}

[System.Serializable]
public class OceloPlayer_input : OceloPlayer,IKomaPut
{
    public bool isRTC { get; set; } = true;

    public OceloPlayer_input(OceloController octrl, GameControlOrder order, Koma_ocelo.KomaType myType):base(octrl,order,myType)
    {
    }

    public override void TurnAction()
    {
        base.TurnAction();
    }

    void IKomaPut.SetKoma(Vector2Int pos)
    {
        if (!IsMoveAble()) return;
        var message = new GameControlMessage_putKoma(_MyPlType, pos);
        _myOrder.MessageAction(message);
        if (isRTC)
        {
            var remoteMessage = new GameControlMessage_putKoma(_MyPlType,pos);
            Dcr.SendOceloMessage(new OceloMessage(remoteMessage));
        }
    }
}
public class OceloPlayer_auto : OceloPlayer
{
    public OceloPlayer_auto(OceloController octrl, GameControlOrder order, Koma_ocelo.KomaType myType) : base(octrl, order, myType)
    {
    }

    public override void TurnAction()
    {
        base.TurnAction();
        WaitAction.Instance.CoalWaitAction(()=> {
            var list = GameLogic_ocelo.GetPutEnable(_myOctrl._MyBan, _MyPlType);
            var message = new GameControlMessage_putKoma(_MyPlType, list[0]);
            _myOrder.MessageAction(message);
        },1.0f);
    }
}

public class OceloPlayer_rtc : OceloPlayer
{
    public OceloPlayer_rtc(OceloController octrl, GameControlOrder order, Koma_ocelo.KomaType myType) : base(octrl, order, myType)
    {
    }
}
#endregion
[System.Serializable]
public class OceloController
{
    public enum GameState
    {
        WaitInput,
        Display,
        PlChenge
    }
    [SerializeField,NonEditable]GameState _gamestate;
    [SerializeField,NonEditable]Koma_ocelo.KomaType _nowPlType;
    public Koma_ocelo.KomaType _NowPlType { get { return _nowPlType; } }
    public OceloDataChannelReciever _dcr { get; private set; }

    public GameControlOrder _myorder { get; private set; }

    [SerializeField] Ban<Koma_ocelo> _myBan;
    public Ban<Koma_ocelo> _MyBan { get { return _myBan; } }

    //ストラテジーにする？
    public OceloPlayer myPl;
    public OceloPlayer enemyPl;

    public Action _callback_gameStart;
    public Action _callback_display;
    public Action _callback_plChenge;
    public Action _callback_waitInput;
    public Action _callback_skipTurn;
    public Action<Koma_ocelo.KomaType> _callback_endGame;


    public OceloController(OceloDataChannelReciever dcr)
    {
        _myBan = new Ban<Koma_ocelo>(new Vector2Int(8, 8));
        _myorder = new GameControlOrder(this);
        _gamestate = GameState.Display;
        _nowPlType = Koma_ocelo.KomaType.White;
        _dcr = dcr;
    }

    public void SetMyPl(OceloPlayer.PlTYPE plType,OceloPlayer.PlTYPE enemyType,Koma_ocelo.KomaType playerType)
    {
        myPl =OceloPlayer.CreatePlayer(plType, this, _myorder, playerType);
        enemyPl= OceloPlayer.CreatePlayer(enemyType, this, _myorder, Koma_ocelo.GetAnatherType(playerType));
    }
    #region turnAction
    public void TurnAction()
    {
        switch (_gamestate)
        {
            case GameState.WaitInput:
                GetPl(_nowPlType).TurnAction();
                _callback_waitInput?.Invoke();
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
                        var win = GameLogic_ocelo.CheckWin(_myBan, Koma_ocelo.KomaType.Black);
                        _callback_endGame?.Invoke((win)? Koma_ocelo.KomaType.Black: Koma_ocelo.KomaType.White);
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
        _nowPlType = (_nowPlType == Koma_ocelo.KomaType.Black) ? Koma_ocelo.KomaType.White : Koma_ocelo.KomaType.Black;
    }
    #endregion
    public void SetKoma(Vector2Int putPos,OceloPlayer pl)
    {
        if (!pl.IsMoveAble())return;
        if (GameLogic_ocelo.IsPutEnable(_myBan, _nowPlType, putPos))
        {
            var koma = new Koma_ocelo(_nowPlType);
            _myBan.SetMasu(koma, putPos);
            GameLogic_ocelo.Reverse(_myBan, putPos);
            _gamestate = GameState.Display;
        }
        else
        {
            Debug.LogWarning($"putpos is not put enable:{putPos}");
        }
    }


    public string OutLogBan()
    {
        string result = "";
        for(int x = 0; x < 8; x++)
        {
            for(int y = 0; y < 8; y++)
            {
                var masu = _myBan.GetKoma(new Vector2Int(x, y));
                if (masu == null) result += "x";
                else if (masu._type == Koma_ocelo.KomaType.Black) result += "●";
                else if (masu._type == Koma_ocelo.KomaType.White) result += "○";
            }
            result += "\n";
        }
        return result;
    }

    public void Init()
    {
        _MyBan.SetMasu(new Koma_ocelo(Koma_ocelo.KomaType.White), new Vector2Int(3, 3));
        _MyBan.SetMasu(new Koma_ocelo(Koma_ocelo.KomaType.Black), new Vector2Int(3, 4));
        _MyBan.SetMasu(new Koma_ocelo(Koma_ocelo.KomaType.Black), new Vector2Int(4, 3));
        _MyBan.SetMasu(new Koma_ocelo(Koma_ocelo.KomaType.White), new Vector2Int(4, 4));
        _callback_gameStart?.Invoke();
    }

    public OceloPlayer GetPl(Koma_ocelo.KomaType plType)
    {
        if (myPl._MyPlType == plType)
        {
            return myPl;
        }
        else
        {
            return enemyPl;
        }
    }

    bool PutEnable()
    {
        var targetpl = GetPl(_nowPlType);
        var list= GameLogic_ocelo.GetPutEnable(_MyBan, _nowPlType);
        return list.Count > 0;
    }
}
