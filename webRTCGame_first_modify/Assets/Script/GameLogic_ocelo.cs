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
        foreach(Vector2Int vec in EightVector.GetVec())
        {
            if (IsSand(ban, pos, vec))
            {
                SandAction(ban, pos, vec, (check) => check.Reverse());
            }
        }
    }


    //選択可能箇所取得

    #endregion

    public static bool IsSand(Ban<Koma_ocelo> ban,Vector2Int pos,Vector2Int vec)
    {
        int count = 0;
        var origine = ban.GetKoma(pos);
        SandAction(ban, pos, vec,(check)=> {
            if (check == null)
            {
                count = 0;
            }
            else if(check._type==origine._type)
            {
            }
            else
            {
                count++;
            }
        });
        return count > 0;
        //var origine=ban.GetKoma(pos);
        //if (origine == null)
        //{
        //    Debug.LogError($"pos is invalid num:pos={pos}");
        //    return false;
        //}

        //int count = 1;
        //var check = ban.GetKoma(pos + vec*count);
        //while (true)
        //{
        //    if (check == null || check._type == origine._type)
        //    {
        //        break;
        //    }
        //    else
        //    {
        //        count++;
        //        check = ban.GetKoma(pos + vec*count);
        //        checkAction?.Invoke(check);
        //    }
        //}

        //int reverceCout = (count - 1);

        //return reverceCout > 0;
    }

    static void SandAction(Ban<Koma_ocelo> ban, Vector2Int pos, Vector2Int vec, Action<Koma_ocelo> checkAction = null)
    {
        var origine = ban.GetKoma(pos);
        if (origine == null)
        {
            Debug.LogError($"pos is invalid num:pos={pos}");
            return;
        }

        int count = 1;
        var check = ban.GetKoma(pos + vec * count);
        while (true)
        {
            checkAction?.Invoke(check);
            if (check == null)
            {
                break;
            }else if(check._type == origine._type)
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
