using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Traveling
{
    class Program
    {
        static void Main(string[] args)
        {

            Point p0 = new Point(0, 0);
            Point p1 = new Point(200, 400);
            Point p2 = new Point(500, 302);
            Point p3 = new Point(601, 203);
            Point p4 = new Point(217, 12);
            Point p5 = new Point(59, 91);
            Point p6 = new Point(150, 300);
            Point p7 = new Point(300, 300);
            Point p8 = new Point(350, 150);
            Point p9 = new Point(400, 150);

            List<Point> pointList = new List<Point> { p0, p1, p2, p3, p4, p5, p6, p7, p8, p9 };

            Console.WriteLine("Path length: " + PathLength(pointList));

            List<int> minIndexList = BruteForce(pointList);





            List<List<int>> intListList = BuildIntListList(pointList, minIndexList);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Travelling.Form1(intListList));


        }

        static List<int> BruteForce(List<Point> pointList)
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

            Console.WriteLine("Permutation List Complete");

            // --- Begin analyzing paths ---

            double minDistance = 1000000;
            double maxDistance = 0;

            List<int> minIndexList = new List<int> { };
            List<int> maxIndexList = new List<int> { };

            for (int i = 0; i < intListList.Count; i++)
            {
                List<Point> newOrder = new List<Point> { };
                for (int j = 0; j < pointList.Count; j++)
                {
                    newOrder.Add(pointList[intListList[i][j]]);
                }

                double path = PathLength(newOrder);

                if (minDistance > path)
                {
                    minDistance = path;
                    minIndexList.Clear();
                    minIndexList.AddRange(intListList[i]);

                }
                else if (maxDistance < path)
                {
                    maxDistance = path;
                    maxIndexList.Clear();
                    maxIndexList.AddRange(intListList[i]);
                }
            }

            Console.Write("\nSmallest path possible: " + Math.Round(minDistance, 2) + "\nUsing path: ");
            PrintPath(minIndexList);
            Console.Write("\nLongest path possible: " + Math.Round(maxDistance, 2) + "\nUsing path: ");
            PrintPath(maxIndexList);

            return minIndexList;
        }

        public static List<List<int>> BuildIntListList(List<Point> pointList, List<int> indexList)
        {
            List<List<int>> intListList = new List<List<int>> { };

            for (int i = 0; i < pointList.Count; i++)
            {
                List<int> intList = new List<int> { 0, 0, 0 };
                intList[0] = pointList[indexList[i]].x;
                intList[1] = pointList[indexList[i]].y;
                intList[2] = indexList[i];
                intListList.Add(intList);
            }
            return intListList;
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
            Console.WriteLine("Point " + intList[0]);
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
