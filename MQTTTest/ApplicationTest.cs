using Microsoft.VisualStudio.TestTools.UnitTesting;
using MQTTApplication;

using System.Collections;
using System.Collections.Generic;


namespace MQTTTest
{
    [TestClass]
    public class ApplicationTest
    {

        MQTTManager manager;
        
       

        public void TestSetUp()
        {
            manager = new MQTTManager();

            manager.publishData(new string[] { "sampleClient", "Hell0", "0", "true" });
        }
        public void TestBreakDown()
        {
            manager.Dispose();
        }

        [TestMethod]
        public void PublishMessage_ConfirmFormat()
        {

            this.TestSetUp();
            
                Assert.AreEqual(true, manager.checkDataFormat(RandomMessage()), "Manager is Not checking format of published messages correctly");
            this.TestBreakDown();
        }
        
       
        

        public string[] RandomMessage()
        {
            Dictionary<int, string[]> randomPhrases = new Dictionary<int, string[]>() { { 1, new string[] { "1111", "2018-04-18 04:18 04", "0.3" } } ,
                 { 2, new string[] { "1111", "2018-01-03 01:18 04", "20" } } ,
                 { 3, new string[] { "1212", "2018-04-18 04:18 04", "5" } } ,
                 { 4, new string[] { "1111", "2013-04-08 04:18 04", "3000" } } ,
                { 5, new string[] { "1333", "2018-04-18 04:18 04", "200" } } };

            System.Random random = new System.Random();
            return randomPhrases[random.Next(0, randomPhrases.Count)];


        }

       

    }
}
