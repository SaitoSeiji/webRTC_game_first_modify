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
            _octrl = new LocalOceloController();
            _octrl.StartGame();
        }

        [Test]
        public void Test_OutLogBan()
        {
            var log = _octrl.OutLogBan();
            Debug.Log(log);
        }
        
        [Test]
        public void Test_setKoma()
        {
            GameControllData.PlayerColor targetColor = _octrl._NowPlType;
            var list = GameLogic_ocelo.GetPutEnable(_octrl._BanData,(int) targetColor);
            var pos = list[0];
            bool put = _octrl.SetKoma(pos, targetColor);
            Assert.AreEqual(true, put);
            Assert.AreEqual((int)targetColor,_octrl._BanData[pos.x,pos.y]);
        }

        //[TestCase(2, 3, 4, 2)]
        //public void Test_TurnAction(int x, int y, int x2, int y2)
        //{
        //    _octrl.TurnAction();
        //    _octrl.TurnAction();
        //    _octrl.TurnAction();
        //    Assert.AreEqual(OceloController.GameState.WaitInput, _octrl._gamestate);
        //    _octrl.SetKoma(new Vector2Int(x, y), _octrl._NowPlType);
        //    Test_OutLogBan();
        //    Assert.AreEqual((int)_octrl._NowPlType, _octrl._BanData[x, y]);
        //    _octrl.TurnAction();
        //    _octrl.TurnAction();
        //    _octrl.TurnAction();
        //    Assert.AreEqual(OceloController.GameState.WaitInput, _octrl._gamestate);
        //}
    }
}
