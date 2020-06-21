using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OceloDataChannelReciever : AbstractDataChannelReciever
{
    [SerializeField] OceloController_mono _octrl;
    GameControllData _controllData { get { return _octrl._myoceloCtrl._processData; }
                                     set  { _octrl._myoceloCtrl._processData = value; }
        }
    Ban_new _ban { get { return _octrl._myoceloCtrl._ban; }
                  //set { _octrl._myoceloCtrl._ban = value; }
    }
    //GameControlOrder _order { get { return _octrl._myoceloCtrl._myorder; } }
    public NCMBSendData _ncmbSendData  { get; private set; } =new NCMBSendData();

    #region messageCode
    public const string messageCode_startGame = "startGame";
    public const string messageCode_playUser = "play";
    #endregion

    public override void AwakeMessage()
    {
        //初回のみの行動
        //アクティブにしてゲーム開始
        _octrl.gameObject.SetActive(true);
        WaitAction.Instance.CoalWaitAction_frame(() =>
        {
            if (_myRTCType == MyRTCEnum.RTCTYPE.OFFER)
            {
                _controllData.SetColor( GameControllData.PlayerColor.BLACK, GameControllData.PlayerColor.WHITE);//後でランダムにしたい
                _octrl._myPlNum = 1;
                _octrl.StartGame();

                var json_gameData = JsonConverter.ToJson(_controllData);
                var json_banData = JsonConverter.ToJson_full(_ban);
                _ncmbSendData.CreateObject();
                _ncmbSendData._myNCMBObject[NCMBSendData._objKey_banData] = json_banData;
                _ncmbSendData._myNCMBObject[NCMBSendData._objKey_gameData] = json_gameData;
                _ncmbSendData.SaveObject((id)=> {
                    SendRTCMessage(messageCode_startGame + $",{id}");
                });
                //var offerPlType = SetPl();
                //var answerType = Koma_ocelo.GetAnatherType(offerPlType);
                //var plmessage = new GameControlMessage_setpl(OceloPlayer.PlTYPE.INPUT, OceloPlayer.PlTYPE.RTC, answerType);
                //SendOceloMessage(new OceloMessage(plmessage));

                //StartGame();
                //var startMessage = new GameControlMessage_startGame();
                //SendOceloMessage(new OceloMessage(startMessage));
            }
        }, 1);
    }
    public override void RecieveMessage(string msg)
    {
        Debug.Log($"recieve:{msg}");
        var input = msg.Split(',');
        var code = input[0];
        switch (code)
        {
            case messageCode_startGame:
                _ncmbSendData.CreateObject(input[1]);
                _ncmbSendData.FetchObject((obj)=> {
                    var json = obj[NCMBSendData._objKey_gameData].ToString();
                    _controllData = JsonConverter.FromJson<GameControllData>(json);
                });
                _octrl._myPlNum = 2;
                _octrl.StartGame();
                break;
            case messageCode_playUser:
                _ncmbSendData.FetchObject((obj) =>
                {
                    var json_game = obj[NCMBSendData._objKey_gameData].ToString();
                    _controllData = JsonConverter.FromJson<GameControllData>(json_game);
                    var json_ban = obj[NCMBSendData._objKey_banData].ToString();
                    _octrl._myoceloCtrl.SetKoma(JsonConverter.FromJson_full<Ban_new>(json_ban));
                    _octrl._myoceloCtrl.TurnAction();
                    _octrl._myoceloCtrl.TurnAction();
                    _octrl._myoceloCtrl.TurnAction();
                });
                break;
        }
    }


    public void SendOceloMessage(string message)
    {
        SendRTCMessage(message);
    }

    #region local
    //Koma_ocelo.KomaType SetPl()
    //{
    //    var rand = Random.Range(0, 2);
    //    var setType = (rand == 0) ? Koma_ocelo.KomaType.Black : Koma_ocelo.KomaType.White;
    //    _order.MessageAction(new GameControlMessage_setpl( OceloPlayer.PlTYPE.INPUT , OceloPlayer.PlTYPE.RTC,setType));
    //    return setType;
    //}
    //void SetPl(Koma_ocelo.KomaType type)
    //{
    //    _order.MessageAction(new GameControlMessage_setpl(OceloPlayer.PlTYPE.INPUT, OceloPlayer.PlTYPE.RTC, type));
    //}

    //void StartGame()
    //{
    //    _order.MessageAction(new GameControlMessage_startGame());
    //}
    #endregion
}
