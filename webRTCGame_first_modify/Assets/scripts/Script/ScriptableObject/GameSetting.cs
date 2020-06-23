using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "gameSetting", fileName = "settingFile_default")]
public class GameSetting : ScriptableObject
{
    public enum GameStartType
    {
        Awake,
        Hand,
        Remote
    }
    public GameStartType _gameStartType;

    public PlayerSetter.PlType _pltype_1p;
    public PlayerSetter.PlType _pltype_2p;
}
