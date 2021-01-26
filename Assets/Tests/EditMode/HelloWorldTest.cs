using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class HelloWorldTest
    {

        [Test]
        public void HelloWorldTestSimplePasses()
        {
            Assert.That(true);
        }

        [Test]
        public void CategoriesFromXMLContains()
        {
            Assert.Contains("animal",WordsContainer.GetAllCategories());
        }
        
        [Test]
        public void CategoriesFromXMLNoDuplicates()
        {
            Assert.AreEqual(2,WordsContainer.GetAllCategories());
        }

    }
}
