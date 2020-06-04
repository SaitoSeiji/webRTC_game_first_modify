using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class OceloPlayer
{
    [SerializeField] Koma_ocelo.Type _myPlType;
    public Koma_ocelo.Type _MyPlType { get { return _myPlType; } }

    protected OceloController _myOctrl;
    protected GameControlOrder _myOrder;
    protected DataChannelReciever Dcr { get { return _myOctrl._dcr; } }
    public OceloPlayer(OceloController octrl,GameControlOrder order,Koma_ocelo.Type myType)
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
}

[System.Serializable]
public class OceloPlayer_input : OceloPlayer
{
    public bool isRTC { get; set; } = true;

    public OceloPlayer_input(OceloController octrl, GameControlOrder order, Koma_ocelo.Type myType):base(octrl,order,myType)
    {
    }

    public override void TurnAction()
    {
        base.TurnAction();
    }

    public void SetKoma(Vector2Int pos)
    {
        if (!IsMoveAble()) return;
        var message = new GameControlMessage_putKoma(this, pos);
        _myOrder.MessageAction(message);
        if (isRTC)
        {
            var remoteMessage = new GameControlMessage_RemotePut(pos);
            var json = JsonConverter.ToJson_full(remoteMessage);
            Dcr.SendRTCMessage(json);
        }
    }
}
public class OceloPlayer_auto : OceloPlayer
{
    public OceloPlayer_auto(OceloController octrl, GameControlOrder order, Koma_ocelo.Type myType) : base(octrl, order, myType)
    {
    }

    public override void TurnAction()
    {
        base.TurnAction();
        WaitAction.Instance.CoalWaitAction(()=> {
            var list = GameLogic_ocelo.GetPutEnable(_myOctrl._MyBan, new Koma_ocelo(_MyPlType));
            var message = new GameControlMessage_putKoma(this, list[0]);
            _myOrder.MessageAction(message);
        },1.0f);
    }
}

public class OceloPlayer_rtc : OceloPlayer
{
    public OceloPlayer_rtc(OceloController octrl, GameControlOrder order, Koma_ocelo.Type myType) : base(octrl, order, myType)
    {
    }
}

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
    [SerializeField,NonEditable]Koma_ocelo.Type _nowPlType;
    public Koma_ocelo.Type _NowPlType { get { return _nowPlType; } }
    public DataChannelReciever _dcr { get; private set; }

    public GameControlOrder _myorder { get; private set; }

    [SerializeField] Ban<Koma_ocelo> _myBan;
    public Ban<Koma_ocelo> _MyBan { get { return _myBan; } }

    public OceloPlayer_input myPl;
    public OceloPlayer_rtc enemyPl;

    public Action _callback_gameStart;
    public Action _callback_display;
    public Action _callback_plChenge;
    public Action _callback_waitInput;

    public OceloController(DataChannelReciever dcr)
    {
        _myBan = new Ban<Koma_ocelo>(new Vector2Int(8, 8));
        _myorder = new GameControlOrder(this);
        _gamestate = GameState.Display;
        _nowPlType = Koma_ocelo.Type.White;
        _dcr = dcr;
    }

    public void SetMyPl(Koma_ocelo.Type playerType)
    {
        if(playerType == Koma_ocelo.Type.Black)
        {

            myPl = new OceloPlayer_input(this,_myorder, Koma_ocelo.Type.Black);
            //enemyPl = new OceloPlayer_auto(this, _myorder, Koma_ocelo.Type.White);
            enemyPl = new OceloPlayer_rtc(this, _myorder, Koma_ocelo.Type.White);
        }
        else
        {
            myPl = new OceloPlayer_input(this, _myorder, Koma_ocelo.Type.White);
            //enemyPl = new OceloPlayer_auto(this, _myorder, Koma_ocelo.Type.Black);
            enemyPl = new OceloPlayer_rtc(this, _myorder, Koma_ocelo.Type.Black);
        }
    }

    public void Action()
    {
        switch (_gamestate)
        {
            case GameState.WaitInput:
                if (enemyPl.IsMoveAble()) enemyPl.TurnAction();
                if (myPl.IsMoveAble()) myPl.TurnAction();
                _callback_waitInput?.Invoke();
                break;
            case GameState.Display:
                Display();
                _gamestate = GameState.PlChenge;
                _callback_display?.Invoke();
                break;
            case GameState.PlChenge:
                PlChenge();
                _gamestate = GameState.WaitInput;
                _callback_plChenge?.Invoke();
                break;
        }
    }

    void Display()
    {
        //Debug.Log( OutLogBan());
    }

    void PlChenge()
    {
        _nowPlType = (_nowPlType == Koma_ocelo.Type.Black) ? Koma_ocelo.Type.White : Koma_ocelo.Type.Black;
    }

    public void SetKoma(Vector2Int putPos,OceloPlayer pl)
    {
        if (!pl.IsMoveAble())return;
        var koma = new Koma_ocelo(_nowPlType);
        if (GameLogic_ocelo.IsPutEnable(_myBan, koma, putPos))
        {
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
                else if (masu._type == Koma_ocelo.Type.Black) result += "●";
                else if (masu._type == Koma_ocelo.Type.White) result += "○";
            }
            result += "\n";
        }
        return result;
    }

    public void Init()
    {
        _MyBan.SetMasu(new Koma_ocelo(Koma_ocelo.Type.White), new Vector2Int(3, 3));
        _MyBan.SetMasu(new Koma_ocelo(Koma_ocelo.Type.Black), new Vector2Int(3, 4));
        _MyBan.SetMasu(new Koma_ocelo(Koma_ocelo.Type.Black), new Vector2Int(4, 3));
        _MyBan.SetMasu(new Koma_ocelo(Koma_ocelo.Type.White), new Vector2Int(4, 4));
        _callback_gameStart?.Invoke();
    }
}
