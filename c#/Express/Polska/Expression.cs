using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Polska
{
    public class Expression
    {
        List<string> exp;
        List<Variable> vars = new List<Variable>();
        
        Equals eq = new Equals();
        //------------------------------------------------------
        public Expression(string exp)
        {
            this.exp = ParseExp(exp);
        }
        //------------------------------------------------------
        public Expression(string exp, string vars)
        {
            this.exp = ParseExp(exp);
            this.vars = ParseVars(vars);
        }
        //------------------------------------------------------
        public List<string> Exp
        {
            get
            {
                return exp;
            }
            set
            {
                exp = value;
            }
        }
        //------------------------------------------------------
        public List<Variable> Vars
        {
            get
            {
                return vars;
            }
            set
            {
                vars = value;
            }
        }
        //------------------------------------------------------
        public List<string> ParseExp(string exp)
        {
            List<string> arr = new List<string>();
            if (exp != "")
            {
                try
                {
                    exp = exp.Replace(" ", "").Replace(".", ",").Replace("(-", "(_");
                    if (exp[0] == '-')
                        exp = "_" + exp.Substring(1, exp.Length - 1);
                }
                catch
                {
                    return new List<string>();
                }

                for (int i = 0; i < exp.Length; i++)
                {
                    if (eq.IsNum($"{exp[i]}") || exp[i] == '_')
                    {
                        string item = $"{exp[i]}";
                        i++;
                        while (i < exp.Length && (eq.IsNum($"{exp[i]}") || exp[i] == ','))
                        {
                            item += $"{exp[i]}";
                            i++;
                        }
                        arr.Add(item);
                        i--;
                    }
                    else if (eq.IsOperation($"{exp[i]}"))
                    {
                        string item = $"{exp[i]}";
                        i++;
                        while (i < exp.Length && eq.IsOperation($"{exp[i]}"))
                        {
                            item += $"{exp[i]}";
                            i++;
                        }
                        arr.Add(item);
                        i--;
                    }
                    else if (eq.IsString($"{exp[i]}"))
                    {
                        string item = $"{exp[i]}";
                        i++;
                        while (i < exp.Length && (eq.IsString($"{exp[i]}") || eq.IsNum($"{exp[i]}")))
                        {
                            item += $"{exp[i]}";
                            i++;
                        }
                        arr.Add(item);
                        i--;
                    }
                    else if (eq.IsBkt($"{exp[i]}") || eq.IsFact($"{exp[i]}"))
                    {
                        arr.Add($"{exp[i]}");
                    }
                    else
                    {
                        return new List<string>();
                    }
                }
                for (int i = 0; i < arr.Count; i++)
                {
                    if (arr[i].IndexOf("_") != -1)
                        arr[i] = arr[i].Replace("_", "-");
                }
            }
            if (arr.Count > 0 && (eq.IsOperation(arr[0]) || eq.IsOperation(arr[arr.Count - 1]) || eq.IsFact(arr[0])))
                return new List<string>();
            return arr;
        }
        //------------------------------------------------------
        public List<Variable> ParseVars(string str) // x1=3;x2=7;z=1...
        {
            List<Variable> varsList = new List<Variable>();
            string[] varsStr;
            if (str != "")
            {
                try
                {
                    str = str.Replace(" ", "").Replace(".", ",");
                    if (str[str.Length - 1] == ';')
                        str = str.Substring(0, str.Length - 1);
                    varsStr = str.Split(';');
                }
                 catch
                {
                    return new List<Variable>();
                }
                

                for (int i = 0; i < varsStr.Length; i++)
                {
                    if (varsStr[i].IndexOf("=") == -1)
                        continue;
                    string[] item = varsStr[i].Split('=');
                    if (item.Length > 2 || item[0] == "" || item[1] == "")
                        continue;
                    try
                    {
                        varsList.Add(new Variable(item[0], Convert.ToDouble(item[1])));
                    }
                    catch
                    {
                        return new List<Variable>();
                    }
                }
                for (int i = 1; i < varsList.Count; i++)
                {
                    for (int j = 0; j < i; j++)
                    {
                        if (varsList[i].Name == varsList[j].Name)
                        {
                            varsList.RemoveAt(i);
                            i--;
                            break;
                        } 
                    }
                }
            }
            
            return varsList;
        }
        //------------------------------------------------------
        public void AddVar(string name, double value)
        {
            vars.Add(new Variable(name, value));
        }
        //------------------------------------------------------
        public void RemoveVar(string name)
        {
            for (int i = 0; i < vars.Count; i++)
            {
                if (vars[i].Name == name)
                {
                    vars.RemoveAt(i);
                }
            }
        }
        //------------------------------------------------------
        public void ClearVars()
        {
            vars.Clear();
        }
        //------------------------------------------------------
        public void ChangeVar(string name, double value)
        {
            for (int i = 0; i < vars.Count; i++)
            {
                if (vars[i].Name == name)
                {
                    vars[i].Value = value;
                }
            }
        }
        //------------------------------------------------------
        public List<string> SetVars(List<string> exp, List<Variable> vars)
        {
            for (int i = 0; i < exp.Count; i++)
            {
                if (exp[i] == "e" || exp[i] == "E")
                {
                    exp[i] = $"{Math.E}";
                }
                else if (exp[i] == "pi" || exp[i] == "PI" || exp[i] == "Pi" || exp[i] == "pI")
                {
                    exp[i] = $"{Math.PI}";
                }
                for (int k = 0; k < vars.Count; k++)
                {
                    if (exp[i] == vars[k].Name)
                    {
                        exp[i] = $"{vars[k].Value}";
                        break;
                    }
                }
            }
            return exp;
        }
        //------------------------------------------------------
        public List<string> GetPolska(List<string> arr)
        {
            if (arr.Count == 0)
                return new List<string>();

            if (eq.IsOperation(arr[0]) || eq.IsOperation(arr[arr.Count - 1]) || eq.IsFact(arr[0]))
            {
                return new List<string>();
            }

            Stack<string> stack = new Stack<string>();
            List<string> output = new List<string>();

            for (int i = 0; i < arr.Count; i++)
            {
                if (eq.IsNum(arr[i]) || eq.IsFact(arr[i]))
                {
                    output.Add(arr[i]);
                }
                else if (eq.IsString(arr[i]) || eq.IsOpenBkt(arr[i]))
                {
                    stack.Push(arr[i]);
                }
                else if (eq.IsCloseBkt(arr[i]))
                {
                    while (stack.Count > 0 && !eq.IsOpenBkt(stack.Peek()))
                    {
                        output.Add(stack.Pop());
                    }
                    if (stack.Count == 0)
                        return new List<string>();
                    stack.Pop();

                }
                else if (eq.IsOperation(arr[i]))
                {
                    if (stack.Count > 0)
                    {
                        while (stack.Count > 0 && eq.GetPrior(stack.Peek()) >= eq.GetPrior(arr[i]))
                        {
                            output.Add(stack.Pop());
                        } 
                        stack.Push(arr[i]);
                    } else
                    {
                        stack.Push(arr[i]);
                    }
                }
            }
            while (stack.Count > 0)
            {
                if (eq.IsBkt(stack.Peek()))
                    return new List<string>();
                output.Add(stack.Pop());
            }

            return output;
        }
        //------------------------------------------------------
        public double ExecutePolska(List<string> polska)
        {
            if (polska.Count == 0)
                return 0;

            for (int i = 0; i < polska.Count; i++)
            {
                if (i > 0 && eq.IsFact(polska[i]))
                {
                    polska[i - 1] = $"{eq.GetFact(Convert.ToDouble(polska[i - 1]))}";
                    polska.RemoveAt(i);
                    i--;
                }
                else if (i > 1 && eq.IsOperation(polska[i]))
                {
                    polska[i] = $"{eq.SolveBinary(polska[i - 2], polska[i - 1], polska[i])}";
                    polska.RemoveAt(i - 1);
                    polska.RemoveAt(i - 2);
                    i -= 2;
                }
                else if (i > 0 && !eq.IsNum(polska[i]))
                {
                    polska[i] = $"{eq.SolveFunction(polska[i - 1], polska[i])}";
                    polska.RemoveAt(i - 1);
                    i--;
                }
            }
            double result = 0;
            try
            {
                result = Convert.ToDouble(polska[0]);
            }
            catch
            {
                result = 0;
            }
            return result;
        }
        //------------------------------------------------------
        public double GetResult()
        {
            return ExecutePolska(GetPolska(SetVars(this.exp, vars)));
        }
    }
}
