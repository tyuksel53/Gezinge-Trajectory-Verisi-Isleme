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
            List<Dictionary<string,string>> zundi = new List<Dictionary<string,string>>();
            var kads = new Dictionary<string,string>();
            kads.Add("mundi","zundi");
            kads.Add("zundi","kundi");
            kads.Add("kundi","yundi");
            return Request.CreateResponse(HttpStatusCode.OK, kads );
        }


        [HttpPost]
        public HttpResponseMessage DataSimplify([FromBody]string data)
        {
            List<Coordinates> coordinates = new List<Coordinates>();

            JArray a = JArray.Parse(data);

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

            var simplifiedData = VeriIndirge.SimplifyLine(coordinates, 1);

            return Request.CreateResponse(HttpStatusCode.OK, simplifiedData);
        }
    }
}
