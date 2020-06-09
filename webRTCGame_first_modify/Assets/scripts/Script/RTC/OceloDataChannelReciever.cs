using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OceloDataChannelReciever : AbstractDataChannelReciever
{
    [SerializeField] OceloController_mono _octrl;
    GameControlOrder _order { get { return _octrl._myoceloCtrl._myorder; } }
    public override void AwakeMessage()
    {
        //初回のみの行動
        //アクティブにしてゲーム開始
        _octrl.gameObject.SetActive(true);
        WaitAction.Instance.CoalWaitAction_frame(() =>
        {
            if (_myRTCType == MyRTCEnum.RTCTYPE.OFFER)
            {
                var offerPlType = SetPl();
                var answerType = Koma_ocelo.GetAnatherType(offerPlType);
                var plmessage = new GameControlMessage_setpl(OceloPlayer.PlTYPE.INPUT, OceloPlayer.PlTYPE.RTC, answerType);
                SendOceloMessage(new OceloMessage(plmessage));

                StartGame();
                var startMessage = new GameControlMessage_startGame();
                SendOceloMessage(new OceloMessage(startMessage));
            }
        }, 1);
    }
    public override void RecieveMessage(string msg)
    {
        Debug.Log($"recieve:{msg}");
        try
        {
            var data = JsonConverter.FromJson<OceloMessage>(msg);
            _order.MessageAction(data);
        }
        catch (System.Exception e)
        {
            Debug.LogWarning($"recieveworning:{e}");
        }
    }


    public void SendOceloMessage(OceloMessage omessage)
    {
        SendRTCMessage(JsonConverter.ToJson(omessage));
    }

    #region local
    Koma_ocelo.KomaType SetPl()
    {
        var rand = Random.Range(0, 2);
        var setType = (rand == 0) ? Koma_ocelo.KomaType.Black : Koma_ocelo.KomaType.White;
        _order.MessageAction(new GameControlMessage_setpl( OceloPlayer.PlTYPE.INPUT , OceloPlayer.PlTYPE.RTC,setType));
        return setType;
    }
    void SetPl(Koma_ocelo.KomaType type)
    {
        _order.MessageAction(new GameControlMessage_setpl(OceloPlayer.PlTYPE.INPUT, OceloPlayer.PlTYPE.RTC, type));
    }

    void StartGame()
    {
        _order.MessageAction(new GameControlMessage_startGame());
    }
    #endregion
}
