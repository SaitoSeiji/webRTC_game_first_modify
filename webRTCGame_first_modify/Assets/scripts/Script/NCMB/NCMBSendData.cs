using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NCMB;
using System;

public class NCMBSendData
{
    //myclass["banData"]
    //myclass["gameControlData"]

    public string _objectID { get;private set; } = "";
    public NCMBObject _myNCMBObject { get; private set; }

    public const string _objKey_banData= "banData";
    public const string _objKey_gameData= "gameControlData";
    public void CreateObject(string objectId="")
    {
        _myNCMBObject = new NCMBObject("OceloData");
        if (!string.IsNullOrEmpty(objectId))
        {
            _myNCMBObject.ObjectId = objectId;
            _objectID = objectId;
        }
    }
    public void SaveObject(Action<string> act=null)
    {
        _myNCMBObject.SaveAsync((NCMBException e) =>
        {
            if (e != null)
            {

            }
            else
            {
                _objectID = _myNCMBObject.ObjectId;
                act?.Invoke(_objectID);
                Debug.Log("save end");
            }
        });
    }

    public void FetchObject(Action<NCMBObject> act)
    {
        _myNCMBObject.FetchAsync((NCMBException e) => {
            if (e != null)
            {

            }
            else
            {
                act?.Invoke(_myNCMBObject);
            }
        });
    }

    public void UpdateObject(Action<NCMBObject> act=null)
    {
        _myNCMBObject.SaveAsync((NCMBException e) =>
        {
            if (e != null)
            {

            }
            else
            {
                act?.Invoke(_myNCMBObject);
                Debug.Log("update end");
            }
        });
    }

    public void DeleteObject()
    {
        _myNCMBObject.DeleteAsync((NCMBException e) =>
        {
            if (e != null)
            {

            }
            else
            {
                Debug.Log("delete end");
            }
        });
    }

}
