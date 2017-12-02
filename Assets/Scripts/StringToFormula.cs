using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using UnityEngine;

/*
 * Modified from https://social.msdn.microsoft.com/Forums/vstudio/en-US/7f62b87d-a35c-4074-a0f0-84a9dd7ff0a5/convert-string-to-formula?forum=csharpgeneral
 */
public class StringToFormula
{

    private string[] _operators = { "-", "+", "/", "×", "^" };
    private Func<double, double, double>[] _operations = {
        (a1, a2) => a1 - a2,
        (a1, a2) => a1 + a2,
        (a1, a2) => a1 / a2,
        (a1, a2) => a1 * a2,
        (a1, a2) => Math.Pow(a1, a2)
    };

    public double Eval(string expression)
    {
        List<string> tokens = getTokens(expression);
        Stack<double> operandStack = new Stack<double>();
        Stack<string> operatorStack = new Stack<string>();
        int tokenIndex = 0;

        tokens.Reverse();
        foreach(string token in tokens)
        {
            // If this is an operator  
            if(Array.IndexOf(_operators, token) >= 0)
            {
                operatorStack.Push(token);
            }
            else
            {
                operandStack.Push(double.Parse(token));
            }
        }



        /*
        while (tokenIndex < tokens.Count)
        {
            string token = tokens[tokenIndex];

            Debug.Log("Token: " + token);

            if (token == "(")
            {
                string subExpr = getSubExpression(tokens, ref tokenIndex);
                operandStack.Push(Eval(subExpr));
                continue;
            }
            if (token == ")")
            {
                throw new ArgumentException("Mis-matched parentheses in expression");
            }

            // If this is an operator  
            if(Array.IndexOf(_operators, token) >= 0)
            {
                while (operatorStack.Count > 0 && Array.IndexOf(_operators, token) < Array.IndexOf(_operators, operatorStack.Peek()))
                {
                    string op = operatorStack.Pop();
                    double arg1 = operandStack.Pop();
                    double arg2 = operandStack.Pop();

                    Debug.Log(String.Format("Evaluating expression: {0} {1} {2}", arg1, op, arg2));

                    operandStack.Push(_operations[Array.IndexOf(_operators, op)](arg1, arg2));
                }
                operatorStack.Push(token);
            }
            else
            {
                
                operandStack.Push(double.Parse(token));
            }
            tokenIndex++;
        }
        */

        while (operatorStack.Count > 0)
        {
            string op = operatorStack.Pop();
            double arg1 = operandStack.Pop();
            double arg2 = operandStack.Pop();

            Debug.Log(String.Format("Evaluating expression: {0} {1} {2}", arg1, op, arg2));

            operandStack.Push(_operations[Array.IndexOf(_operators, op)](arg1, arg2));
        }
        return operandStack.Pop();
    }

    private string getSubExpression(List<string> tokens, ref int index)
    {
        StringBuilder subExpr = new StringBuilder();
        int parenlevels = 1;
        index++;
        while (index < tokens.Count && parenlevels > 0)
        {
            string token = tokens[index];
            if (tokens[index] == "(")
            {
                parenlevels++;
            }

            if (tokens[index] == ")")
            {
                parenlevels--;
            }

            if (parenlevels > 0)
            {
                subExpr.Append(token);
            }

            index++;
        }

        if ((parenlevels > 0))
        {
            throw new ArgumentException("Mis-matched parentheses in expression");
        }
        return subExpr.ToString();
    }

    private List<string> getTokens(string expression)
    {
        string operators = "()^×/+-";
        List<string> tokens = new List<string>();
        StringBuilder sb = new StringBuilder();

        foreach (char c in expression.Replace(" ", string.Empty))
        {
            if (operators.IndexOf(c) >= 0)
            {
                if ((sb.Length > 0))
                {
                    tokens.Add(sb.ToString());
                    sb.Length = 0;
                }
                tokens.Add(c.ToString());
            }
            else {
                sb.Append(c);
            }
        }

        if ((sb.Length > 0))
        {
            tokens.Add(sb.ToString());
        }

        return tokens;
    }
}
