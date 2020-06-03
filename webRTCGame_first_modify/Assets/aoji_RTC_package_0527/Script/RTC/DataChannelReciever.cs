using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataChannelReciever:MonoBehaviour
{
    int count = 0;
    [SerializeField] OceloController_mono _octrl;
    [SerializeField] RTCObject_server _rtcObj;
    GameControlOrder _order { get { return _octrl._myoceloCtrl._myorder; } }
    public void RecieveMessage(string msg)
    {
        Debug.Log($"recieve:{msg}");
        count++;
        if (count == 1)
        {
            _octrl.gameObject.SetActive(true);
            WaitAction.Instance.CoalWaitAction_frame(() =>
            {
                if (_rtcObj._RtcType == MyRTCEnum.RTCTYPE.OFFER)
                {
                    SetPl(Koma_ocelo.Type.Black);
                    StartGame();
                }
                else
                {
                    SetPl(Koma_ocelo.Type.White);
                    StartGame();
                }
            }, 1);

        }
        else
        {
            try
            {
                var data = JsonConverter.FromJson_full<GameControlMessage>(msg);
                switch (data.type)
                {
                    case GameControlMessage.Type.PUTKOMA:
                        var send= JsonConverter.FromJson_full<GameControlMessage_putKoma>(msg);
                        _order.MessageAction(send);
                        break;
                }
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"recieveworning:{e}");
            }
        }
    }

    public void SendRTCMessage(string msg)
    {
        _rtcObj.SendMsg_data(msg);
    }

    void SetPl()
    {
        var rand = Random.Range(0, 2);
        if (rand == 0) _order.MessageAction(new GameControlMessage_setpl(Koma_ocelo.Type.Black));
        else _order.MessageAction(new GameControlMessage_setpl(Koma_ocelo.Type.White));
    }
    void SetPl(Koma_ocelo.Type type)
    {
        _order.MessageAction(new GameControlMessage_setpl(type));
    }

    void StartGame()
    {
        _order.MessageAction(new GameControlMessage_startGame());
    }
}
