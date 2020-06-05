using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class Test_ban
    {
        Ban<Koma_ocelo> _myBan;

        [SetUp]
        public void SetUp()
        {
            _myBan = new Ban<Koma_ocelo>(new Vector2Int(8,8));
        }

        [TestCase(1,1,false)]
        [TestCase(4,4,false)]
        [TestCase(8,4,true)]
        [TestCase(5,-1,true)]
        public void Test_SetGetMasu(int x,int y,bool isOut)
        {
            var koma = new Koma_ocelo( Koma_ocelo.KomaType.Black);
            _myBan.SetMasu(koma,new Vector2Int(x,y));
            var result = (isOut) ? null : koma;
            Assert.AreEqual(result, _myBan.GetKoma(new Vector2Int(x,y)));
        }
    }
}
