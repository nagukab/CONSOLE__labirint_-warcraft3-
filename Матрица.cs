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
        static int высотаОкна = Console.WindowHeight, ширинаОкна = Console.WindowWidth;
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
                Макс_ДлиннаСтроки = рандом.Next(4, 15); // задает максимальную длинну строки для этой итерации цикла
                длиннаСтроки = 0;                       // стартовая длинна строки


                int задержка = рандом.Next(1, рандом.Next(400, 800));
                bool увеличиватьСтроку = true, создатьЕщёЦикл = true;

                for (int цикл = 0; цикл < высотаОкна + Макс_ДлиннаСтроки; цикл++)
                {

                    if (создатьЕщёЦикл) // чтоб только 1 раз за весь цикл мог создать дополнительный
                    {// когда зациклить = true (этот поток только может пораждать новые, в производных будет false и они не смогут пораждать новые потоки и будут одно-итерационные)                        
                        if (Макс_ДлиннаСтроки < 10 && рандом.Next(0, 3) > 0 && (bool)зациклить && цикл >= высотаОкна / 2)    // если строка получилось коротка, и случайный рол совпал с 0 и, то создать ещё поток одноразовый
                        { new Thread(new Матрица(столб).Цепочка).Start(false); создатьЕщёЦикл = false; }         //и породит когда базовый цикл примерно на половине пути и ниже
                    }

                    Thread.Sleep(задержка);

                    lock (блок)            //блокировщик 
                    {
                        if (столб < Console.WindowWidth && столб < Console.BufferWidth) // проверка, чтоб если размеры окна изменились, не пыталось рисовать вне окна (не всегда успевает сработать)
                        {
                            if (ширинаОкна > Console.WindowWidth)  // проверка на изменение размера окна, если изменилось то обновить
                            { ширинаОкна = Console.WindowWidth; Console.Clear(); }
                            else if (ширинаОкна < Console.WindowWidth)
                            { ширинаОкна = Console.WindowWidth; Console.Clear(); }
                            else
                            { ширинаОкна = Console.WindowWidth; }


                            высотаОкна = Console.WindowHeight;                                               // получает текущую длинну окна, вдруг изменилась
                            char символ = строкаНольОдин.ToArray()[рандом.Next(0, строкаНольОдин.Length)];

                            //Console.ForegroundColor = ConsoleColor.Red;

                            if (длиннаСтроки < Макс_ДлиннаСтроки && увеличиватьСтроку) { длиннаСтроки++; } // постепенное наращивание предпологаемой длинны строки

                            if (цикл >= 0 && цикл < высотаОкна)             // сначала отрисовывает символы белым, от самого начала до конца
                            {
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.SetCursorPosition(столб, цикл);
                                Console.Write($"{символ}");                          // только цифра 0 или 1
                            }
                            if (цикл >= 1 && цикл < высотаОкна)            // потом перекрашивает в зеленый но не с самого начала-1 и не до самого конца-1
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.SetCursorPosition(столб, цикл - 1);
                                Console.Write($"{строкаЦифр.ToArray()[рандом.Next(0, строкаЦифр.Length)]}");          // любая цифра
                            }
                            if (цикл >= 2 && цикл < высотаОкна)                                         // потом в темнозеленый перекрашивает от начало-2 и до конца-2
                            {
                                Console.ForegroundColor = ConsoleColor.DarkGreen;
                                Console.SetCursorPosition(столб, цикл - 2);
                                Console.Write($"{строкаЦифр.ToArray()[рандом.Next(0, строкаЦифр.Length)]}");          // любая цифра

                                if (рандом.Next(0, 2) > 0)                                              // рандомно срабатывающий иф, чтоб в последний раз изменить цвет и символ (чтоб после него уже не перерисовывалось другим)
                                {
                                    int масимумРандома = Макс_ДлиннаСтроки > цикл ? цикл : Макс_ДлиннаСтроки;
                                    int случайно = рандом.Next(0, масимумРандома);
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.SetCursorPosition(столб, цикл - случайно);
                                    Console.Write($"{строкаБукв.ToArray()[рандом.Next(0, строкаБукв.Length)]}");                // любая буква
                                }
                            }

                            if (цикл >= длиннаСтроки)                       // иф затирающий хвост строки
                            {
                                Console.SetCursorPosition(столб, цикл - длиннаСтроки);
                                Console.Write($" ");
                            }
                        }
                    }
                }

            } while ((bool)зациклить);
        }

    }
}
