using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class Test_gameProcessData
    {
        GameControllData gameProcessData;

        [SetUp]
        public void SetUp()
        {
            gameProcessData = new GameControllData(GameControllData.PlayerColor.BLACK);
        }

        [TestCase(GameControllData.PlayerColor.BLACK)]
        [TestCase(GameControllData.PlayerColor.WHITE)]
        public void Test_jsonSave(GameControllData.PlayerColor plcolor)
        {
            gameProcessData = new GameControllData(plcolor);
            var json=JsonConverter.ToJson(gameProcessData);
            var data = JsonConverter.FromJson<GameControllData>(json);
            Assert.AreEqual(gameProcessData._ActivePlayerColor, data._ActivePlayerColor);
        }

        [TestCase(GameControllData.PlayerColor.BLACK)]
        [TestCase(GameControllData.PlayerColor.WHITE)]
        public void Test_SwichPl(GameControllData.PlayerColor plcolor)
        {
            gameProcessData = new GameControllData(plcolor);
            gameProcessData.SwichActivePlayer();
            Assert.AreNotEqual(plcolor, gameProcessData._ActivePlayerColor);
        }
    }
}
