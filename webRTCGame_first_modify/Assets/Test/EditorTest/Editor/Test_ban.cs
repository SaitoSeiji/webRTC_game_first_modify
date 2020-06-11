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
        [System.Serializable]
        public class Test_jsonClass
        {
            [SerializeField] List<List<int>> ban;

            public Test_jsonClass(int size)
            {
                ban = new List<List<int>>();
                for (int i = 0; i < size; i++) ban.Add(new List<int>());
                ban.ForEach(x =>
                {
                    for (int i = 0; i < size; i++) x.Add(0);
                });
            }

            public int this[int x,int y]
            {
                get
                {
                    return ban[x][y];
                }
                set
                {
                    ban[x][y] = value;
                }
            }
        }

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

        [Test]
        public void Test_testHairetujson()
        {
            var ban = new Test_jsonClass(5);
            ban[1, 1] = 5;
            var json = JsonConverter.ToJson_full(ban);
            Debug.Log(json);
            var data = JsonConverter.FromJson_full<Test_jsonClass>(json);
            Assert.AreEqual(ban[1, 1], data[1, 1]);
        }
        [Test]
        public void Test_banNew()
        {
            var ban = new Ban_new(5);
            ban[1, 1] = new Koma_ocelo(Koma_ocelo.KomaType.Black);
            var json = JsonConverter.ToJson_full(ban);
            Debug.Log(json);
            var data = JsonConverter.FromJson_full<Ban_new>(json);
            Assert.AreEqual(Koma_ocelo.KomaType.Black, data[1, 1]._Type);
        }

        [Test]
        public void Test_banNew_syncBan()
        {
            var ban = new Ban_new(5);
            var input = new int[,] { { 0,0,0,0,0}
                                    ,{ 0,1,0,2,0}
                                    ,{ 0,0,1,0,0}
                                    ,{ 0,2,0,1,0}
                                    ,{ 0,0,0,0,0} };
            ban.SetBan(input);
            Assert.AreEqual(input[2, 2], (int)ban[2, 2]._Type);
        }
    }
}
