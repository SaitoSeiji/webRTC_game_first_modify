using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#region message
public abstract class GameControlMessage
{

}


public class GameControlMessage_setpl: GameControlMessage
{
    public Koma_ocelo.Type _playerType;

    public GameControlMessage_setpl(Koma_ocelo.Type playerType)
    {
        _playerType = playerType;
    }
}

public class GameControlMessage_startGame : GameControlMessage
{
}
public class GameControlMessage_putKoma : GameControlMessage
{
    public OceloPlayer _player;
    public Vector2Int _putpos;

    public GameControlMessage_putKoma(OceloPlayer player,Vector2Int putpos)
    {
        _player = player;
        _putpos = putpos;
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
            _myOctrl.SetKoma(mymessage._putpos,mymessage._player);
            _myOctrl.Action();
            _myOctrl.Action();
            _myOctrl.Action();
        }
    }
}
