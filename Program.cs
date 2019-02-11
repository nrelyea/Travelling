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

            //List<int> lst = new List<int> { 1, 2, 3 };
            //List<List<int>> Permutations = Perms(lst);
            String str = "123";
            int n = str.Length;
            PrintList(Permute(str, 0, n - 1));

        }
        public static List<string> Permute(String str, int l, int r)
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
                    lst.AddRange(Permute(str, l + 1, r));
                    str = Swap(str, l, i);
                }
            }

            return lst;
        }

        /** 
        * Swap Characters at position 
        * @param a string value 
        * @param i position 1 
        * @param j position 2 
        * @return swapped string 
        */
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

        static void PrintListList(List<List<string>> lstlst)
        {
            for (int i = 0; i < lstlst.Count; i++)
            {
                PrintList(lstlst[i]);
            }
        }

        static void PrintList(List<string> lst)
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
