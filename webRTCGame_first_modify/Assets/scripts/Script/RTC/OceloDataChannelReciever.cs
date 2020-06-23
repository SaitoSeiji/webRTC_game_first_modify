using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using NCMB;

public class OceloDataChannelReciever : AbstractDataChannelReciever
{
    [SerializeField] OceloController_mono _octrl_mono;
    RemoteOceloController _octrl { get { return (RemoteOceloController)_octrl_mono._myoceloCtrl; } }
    
    #region messageCode
    public const string messageCode_startGame = "startGame";
    public const string messageCode_playUser = "play";
    #endregion

    public override void AwakeMessage()
    {
        //初回のみの行動
        //アクティブにしてゲーム開始
        _octrl_mono.gameObject.SetActive(true);
        WaitAction.Instance.CoalWaitAction_frame(() =>
        {
            if (_myRTCType == MyRTCEnum.RTCTYPE.OFFER)
            {
                _octrl_mono._myPlNum = 1;
                _octrl.PrepareGame();
                _octrl.StartGame();
                _octrl.CreateNCMB();
                _octrl.StartTurn();
            }
            else
            {
                _octrl_mono._myPlNum = 2;
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
                _octrl.CreateNCMB(input[1]);
                _octrl.FetchGameState();
                break;
            case messageCode_playUser:
                _octrl.FetchGameState();
                break;
        }
    }


    public void SendOceloMessage(string message,string[] additionalMessage=null)
    {
        var result = message;
        if (additionalMessage != null)
        {
            result=$"{result},{string.Join(",", additionalMessage)}";
        }
        SendRTCMessage(result);
    }
}
