using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MQTTAPI.Controllers;
//using MQTTAPI.Models;

namespace MQTTTest
{
    [TestClass]
    public class APITest
    {
       /*
        TestDeviceDataController testController;
        List<DeviceData> testData;
        public void TestSetup()
        {
            testData = GetTestData();
            testController = new TestDeviceDataController(testData);
           

        }
     
        [TestMethod]
        public void GetAllDeviceData()
        {

            TestSetup();
            var result = testController.Get() as List<DeviceData>;
            Assert.AreEqual(testData.Count, result.Count, "Test Controller is not returning all of the test data");
        }

        [TestMethod]
        public void GetDeviceByID()
        {
            TestSetup();
            Random random = new Random();
            int devicetoGet = random.Next(0, testData.Count);
            int deviceiD = testData[devicetoGet].DeviceId;
            ObjectResult result = (ObjectResult)testController.Get(deviceiD);
            
            Assert.IsNotNull(result);

            DeviceData d = (DeviceData)result.Value;
            Assert.AreEqual(deviceiD, d.DeviceId, "Test Controller Did Not Find The Device ID");
        }

        [TestMethod]
        public void GetDeviceByID_ShouldBeNull()
        {
            TestSetup();
            int deviceiD = 999;
            var result = testController.Get(deviceiD) as DeviceData;
            Assert.IsNull(result, "Test Controller Did Not Find The Device ID");

        }


        private List<DeviceData> GetTestData()
        {
            var testData = new List<DeviceData>();
            testData.Add(new DeviceData { DeviceId = 011, Timestamp = new DateTime(01/03/2017), Data="Welcome" });
            testData.Add(new DeviceData { DeviceId = 012, Timestamp = new DateTime(01 / 03 / 2017), Data = "Hey" });
            testData.Add(new DeviceData { DeviceId = 013, Timestamp = new DateTime(01 / 04 / 2017), Data = "Bye" });
            testData.Add(new DeviceData { DeviceId = 014, Timestamp = new DateTime(01 / 05 / 2017), Data = "Thanks" });

            return testData;

        }*/

    }
}
