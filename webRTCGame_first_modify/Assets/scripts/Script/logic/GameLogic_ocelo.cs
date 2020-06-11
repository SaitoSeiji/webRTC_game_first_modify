using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class GameLogic_ocelo
{
    #region public
    //反転
    public static void Reverse(Ban<Koma_ocelo> ban, Vector2Int pos)
    {
        var komaType = ban.GetKoma(pos)._Type;
        HashSet<Koma_ocelo> reverseList = new HashSet<Koma_ocelo>();
        foreach(Vector2Int vec in EightVector.GetVec())
        {
            if (IsSand(ban, komaType, pos, vec))
            {
                SandAction(ban, komaType, pos, vec, (check) =>
                {
                    if (check._Type != komaType) reverseList.Add(check);
                });
            }
        }
        foreach(var target in reverseList)
        {
            target.Reverse();
        }
    }
    
    //選択可能かどうか取得
    public static bool IsPutEnable(Ban<Koma_ocelo> ban, Koma_ocelo.KomaType komaType, Vector2Int pos)
    {
        if (ban.GetKoma(pos) != null) return false;
        foreach (Vector2Int vec in EightVector.GetVec())
        {
            if (IsSand(ban, komaType, pos, vec))
            {
                return true;
            }
        }
        return false;
    }

    public static List<Vector2Int> GetPutEnable(Ban<Koma_ocelo> ban, Koma_ocelo.KomaType komaType)
    {
        var result= new List<Vector2Int>();
        for(int x = 0; x < 8; x++)
        {
            for(int y = 0; y < 8; y++)
            {
                var checkPos = new Vector2Int(x, y);
                if (IsPutEnable(ban, komaType, checkPos)) result.Add(checkPos);
            }
        }
        return result;
    }

    public static bool CheckWin(Ban<Koma_ocelo> ban, Koma_ocelo.KomaType komaType)
    {
        int self=0;
        int enemy=0;
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                var koma = ban.GetKoma(new Vector2Int(x, y));
                if (koma == null) continue;
                if (koma._Type == komaType) self++;
                else enemy++;
            }
        }
        return self >= enemy;
    }
    #endregion

    public static bool IsSand(Ban<Koma_ocelo> ban, Koma_ocelo.KomaType komaType, Vector2Int pos,Vector2Int vec)
    {
        int count = 0;
        SandAction(ban,komaType, pos, vec,(check)=> {
            if (check == null)
            {
                count = 0;
            }
            else if(check._Type==komaType)
            {
            }
            else
            {
                count++;
            }
        });
        return count > 0;
    }

    static void SandAction(Ban<Koma_ocelo> ban,Koma_ocelo.KomaType komaType, Vector2Int pos, Vector2Int vec, Action<Koma_ocelo> checkAction = null)
    {
        //var origine = komaType;
        //if (origine == null)
        //{
        //    Debug.LogError($"pos is invalid num:pos={pos}");
        //    return;
        //}

        int count = 1;
        var check = ban.GetKoma(pos + vec * count);
        while (true)
        {
            checkAction?.Invoke(check);
            if (check == null)
            {
                break;
            }else if(check._Type == komaType)
            {
                break;
            }
            else
            {
                count++;
                check = ban.GetKoma(pos + vec * count);
            }
        }

    }


    public static class EightVector
    {


        public static IEnumerable<Vector2Int> GetVec()
        {
            yield return new Vector2Int(-1, -1);
            yield return new Vector2Int(-1, 0);
            yield return new Vector2Int(-1, 1);

            yield return new Vector2Int(0, -1);
            yield return new Vector2Int(0, 1);

            yield return new Vector2Int(1, -1);
            yield return new Vector2Int(1, 0);
            yield return new Vector2Int(1, 1);
        }
    }

}
