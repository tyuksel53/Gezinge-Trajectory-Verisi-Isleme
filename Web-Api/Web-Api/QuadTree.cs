using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web_Api.Models;

namespace Web_Api
{
    public class QuadTree
    {
        public double lat;
        public double lng;
        public QuadTree Qtx1y1, Qtx1y2, Qtx2y1, Qtx2y2;

        public QuadTree(double lat,double lng)
        {
            this.lat = lat;
            this.lng = lng;
            this.Qtx1y1 = null;
            this.Qtx1y2 = null;
            this.Qtx2y1 = null;
            this.Qtx2y1 = null;
            this.Qtx2y2 = null;
        }

    }
}