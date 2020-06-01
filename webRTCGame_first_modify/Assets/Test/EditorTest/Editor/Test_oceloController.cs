using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class Test_oceloController
    {
        OceloController _octrl;

        [SetUp]
        public void SetUp()
        {
            _octrl = new OceloController();
            _octrl._MyBan.SetMasu(new Koma_ocelo(Koma_ocelo.Type.White), new Vector2Int(3, 3));
            _octrl._MyBan.SetMasu(new Koma_ocelo(Koma_ocelo.Type.Black), new Vector2Int(3, 4));
            _octrl._MyBan.SetMasu(new Koma_ocelo(Koma_ocelo.Type.Black), new Vector2Int(4, 3));
            _octrl._MyBan.SetMasu(new Koma_ocelo(Koma_ocelo.Type.White), new Vector2Int(4, 4));
        }

        [Test]
        public void Test_OutLogBan()
        {
            var log=_octrl.OutLogBan();
            Debug.Log(log);
        }

        [TestCase(2, 3, 4, 2)]
        public void Test_Action(int x,int y,int x2,int y2)
        {
            //_octrl.Action();
            //_octrl.Action();
            //_octrl.SetKoma(new Vector2Int(x, y));
            //_octrl.Action();
            //_octrl.Action();
            //_octrl.SetKoma(new Vector2Int(x2, y2));
            //_octrl.Action();
            //_octrl.Action();
        }
    }
}
