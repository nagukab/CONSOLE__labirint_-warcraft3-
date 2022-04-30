using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace консоль_лабиринт
{
    internal class Program
    {
        static System.Timers.Timer таймер = new System.Timers.Timer();
        static bool сделатьХодПотаймеру = false;
        static void Main(string[] args)
        {

            double времяТаймера = 2000, множительТаймера =0.1;
            таймер.Interval = времяТаймера;
            таймер.Elapsed += Таймер_Elapsed;
            ConsoleColor стартовыйЦветТекстаКонсоли2 = Console.ForegroundColor;  // запоминает в стартовыйЦветКонсоли цвет текста консоли 
            ConsoleColor стартовыйЦветФонаКонсоли2 = Console.BackgroundColor;  // запоминает в стартовыйЦветКонсоли цвет  консоли
            Random генератор2 = new Random();

            Console.CursorVisible = false;
            Random рандом = new Random();
            int Arthas_X = 0, Arthas_Y = 0,  Arthas_X_движение_X = 0, Arthas_X_движение_Y = 0, жжизней = 10, жжизнейМакс = 10, счетчиК = 0, случайноеЧисло_Х, случайноеЧисло_У,
                Uther_x = 0, Uther_y = 0;
            bool игрокЕщёЖИв = true, поехали = false, утерЖив = true, проигратьЗвукПобеды = true;
            char подобранныйЧар = ' ', чарДляПоиска = '+', временноКудаНаступилУтер = ' ';
            string словоДляПоиска = "frostmourne";

            //string[] картаСтроками = Properties.Resources.mapTEST.Split('\n');

            string[] картаСтроками = File.ReadAllLines("map1"); // считываю карту с файла построчно и запихиваю в массив строк            

            char[,] картаСимволами = new char[картаСтроками[0].Length, картаСтроками.Length]; // создает 2-умерный массив символов размером с рязмер массива строк НА размер вложенного в массив строк массива
            char[] сумкА = new char[0];

            for (int x = 0; x < картаСимволами.GetLength(0); x++) //цикл для заполнения массива символов
            {
                for (int y = 0; y < картаСимволами.GetLength(1); y++)
                {
                    картаСимволами[x, y] = картаСтроками[y][x];// в двумерный массив символов запихивается символ из массива строк
                    if (картаСимволами[x, y] == '@')
                    { // если найдена @ то артасу присваивается координата
                        Arthas_X = x; Arthas_Y = y;
                    }
                    else if (картаСимволами[x, y] == 'U')
                    { // если найдена U то утеру присваивается координата
                        Uther_x = x; Uther_y = y;
                    }
                }
            }
            for (int x = 0; x < картаСимволами.GetLength(1); x++) //цикл для отрисовки массива символов
            {
                for (int y = 0; y < картаСимволами.GetLength(0); y++)
                { Console.Write(картаСимволами[y, x]); }
                Console.WriteLine();
            }
            Console.SetCursorPosition(0, картаСимволами.GetLength(1));
            Console.WriteLine("управление стрелками, # это стена, надо искать букву и не тыкаться в стены, утера можно победить накопив 26хп");
            while (игрокЕщёЖИв) //////////////////////////////////////////////////////////////////////////         ЦИКЛ
            {

                if (жжизней >= 100 && проигратьЗвукПобеды && !утерЖив)
                {
                    проигратьЗвукПобеды = false; // 1 раз зайдёт сюда проиграет звук и все
                    using (MemoryStream звук = new MemoryStream(Properties.Resources.EvilArthasWarcry1)) new SoundPlayer(звук).Play();
                }
                else if (жжизней > 0)
                {
                    Console.SetCursorPosition(0, картаСимволами.GetLength(1) + 1);
                    Console.Write("[");
                    Console.BackgroundColor = ConsoleColor.Green;
                    for (int i = 1; i <= жжизней; i++) { Console.Write(" "); }             //отрисовываю жизни

                    Console.BackgroundColor = стартовыйЦветФонаКонсоли2;
                    for (int i = жжизней; i < жжизнейМакс; i++) { Console.Write(" "); }    //отрисовываю пустоту
                    Console.Write("]");
                }
                else if (жжизней == 0)
                {
                    using (MemoryStream звук = new MemoryStream(Properties.Resources.NecromancerYesAttack1)) new SoundPlayer(звук).Play();
                    игрокЕщёЖИв = false;
                    Console.BackgroundColor = ConsoleColor.Black; Console.ForegroundColor = ConsoleColor.Red;
                    Console.Clear();
                    Console.SetCursorPosition(30, 10);
                    Console.WriteLine("GAME OVER");
                    Console.ReadKey();
                }
                else if (жжизней < 0)
                {
                    игрокЕщёЖИв = false;
                    using (MemoryStream звук = new MemoryStream(Properties.Resources.UtherYesAttack3)) new SoundPlayer(звук).Play();
                    Console.BackgroundColor = ConsoleColor.White; Console.ForegroundColor = ConsoleColor.Black;
                    Console.Clear();
                    Console.SetCursorPosition(30, 10);
                    Console.WriteLine("ПОЗНАЙ ГНЕВ СВЕТА");
                    Console.ReadKey();
                }
                if (поехали && утерЖив && сделатьХодПотаймеру)
                {
                    bool покаНеУперся = true;
                    while (покаНеУперся)
                    {
                        int ВыборНаправления = генератор2.Next(0, 4);
                        int Uther_x_движ = Uther_x;
                        int Uther_y_движ = Uther_y;
                        switch (ВыборНаправления)
                        {
                            case 0: Uther_x_движ++; break;
                            case 1: Uther_x_движ--; break;
                            case 2: Uther_y_движ++; break;
                            case 3: Uther_y_движ--; break;
                        }

                        if (картаСимволами[Uther_x_движ, Uther_y_движ] != '#')
                        {
                            if (картаСимволами[Uther_x_движ, Uther_y_движ] == '@' && жжизней <= 25)
                            {
                                жжизней += -21;
                            }
                            else if (картаСимволами[Uther_x_движ, Uther_y_движ] == '@' && жжизней > 25)
                            {
                                жжизней += 100; жжизнейМакс += 100; утерЖив = false;
                            }
                            картаСимволами[Uther_x, Uther_y] = временноКудаНаступилУтер; Console.SetCursorPosition(Uther_x, Uther_y); Console.Write(временноКудаНаступилУтер); // написать на старом месте то чё там было до этого
                            Uther_x = Uther_x_движ;
                            Uther_y = Uther_y_движ;
                            временноКудаНаступилУтер = картаСимволами[Uther_x, Uther_y];
                            картаСимволами[Uther_x, Uther_y] = 'U';
                            Console.SetCursorPosition(Uther_x, Uther_y);
                            Console.BackgroundColor = ConsoleColor.Yellow; Console.ForegroundColor = ConsoleColor.Black;
                            Console.Write('U');
                            Console.BackgroundColor = стартовыйЦветФонаКонсоли2; Console.ForegroundColor = стартовыйЦветТекстаКонсоли2;

                            покаНеУперся = false; сделатьХодПотаймеру = false;                            
                        }
                    }
                }
                if (Console.KeyAvailable)
                {
                    поехали = true;
                    таймер.Start();
                    ConsoleKeyInfo нажатаяКнопка = Console.ReadKey(true);
                    switch (нажатаяКнопка.Key)
                    {
                        case ConsoleKey.UpArrow:
                            Arthas_X_движение_X = 0; Arthas_X_движение_Y = -1; break;

                        case ConsoleKey.DownArrow:
                            Arthas_X_движение_X = 0; Arthas_X_движение_Y = +1; break;

                        case ConsoleKey.LeftArrow:
                            Arthas_X_движение_X = -1; Arthas_X_движение_Y = 0; break;

                        case ConsoleKey.RightArrow:
                            Arthas_X_движение_X = +1; Arthas_X_движение_Y = 0; break;
                    }
                    //////////////////////////////////////////           ПЕРЕДВИЖЕНИЕ
                    if (картаСимволами[Arthas_X + Arthas_X_движение_X, Arthas_Y + Arthas_X_движение_Y] == 'U' && жжизней <= 25) { жжизней += -21; }
                    if (картаСимволами[Arthas_X + Arthas_X_движение_X, Arthas_Y + Arthas_X_движение_Y] == 'U' && жжизней > 25) // убийство утера, надо затереть его символ ели я на него наступаю
                    {
                        жжизней += 100; жжизнейМакс += 100; утерЖив = false;
                        Console.SetCursorPosition(Arthas_X, Arthas_Y); // ставит курсор на старую позицию
                        Console.WriteLine(" "); // стирает  старую позицию
                        картаСимволами[Arthas_X, Arthas_Y] = ' '; // стирает в массиве

                        Arthas_X += Arthas_X_движение_X; Arthas_Y += Arthas_X_движение_Y; // вычисляет новую позицию
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.SetCursorPosition(Arthas_X, Arthas_Y); Console.Write("@");    // ставит курсор на новую позицию и рисует символ                        
                        Console.ForegroundColor = стартовыйЦветТекстаКонсоли2;
                    }
                    else if (картаСимволами[Arthas_X + Arthas_X_движение_X, Arthas_Y + Arthas_X_движение_Y] == '#') { жжизней--; } // если уперся в стену то отимает жизни
                    else if (картаСимволами[Arthas_X + Arthas_X_движение_X, Arthas_Y + Arthas_X_движение_Y] != '#') // если в будующей координате нет стены, то ТРУ
                    {
                        подобранныйЧар = картаСимволами[Arthas_X, Arthas_Y];// подобранное запоминает
                        Console.SetCursorPosition(Arthas_X, Arthas_Y); // ставит курсор на старую позицию
                        Console.WriteLine(" "); // стирает  старую позицию
                        картаСимволами[Arthas_X, Arthas_Y] = ' '; // стирает в массиве

                        Arthas_X += Arthas_X_движение_X; Arthas_Y += Arthas_X_движение_Y; // вычисляет новую позицию
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.SetCursorPosition(Arthas_X, Arthas_Y); Console.Write("@");    // ставит курсор на новую позицию и рисует символ                        
                        Console.ForegroundColor = стартовыйЦветТекстаКонсоли2;
                    }

                    ////////////////////////////////////////         ПРОВЕРКА ПОДБОРА
                    if (картаСимволами[Arthas_X, Arthas_Y] != ' ' && картаСимволами[Arthas_X, Arthas_Y] != '@' && картаСимволами[Arthas_X, Arthas_Y] != '#' && картаСимволами[Arthas_X, Arthas_Y] != 'U') // после передвижения в точке положения оказывается какой то символ то лутать его
                    {
                        жжизней++; жжизнейМакс++; // получается когда полезное подбирается, увеличивается хп
                        char[] temp_сумка = new char[сумкА.Length + 1]; // создает временную сумку под новый символ
                        for (int i = 0; i < сумкА.Length; i++)
                        {
                            temp_сумка[i] = сумкА[i];  // перекладывает из старой сумки в новую
                        }
                        temp_сумка[temp_сумка.Length - 1] = подобранныйЧар; // запихивает в последний слот сумки то чё подобрал
                        сумкА = temp_сумка; // копирует из временной в основную
                        счетчиК++; // прибавить счетчико подобранных
                                   //////////////////////////////////////    ВЫБОР ЗВУКА ПРИ ПОДБОРЕ
                        if (счетчиК <= словоДляПоиска.Length)
                        {
                            чарДляПоиска = словоДляПоиска[счетчиК - 1]; using (MemoryStream звук = new MemoryStream(Properties.Resources.EvilArthasYesAttack6)) new SoundPlayer(звук).Play();
                        }
                        else if (счетчиК == словоДляПоиска.Length + 1)
                        {
                            чарДляПоиска = '+'; using (MemoryStream звук = new MemoryStream(Properties.Resources.EvilArthasYesAttack2_фросморн)) new SoundPlayer(звук).Play();
                            множительТаймера += 0.05;
                        }
                        else if (счетчиК <= словоДляПоиска.Length + 6)
                        {
                            чарДляПоиска = 'X'; using (MemoryStream звук = new MemoryStream(Properties.Resources.EvilArthasYes2_фросморнжаждиткрови)) new SoundPlayer(звук).Play();
                            множительТаймера += 0.1;
                        }
                        else { чарДляПоиска = '.'; using (MemoryStream звук = new MemoryStream(Properties.Resources.EvilArthasYesAttack5)) new SoundPlayer(звук).Play(); множительТаймера += 0.15; }

                        if (времяТаймера - времяТаймера * множительТаймера > 0) // проверка чтоб таймер в минус не ушел
                        {
                            времяТаймера -= времяТаймера * множительТаймера; // уменьшает время
                            таймер.Interval = времяТаймера;  // и получается таймер все короче становится и утер чаще ходит
                        }
                        bool искатьПустоеПоле = true;
                        while (искатьПустоеПоле)
                        {
                            случайноеЧисло_Х = рандом.Next(0, картаСимволами.GetLength(0));
                            случайноеЧисло_У = рандом.Next(0, картаСимволами.GetLength(1));
                            if (картаСимволами[случайноеЧисло_Х, случайноеЧисло_У] == ' ')
                            {
                                картаСимволами[случайноеЧисло_Х, случайноеЧисло_У] = чарДляПоиска;
                                Console.SetCursorPosition(случайноеЧисло_Х, случайноеЧисло_У);
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.Write(чарДляПоиска);
                                Console.ForegroundColor = стартовыйЦветТекстаКонсоли2;
                                искатьПустоеПоле = false;
                            }
                        }
                    }
                    картаСимволами[Arthas_X, Arthas_Y] = '@';    // записывает себя в массив
                }
            }
            while (true)
            {
                Console.ReadLine();
            }
        }

        private static void Таймер_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        { // каждый тик таймера разрешает утеру сделать ход
            сделатьХодПотаймеру = true;
        }
    }
}

