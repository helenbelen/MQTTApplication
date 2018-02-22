using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MQTTUserInterface.Models;

namespace MQTTUserInterface.Controllers
{
    public class HomeController : Controller
    {

        static DatabaseModel dbModel = new DatabaseModel();
        public IActionResult Index()
        {
            
            return View();
        }

        public IActionResult Devices()
        {
            ViewData["Message"] = "Your Devices.";

            return View();
        }

        public IActionResult Graphs()
        {
            ViewBag.DeviceList = new List<string>() { "Device One", "Two Device", "Three Device" }; //dbModel.GetAllDevices();
            ViewBag.GraphTypes = new List<string>() { "Bar", "Line" };
            
            ViewData["Message"] = "Page To Create Graphs";

            return View();
        }

        public IActionResult GraphResult(string deviceSelected, string graphType, string graphTitle)
        {
            ViewData["SelectedDevice"] = deviceSelected;
            ViewData["GraphTypeSelected"] = graphType;
            ViewBag.GraphTitle = graphTitle;
            ViewBag.Times = new List<string>() { "11/21/2017", "12/03/2018", "01/04/2019" };

            ViewData["Message"] = "Graph for " + ViewData["SelectedDevice"] ;

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
