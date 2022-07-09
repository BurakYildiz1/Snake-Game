using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Snake_Game
{
    class Program
    {

        static readonly int x = 90;
        static readonly int y = 25;
        static Cell[,] grid = new Cell[y, x];
        static Cell currentCell;
        static Cell yem;
        static int yemsayma;
        static int yemsayi = 0;
        static int yon;
        static readonly int hiz = 1;
        static bool Populated = false;
        static bool kayip = false;
        static int yilanboyut;

        static void Main(string[] args)
        {

            if (!Populated)
            {
                yemsayma = 0;
                yilanboyut = 5;
                populateGrid();
                currentCell = grid[(int)Math.Ceiling((double)y / 2), (int)Math.Ceiling((double)x / 2)];
                yılankafasiYonu();
                yemEkle();
                Populated = true;
            }

            while (!kayip)
            {
                Restart();
            }
        }

        static void Restart()
        {
            Console.SetCursorPosition(0, 0);
            duvarCiz();
            Console.SetCursorPosition(36, 26);
            Console.WriteLine("Uzunluk: {0}", yilanboyut);
            getInput();
        }

        static void updateScreen()
        {
            Console.SetCursorPosition(0, 0);
            duvarCiz();
            Console.SetCursorPosition(36, 26);
            Console.WriteLine("Uzunluk: {0}", yilanboyut);
        }

        static void getInput()
        {
            ConsoleKeyInfo input;
            while (!Console.KeyAvailable)
            {
                hareket();
                updateScreen();
            }
            input = Console.ReadKey();
            doInput(input.KeyChar);
        }

        static void checkCell(Cell cell)
        {
            if (cell.val == "%")
            {
                yemyeme();
            }
            if (cell.visited)
            {
                kaybettin();
            }
        }

        static void kaybettin()
        {
            Console.WriteLine("\n Kaybettin!\nTekrar oynamak ister misin? [E]/[H]");
            string yenioyun = Console.ReadLine();
            if (yenioyun == "e" || yenioyun == "E")
            {
                Process.Start(System.Reflection.Assembly.GetExecutingAssembly().Location);
                Environment.Exit(-1);
            }
            else if (yenioyun == "h" || yenioyun == "H")
            {
                Console.WriteLine("Oyun sonlandı");
                Environment.Exit(-1);

            }
            Console.ReadKey();

        }
        static void kazandin()
        {
            Console.WriteLine("\n Kazandın!\nTekrar oynamak ister misin? [E]/[H]");
            string yenioyun = Console.ReadLine();
            if (yenioyun == "e" || yenioyun == "E")
            {
                Process.Start(System.Reflection.Assembly.GetExecutingAssembly().Location);
                Environment.Exit(-1);
            }
            else if (yenioyun == "h" || yenioyun == "H")
            {
                Console.WriteLine("Oyun sonlandı");
                Environment.Exit(-1);

            }
            Console.ReadKey();
        }

        static void doInput(char inp)
        {
            switch (inp)
            {
                case 'w':
                    yukariGit();
                    break;
                case 's':
                    asagiGit();
                    break;
                case 'a':
                    sagGit();
                    break;
                case 'd':
                    solaGit();
                    break;
            }
        }

        static void yemEkle()
        {
            Random r = new Random();
            Cell cell;
            while (yemsayi < 5)
            {
                cell = grid[r.Next(grid.GetLength(0)), r.Next(grid.GetLength(1))];
                if (cell.val == " ")
                    cell.val = "%";
                yemsayi++;
                break;
            }
            if (yemsayi == 5)
            {
                Console.SetCursorPosition(100, 100);
                kazandin();
            }
        }

        static void yemyeme()
        {
            yilanboyut += 1;
            yemEkle();
        }

        static void yukariGit()
        {
            if (yon == 2)
                return;
            yon = 0;
        }

        static void sagGit()
        {
            if (yon == 3)
                return;
            yon = 1;
        }

        static void asagiGit()
        {
            if (yon == 0)
                return;
            yon = 2;
        }

        static void solaGit()
        {
            if (yon == 1)
                return;
            yon = 3;
        }

        static void hareket()
        {
            if (yon == 0)
            {
                if (grid[currentCell.y - 1, currentCell.x].val == "*")
                {
                    kaybettin();
                    return;
                }
                visitCell(grid[currentCell.y - 1, currentCell.x]);
            }
            else if (yon == 1)
            {
                if (grid[currentCell.y, currentCell.x - 1].val == "*")
                {
                    kaybettin();
                    return;
                }
                visitCell(grid[currentCell.y, currentCell.x - 1]);
            }
            else if (yon == 2)
            {
                if (grid[currentCell.y + 1, currentCell.x].val == "*")
                {
                    kaybettin();
                    return;
                }
                visitCell(grid[currentCell.y + 1, currentCell.x]);
            }
            else if (yon == 3)
            {
                if (grid[currentCell.y, currentCell.x + 1].val == "*")
                {
                    kaybettin();
                    return;
                }
                visitCell(grid[currentCell.y, currentCell.x + 1]);
            }
            Thread.Sleep(hiz * 100);
        }

        static void visitCell(Cell cell)
        {
            currentCell.val = "*";
            currentCell.visited = true;
            currentCell.decay = yilanboyut;
            checkCell(cell);
            currentCell = cell;
            yılankafasiYonu();
        }

        static void yılankafasiYonu()
        {

            currentCell.Set("@");
            if (yon == 0)
            {
                currentCell.val = "^";
            }
            else if (yon == 1)
            {
                currentCell.val = "<";
            }
            else if (yon == 2)
            {
                currentCell.val = "v";
            }
            else if (yon == 3)
            {
                currentCell.val = ">";
            }

            currentCell.visited = false;
            return;
        }

        static void populateGrid()
        {
            Random random = new Random();
            for (int col = 0; col < y; col++)
            {
                for (int row = 0; row < x; row++)
                {
                    Cell cell = new Cell();
                    cell.x = row;
                    cell.y = col;
                    cell.visited = false;
                    if (cell.x == 0 || cell.x > x - 2 || cell.y == 0 || cell.y > y - 2)
                        cell.Set("*");
                    else
                        cell.Clear();
                    grid[col, row] = cell;
                }
            }
        }

        static void duvarCiz()
        {
            string toPrint = "";
            for (int col = 0; col < y; col++)
            {
                for (int row = 0; row < x; row++)
                {
                    grid[col, row].decaySnake();
                    toPrint += grid[col, row].val;

                }
                toPrint += "\n";
            }
            Console.WriteLine(toPrint);
        }
        public class Cell
        {
            public string val
            {
                get;
                set;
            }
            public int x
            {
                get;
                set;
            }
            public int y
            {
                get;
                set;
            }
            public bool visited
            {
                get;
                set;
            }
            public int decay
            {
                get;
                set;
            }

            public void decaySnake()
            {
                decay -= 1;
                if (decay == 0)
                {
                    visited = false;
                    val = " ";
                }
            }

            public void Clear()
            {
                val = " ";
            }

            public void Set(string newVal)
            {
                val = newVal;
            }
        }
    }
}
