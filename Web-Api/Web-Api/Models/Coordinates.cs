﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_Api.Models
{
    public class Coordinates
    {
        public double lat { get; set; }
        public double lng { get; set; }

        public Coordinates()
        {

        }

        public Coordinates(double lat, double lng)
        {
            this.lng = lng;
            this.lat = lat;
        }
    }
}