using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace _5lb
{
    class Program
    {
        static async Task Main(string[] args)
        {



            string path = "file.txt";
            // чтение из файла
            StreamReader reader = new StreamReader(path);


               string s = await reader.ReadToEndAsync();
          Console.WriteLine("Cодержимое файла :" + s);


       // String s = "(4|5)&(6^3)";
            Console.WriteLine("Инфиксная форма: " + s);
            String s1 = infixToPrefix(s);
            Console.WriteLine("Префиксная форма: " + s1);
            Console.WriteLine("Результат: ");
            result(s1);



            /*Это функция проверяет данный символ является ли  оператором или нет*/
            static bool isOperator(char c)
            {
                return (!(c >= '0' && c <= '9'));
            }

            /*Приоритет*/
            static int getPriority(char C)
            {
                if (C == '&' || C == '|')
                    return 1;
                else if (C == '^' || C == '/')
                    return 2;
                return 0;
            }

            /* Это функция переводит с инфиксной формы на префиксный */
            static String infixToPrefix(String infix)
            {
                /* Создаем 2 стека
                 1. для операторов
                 2. для операндов*/
                Stack<char> operators = new Stack<char>();
                Stack<String> operands = new Stack<String>();

                for (int i = 0; i < infix.Length; i++)
                {
                    /*Если символ является открывающей скобкой то ее кидаем в стек*/
                    if (infix[i] == '(')
                    {
                        operators.Push(infix[i]);
                        /*Если символ является закрывающей скобкой то цикл будет выполнятся если стек операторов не пусто
                         и самый последний элемент стека оператора не является открывающей скобкой */
                    }
                    else if (infix[i] == ')')
                    {
                        while ((operators.Count != 0) && operators.Peek() != '(')
                        {
                            String op1 = operands.Peek();
                            operands.Pop();

                            String op2 = operands.Peek();
                            operands.Pop();

                            char op = operators.Peek();
                            operators.Pop();

                            String tmp = op + op2 + op1;
                            operands.Push(tmp);
                        }
                        operators.Pop();
                    }
                    /*Если данный символ является операндом то его кидаем в стек операндов*/
                    else if (!isOperator(infix[i]))
                    {
                        operands.Push(infix[i] + "");
                    }
                    else
                    {
                        /* Иначе цикл будет выполняться если стек операторов не пусто и приоритет
                         данного символа меньше или равно оператора самого последнего элмента стека оператора */
                        while (!((operators.Count == 0) || getPriority(infix[i]) >= getPriority(operators.Peek())))
                        {
                            // в переменный op1 складывем верхний элемент стека операнда
                            String op1 = operands.Peek();
                            // И потом удаляем этот операнд
                            operands.Pop();

                            // Здесь также работает как и в первой
                            String op2 = operands.Peek();
                            operands.Pop();

                            // В переменный op заносим оператор и потом удаляем этот оператор и стека
                            char op = operators.Peek();
                            operators.Pop();

                            /*Здесь просто складываем*/
                            String tmp = op + op2 + op1;
                            operands.Push(tmp);
                        }
                        operators.Push(infix[i]);
                    }
                }
                /* Если стек не пусто из стеков вытаскиваем элементы складываем друг к другу */
                while ((operators.Count != 0))
                {
                 
                    String op1 = operands.Peek();
                    operands.Pop();       
                   String op2 = operands.Peek();
                    operands.Pop();
                   
                    char op = operators.Peek();
                    operators.Pop();

                    String tmp = op + op2 + op1;
                    operands.Push(tmp);
                   
                }
               
                /* Здесь уже лежит готовый префиксная форма нашего выражения */
                return operands.Peek();
            }

            /*Префиксную форму кидаем на эту функцию и это функция начинает выполнять выражению с конца */
            static String result(String value)
            {
                // Создаем 2 стека и 1 переменный для хранение
                Stack<char> operators2 = new Stack<char>();
                Stack<string> operands2 = new Stack<string>();
                bool var = true;
                int result = 0;

                Console.WriteLine("Операция " + "Операнд1 " + "Операнд2 " + "Результат ");
                for (int i = value.Length - 1; i >= 0; i--)
                {
                    /*Если данный символ является операндом то его кидаем в стек операндов*/
                    if (!isOperator(value[i]))
                    {
                        operands2.Push(value[i] + "");
                    }
                    else
                    {
                        // Иначе кидаем данный символ в стек операторов
                        operators2.Push(value[i]);
                        while (operators2.Count != 0)
                        {
                            char operator_ = operators2.Peek();
                            Console.Write("\n" + operator_ + " ");
                            operators2.Pop();

                            String operand_1 = operands2.Peek();
                            Console.Write("\t\t" + operand_1);
                            operands2.Pop();

                            String operand_2 = operands2.Peek();
                            Console.Write("\t " + operand_2);
                            operands2.Pop();

                            /* Логические операции: or,and,xor,(,)*/

                            if ((operator_ == '|'))
                            {
                                var = float.Parse(operand_1) < float.Parse(operand_2);
                            }
                            else if (operator_ == '&')
                            {
                                var = (operand_1.ToString() == operand_2.ToString()); ;
                            }
                            else if (operator_ == '^')
                            {
                                var = (operand_1.ToString() != operand_2.ToString());
                            }




                            /* Арифметические операции */

                            if ((operator_ == '+'))
                            {
                                result = int.Parse(operand_1) + int.Parse(operand_2);
                            }
                            else if (operator_ == '-')
                            {
                                result = int.Parse(operand_1) - int.Parse(operand_2);
                            }
                            else if (operator_ == '*')
                            {
                                result = int.Parse(operand_1) * int.Parse(operand_2);
                            }
                            else if (operator_ == '/')
                            {
                                result = int.Parse(operand_1) / int.Parse(operand_2);
                            }


                         

                            Console.Write("      " + result);

                        
                            String temp2 = result.ToString();
                              operands2.Push(temp2);

                        //   Console.WriteLine("\t\t     " + var);
                            String temp = var.ToString();
                          


                            string path2 = "file2.txt";
                            using (StreamWriter sr = File.AppendText(path2))
                            {
                                sr.WriteLine("Решение: " + temp2);
                                sr.Close();
                            }
                        }
                    }
                }
                return operands2.Peek();


            }
        }
        }
    }
    