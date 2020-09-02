using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Kraskal
{
    class Program
    {
        static void Main(string[] args)
        {
            // Пример графа: a e 1; c d 2; a b 3; b e 4; b c 5; e c 6; e d 7;
            Console.WriteLine("Введите граф");
            string str;
            Regex temp = new Regex(@"^\s?\w+\s{1}\w+\s{1}[1-9]\d*\s?$");
            bool input = false;
            List<string[]> graf;
            List<string[]> v;
            do
            {
                input = true;
                str = Console.ReadLine(); // вершина вершина вес;
                graf = new List<string[]>();
                v = new List<string[]>();
                if (str == "")
                {
                    Console.WriteLine("Введите граф");
                    input = false;
                }
                else
                {
                    if (str.Substring(str.Length - 1) == ";")
                    {
                        str = str.Remove(str.Length - 1, 1);
                    }

                    string[] arr = str.Split(';');

                    for (int i = 0; i < arr.Length; i++)
                    {
                        MatchCollection matches = temp.Matches(arr[i]);
                        if (matches.Count == 1)
                        {
                            graf.Add(arr[i].Trim(' ').Split(' '));
                        }
                        else
                        {
                            Console.WriteLine("Граф введен неверно");
                            input = false;
                            break;
                        }
                    }
                    if (input)
                    {
                        for (int i = 0; i < graf.Count; i++)
                        {
                            if (graf[i][0] == graf[i][1])
                            {
                                Console.WriteLine("Петля в графе");
                                input = false;
                                break;
                            }
                        }
                        for (int i = 0; i < 2; i++)
                        {
                            string[] vItem = { graf[0][i], "1" };
                            v.Add(vItem);
                        }

                        for (int i = 1; i < graf.Count; i++)
                        {
                            for (int V = 0; V < 2; V++)
                            {
                                bool flag = false;
                                for (int l = 0; l < v.Count; l++)
                                {
                                    if (graf[i][V] == v[l][0])
                                    {
                                        flag = true;
                                        break;
                                    }
                                }
                                if (!flag)
                                {
                                    string[] vItem = { graf[i][V], "1" };
                                    v.Add(vItem);
                                }
                            }
                        }
                        // Проверка на дубликаты и ориентацию
                        for (int i = 0; i < graf.Count; i++)
                        {
                            for (int l = i + 1; l < graf.Count; l++)
                            {
                                if (graf[i][0] == graf[l][1] && graf[i][1] == graf[l][0])
                                {
                                    Console.WriteLine("Граф не должен быть ориентированым");
                                    input = false;
                                }
                                if (graf[i][0] == graf[l][0] && graf[i][1] == graf[l][1])
                                {
                                    Console.WriteLine("В графе не должны дублироваться ребра");
                                    input = false;
                                }
                            }
                            if (!input)
                            {

                                break;
                            }
                        }
                        // Проверка на связность
                        Queue<string[]> vConnected = new Queue<string[]>();
                        vConnected.Enqueue(v[0]);
                        v[0][1] = "0";
                        while (vConnected.Count > 0)
                        {

                            string[] item = vConnected.Dequeue();

                            for (int i = 0; i < graf.Count; i++)
                            {
                                if (item[0] == graf[i][0])
                                {
                                    for (int k = 0; k < v.Count; k++)
                                    {
                                        if (v[k][0] == graf[i][0])
                                        {
                                            for (int k2 = 0; k2 < v.Count; k2++)
                                            {
                                                if (v[k2][0] == graf[i][1] && v[k2][1] == "1")
                                                {
                                                    vConnected.Enqueue(v[k2]);
                                                    v[k2][1] = "0";
                                                    break;
                                                }
                                            }

                                        }
                                    }
                                }
                                else if (item[0] == graf[i][1])
                                {
                                    for (int k = 0; k < v.Count; k++)
                                    {
                                        if (v[k][0] == graf[i][1])
                                        {
                                            for (int k2 = 0; k2 < v.Count; k2++)
                                            {
                                                if (v[k2][0] == graf[i][0] && v[k2][1] == "1")
                                                {
                                                    vConnected.Enqueue(v[k2]);
                                                    v[k2][1] = "0";
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        int vConnectedCounter = 0;
                        for (int i = 0; i < v.Count; i++)
                        {
                            if (v[i][1] == "0")
                            {
                                vConnectedCounter++;
                            }
                        }
                        if (vConnectedCounter != v.Count)
                        {
                            Console.WriteLine("Граф несвязаный");
                            input = false;
                        }
                    }

                }

            }
            while (!input);

            int len = graf.Count;
            // Сортировка
            bool sorted = false;
            while (!sorted)
            {
                sorted = true;
                for (int i = 1; i < len; i++)
                {
                    string[] el = graf[i];
                    if (Convert.ToInt32(el[2]) < Convert.ToInt32(graf[i - 1][2]))
                    {
                        graf[i] = graf[i - 1];
                        graf[i - 1] = el;
                        sorted = false;
                    }
                }

            }

            Queue<string[]> sortedGraf = new Queue<string[]>();

            for (int i = 0; i < len; i++)
            {
                sortedGraf.Enqueue(graf[i]);
            }
            graf.Clear();

            // Алгоритм Краскала
            Queue<string[]> spanningTree = new Queue<string[]>();
            int group = 1;
            for (int i = 0; i < len; i++)
            {
                string[] d = sortedGraf.Dequeue();

                for (int k = 0; k < v.Count; k++)
                {
                    if (v[k][0] == d[0] && v[k][1] == "0")
                    {
                        for (int k2 = 0; k2 < v.Count; k2++)
                        {
                            if (v[k2][0] == d[1] && v[k2][1] == "0")
                            {
                                v[k][1] = group + "";
                                v[k2][1] = group + "";
                                group++;
                                spanningTree.Enqueue(d);
                                break;
                            }
                            else if (v[k2][0] == d[1] && v[k2][1] != "0")
                            {
                                v[k][1] = v[k2][1];
                                spanningTree.Enqueue(d);
                                break;
                            }
                        }
                    }
                    else if (v[k][0] == d[0] && v[k][1] != "0")
                    {
                        for (int k2 = 0; k2 < v.Count; k2++)
                        {
                            if (v[k2][0] == d[1] && v[k2][1] == "0")
                            {
                                v[k2][1] = v[k][1];
                                spanningTree.Enqueue(d);
                                break;
                            }
                            else if (v[k2][0] == d[1] && v[k2][1] != "0" && v[k][1] != v[k2][1])
                            {
                                string oldGroup = v[k2][1];
                                for (int k3 = 0; k3 < v.Count; k3++)
                                {

                                    if (v[k3][1] == oldGroup)
                                    {
                                        v[k3][1] = v[k][1];
                                    }
                                }
                                spanningTree.Enqueue(d);
                                break;
                            }
                        }
                    }

                }

            }

            len = spanningTree.Count();
            Console.WriteLine("Минимальный остов:");
            for (int i = 0; i < len; i++)
            {

                string[] d = spanningTree.Dequeue();
                Console.WriteLine(d[0] + " " + d[1] + " " + d[2]);
            }



            Console.ReadKey();
        }
    }
}
