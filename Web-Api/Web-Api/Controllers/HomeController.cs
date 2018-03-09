using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Web_Api.Models;

namespace Web_Api.Controllers
{
    public class HomeController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage Tarih()
        {

            return Request.CreateResponse(HttpStatusCode.OK, DateTime.Now.ToString("dd-MM-yyyy"));
        }


        [HttpPost]
        public HttpResponseMessage DataSimplify([FromBody]string data)
        {
            List<Coordinates> coordinates = new List<Coordinates>();

            JObject myObject = JObject.Parse(data);

            double tolerans = (double) myObject.SelectToken("tolerans");

            JArray a = JArray.Parse(myObject.SelectToken("coordinates").ToString());

            foreach (JObject o in a.Children<JObject>())
            {
                var coordinate = new Coordinates();
                foreach (JProperty p in o.Properties())
                {
                    string name = p.Name;
                    if (name == "lat")
                    {
                        coordinate.lat = Convert.ToDouble(p.Value);
                    }
                    else
                    {
                        coordinate.lng = Convert.ToDouble(p.Value);
                    }
                }
                coordinates.Add(coordinate);
            }


            var simplifiedData = VeriIndirge.DouglasPeuckerReduction(coordinates, tolerans);

            return Request.CreateResponse(HttpStatusCode.OK, simplifiedData);
        }

        [HttpPost]
        public HttpResponseMessage AramaHam([FromBody]string data)
        {
            List<Coordinates> coordinates = new List<Coordinates>();

            JObject myObject = JObject.Parse(data);

            string limit = myObject.SelectToken("tolerans").ToString();

            JArray a = JArray.Parse(myObject.SelectToken("kordinatlar").ToString());

            foreach (JObject o in a.Children<JObject>())
            {
                var coordinate = new Coordinates();
                foreach (JProperty p in o.Properties())
                {
                    string name = p.Name;
                    if (name == "lat")
                    {
                        coordinate.lat = Convert.ToDouble(p.Value);
                    }
                    else
                    {
                        coordinate.lng = Convert.ToDouble(p.Value);
                    }
                }
                coordinates.Add(coordinate);
            }

            var limits = CoordinateParse(data);

            return Request.CreateResponse(HttpStatusCode.OK, "basarili");
        }

        [HttpPost]
        public HttpResponseMessage AramaIndirgenmis([FromBody]string data)
        {
            var coordinates = CoordinateParse(data);

            return Request.CreateResponse(HttpStatusCode.OK, "basarili");
        }

        private double[] CoordinateParse(string data)
        {
            //(61.16889734905685, -52.559965499999976) (42.267893574814714, -116.01699674999998)
            data = data.Replace("(", "").Replace(")", ",").Replace(" ", "");
            string[] dummy = data.Split(',');
            double[] coordinates = new double[dummy.Length-1];
            for (int i = 0; i < coordinates.Length; i++)
            {
                coordinates[i] = Convert.ToDouble(dummy[i]);
            }

            return coordinates;
        }
    }

}
