using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Web_Client.Models;

namespace Web_Client.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult getLatLong(HttpPostedFileBase uploadFile)
        {
            string[] lines;
            if (uploadFile != null)
            {
                string fileFullPath = "";
                bool control = false;
                try
                {
                    string filePath = Path.GetFileName(uploadFile.FileName);
                    fileFullPath = Path.Combine(Server.MapPath("~/Uploads"), filePath);
                    uploadFile.SaveAs(fileFullPath);
                    control = true;
                }
                catch (Exception e)
                {
                    
                }

                if (control == true)
                {
                    lines = System.IO.File.ReadAllLines(fileFullPath);
                    List<Coordinates> coordinates = new List<Coordinates>();
                    for (int i = 0; i < lines.Length; i++)
                    {
                        var coordinate = lines[i].Split(',');
                        Coordinates newCoordinates = new Coordinates()
                        {
                            lat = Convert.ToDouble(coordinate[0]),
                            lng = Convert.ToDouble(coordinate[1])
                        };
                        coordinates.Add(newCoordinates);
                    }

                    return Json( JsonConvert.SerializeObject(coordinates) );
                }
                
            }

            
            return Json(null);
        }
    }
}