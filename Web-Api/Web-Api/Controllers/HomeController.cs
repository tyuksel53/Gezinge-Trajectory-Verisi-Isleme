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
        public static QuadTree hamRoot = null;
        public static QuadTree IndirgenmisRoot = null;
        public static List<Coordinates> HamArananlar = null;
        public static List<Coordinates> IndirgenmisArananlar = null;

        [HttpPost]
        public HttpResponseMessage DataSimplify([FromBody]string data)
        {


            List<Coordinates> coordinates = JsonArrayParse("coordinates", data);
            double tolerans =  Convert.ToDouble(JsonParseSingleProperty("tolerans", data));
            
            var simplifiedData = VeriIndirge.DouglasPeuckerReduction(coordinates, tolerans);

            return Request.CreateResponse(HttpStatusCode.OK, simplifiedData);
        }

        [HttpPost]
        public HttpResponseMessage AramaHam([FromBody]string data)
        {
            HamArananlar = new List<Coordinates>();
            hamRoot = null;

            List<Coordinates> coordinates = JsonArrayParse("kordinatlar", data);
            string limit = JsonParseSingleProperty("limit", data);

            var limits = CoordinateParse(limit);

            for (int i = 0; i < coordinates.Count; i++)
            {
                Insert(coordinates[i], isHamRoot:true);
            }

            HamSearch(hamRoot, limits);

            return Request.CreateResponse(HttpStatusCode.OK, HamArananlar);
        }

        [HttpPost]
        public HttpResponseMessage AramaIndirgenmis([FromBody]string data)
        {
            IndirgenmisArananlar = new List<Coordinates>();
            IndirgenmisRoot = null;
            List<Coordinates> coordinates = JsonArrayParse("kordinatlar", data);
            string limit = JsonParseSingleProperty("limit", data);

            var limits = CoordinateParse(limit);

            for (int i = 0; i < coordinates.Count; i++)
            {
                Insert(coordinates[i], isHamRoot:false);
            }

            IndirgenmisSearch(IndirgenmisRoot,limits);

            return Request.CreateResponse(HttpStatusCode.OK, IndirgenmisArananlar);
        }

        private string JsonParseSingleProperty(string token,string data)
        {
            List<Coordinates> coordinates = new List<Coordinates>();

            JObject myObject = JObject.Parse(data);

            string limit = myObject.SelectToken(token).ToString();

            return limit;
        }

        private List<Coordinates> JsonArrayParse(string token, string data)
        {
            List<Coordinates> coordinates = new List<Coordinates>();

            JObject myObject = JObject.Parse(data);

            JArray a = JArray.Parse(myObject.SelectToken(token).ToString());

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

            return coordinates;
        }

        private double[] CoordinateParse(string data)
        {
            data = data.Replace("(", "").Replace(")", ",").Replace(" ", "");
            string[] dummy = data.Split(',');
            double[] coordinates = new double[dummy.Length-1];
            for (int i = 0; i < coordinates.Length; i++)
            {
                coordinates[i] = Convert.ToDouble(dummy[i]);
            }

            return coordinates;
        }

        private void Insert(Coordinates newCoordinate,bool isHamRoot)
        {
            var newQuadTreeNode = new QuadTree(newCoordinate.lat, newCoordinate.lng);
            QuadTree current = null;
            QuadTree parent = null;

            if (isHamRoot)
            {
                if (hamRoot == null)
                {
                    hamRoot = newQuadTreeNode;
                    return;
                }

                current = hamRoot;
            }
            else
            {
                if (IndirgenmisRoot == null)
                {
                    IndirgenmisRoot = newQuadTreeNode;
                    return;
                }

                current = IndirgenmisRoot;
            }
           
            while (true)
            {
                parent = current;
                if (newCoordinate.lat < current.lat && newCoordinate.lng < current.lng)
                {
                    current = current.Qtx1y1;
                    if (current == null)
                    {
                        parent.Qtx1y1 = newQuadTreeNode;
                        return;
                    }
                }
                else if (newCoordinate.lat < current.lat && newCoordinate.lng > current.lng)
                {
                    current = current.Qtx1y2;
                    if (current == null)
                    {
                        parent.Qtx1y2 = newQuadTreeNode;
                        return;
                    }
                }else if (newCoordinate.lat > current.lat && newCoordinate.lng > current.lng)
                {
                    current = current.Qtx2y1;
                    if (current == null)
                    {
                        parent.Qtx2y1 = newQuadTreeNode;
                        return;
                    }
                }else if (newCoordinate.lat > current.lat && newCoordinate.lng < current.lng)
                {
                    current = current.Qtx2y2;
                    if (current == null)
                    {
                        parent.Qtx2y2 = newQuadTreeNode;
                        return;
                    }
                }
            }
        }

        private void HamSearch(QuadTree node,double[] limits)
        {
            if (node != null)
            {
                if (limits[0] > node.lat && limits[1] > node.lng && limits[2] < node.lat && limits[3] < node.lng)
                {
                    HamArananlar.Add(new Coordinates(node.lat,node.lng));
                }

                HamSearch(node.Qtx1y1,limits);
                HamSearch(node.Qtx1y2, limits);
                HamSearch(node.Qtx2y1, limits);
                HamSearch(node.Qtx2y2, limits);
            }

        }

        private void IndirgenmisSearch(QuadTree node, double[] limits)
        {
            if (node != null)
            {
                if (limits[0] > node.lat && limits[1] > node.lng && limits[2] < node.lat && limits[3] < node.lng)
                {
                    IndirgenmisArananlar.Add(new Coordinates(node.lat, node.lng));
                }

                IndirgenmisSearch(node.Qtx1y1, limits);
                IndirgenmisSearch(node.Qtx1y2, limits);
                IndirgenmisSearch(node.Qtx2y1, limits);
                IndirgenmisSearch(node.Qtx2y2, limits);
            }

        }
    }

}
