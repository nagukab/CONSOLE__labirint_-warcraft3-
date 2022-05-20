using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace консоль_лабиринт
{
    internal class Матрица
    {
       
            Random рандом;
            public Матрица(int столб)
            {
                this.столб = столб; рандом = new Random((int)DateTime.Now.Ticks);
            }
            static public object блок = new object();
            static int длиннаОкна = Console.WindowHeight;
            int Макс_ДлиннаСтроки, длиннаСтроки;
            static string строкаЦифр = "1234567890";
            static string строкаБукв = "QWERTYUIOPASDFGHJKLZXCVBNM@#$%&";
            static string строкаНольОдин = "01";

            int столб { get; set; }
            public void Цепочка(object циклить)
            {
                bool? зациклить = циклить as bool?;
                Console.CursorVisible = false;
                do
                {

                    Макс_ДлиннаСтроки = рандом.Next(4, 15);
                    длиннаСтроки = 0;

                    if (Макс_ДлиннаСтроки < 10 && рандом.Next(0, 3) > 0)
                    { new Thread(new Матрица(столб).Цепочка).Start(false); } // если строка получилось коротка, и случайный рол совпал с 1, то создать ещё поток одноразовый
                    int задержка = рандом.Next(1, 100);
                    bool увеличиватьСтроку = true;

                    for (int цикл = 0; цикл < длиннаОкна + Макс_ДлиннаСтроки; цикл++)
                    {
                        Thread.Sleep(задержка);

                        lock (блок)
                        {
                            длиннаОкна = Console.WindowHeight;
                            char символ = строкаНольОдин.ToArray()[рандом.Next(0, строкаНольОдин.Length)];

                            Console.ForegroundColor = ConsoleColor.Gray;


                            if (длиннаСтроки < Макс_ДлиннаСтроки && увеличиватьСтроку) { длиннаСтроки++; }

                            if (цикл >= 0) { }
                            if (цикл >= 0 && цикл < длиннаОкна)
                            {
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.SetCursorPosition(столб, цикл);
                                Console.Write($"{символ}");
                            }
                            if (цикл >= 1 && цикл < длиннаОкна)
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.SetCursorPosition(столб, цикл - 1);
                                Console.Write($"{строкаЦифр.ToArray()[рандом.Next(0, строкаЦифр.Length)]}");
                            }
                            if (цикл >= 2 && цикл < длиннаОкна)
                            {
                                Console.ForegroundColor = ConsoleColor.DarkGreen;
                                Console.SetCursorPosition(столб, цикл - 2);
                                Console.Write($"{строкаЦифр.ToArray()[рандом.Next(0, строкаЦифр.Length)]}");

                                if (рандом.Next(0, 2) > 0) //рандомно срабатывающий иф
                                {
                                    int масимумРандома = Макс_ДлиннаСтроки > цикл ? цикл : Макс_ДлиннаСтроки;
                                    int случайно = рандом.Next(0, масимумРандома);
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.SetCursorPosition(столб, цикл - случайно);
                                    Console.Write($"{строкаБукв.ToArray()[рандом.Next(0, строкаБукв.Length)]}");
                                }

                            }

                            if (цикл >= длиннаСтроки)
                            {
                                //if(цикл+длиннаСтроки>Console.WindowHeight - 1) 
                                //{ длиннаСтроки--;увеличиватьСтроку = false; }
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.SetCursorPosition(столб, цикл - длиннаСтроки);
                                Console.Write($" ");
                            }
                        }
                    }

                } while ((bool)зациклить);
            }
       
    }
}
