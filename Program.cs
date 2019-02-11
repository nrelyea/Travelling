using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Traveling
{
    class Program
    {
        static void Main(string[] args)
        {

            Point p1 = new Point(1, 1);
            Point p2 = new Point(2, 2);
            Point p3 = new Point(3, 3);
            //Point p4 = new Point(0, 10);

            List<Point> pointList = new List<Point> { p1, p2, p3 };

            Console.WriteLine("Path length: " + PathLength(pointList));

            BruteForce(pointList);

        }

        static void BruteForce(List<Point> pointList)
        {

            List<List<Point>> allLists = AllLists(pointList);

            //PrintAllLists(allLists);
        }

        static List<List<Point>> AllLists(List<Point> pointList)
        {
            List<List<Point>> all = new List<List<Point>> { };

            for (int i = 0; i < pointList.Count; i++)
            {
                PrintList(pointList);

                Point headPoint = pointList[i];

                pointList.RemoveAt(i);

                List<List<Point>> subListList = new List<List<Point>> { };
                subListList.AddRange(AllLists(pointList));

                for (int j = 0; j < subListList.Count; j++)
                {
                    subListList[j].Insert(0, headPoint);
                }

                all.AddRange(subListList);

                Console.WriteLine("subListList length: " + subListList.Count);
            }

            

            return all;
        }

        static void PrintList(List<Point> lst)
        {
            for(int i = 0; i < lst.Count; i++)
            {
                Console.Write(lst[i].x + ",");
            }
        }

        static double PathLength(List<Point> pointList)
        {
            double length = 0;

            for(int i = 0; i < pointList.Count-1; i++)
            {
                length += Distance(pointList[i], pointList[i + 1]);
            }

            length += Distance(pointList[pointList.Count-1], pointList[0]);

            return length;
        }

        static double Distance(Point p1, Point p2)
        {
            int xds = Square(p1.x - p2.x);
            int yds = Square(p1.y - p2.y);

            return Math.Sqrt(xds+yds);
        }

        static int Square(int a)
        {
            return a * a;
        }
    }

    public class Point
    {
        public int x { get; set; }
        public int y { get; set; }

        public Point(int xcoord, int ycoord)
        {
            x = xcoord;
            y = ycoord;
        }
    }
}
