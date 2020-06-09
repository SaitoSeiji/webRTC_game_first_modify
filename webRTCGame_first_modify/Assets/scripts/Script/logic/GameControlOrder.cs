using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#region message

[System.Serializable]
public abstract class GameControlMessage
{
    public enum MessageType
    {
        NONE,
        SETPL,
        STARTGAME,
        PUTKOMA,
    }
    public MessageType type { get; private set; } = MessageType.NONE;
    public GameControlMessage()
    {
        type = GetMessageType();
    }

    protected abstract MessageType GetMessageType();
}


public class GameControlMessage_setpl: GameControlMessage
{
    public Koma_ocelo.KomaType _playerKomaType;
    public OceloPlayer.PlTYPE _plType;
    public OceloPlayer.PlTYPE _enemyType;

    public GameControlMessage_setpl(OceloPlayer.PlTYPE plType, OceloPlayer.PlTYPE enemyType, Koma_ocelo.KomaType playerType)
    {
        _plType = plType;
        _enemyType = enemyType;
        _playerKomaType = playerType;
    }

    protected override MessageType GetMessageType()
    {
        return MessageType.SETPL;
    }
}

public class GameControlMessage_startGame : GameControlMessage
{
    protected override MessageType GetMessageType()
    {
        return MessageType.STARTGAME;
    }
}
[System.Serializable]
public class GameControlMessage_putKoma : GameControlMessage
{
    public Koma_ocelo.KomaType _playerType;

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
    public GameControlMessage_putKoma(Koma_ocelo.KomaType pltype, Vector2Int putpos)
    {
        _putpos = putpos;
        _playerType = pltype;
    }

    protected override MessageType GetMessageType()
    {
        return MessageType.PUTKOMA;
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
        switch (message.type)
        {
            case GameControlMessage.MessageType.SETPL:
                {
                    var mymessage = (GameControlMessage_setpl)message;
                    _myOctrl.SetMyPl(mymessage._plType,mymessage._enemyType,mymessage._playerKomaType);
                    break;
                }
            case GameControlMessage.MessageType.STARTGAME:
                _myOctrl.Init();
                _myOctrl.TurnAction();
                _myOctrl.TurnAction();
                _myOctrl.TurnAction();
                break;
            case GameControlMessage.MessageType.PUTKOMA:
                {
                    var mymessage = (GameControlMessage_putKoma)message;
                    _myOctrl.SetKoma(mymessage._putpos, _myOctrl.GetPl(mymessage._playerType));
                    _myOctrl.TurnAction();
                    _myOctrl.TurnAction();
                    _myOctrl.TurnAction();
                    break;
                }
        }
    }

    public void MessageAction(OceloMessage omessage)
    {
        try
        {
            switch (omessage.GetMessageType())
            {
                case GameControlMessage.MessageType.SETPL:
                    {
                        var send = JsonConverter.FromJson_full<GameControlMessage_setpl>(omessage.GetJson());
                        MessageAction(send);
                    }
                    break;
                case GameControlMessage.MessageType.STARTGAME:
                    {
                        var send = JsonConverter.FromJson_full<GameControlMessage_startGame>(omessage.GetJson());
                        MessageAction(send);
                    }
                    break;
                case GameControlMessage.MessageType.PUTKOMA:
                    {
                        var send = JsonConverter.FromJson_full<GameControlMessage_putKoma>(omessage.GetJson());
                        MessageAction(send);
                    }
                    break;
                default:
                    Debug.LogWarning($"not use json message:json={omessage.GetMessageType()}");
                    break;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogWarning($"recieveworning:{e}");
        }
    }
}
