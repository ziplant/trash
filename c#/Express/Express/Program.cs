using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Polska;

namespace Express
{
    class Program
    {
        static void Main(string[] args)
        {
            Expression e;
            
            Equals eq = new Equals();
            string exp = "";
            string vars = "";
            bool repeat = true;
            while (repeat)
            {
                exp = "";
                vars = "";
                while (true)
                {
                    Console.WriteLine("Введите выражение");
                    try
                    {
                        exp = Console.ReadLine();
                        while (true)
                        {
                            Console.WriteLine("Введите переменные (пример: x1=3;x2=7;z=1...)");
                            try
                            {
                                vars = Console.ReadLine();
                                break;
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                        break;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                //--------------------------------------------------------
                e = new Expression(exp, vars);
                // TESTS
                List<string> test1 = e.ParseExp(exp);
                string rezz1 = "";
                for (int i = 0; i < test1.Count; i++)
                {
                    rezz1 += $"{test1[i]} ";
                }
                Console.WriteLine($"Выражение: {rezz1}");

                List<Variable> test2 = e.ParseVars(vars);
                string rezz2 = "";
                for (int i = 0; i < test2.Count; i++)
                {
                    rezz2 += $"{test2[i].Name} = {test2[i].Value}; ";
                }
                Console.WriteLine($"Переменные: {rezz2}");

                List<string> test3 = e.SetVars(test1, test2);
                string rezz3 = "";
                for (int i = 0; i < test3.Count; i++)
                {
                    rezz3 += $"{test3[i]} ";
                }
                Console.WriteLine($"Подстановка: {rezz3}");


                List<string> test4 = e.GetPolska(test3);
                string rezz4 = "";
                for (int i = 0; i < test4.Count; i++)
                {
                    rezz4 += $"{test4[i]} ";
                }
                Console.WriteLine($"Польская запись: {rezz4}");

                Console.WriteLine($"Результат: {e.ExecutePolska(test4)}");

                //--------------------------------------------------------
                Console.WriteLine("Работа завершена, начать сначала?(y/n)");
                string s = Console.ReadLine().ToLower();
                if (s == "n" || s == "т")
                    repeat = false;

            }
        }
    }
}
