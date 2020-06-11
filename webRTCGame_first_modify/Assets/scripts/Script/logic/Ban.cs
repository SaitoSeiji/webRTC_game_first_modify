using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#region archibe
#region koma
public class Koma
{

}

[System.Serializable]
public class Koma_ocelo:Koma
{
    public enum KomaType
    {
        NONE=0,
        Black=1,
        White=2
    }
    [SerializeField] KomaType _type;
    public KomaType _Type { get { return _type; }set { _type = value; } }

    public Koma_ocelo(KomaType type)
    {
        _Type = type;
    }

    public Koma_ocelo(int num)
    {
        _type = (KomaType)Enum.ToObject(typeof(KomaType), num);
    }

    public void Reverse()
    {
        _Type = (_Type == KomaType.Black) ? KomaType.White : KomaType.Black;
    }

    public void SetType(KomaType type)
    {
        _Type = type;
    }

    public static KomaType GetAnatherType(KomaType type)
    {
        return (type == KomaType.Black) ? KomaType.White : KomaType.Black;
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
#endregion

public class Ban_new
{
    [SerializeField] List<List<int>> ban;

    public Ban_new(int size)
    {
        ban = new List<List<int>>();
        for (int i = 0; i < size; i++) ban.Add(new List<int>());
        ban.ForEach(x =>
        {
            for (int i = 0; i < size; i++) x.Add(0);
        });
    }

    public Koma_ocelo this[int x, int y]
    {
        get
        {
            var num = ban[x][y];
            var result = new Koma_ocelo(num);
            return result;
        }
        set
        {
            int num = (int)value._Type;
            ban[x][y] = num;
        }
    }

    public void SetBan(int[,] input)
    {
        try
        {
            for (int x = 0; x < ban.Count; x++)
            {
                for(int y = 0; y < ban[x].Count; y++)
                {
                    this[x, y] =new Koma_ocelo( input[x, y]);
                }
            }
        }
        catch(IndexOutOfRangeException e)
        {
            Debug.LogError($"サイズが違います:{e}");
        }
        catch(Exception e)
        {
            Debug.LogError($"その他の例外:{e}");
        }
    }
}
