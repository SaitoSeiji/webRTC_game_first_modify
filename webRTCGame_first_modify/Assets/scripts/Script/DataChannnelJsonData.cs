using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class DataChannnelJsonData
{
    [SerializeField]public List<string> keyList=new List<string>();
    [SerializeField]public List<string> dataList=new List<string>();
    
    public string this[string key]
    {
        get
        {
            var index = keyList.IndexOf(key);
            return dataList[index];
        }
        set
        {
            if (!keyList.Contains(key))
            {
                keyList.Add(key);
                dataList.Add("");
            }
            var index = keyList.IndexOf(key);
            dataList[index] = value;
        }
    }
}

//[System.Serializable]
//public class OceloMessage : DataChannnelJsonData
//{
//    public OceloMessage(GameControlMessage data)
//    {
//        this["type"]= data.type.ToString();
//        this["classJson"]= JsonConverter.ToJson_full(data);
//        //_DataSet["type"] = data.type.ToString();
//        //_DataSet["classJson"] = JsonConverter.ToJson_full(data);
//    }

//    public GameControlMessage.MessageType GetMessageType()
//    {
//        Enum.TryParse(this["type"], out GameControlMessage.MessageType result);
//        return result;
//    }

//    public string GetJson()
//    {
//        return this["classJson"];
//    }
//}
