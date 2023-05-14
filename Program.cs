using System;
using System.Collections.Generic;
using System.IO;
namespace PolishReverseForm
{
    class Program
    {
        public static string MainStringPerem;
        static void Main(string[] args)
        {
            string input;
            string path = "input.txt";
            using (StreamReader reader = new StreamReader(path))
            input = reader.ReadToEnd();
            Console.WriteLine("Input: " + input);
            ConvertToInversePolishForm(input);
            List<string> tokens = GetTokens(MainStringPerem);
            Console.WriteLine($"Tokens: {string.Join(", ", tokens)}");
            double result = CalculateResult(tokens);
            Console.WriteLine($"Result: {result}");
            path = "output.txt";
            using (StreamWriter writer = new StreamWriter(path, false))
            {
                writer.WriteLine($"Tokens: {string.Join(", ", tokens)}");
                writer.Write($"Result: {result}");
            }
        }
        static List<string> GetTokens(string input)//конструктор получения списка всех токенов
        {
            List<string> tokens = new List<string>();
            string[] splitInput = input.Split(' ');
            foreach (string s in splitInput)
            {
                if (!string.IsNullOrWhiteSpace(s))
                {
                    tokens.Add(s.Trim());
                }
            }
            return tokens;
        }
        static double CalculateResult(List<string> tokens)//метод вычислящий результат
        {
            Stack<double> stack = new Stack<double>();//создание стека
            foreach (string token in tokens)//объявление цикла размером с лист
            {
                if (double.TryParse(token, out double number))//если токен является числом то записываем его в стэк
                {
                    stack.Push(number);
                }
                else
                {
                    double operand2 = stack.Pop();
                    double operand1 = stack.Pop();
                    switch (token)
                    {
                        case "+":
                            stack.Push(operand1 + operand2);
                            break;
                        case "-":
                            stack.Push(operand1 - operand2);
                            break;
                        case "*":
                            stack.Push(operand1 * operand2);
                            break;
                        case "/":
                            stack.Push(operand1 / operand2);
                            break;
                        case "^":
                            stack.Push(Math.Pow(operand1,operand2));
                            break;
                        default:
                            throw new ArgumentException("Invalid token: " + token);
                    }
                }
            }
            return stack.Pop();
        }
        public static string ReverseString(string s)
        {
            char[] arr = s.ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
        }//Инверсировать текстовую переменную
        public static int GetPriority(string token)
        {
            int result = 0;
            switch (token)
            {
                case "(":
                    result = 1;
                    break;
                ////
                case "+":
                    result = 2;
                    break;
                case "-":
                    result = 2;
                    break;
                ////
                case "*":
                    result = 3;
                    break;
                case "/":
                    result = 3;
                    break;
                ////
                case "^":
                    result = 4;
                    break;
                case "!":
                    result = 4;
                    break;
                ////
                case "e":
                    result = 5;
                    break;
                ////
                case "n":
                    result = 6;
                    break;
                default:
                    break;
            }
            return result;
        }//Получение приоритета токена
        public static void ConvertToInversePolishForm(string input)
        {
            Stack<string> stack = new Stack<string>();//основной стек, для чисел и знаков
            Stack<string> temp = new Stack<string>();//временный стек для знаков
            List<String> Tokens = new List<String>();//все токены, то есть любой символ кроме пробела
            string[] perem = input.Split(' ');
            foreach (string token in perem)
            {
                if (!string.IsNullOrWhiteSpace(token))
                {
                    Tokens.Add(token.Trim());
                }
            }
            foreach (string token in Tokens)//цикл по всем символам в листе
            {
                if (double.TryParse(token, out double number))//проверка на число
                {
                        stack.Push($"{number}");
                }
                if (token == "+" || token == "-" || token == "*" || token == "/" || token == "^" || token == "!" || token == "e" || token == "n")//если это знак
                {
                    if (temp.Count == 0)// если стек пустой то сразу добавляем
                    {
                        temp.Push((token));
                    }
                    else
                    {
                        if (GetPriority((token)) > GetPriority(temp.Peek()))//если приоритет токена выше верхнего стекового то добавляем
                        {
                            temp.Push((token));
                        }
                        else// иначе выгружаем весь стек для знаков в основной и заносим текущий токен во временный стек
                        {
                            while (true)
                            {
                                stack.Push(temp.Pop());
                                if (temp.Count == 0)
                                    break;
                                if (GetPriority(token) > GetPriority(temp.Peek())) break;

                            }
                            temp.Push(token);
                        }


                    }
                }
                if (token == "(")//если токен это открывающая скобка заносим ее во временный стек
                {
                    temp.Push(token);
                }
                if ((token) == ")")//если токен закрывающая скобка то выгружаем все знаки которые были внутри до открывающей скобки и в последствии стираем скобки их стека
                {
                    while (true)
                    {
                        if (temp.Count == 0)
                            break;
                        if (temp.Peek() == "(")
                        {
                            temp.Pop();
                            break;
                        }
                        stack.Push((temp.Pop()));
                    }
                }
            }
            if (temp.Count > 0 | temp.Count < 0)//финальная проверка веременного стека, если он не пуст, то выгружаем все знаки в основной
            {
                while (temp.Count > 0)
                {
                    stack.Push((temp.Pop()));
                }
            }
            Stack<string> PerevorotNaRussy= new Stack<string>();
            while (stack.Count > 0)
            {
                PerevorotNaRussy.Push(stack.Pop());
            }
            Console.WriteLine("//////////////////////////////////");
            int u = 0;
            while (PerevorotNaRussy.Count > 0)
            {
                if (u == PerevorotNaRussy.Count)
                {
                    MainStringPerem += PerevorotNaRussy.Pop();
                }
                else
                    MainStringPerem += PerevorotNaRussy.Pop() + " ";
                u++;
            }
            Console.WriteLine("In reverse polish form that seems like:");
            Console.WriteLine(MainStringPerem);
            Console.WriteLine("//////////////////////////////////");
        }//Перевод строки с формулой в обратную польскую запись
    }
}