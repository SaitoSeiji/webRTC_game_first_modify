using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#region message

[System.Serializable]
public class GameControlMessage
{
    public enum Type
    {
        NONE,
        SETPL,
        PUTKOMA,
        PUTKOMA_REMOTE
    }
    public Type type= Type.NONE;
}


public class GameControlMessage_setpl: GameControlMessage
{
    public Koma_ocelo.Type _playerType;

    public GameControlMessage_setpl(Koma_ocelo.Type playerType)
    {
        _playerType = playerType;
        type = Type.SETPL;
    }
}

public class GameControlMessage_startGame : GameControlMessage
{
}
[System.Serializable]
public class GameControlMessage_putKoma : GameControlMessage
{
    [SerializeField]public OceloPlayer _player;
    [SerializeField] int posx;
    [SerializeField] int posy;
    public Vector2Int _putpos
    {
        get
        {
            return new Vector2Int(posx, posy);
        }
        set
        {
            posx = value.x;
            posy = value.y;
        }
    }
    public GameControlMessage_putKoma(OceloPlayer player,Vector2Int putpos)
    {
        _player = player;
        _putpos = putpos;
        type = Type.PUTKOMA;
    }
}

public class GameControlMessage_RemotePut: GameControlMessage
{
    [SerializeField] int posx;
    [SerializeField] int posy;
    public Vector2Int _putpos
    {
        get
        {
            return new Vector2Int(posx, posy);
        }
        set
        {
            posx = value.x;
            posy = value.y;
        }
    }
    public GameControlMessage_RemotePut(Vector2Int putpos)
    {
        _putpos = putpos;
        type = Type.PUTKOMA_REMOTE;
    }
}
#endregion
public class GameControlOrder
{
    OceloController _myOctrl;

    public GameControlOrder(OceloController octrl)
    {
        _myOctrl = octrl;
    }

    public void MessageAction(GameControlMessage message)
    {
        if (message is GameControlMessage_setpl)
        {
            var mymessage = (GameControlMessage_setpl)message;
            _myOctrl.SetMyPl(mymessage._playerType);
        }
        else if(message is GameControlMessage_startGame)
        {
            _myOctrl.Init();
            _myOctrl.Action();
            _myOctrl.Action();
            _myOctrl.Action();
        }
        else if (message is GameControlMessage_putKoma)
        {
            var mymessage = (GameControlMessage_putKoma)message;
            _myOctrl.SetKoma(mymessage._putpos, mymessage._player);
            _myOctrl.Action();
            _myOctrl.Action();
            _myOctrl.Action();
        }
        else if (message is GameControlMessage_RemotePut)
        {
            var mymessage = (GameControlMessage_RemotePut)message;
            _myOctrl.SetKoma(mymessage._putpos, _myOctrl.enemyPl);
            _myOctrl.Action();
            _myOctrl.Action();
            _myOctrl.Action();
        }
    }
}
