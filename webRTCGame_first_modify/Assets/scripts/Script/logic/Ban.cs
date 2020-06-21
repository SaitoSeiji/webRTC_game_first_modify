using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

    public int this[int x, int y]
    {
        get
        {
            return ban[x][y];
        }
        set
        {
            ban[x][y] = value;
        }
    }

    public int[,] GetBanData()
    {
        int[,] result = new int[ban.Count, ban.Count];
        for(int x = 0; x < ban.Count; x++)
        {
            for(int y = 0; y < ban.Count; y++)
            {
                result[x, y] = this[x, y];
            }
        }
        return result;
    }

    public void SetBan(int[,] input)
    {
        try
        {
            for (int x = 0; x < ban.Count; x++)
            {
                for(int y = 0; y < ban[x].Count; y++)
                {
                    this[x, y] =input[x,y];
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
