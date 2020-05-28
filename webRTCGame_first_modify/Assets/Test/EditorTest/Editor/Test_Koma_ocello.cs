using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    
    public class Test_Koma_ocello
    {
        private Koma_ocelo _myKoma;

        [SetUp]
        public void SetUp()
        {
            _myKoma = new Koma_ocelo(Koma_ocelo.Type.Black);
        }

        [TestCase(Koma_ocelo.Type.Black)] 
        [TestCase(Koma_ocelo.Type.White)]
        public void Test_Reverse(Koma_ocelo.Type type)
        {
            _myKoma.SetType(type);
            _myKoma.Reverse();
            Assert.AreNotEqual(type, _myKoma);
        }
    }
}
