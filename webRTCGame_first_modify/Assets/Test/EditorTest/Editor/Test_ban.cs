using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Linq;
namespace Tests
{
    public class Test_ban
    {
        Ban_new ban;

        [SetUp]
        public void SetUp()
        {
            ban = new Ban_new(8);

            var input = new int[,] {
            // 1 2 3 4 5 6 7 8
             { 0,0,0,0,0,0,0,0}//1
            ,{ 0,0,0,0,0,0,0,0}//2
            ,{ 0,0,0,0,0,0,0,0}//3
            ,{ 0,0,0,1,2,0,0,0}//4
            ,{ 0,0,0,2,1,0,0,0}//5
            ,{ 0,0,0,0,0,0,0,0}//6
            ,{ 0,0,0,0,0,0,0,0}//7
            ,{ 0,0,0,0,0,0,0,0}//8
            };
            ban.SetBan(input);
        }
        
        [Test]
        public void Test_setBan()
        {
            ban = new Ban_new(2);
            var input = new int[,]
            {
                {1,2 }
               ,{2,1 }
            };
            ban.SetBan(input);
            Assert.AreEqual(1, ban[0, 0]);
            Assert.AreEqual(2, ban[1, 0]);
        }

        [Test]
        public void Test_getBanData()
        {
            var get = ban.GetBanData();
            Assert.AreEqual(1, get[3, 3]);
        }
    }
}
