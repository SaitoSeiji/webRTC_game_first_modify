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
            _myBan.SetMasu(new Koma_ocelo(Koma_ocelo.Type.Black), new Vector2Int(3, 3));
            _myBan.SetMasu(new Koma_ocelo(Koma_ocelo.Type.White), new Vector2Int(3, 4));
            _myBan.SetMasu(new Koma_ocelo(Koma_ocelo.Type.Black), new Vector2Int(4, 3));
            _myBan.SetMasu(new Koma_ocelo(Koma_ocelo.Type.White), new Vector2Int(4, 4));
        }

        [TestCase(2,3,-1,0,true)]
        public void Test_isSand(int x,int y,int vx,int vy,bool expect)
        {
            var result=GameLogic_ocelo.IsSand(_myBan, new Vector2Int(x, y),new Vector2Int(vx,vy));
            Assert.AreEqual(expect, result);
        }
    }
}
