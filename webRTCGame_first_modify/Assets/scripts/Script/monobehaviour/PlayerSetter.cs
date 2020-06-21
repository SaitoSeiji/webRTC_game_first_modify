using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetter : MonoBehaviour
{
    public enum PlType
    {
        Hand,
        Auto,
        Remote,
        None
    }
    [SerializeField]public PlType _myPlType;
    [SerializeField] GameControllData.PlayerColor _myColor;
    public GameControllData.PlayerColor _MyColor { get { return _myColor; }set { _myColor = value; } }
    
    bool handPut
    {
        get {return _myPlType == PlType.Hand; }
    }

    public OceloPlayer CreatePlayer()
    {
        switch (_myPlType)
        {
            case PlType.Auto:
                return new AutoOceloPlayer(_myColor);
            case PlType.Hand:
                return new HandOceloPlayer(_myColor);
            case PlType.Remote:
                return new HandOceloPlayer(_myColor);
        }
        return null;
    }


    public bool Onclick_putKoma(OceloController octrl,Vector2Int pos)
    {
        if (!handPut) return false;
        bool putenable = octrl.SetKoma(pos, _myColor);
        if (!putenable) return false;
        octrl.TurnAction();
        octrl.TurnAction();
        octrl.TurnAction();
        return true;
    }

}
