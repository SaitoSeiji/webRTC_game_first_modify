using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class Test_gameLogic_ocello
    {
        Ban<Koma_ocelo> _myBan;

        [SetUp]
        public void SetUp()
        {
            _myBan = new Ban<Koma_ocelo>(new Vector2Int(8, 8));
            _myBan.SetMasu(new Koma_ocelo(Koma_ocelo.KomaType.White), new Vector2Int(3, 3));
            _myBan.SetMasu(new Koma_ocelo(Koma_ocelo.KomaType.Black), new Vector2Int(3, 4));
            _myBan.SetMasu(new Koma_ocelo(Koma_ocelo.KomaType.Black), new Vector2Int(4, 3));
            _myBan.SetMasu(new Koma_ocelo(Koma_ocelo.KomaType.White), new Vector2Int(4, 4));
        }

        //挟んでいるかの確認
        [TestCase(Koma_ocelo.KomaType.Black,2,3,1,0,true)]
        [TestCase(Koma_ocelo.KomaType.White,4,2,0,1,true)]
        public void Test_isSand(Koma_ocelo.KomaType komaType,int x,int y,int vx,int vy,bool expect)
        {
            var koma = new Koma_ocelo(komaType);
            var result=GameLogic_ocelo.IsSand(_myBan,komaType, new Vector2Int(x, y),new Vector2Int(vx,vy));
            Assert.AreEqual(expect, result);
        }

        [TestCase(Koma_ocelo.KomaType.Black, 2, 3, true)]
        [TestCase(Koma_ocelo.KomaType.White, 2, 3, false)]
        [TestCase(Koma_ocelo.KomaType.White, 4, 2, true)]
        public void Test_putEnable(Koma_ocelo.KomaType komaType, int x, int y, bool expect)
        {
            var result = GameLogic_ocelo.IsPutEnable(_myBan,komaType, new Vector2Int(x, y));
            Assert.AreEqual(expect, result);
        }

        //ひっくり返しているかの確認
        [TestCase(Koma_ocelo.KomaType.Black, 2, 3, 3, 3, Koma_ocelo.KomaType.Black)]
        [TestCase(Koma_ocelo.KomaType.White, 4, 2, 4, 3, Koma_ocelo.KomaType.White)]
        public void Test_reverse(Koma_ocelo.KomaType komaType, int x, int y, int ex, int ey, Koma_ocelo.KomaType checkType)
        {
            var koma = new Koma_ocelo(komaType);
            _myBan.SetMasu(koma, new Vector2Int(x, y));
            GameLogic_ocelo.Reverse(_myBan, new Vector2Int(x, y));
            var check = _myBan.GetKoma(new Vector2Int(ex, ey));
            Assert.AreEqual(checkType, check._Type);
        }
    }
}
