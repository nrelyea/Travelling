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

            List<Point> pointList = new List<Point> { p1, p2 };

            Console.WriteLine("Path length: " + PathLength(pointList));

            BruteForce(pointList);

        }

        static void BruteForce(List<Point> pointList)
        {

            //List<List<Point>> allLists = Perm(pointList);

            //List<List<int>> Permutations = Perms(4);

        }

        static List<List<Point>> Perm(List<Point> pointList)
        {


            if (pointList.Count == 1)
            {
                Console.WriteLine("Returning " + pointList[0].x);
                List<List<Point>> ret = new List<List<Point>> { };
                ret.Add(pointList);
                return ret;
            }

            List<List<Point>> all = new List<List<Point>> { };

            for (int i = 0; i < pointList.Count; i++)
            {
                Console.WriteLine("Starting All:");
                for (int k = 0; k < all.Count; k++)
                {
                    PrintList(all[k]);
                }

                Point headPoint = pointList[i];

                pointList.RemoveAt(i);

                Console.Write("\nHead Point: " + headPoint.x + "\tRemaining List: ");
                PrintList(pointList);

                List<List<Point>> subListList = new List<List<Point>> { };
                subListList.AddRange(Perm(pointList));

                Console.WriteLine("Current pre All:");
                for (int k = 0; k < all.Count; k++)
                {
                    PrintList(all[k]);
                }

                for (int j = 0; j < subListList.Count; j++)
                {
                    subListList[j].Insert(0, headPoint);
                }
                Console.WriteLine("Sublistlist: ");
                for (int k = 0; k < subListList.Count; k++)
                {
                    PrintList(subListList[k]);
                }

                Console.WriteLine("Current All:");
                for (int k = 0; k < all.Count; k++)
                {
                    PrintList(all[k]);
                }

                all.AddRange(subListList);
                Console.WriteLine("All List: ");
                for (int k = 0; k < all.Count; k++)
                {
                    PrintList(all[k]);
                }

                //Console.WriteLine("subListList length: " + subListList.Count);
            }



            return all;
        }

        static void PrintList(List<Point> lst)
        {
            for (int i = 0; i < lst.Count; i++)
            {
                Console.Write(lst[i].x + ",");
            }
            Console.WriteLine();
        }

        static double PathLength(List<Point> pointList)
        {
            double length = 0;

            for (int i = 0; i < pointList.Count - 1; i++)
            {
                length += Distance(pointList[i], pointList[i + 1]);
            }

            length += Distance(pointList[pointList.Count - 1], pointList[0]);

            return length;
        }

        static double Distance(Point p1, Point p2)
        {
            int xds = Square(p1.x - p2.x);
            int yds = Square(p1.y - p2.y);

            return Math.Sqrt(xds + yds);
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
