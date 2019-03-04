using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Traveling
{
    class Program
    {
        static void Main(string[] args)
        {

            Point p0 = new Point(100, 100);
            Point p1 = new Point(401, 401);
            Point p2 = new Point(152, 202);
            Point p3 = new Point(353, 203);
            Point p4 = new Point(200, 200);
            Point p5 = new Point(200, 300);
            Point p6 = new Point(300, 300);
            Point p7 = new Point(300, 400);
            Point p8 = new Point(350, 150);
            Point p9 = new Point(400, 150);

            List<Point> pointList = new List<Point> { p0, p1, p2, p3, p4, p5, p6, p7, p8, p9 };



            Console.WriteLine("Path length: " + PathLength(pointList));

            //List<int> indexList = OriginalOrder(pointList);
            //List<int> indexList = BruteForce(pointList);
            //List<int> indexList = ClosestPointNext(pointList);
            //List<int> indexList = SmallestFromSwaps(pointList);
            List<int> indexList = SmallestFromIntersectionCount(pointList);

            List<List<int>> intListList = BuildIntListList(pointList, indexList);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Travelling.Form1(intListList));




        }

        static List<int> SmallestFromSwaps(List<Point> pointList)
        {
            List<int> intList = OriginalOrder(pointList);

            List<Point> tempPointList = new List<Point> { };

            for (int i = 0; i < pointList.Count; i++)
            {
                int x = pointList[i].x;
                int y = pointList[i].y;
                Point newPoint = new Point(0, 0);
                newPoint.x = x;
                newPoint.y = y;
                tempPointList.Add(newPoint);
            }

            Console.WriteLine("Original:");
            PrintPointList(tempPointList);

            bool complete = false;

            double minPath = PathLength(tempPointList);

            int whileLoopCount = 0;

            while (!complete)
            {
                complete = true;

                for (int i = 0; i < tempPointList.Count; i++)
                {
                    for (int j = 0; j < tempPointList.Count; j++)
                    {
                        if (i != j)
                        {
                            SwapPoints(tempPointList, i, j);
                            SwapInts(intList, i, j);

                            double newPathLength = PathLength(tempPointList);

                            if (newPathLength < minPath)
                            {
                                complete = false;

                                minPath = newPathLength;

                                break;
                            }
                            else
                            {
                                SwapPoints(tempPointList, i, j);
                                SwapInts(intList, i, j);
                            }
                        }
                    }
                }



                whileLoopCount++;
            }

            Console.WriteLine("While loop count: " + whileLoopCount);

            Console.WriteLine("\nFinal Assessment:");
            PrintIntList(intList);
            Console.WriteLine("Path Length: " + Math.Round(minPath, 2));

            EvaluateResult(minPath);

            return intList;
        }

        static List<int> OriginalOrder(List<Point> pointList)
        {
            List<int> intList = new List<int> { };

            for (int i = 0; i < pointList.Count; i++)
            {
                intList.Add(i);
            }

            return intList;
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

            Console.WriteLine("Permutation List Complete: " + strList.Count + " permutations formed");

            // --- Begin analyzing paths ---

            double minDistance = 1000000;
            double maxDistance = 0;
            double sumDistance = 0;

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
                sumDistance += path;

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
            Console.Write("\nAverage path length: " + Math.Round((sumDistance / strList.Count), 2));





            JObject bruteForceLengths = new JObject(
                new JProperty("min", minDistance),
                new JProperty("max", maxDistance),
                new JProperty("average", (sumDistance / strList.Count))
            );

            File.WriteAllText(@"c:../../brute_force_lengths.json", bruteForceLengths.ToString());

            return minIndexList;
        }

        static List<int> ClosestPointNext(List<Point> pointList)
        {
            List<int> intList = new List<int> { };

            List<Point> newPointList = new List<Point> { };

            newPointList.Add(pointList[0]);
            intList.Add(0);

            for (int i = 0; i < pointList.Count - 1; i++)
            {
                double min = 10000;
                int minIndex = i + 1;
                Point minPoint = new Point(0, 0);

                for (int j = 0; j < pointList.Count; j++)
                {
                    if (Distance(newPointList[i], pointList[j]) < min && !(intList.Contains(j)))
                    {
                        min = Distance(newPointList[i], pointList[j]);
                        minIndex = j;
                        minPoint.x = pointList[j].x;
                        minPoint.y = pointList[j].y;
                    }
                }
                Console.WriteLine("Point " + intList[i] + " is closest to Point " + minIndex);
                intList.Add(minIndex);
                newPointList.Add(minPoint);

            }

            Console.Write("\nPath Formed: ");
            PrintPath(intList);
            Console.WriteLine("Path Distance: " + Math.Round(PathLength(newPointList), 2));

            EvaluateResult(PathLength(newPointList));

            return intList;
        }



        static List<int> SmallestFromIntersectionCount(List<Point> pointList)
        {
            List<int> intList = OriginalOrder(pointList);

            List<Point> tempPointList = new List<Point> { };

            for (int i = 0; i < pointList.Count; i++)
            {
                int x = pointList[i].x;
                int y = pointList[i].y;
                Point newPoint = new Point(0, 0);
                newPoint.x = x;
                newPoint.y = y;
                tempPointList.Add(newPoint);
            }

            Console.WriteLine("Original:");
            PrintPointList(tempPointList);

            bool complete = false;

            int minIntersections = IntersectionCount(tempPointList);

            int whileLoopCount = 0;

            while (!complete)
            {
                complete = true;

                for (int i = 0; i < tempPointList.Count; i++)
                {
                    for (int j = 0; j < tempPointList.Count; j++)
                    {
                        if (i != j)
                        {
                            SwapPoints(tempPointList, i, j);
                            SwapInts(intList, i, j);

                            int newIntersectionCount = IntersectionCount(tempPointList);

                            if (newIntersectionCount < minIntersections)
                            {
                                complete = false;

                                minIntersections = newIntersectionCount;

                                break;
                            }
                            else
                            {
                                SwapPoints(tempPointList, i, j);
                                SwapInts(intList, i, j);
                            }
                        }
                    }
                }



                whileLoopCount++;
            }

            Console.WriteLine("While loop count: " + whileLoopCount);

            Console.WriteLine("\nFinal Assessment:");
            PrintIntList(intList);
            double minPath = PathLength(tempPointList);
            Console.WriteLine("Path Length: " + Math.Round(minPath, 2));

            EvaluateResult(minPath);

            return intList;
        }


        static void EvaluateResult(double resultDistance)
        {
            JObject bruteForceLengths = JObject.Parse(File.ReadAllText(@"c:../../brute_force_lengths.json"));
            double bruteForceMin = (double)(bruteForceLengths).GetValue("min");
            double bruteForceMax = (double)(bruteForceLengths).GetValue("max");
            double bruteForceAverage = (double)(bruteForceLengths).GetValue("average");

            double averageDev = Math.Round((((bruteForceAverage - bruteForceMin) / (bruteForceMax - bruteForceMin)) * 100), 3);
            double evalDevMax = Math.Round((((resultDistance - bruteForceMin) / (bruteForceMax - bruteForceMin)) * 100), 3);
            double evalDevAvg = Math.Round((((resultDistance - bruteForceMin) / (bruteForceAverage - bruteForceMin)) * 100), 3);


            Console.WriteLine("\n\n--- Comparison to Brute Force Results ---\n");
            Console.WriteLine("Brute force min:     " + Math.Round(bruteForceMin, 2) + "\tDeviation from min->max: 0.000% \tDeviation from min->avg: 0.000%");
            Console.WriteLine("EVALUATED DISTANCE:  " + Math.Round(resultDistance, 2) + "\tDeviation from min->max: " + evalDevMax + "% \tDeviation from min->avg: " + evalDevAvg + "%");
            Console.WriteLine("Brute force average: " + Math.Round(bruteForceAverage, 2) + "\tDeviation from min->max: " + averageDev + "% \tDeviation from min->avg: 100%");
            Console.WriteLine("Brute force max:     " + Math.Round(bruteForceMax, 2) + "\tDeviation from min->max: 100%");




        }

        static int IntersectionCount(List<Point> pointList)
        {
            int count = 0;

            List<int> intList = OriginalOrder(pointList);

            for (int i = 0; i < pointList.Count; i++)
            {
                for (int j = i + 2; j < pointList.Count - 1; j++)
                {
                    if (AreIntersecting(pointList[i], pointList[i + 1], pointList[j], pointList[j + 1]))
                    {
                        //Console.WriteLine("p" + intList[i] + " --- p" + intList[i + 1] + " intersects p" + intList[j] + " --- p" + intList[j + 1]);
                        count++;
                    }
                }
            }

            for (int i = 1; i < pointList.Count - 2; i++)
            {
                if (AreIntersecting(pointList[pointList.Count - 1], pointList[0], pointList[i], pointList[i + 1]))
                {
                    Console.WriteLine("p" + intList[pointList.Count - 1] + " --- p" + intList[0] + " intersects p" + intList[i] + " --- p" + intList[i + 1]);
                    count++;
                }
            }

            return count;
        }

        static bool AreIntersecting(Point start1, Point end1, Point start2, Point end2)
        {
            if (Math.Max(start1.x, end1.x) < Math.Min(start2.x, end2.x))
            {
                return false;
            }

            double A1 = ((double)start1.y - (double)end1.y) / ((double)start1.x - (double)end1.x);
            double A2 = ((double)start2.y - (double)end2.y) / ((double)start2.x - (double)end2.x);

            if (A1 == A2)
            {
                return false;
            }

            double b1 = (double)start1.y - (A1 * (double)start1.x);
            double b2 = (double)start2.y - (A2 * (double)start2.x);

            double Xa = (b2 - b1) / (A1 - A2);

            double iMax = Math.Max(Math.Min(start1.x, end1.x), Math.Min(start2.x, end2.x));
            double iMin = Math.Min(Math.Max(start1.x, end1.x), Math.Max(start2.x, end2.x));
            if (Xa < iMax || Xa > iMin)
            {
                return false;
            }

            return true;
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

        static void PrintPointList(List<Point> pointList)
        {
            for (int i = 0; i < pointList.Count; i++)
            {
                Console.Write(pointList[i].x + "," + pointList[i].y + "    ");
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

        static List<Point> SwapPoints(List<Point> pointList, int index1, int index2)
        {
            Point tempPoint = new Point(pointList[index1].x, pointList[index1].y);

            pointList[index1].x = pointList[index2].x;
            pointList[index1].y = pointList[index2].y;

            pointList[index2].x = tempPoint.x;
            pointList[index2].y = tempPoint.y;

            return pointList;
        }

        static List<int> SwapInts(List<int> intList, int index1, int index2)
        {
            int temp = intList[index1];
            intList[index1] = intList[index2];
            intList[index2] = temp;

            return intList;
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
