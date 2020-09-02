using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Polska
{
    public class Equals
    {
        public bool IsOperation(string arg)
        {
            string[] operations = { "^", "/", "*", "+", "-", "%" };
            return operations.Contains(arg);
        }
        //------------------------------------------------------
        public bool IsNum(string arg)
        {
            return double.TryParse(arg, out double num);
        }
        //------------------------------------------------------
        public bool IsString(string arg)
        {
            Regex r = new Regex(@"[a-zA-Z]+\d*");
            return r.IsMatch(arg);
        }
        //------------------------------------------------------
        public bool IsBkt(string arg)
        {
            if (arg == "(" || arg == ")")
                return true;
            return false;
        }
        //------------------------------------------------------
        public bool IsOpenBkt(string arg)
        {
            if (arg == "(")
                return true;
            return false;
        }
        //------------------------------------------------------
        public bool IsCloseBkt(string arg)
        {
            if (arg == ")")
                return true;
            return false;
        }
        //------------------------------------------------------
        public bool IsFact(string arg)
        {
            if (arg == "!")
                return true;
            return false;
        }
        //------------------------------------------------------
        public double GetFact(double arg)
        {
            for (double i = arg - 1; i >= 1; i--)
                arg *= i;
            return arg;
        }
        //------------------------------------------------------
        public int GetPrior(string operation)
        {
            if (operation == "^") return 4;
            else if (operation == "*") return 3;
            else if (operation == "/") return 3;
            else if (operation == "%") return 3;
            else if (operation == "+") return 2;
            else if (operation == "-") return 2;
            else if (operation == "(") return 0;
            else if (operation == ")") return 0;
            else return 5;
        }
        //------------------------------------------------------
        public double SolveBinary(string arg1, string arg2, string operation)
        {
            double result = 0;
            try
            {
                switch (operation)
                {
                    case "*":
                        result = Convert.ToDouble(arg1) * Convert.ToDouble(arg2);
                        break;
                    case "/":
                        if (Convert.ToDouble(arg2) == 0)
                        {
                            break;
                        }
                        result = Convert.ToDouble(arg1) / Convert.ToDouble(arg2);
                        break;
                    case "-":
                        result = Convert.ToDouble(arg1) - Convert.ToDouble(arg2);
                        break;
                    case "+":
                        result = Convert.ToDouble(arg1) + Convert.ToDouble(arg2);
                        break;
                    case "^":
                        result = Math.Pow(Convert.ToDouble(arg1), Convert.ToDouble(arg2));
                        break;
                    case "%":
                        result = Convert.ToDouble(arg1) % Convert.ToDouble(arg2);
                        break;
                }
            } catch
            {
                result = 0;
            }
            
            return result;
        }
        //------------------------------------------------------
        public double SolveFunction(string arg, string function)
        {
            double result = 0;
            try
            {
                switch (function)
            {
                case "sin":
                    result = Math.Sin(Convert.ToDouble(arg));
                    break;
                case "cos":
                    result = Math.Cos(Convert.ToDouble(arg));
                    break;
                case "abs":
                    result = Math.Abs(Convert.ToDouble(arg));
                    break;
                case "sqrt":
                    result = Math.Sqrt(Convert.ToDouble(arg));
                    break;
                case "exp":
                    result = Math.Exp(Convert.ToDouble(arg));
                    break;
                case "tg":
                    result = Math.Tan(Convert.ToDouble(arg));
                    break;
                case "ctg":
                    result = 1 / Math.Tan(Convert.ToDouble(arg));
                    break;
                case "log":
                    result = Math.Log10(Convert.ToDouble(arg));
                    break;
                case "ln":
                    result = Math.Log(Convert.ToDouble(arg));
                    break;
                case "sec":
                    result = 1 / Math.Cos(Convert.ToDouble(arg));
                    break;
                case "cosec":
                    result = 1 / Math.Sin(Convert.ToDouble(arg));
                    break;
                case "arcsin":
                    result = Math.Asin(Convert.ToDouble(arg));
                    break;
                case "arccos":
                    result = Math.Acos(Convert.ToDouble(arg));
                    break;
                case "arctg":
                    result = Math.Atan(Convert.ToDouble(arg));
                    break;
                case "arcctg":
                    result = Math.PI / 2 - Math.Atan(Convert.ToDouble(arg));
                    break;
                case "sh":
                    result = Math.Sinh(Convert.ToDouble(arg));
                    break;
                case "ch":
                    result = Math.Cosh(Convert.ToDouble(arg));
                    break;
                case "th":
                    result = Math.Tanh(Convert.ToDouble(arg));
                    break;
                case "cth":
                    result = Math.Cosh(Convert.ToDouble(arg)) / Math.Sinh(Convert.ToDouble(arg));
                    break;
                case "sgn":
                    result = Math.Sign(Convert.ToDouble(arg));
                    break;
                default:
                    if (function.IndexOf("log") != -1)
                    {
                        int x = Convert.ToInt32(function.Replace("log", ""));
                        result = Math.Log(Convert.ToDouble(arg)) / Math.Log(x);
                        break;
                    }

                    return 0;
            }
            } catch
            {
                
            }

            return result;
        }
    }
}
