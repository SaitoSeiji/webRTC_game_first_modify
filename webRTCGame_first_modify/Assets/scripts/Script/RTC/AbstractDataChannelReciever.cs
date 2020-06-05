using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyRTCEnum;

public abstract class AbstractDataChannelReciever:MonoBehaviour
{

    [SerializeField] RTCObject_server _rtcObj;
    protected RTCTYPE _myRTCType { get {return _rtcObj._RtcType; } }
    //始めてメッセージを受け取った時の処理
    public abstract void AwakeMessage();
    public abstract void RecieveMessage(string msg);

    protected void SendRTCMessage(string data)
    {
        _rtcObj.SendMsg_data(data);
    }
}
