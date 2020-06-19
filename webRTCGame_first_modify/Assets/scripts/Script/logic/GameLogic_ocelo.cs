using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class GameLogic_ocelo
{
    static int GetReverseNum(int num)
    {
        return GameControllData.GetOtherColor(num);
    }

    #region public
    //反転
    public static int[,] Reverse(int[,] banData, Vector2Int pos)
    {
        var komaType = banData[pos.x,pos.y];
        int[,] result = banData;
        foreach(Vector2Int vec in EightVector.GetVec())
        {
            if (IsSand(banData, komaType, pos, vec))
            {
                SandAction(banData, komaType, pos, vec, (x,y) =>
                {
                    var check = banData[x, y];
                    if (check != komaType) result[x, y] = GetReverseNum(check);
                });
            }
        }

        return result;
    }
    
    //選択可能かどうか取得
    public static bool IsPutEnable(int[,] banData, int komaType, Vector2Int pos)
    {
        if (banData[pos.x,pos.y] != 0) return false;
        foreach (Vector2Int vec in EightVector.GetVec())
        {
            if (IsSand(banData, komaType, pos, vec))
            {
                return true;
            }
        }
        return false;
    }

    public static List<Vector2Int> GetPutEnable(int[,] banData, int komaType)
    {
        var result= new List<Vector2Int>();
        for(int x = 0; x < 8; x++)
        {
            for(int y = 0; y < 8; y++)
            {
                var checkPos = new Vector2Int(x, y);
                if (IsPutEnable(banData, komaType, checkPos)) result.Add(checkPos);
            }
        }
        return result;
    }

    public static bool CheckWin(int[,] banData, int komaType)
    {
        int self=0;
        int enemy=0;
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                var koma = banData[x, y];
                if (koma == 0) continue;
                if (koma == komaType) self++;
                else enemy++;
            }
        }
        return self >= enemy;
    }
    #endregion

    public static bool IsSand(int[,] banData, int komaType, Vector2Int pos,Vector2Int vec)
    {
        int count = 0;
        SandAction(banData,komaType, pos, vec,(x,y)=> {
            var check = banData[x, y];
            if (check == 0)
            {
                count = 0;
            }
            else if(check==komaType)
            {
            }
            else
            {
                count++;
            }
        });
        return count > 0;
    }

    static void SandAction(int[,] banData,int komaType, Vector2Int pos, Vector2Int vec,Action<int,int> checkAction=null)
    {
        //var origine = komaType;
        //if (origine == null)
        //{
        //    Debug.LogError($"pos is invalid num:pos={pos}");
        //    return;
        //}


        int count = 1;
        var targetPos = pos + vec * count;
        if (!IsContainRange(banData, targetPos)) return;
        var check = banData[targetPos.x, targetPos.y];
        //var check = ban.GetKoma(pos + vec * count);
        while (true)
        {
            checkAction?.Invoke(targetPos.x,targetPos.y);
            if (check == 0)
            {
                break;
            }else if(check ==komaType)
            {
                break;
            }
            else
            {
                count++;
                targetPos = pos + vec * count;
                if (!IsContainRange(banData, targetPos)) return;
                check = banData[targetPos.x, targetPos.y];
            }
        }
    }
    
    public static bool IsContainRange(int[,] banData, Vector2Int pos)
    {
        Vector2Int range = new Vector2Int(0, (int)Mathf.Sqrt(banData.Length-1));
        var newPos = new Vector2Int();
        newPos.x = Mathf.Clamp(pos.x, range.x, range.y);
        newPos.y = Mathf.Clamp(pos.y, range.x, range.y);
        return newPos.x == pos.x && newPos.y == pos.y;
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
