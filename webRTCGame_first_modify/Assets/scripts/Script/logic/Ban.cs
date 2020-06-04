using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#region koma
public class Koma
{

}

[System.Serializable]
public class Koma_ocelo:Koma
{
    public enum Type
    {
        Black,
        White
    }
    public Type _type { get; private set; }

    public Koma_ocelo(Type type)
    {
        _type = type;
    }

    public void Reverse()
    {
        _type = (_type == Type.Black) ? Type.White : Type.Black;
    }

    public void SetType(Type type)
    {
        _type = type;
    }
}
#endregion
[System.Serializable]
public class Masu<K>
    where K:Koma
{
    public K _myKoma { get; private set; }
    
    public void SetKoma(K koma)
    {
        _myKoma = koma;
    }
}

[System.Serializable]
public class Ban<K>
    where K:Koma
{
    Masu<K>[][] _ban;//(x,y)

    public Ban(Vector2Int size)
    {
        _ban = new Masu<K>[size.x][];
        for(int i = 0; i < _ban.Length; i++)
        {
            _ban[i] = new Masu<K>[size.y];
            for(int j = 0; j < _ban[i].Length; j++)
            {
                _ban[i][j] = new Masu<K>();
            }
        }
    }

    public void SetMasu(K koma,Vector2Int pos)
    {
        try
        {
            _ban[pos.x][pos.y].SetKoma(koma);
        }
        catch (IndexOutOfRangeException e)
        {
            Debug.LogWarning($"not expected setMasu:pos={pos}");
        }
    }

    Masu<K> GetMasu(Vector2Int pos)
    {
        return _ban[pos.x][pos.y];
    }

    public K GetKoma(Vector2Int pos)
    {
        try
        {
            return _ban[pos.x][pos.y]._myKoma;
        }
        catch(IndexOutOfRangeException e)
        {
            return null;
        }
    }
}
