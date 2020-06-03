using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class JsonTest
    {
        public class TestParent
        {
            public int num;
            public Vector2Int vec;
        }

        public class TestChild : TestParent
        {
            public int childNum;
        }

        [Test]
        public void Test_Tojson()
        {
            var tp = new TestParent();
            tp.num = 5;
            var json = JsonConverter.ToJson(tp);
            var data = JsonConverter.FromJson<TestParent>(json);
            Assert.AreEqual(5, data.num);
        }
        [Test]
        public void Test_TojsonFull()
        {
            var tp = new TestChild();
            tp.num = 5;
            tp.childNum = 10;
            var json = JsonConverter.ToJson_full(tp);
            var data = JsonConverter.FromJson_full<TestChild>(json);
            Assert.AreEqual(5, data.num);
            Assert.AreEqual(10, (data as TestChild).childNum);
        }
        [Test]
        public void Test_TojsonFull2()
        {
            var tp = new TestParent();
            tp.num = 5;
            var json = JsonConverter.ToJson_full(tp);
            var data = JsonConverter.FromJson_full<TestChild>(json);
            Assert.AreEqual(5, data.num);
            Assert.AreNotEqual(1, data.childNum);
        }
        //[Test]
        //public void Test_TojsonFull3()
        //{
        //    var tp = new TestParent();
        //    tp.vec = new Vector2Int(1,1);
        //    var json = JsonConverter.ToJson_full(tp);
        //    var data = JsonConverter.FromJson_full<TestChild>(json);
        //    Assert.AreEqual(1,data.vec.x);
        //}
    }
}
