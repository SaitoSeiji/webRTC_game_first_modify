using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class Test_gameLogic_ocello
    {
        Ban ban;
        [SetUp]
        public void SetUp()
        {
            ban = new Ban(8);

            var input = new int[,] {
            // 0 1 2 3 4 5 6 7
             { 0,0,0,0,0,0,0,0}//0
            ,{ 0,0,0,0,0,0,0,0}//1
            ,{ 0,0,0,0,0,0,0,0}//2
            ,{ 0,0,0,1,2,0,0,0}//3
            ,{ 0,0,0,2,1,0,0,0}//4
            ,{ 0,0,0,0,0,0,0,0}//5
            ,{ 0,0,0,0,0,0,0,0}//6
            ,{ 0,0,0,0,0,0,0,0}//7
            };
            ban.SetBan(input);
        }


        [TestCase(0, 0, true)]
        [TestCase(-1, 0, false)]
        [TestCase(0, -1, false)]
        [TestCase(0, 8, false)]
        [TestCase(0, 7, true)]
        [TestCase(0, 6, true)]
        public void Test_containRange(int x, int y, bool expected)
        {
            var pos = new Vector2Int(x, y);
            var result = GameLogic_ocelo.IsContainRange(ban.GetBanData(), pos);
            Assert.AreEqual(expected, result);
        }
        //挟んでいるかの確認
        [TestCase( GameControllData.PlayerColor.WHITE, 2, 3, 1, 0, true)]
        [TestCase(GameControllData.PlayerColor.BLACK, 4, 2, 0, 1, true)]
        public void Test_isSand(GameControllData.PlayerColor komaType, int x, int y, int vx, int vy, bool expect)
        {
            var result = GameLogic_ocelo.IsSand(ban.GetBanData(), (int)komaType, new Vector2Int(x, y), new Vector2Int(vx, vy));
            Assert.AreEqual(expect, result);
        }

        [Test]
        public void Test_isSand_outban()
        {
            ban = new Ban(3);
            var input = new int[,]
            {
                { 0,0,1},
                { 2,1,0},
                { 0,0,0}
            };
            ban.SetBan(input);
            Vector2Int pos = new Vector2Int(1, 2);
            var result1 = GameLogic_ocelo.IsSand(ban.GetBanData(),2,pos, new Vector2Int(0,-1));
            Assert.AreEqual(true, result1);
            var result2 = GameLogic_ocelo.IsSand(ban.GetBanData(),2,pos, new Vector2Int(-1,0));
            Assert.AreEqual(false, result2);
        }

        [TestCase(GameControllData.PlayerColor.WHITE, 2, 3, true)]
        [TestCase(GameControllData.PlayerColor.BLACK, 2, 3, false)]
        [TestCase(GameControllData.PlayerColor.BLACK, 4, 2, true)]
        public void Test_putEnable(GameControllData.PlayerColor komaType, int x, int y, bool expect)
        {
            var result = GameLogic_ocelo.IsPutEnable(ban.GetBanData(),(int) komaType, new Vector2Int(x, y));
            Assert.AreEqual(expect, result);
        }

        //ひっくり返しているかの確認
        [TestCase(GameControllData.PlayerColor.WHITE,2, 3, 3, 3)]
        [TestCase(GameControllData.PlayerColor.BLACK,4, 2, 4, 3)]
        public void Test_reverse(GameControllData.PlayerColor putType,int x, int y, int ex, int ey)
        {
            ban[x, y] = (int)putType;
            var result=GameLogic_ocelo.Reverse(ban.GetBanData(), new Vector2Int(x, y));
            var before = ban.GetBanData()[ex, ey];
            var after = result[ex, ey];
            Assert.AreNotEqual(before, after);
            ban.SetBan(result);
            Assert.AreEqual(after, ban.GetBanData()[ex,ey]);
        }

        [Test]
        public void Test_skip()
        {
            ban = new Ban(3);
            var input = new int[,]
            {
                { 1,1,1},
                { 2,1,0},
                { 1,1,1}
            };
            ban.SetBan(input);
            var miss = GameLogic_ocelo.CheckAnyPutEnable(ban.GetBanData(), 1);
            var succsess = GameLogic_ocelo.CheckAnyPutEnable(ban.GetBanData(), 2);
            Assert.AreEqual(false, miss);
            Assert.AreEqual(true, succsess);
        }

        [Test]
        public void Test_endGame()
        {
            ban = new Ban(3);
            var input = new int[,]
            {
                { 1,1,1},
                { 2,1,2},
                { 1,1,1}
            };
            ban.SetBan(input);
            var end = GameLogic_ocelo.CheckEndGame(ban.GetBanData());
            Assert.AreEqual(true, end);
            input = new int[,]
            {
                { 1,1,1},
                { 2,1,0},
                { 1,1,1}
            };
            ban.SetBan(input);
            var notend = GameLogic_ocelo.CheckEndGame(ban.GetBanData());
            Assert.AreEqual(false, notend);
        }
    }
}
