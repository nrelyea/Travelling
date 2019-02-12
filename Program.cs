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

            Point p0 = new Point(0, 0);
            Point p1 = new Point(10, 10);
            Point p2 = new Point(10, 0);
            Point p3 = new Point(0, 10);
            Point p4 = new Point(20, 20);
            Point p5 = new Point(5, 5);
            Point p6 = new Point(20, 0);

            List<Point> pointList = new List<Point> { p0, p1, p2, p3, p4, p5, p6 };

            Console.WriteLine("Path length: " + PathLength(pointList));

            BruteForce(pointList);

        }

        static void BruteForce(List<Point> pointList)
        {
            // --- Create int list of all possible permutations of indexes of points ---

            String str = "";
            for (int i = 0; i < pointList.Count; i++)
            {
                str += i.ToString();
            }
            int n = str.Length;

            List<string> strList = StringPermute(str, 0, n - 1);

            List<List<int>> intListList = new List<List<int>> { };

            for (int i = 0; i < strList.Count; i++)
            {
                List<int> intList = strList[i].Select(x => Convert.ToInt32(x.ToString())).ToList();
                intListList.Add(intList);
            }

            //PrintIntListList(intListList);



            // --- Begin analyzing paths ---

            double minDistance = 10000;
            List<int> minIndexList = new List<int> { };

            for (int i = 0; i < intListList.Count; i++)
            {
                List<Point> newOrder = new List<Point> { };
                for (int j = 0; j < pointList.Count; j++)
                {
                    newOrder.Add(pointList[intListList[i][j]]);
                }

                if (minDistance > PathLength(newOrder))
                {
                    minDistance = PathLength(newOrder);
                    minIndexList.Clear();
                    minIndexList.AddRange(intListList[i]);

                }
            }

            Console.Write("\nSmallest path possible: " + minDistance + "\nUsing path: ");
            PrintPath(minIndexList);


        }
        public static List<string> StringPermute(String str, int l, int r)
        {
            List<string> lst = new List<string> { };

            if (l == r)
            {
                lst.Add(str);
            }

            else
            {
                for (int i = l; i <= r; i++)
                {
                    str = Swap(str, l, i);
                    lst.AddRange(StringPermute(str, l + 1, r));
                    str = Swap(str, l, i);
                }
            }

            return lst;
        }

        public static String Swap(String a, int i, int j)
        {
            char temp;
            char[] charArray = a.ToCharArray();
            temp = charArray[i];
            charArray[i] = charArray[j];
            charArray[j] = temp;
            string s = new string(charArray);
            return s;
        }

        static void PrintPath(List<int> intList)
        {
            for (int i = 0; i < intList.Count; i++)
            {
                Console.Write("Point " + intList[i] + " => ");
            }
            Console.WriteLine();
        }

        static void PrintIntListList(List<List<int>> lstlst)
        {
            for (int i = 0; i < lstlst.Count; i++)
            {
                PrintIntList(lstlst[i]);
            }
        }

        static void PrintIntList(List<int> lst)
        {
            for (int i = 0; i < lst.Count; i++)
            {
                Console.Write(lst[i] + ",");
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
