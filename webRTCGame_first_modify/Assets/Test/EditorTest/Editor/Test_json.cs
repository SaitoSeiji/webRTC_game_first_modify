using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class Test_json
    {
        [System.Serializable]
        public class TestClass
        {
            public int a;
            public string b;
        }

        [Test]
        public void Test_jsonConvert_full()
        {
            //var json = JsonConverter.ToJson_full(remoteMessage);
            var origine = new TestClass();
            origine.a = 100;
            var json = JsonConverter.ToJson_full(origine);
            var data = JsonConverter.FromJson_full<TestClass>(json);

            Assert.AreEqual(origine.a, data.a);
        }
        //[Test]
        //public void Test_sendOceloMessage()
        //{
        //    var remoteMessage = new GameControlMessage_putKoma(Koma_ocelo.KomaType.Black, new Vector2Int(0, 0));
        //    //var json = JsonConverter.ToJson_full(remoteMessage);
        //    var json = JsonConverter.ToJson(new OceloMessage(remoteMessage));
        //    var data = JsonConverter.FromJson<OceloMessage>(json);

        //    Assert.AreEqual(remoteMessage.type, data.GetMessageType());
        //}

    }
}
