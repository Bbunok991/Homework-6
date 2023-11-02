using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework_6_Calculator
{
    internal class Program
    {
        class CalculatorException : Exception
        {
            public CalculatorException(string msg, Exception inner, List<CalculatorActionLog> log) : base(msg, inner)
            {
            }
        }

        enum CalculatorAction
        {
            Div,
            Mul,
            Sub,
            Add
        }

        class CalculatorActionLog
        {
            public CalculatorAction CalcAction;
            public double CalcArgument;

            public CalculatorActionLog(CalculatorAction calcAction, double calcArgument)
            {
                this.CalcAction = calcAction;
                this.CalcArgument = calcArgument;
            }
        }

        class Calculator
        {
            public event EventHandler<EventArgs> GotResult;
            public double Result { get; private set; }
            public Stack<double> stack = new Stack<double>();
            public Stack<CalculatorActionLog> actionStack = new Stack<CalculatorActionLog>();

            public void Add(double i) // +=
            {
                try
                {
                    stack.Push(Result);
                    actionStack.Push(new CalculatorActionLog(CalculatorAction.Add, i));
                    Result += i;
                    GotResult(this, new EventArgs());
                }
                catch (Exception e)
                {
                    throw new CalculatorException("Продумайте сообщения", e, actionStack.ToList());
                }

            }


            public void CancelLast()
            {
                if (stack.Count > 0)
                {
                    Result = stack.Pop();
                    actionStack.Pop();
                    GotResult(this, new EventArgs());
                }
            }

            public void Div(double i)
            {
                stack.Push(Result);
                actionStack.Push(new CalculatorActionLog(CalculatorAction.Div, i));

                Result /= i;
                GotResult(this, new EventArgs());
            }

            public void Mul(double i)
            {
                stack.Push(Result);
                actionStack.Push(new CalculatorActionLog(CalculatorAction.Mul, i));
                Result *= i;
                GotResult(this, new EventArgs());
            }

            public void Sub(double i)
            {
                stack.Push(Result);
                actionStack.Push(new CalculatorActionLog(CalculatorAction.Sub, i));

                Result -= i;
                GotResult(this, new EventArgs());
            }
        }
        static void Main(string[] args)
        {
            var calculator = new Calculator();

            calculator.GotResult += Calculator_GotResult;
            calculator.Add(10.5);
            calculator.Add(20.7);
            calculator.Add(40.3);
            calculator.CancelLast();

            Console.ReadLine();

        }

        private static void Calculator_GotResult(object sender, EventArgs e)
        {
            Console.SetCursorPosition(1, 1);
            Console.Write(((Calculator)sender).Result);
        }

    }
}
