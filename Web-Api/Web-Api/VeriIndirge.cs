using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web_Api.Models;

namespace Web_Api
{
    public class VeriIndirge
    {
        public static List<Coordinates> DouglasPeuckerReduction(List<Coordinates> Points, Double Tolerance)
        {
            if (Points == null || Points.Count < 3)
                return Points;

            Int32 firstPoint = 0;
            Int32 lastPoint = Points.Count - 1;
            List<Int32> pointIndexsToKeep = new List<Int32>();

            pointIndexsToKeep.Add(firstPoint);
            pointIndexsToKeep.Add(lastPoint);

            while (Points[firstPoint].Equals(Points[lastPoint]))
            {
                lastPoint--;
            }

            DouglasPeuckerReduction(Points, firstPoint, lastPoint,
            Tolerance, ref pointIndexsToKeep);

            List<Coordinates> returnPoints = new List<Coordinates>();
            pointIndexsToKeep.Sort();
            foreach (Int32 index in pointIndexsToKeep)
            {
                returnPoints.Add(Points[index]);
            }

            return returnPoints;
        }

        private static void DouglasPeuckerReduction(List<Coordinates> points, Int32 firstPoint, Int32 lastPoint, Double tolerance,
            ref List<Int32> pointIndexsToKeep)
        {
            Double maxDistance = 0;
            Int32 indexFarthest = 0;

            for (Int32 index = firstPoint; index < lastPoint; index++)
            {
                Double distance = PerpendicularDistance
                    (points[firstPoint], points[lastPoint], points[index]);
                if (distance > maxDistance)
                {
                    maxDistance = distance;
                    indexFarthest = index;
                }
            }

            if (maxDistance > tolerance && indexFarthest != 0)
            {

                pointIndexsToKeep.Add(indexFarthest);

                DouglasPeuckerReduction(points, firstPoint,
                indexFarthest, tolerance, ref pointIndexsToKeep);
                DouglasPeuckerReduction(points, indexFarthest,
                lastPoint, tolerance, ref pointIndexsToKeep);
            }
        }

        public static Double PerpendicularDistance (Coordinates Point1, Coordinates Point2, Coordinates Point)
        {

            Double area = Math.Abs(.5 * (Point1.lat * Point2.lng + Point2.lat *
            Point.lng + Point.lat * Point1.lng - Point2.lat * Point1.lng - Point.lat *
            Point2.lng - Point1.lat * Point.lng));
            Double bottom = Math.Sqrt(Math.Pow(Point1.lat - Point2.lat, 2) +
            Math.Pow(Point1.lng - Point2.lng, 2));
            Double height = area / bottom * 2;

            return height;
        }
    }
}