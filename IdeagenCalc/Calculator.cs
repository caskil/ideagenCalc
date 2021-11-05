using IdeagenCalc.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace IdeagenCalc
{
   
    public class Calculator
    {       
        public static decimal Calculate(string sum)
        {
            if (string.IsNullOrEmpty(sum))
            {
                throw new Exception("Expression string cannot be null or empty.");
            }

            sum = Regex.Replace(sum, @"\s+", "");
            var result = 0m;
            var s_operator = new List<char>();
            var s_operand = new List<Operand>();
          

            var i = 0;

            var numberSeq = 0;

            while (true)
            {
                sum = sum.Substring(i);

                if (Decimal.TryParse(Regex.Match(sum, @"^\d+(\.\d+)?").Value, out var number))
                {
                    s_operand.Add(new Operand { Sequence = numberSeq , Value = number});
                    i = number.ToString().Length;
                    numberSeq++;
                    continue;
                }
                else
                {
                    var openbracket = Regex.Match(sum, @"^\(");

                    if (openbracket.Success)
                    {
                        s_operator.Add(Char.Parse(openbracket.Value));
                        i = 1;
                        continue;
                    }

                    var closebracket = Regex.Match(sum, @"^\)");

                    if (closebracket.Success)
                    {
                        while (s_operator.Last() != '(')
                        {
                            result = ExecuteCalculation(ref s_operator, ref s_operand);
                        }
                        s_operator.RemoveAt(s_operator.Count - 1);
                        i = 1;
                        continue;
                    }

                    var _operator = Regex.Match(sum, @"\+|\-|\*|\/");

                    if (_operator.Success)
                    {
                        var c_operator = Char.Parse(_operator.Value);

                        if (s_operator.Count > 0 && sum.Length > 1 && !IsHigherPriority(c_operator, s_operator[s_operator.Count - 1]))
                        {
                           result = ExecuteCalculation(ref s_operator, ref s_operand);
                        }

                        s_operator.Add(c_operator);
                        i = 1;
                    }
                    else
                    {
                        while (s_operator.Count > 0)
                        {
                            result = ExecuteCalculation(ref s_operator, ref s_operand);
                        }
                        break;
                    }
                }
            }

            return result;
        }
        private static decimal ExecuteCalculation (ref List<char> s_operator, ref List<Operand> s_operand)
        {
            if (s_operand.Count < 2 || s_operator.Count < 1)
            {
                throw new Exception("Unhandle expression.");
            }

            var count = s_operand.Count;

            var numberA = s_operand[count - 2];
            var numberB = s_operand[count - 1];
            var c_operator = s_operator[s_operator.Count - 1];

            var seqCalc = numberA.Sequence;

            var result = Calculate(c_operator, numberA.Value, numberB.Value);

            s_operand.RemoveAt(count - 1);
            s_operand.RemoveAt(count - 2);
            s_operator.RemoveAt(s_operator.Count - 1);

            s_operand.Add(new Operand { Value = result, Sequence = seqCalc });
            s_operand = s_operand.OrderBy(x => x.Sequence).ToList();

            return result;
        }
        private static decimal Calculate(char _operator, decimal a, decimal b)
        {
            switch (_operator)
            {
                case '+':
                    return a + b;
                case '-':
                    return a - b;
                case '*':
                    return a * b;
                case '/':
                    return a / b;
                default:
                    throw new Exception($"Expression contain invalid operator : {_operator.ToString()}");
            }
        }

        private static readonly Dictionary<char, int> OperatorPriority = new Dictionary<char, int>
        {
             { '*', 3 },
             { '/', 3 },
             { '+', 2 },
             { '-', 2 },
             { '(', 1 },
             { ')', 1 }
        };
        public static bool IsHigherPriority(char operatorA, char operatorB)
        {
            return OperatorPriority[operatorA] > OperatorPriority[operatorB];
        }
    }

}
