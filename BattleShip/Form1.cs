using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BattleShip
{
    public partial class Form1 : Form
    {
        int antiuser;
        int death = 0, edeath = 0;
        int user = 0;
        int x1, x2, y1, y2;
        int score = 0;
        int where = 0;
        int ex = 0, ey = 0;
        bool hirt = false;
        bool create = false;
        bool ehirt = false;
        bool create_x1 = true;
        string window = "menu";
        string name = "";
        int[] ship = new int[4];
        int[,,] map = new int[2, 10, 10];
        string[,] results = new string[2, 5];
        SolidBrush lite = new SolidBrush(Color.FromArgb(255, 122, 134, 252));
        SolidBrush dark = new SolidBrush(Color.FromArgb(255, 63, 71, 204));
        SolidBrush brush = new SolidBrush(Color.White);
        Pen pen = new Pen(Color.FromArgb(255, 0, 0, 0), 4);
        Pen small_pen = new Pen(Color.FromArgb(255, 0, 0, 0), 2);
        Font font = new Font("Arial", 32);
        PointF[] home = new PointF[5];
        Random rand = new Random();

        public Form1()
        {
            this.FormBorderStyle = FormBorderStyle.Fixed3D; InitializeComponent();
            home[0].X = 30; home[0].Y = 530;
            home[1].X = 70; home[1].Y = 530;
            home[2].X = 70; home[2].Y = 490;
            home[3].X = 50; home[3].Y = 460;
            home[4].X = 30; home[4].Y = 490;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.FillRectangle(lite, 0, 0, 1000, 1000);
            g.FillPolygon(dark, home);
            g.FillRectangle(lite, 40, 500, 20, 20);
            switch (window)
            {
                case "menu":
                    g.FillRectangle(dark, 280, 100, 240, 50);
                    g.FillRectangle(dark, 280, 180, 240, 50);
                    g.FillRectangle(dark, 280, 260, 240, 50);
                    g.FillRectangle(dark, 280, 340, 240, 50);
                    g.DrawString("НАЧАТЬ", font, lite, 310, 100);
                    g.DrawString("СПРАВКА", font, lite, 290, 180);
                    g.DrawString("РЕКОРДЫ", font, lite, 290, 260);
                    g.DrawString("ВЫХОД", font, lite, 315, 340);
                    break;
                case "game":
                    g.DrawString(score.ToString(), font, dark, 10, 10);

                    for (int x = 0; x < 10; x++)
                        for (int y = 0; y < 10; y++)
                        {
                            switch (map[0, x, y])
                            {
                                case 1: brush.Color = Color.Aqua; break;
                                case 0: if ((x + y) % 2 == 1) brush.Color = Color.FromArgb(255, 122, 134, 252); else brush.Color = Color.FromArgb(255, 63, 71, 204); break;
                                case 2: brush.Color = Color.FromArgb(255, 17, 129, 130); break;
                                case 3: brush.Color = Color.FromArgb(255, 237, 27, 36); break;
                                case 4: brush.Color = Color.FromArgb(255, 136, 0, 22); break;
                                case 5: brush.Color = Color.FromArgb(255, 17, 255, 130); break;
                            }
                            g.FillRectangle(brush, x * 30 + 50, y * 30 + 110, 30, 30);
                            switch (map[1, x, y])
                            {
                                case 2:
                                case 0:
                                case 5: if ((x + y) % 2 == 1) brush.Color = Color.FromArgb(255, 122, 134, 252); else brush.Color = Color.FromArgb(255, 63, 71, 204); break;
                                case 1: brush.Color = Color.Aqua; break;
                                case 3: brush.Color = Color.FromArgb(255, 237, 27, 36); break;
                                case 4: brush.Color = Color.FromArgb(255, 136, 0, 22); break;
                            }
                            g.FillRectangle(brush, 440 + x * 30, y * 30 + 110, 30, 30);
                        }
                    for (int x = 0; x < 11; x++)
                    {
                        g.DrawLine(small_pen, x * 30 + 50, 110, x * 30 + 50, 10 * 30 + 110);
                        g.DrawLine(small_pen, 50, x * 30 + 110, 8 * 30 + 110, x * 30 + 110);
                        g.DrawLine(small_pen, x * 30 + 440, 110, x * 30 + 440, 10 * 30 + 110);
                        g.DrawLine(small_pen, 440, x * 30 + 110, 440 + 10 * 30, x * 30 + 110);
                    }
                    g.DrawRectangle(pen, 46, 106, 308, 308);
                    g.DrawRectangle(pen, 436, 106, 308, 308);
                    break;
                case "about":
                    brush.Color = Color.Black;
                    font = new Font("Arial", 10);
                    g.DrawString("Играют двое игроков. Игра начинается с подготовки поля. На пользователю предоставляется два квадрата 10×10.", font, brush, 10, 20);
                    g.DrawString("На одном из них будут расставляться свои корабли, в другом будет «вестись огонь» по кораблям противника. ", font, brush, 10, 40);
                    g.DrawString("Стороны квадратов подписываются буквами по горизонтали и цифрами по вертикали, ", font, brush, 10, 60);
                    g.DrawString("Далее, начинается расстановка флотов.Должно быть 4 корабля по 1 клетке, 3 корабля по 2 , 2 по 3 и 1 по 4. ", font, brush, 10, 100);
                    g.DrawString("Все корабли должны быть только прямыми.Корабли располагаются на игровом поле таким образом, чтобы между  ", font, brush, 10, 120);
                    g.DrawString("ними всегда был зазор в одну клетку.При этом корабли могут касаться краёв поля и занимать углы.", font, brush, 10, 140);
                    g.DrawString("Для установки корабля выберите две клетки, являющиеся его началом и концом.", font, brush, 10, 160);
                    g.DrawString("После расстановки, игроки по очереди производят «выстрелы», называя квадраты по их «координатам»: «А1», «В6»...", font, brush, 10, 200);
                    g.DrawString("Если клетка занята кораблём или его частью, противник должен ответить «ранен» или «убит». Клетка помечается", font, brush, 10, 220);
                    g.DrawString("и можно сделаеть ещё выстрел.Если в названной клетке корабля нет, то ход переходит к сопернику.", font, brush, 10, 240);
                    g.DrawString("Игра ведётся до полной победы одного из игроков, то есть, пока не будут потоплены все корабли. По окончании ", font, brush, 10, 280);
                    g.DrawString("игры проигравший может попросить у победителя посмотреть на его расстановку кораблей.", font, brush, 10, 300);
                    break;
                case "mmr":
                    for (int i = 0; i < 5; i++)
                    {
                        g.DrawString(results[0, i], font, dark, 100, 100 + i * 50);

                        g.DrawString(results[1, i], font, dark, 500, 100 + i * 50);
                    }
                    break;
                case "end":
                    font = new Font("Arial", 32);
                    g.DrawString("Очков: " + score.ToString(), font, dark, 250, 150);
                    g.DrawString("Имя: " + name, font, dark, 250, 190);
                    break;
                case "badend":
                    font = new Font("Arial", 32);
                    g.DrawString("Вы проиграли!", font, dark, 250, 150);

                    break;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            open();
            esetup();
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if ((e.X > 30) && (e.X < 70) && (e.Y > 460) && (e.Y < 530))
            {
                window = "menu";
                Invalidate();
            }
            switch (window)
            {
                case "menu":
                    menu(e);
                    break;
                case "game":
                    if (create) game(e); else setup(e);
                    Invalidate();
                    break;
                case "about":
                    break;
                case "mmr":
                    break;
            }
        }

        void menu(MouseEventArgs e)
        {
            font = new Font("Arial", 32);
            if ((e.X > 280) && (e.X < 280 + 240) && (e.Y > 100) && (e.Y < 100 + 50))
            {
                window = "game";
                Invalidate();
            }
            else
            {
                if ((e.X > 280) && (e.X < 280 + 240) && (e.Y > 180) && (e.Y < 180 + 50))
                {
                    window = "about";
                    Invalidate();
                }
                else
                {
                    if ((e.X > 280) && (e.X < 280 + 240) && (e.Y > 260) && (e.Y < 260 + 50))
                    {
                        window = "mmr";
                        Invalidate();
                    }
                    else
                    {
                        if ((e.X > 280) && (e.X < 280 + 240) && (e.Y > 340) && (e.Y < 340 + 50))
                            Application.Exit();
                    }
                }
            }
        }

        void setup(MouseEventArgs e)
        {
            int x = (e.X - 50) / 30; int y = (e.Y - 110) / 30;
            if ((x > -1) && (y > -1) && (x < 10) && (y < 10))
            {
                if (create_x1)
                {
                    x1 = x; y1 = y;
                    if (map[0, x, y] == 0) map[0, x, y] = 5;
                    create_x1 = false;
                }
                else
                {
                    x2 = x; y2 = y;
                    if ((x1 - x2 == 0) || (y1 - y2 == 0))
                    {
                        if (map[0, x1, y1] == 5) map[0, x1, y1] = 0;
                        if (x1 > x2) swap(ref x1, ref x2);
                        if (y1 > y2) swap(ref y1, ref y2);
                        int size = x2 - x1 + y2 - y1;
                        if (size < 4)
                        {
                            bool noship = true;
                            for (int x3 = x1; x3 <= x2; x3++)
                                for (int y3 = y1; y3 <= y2; y3++)
                                    if (map[0, x3, y3] == 2) noship = false;
                            if (noship)
                            {
                                ship[size]++;
                                if ((ship[0] < 5) && (ship[1] < 4) && (ship[2] < 3) && (ship[3] < 2) && (checkship(0, x1, y1, x2, y2)))
                                {
                                    for (int x3 = x1; x3 <= x2; x3++)
                                        for (int y3 = y1; y3 <= y2; y3++)
                                            map[0, x3, y3] = 2;
                                    if (ship[0] + ship[1] + ship[2] + ship[3] == 10) create = true;
                                }
                                else ship[size]--;
                            }
                        }
                    }
                    else map[0, x1, y1] = 0;
                    create_x1 = true;
                }
            }/*
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4 - i; j++)
                {

                    bool b = false;
                    int x = 0, y = 0, where = 0;
                    while (!(b))
                    {
                        where = rand.Next(2);
                        x = rand.Next(10 - i);
                        y = rand.Next(10);
                        if (where == 0) b = checkship(0, x, y, x + i, y); else b = checkship(0, y, x, y, x + i);

                    }
                    for (int l = x; l <= x + i; l++)
                        if (where == 0) map[0, l, y] = 2; else map[0, y, l] = 2;
                }
            create = true;*/
        }

        void game(MouseEventArgs e)
        {

            int x = (e.X - 440) / 30; int y = (e.Y - 110) / 30;
            if ((x > -1) && (y > -1) && (x < 10) && (y < 10) && ((map[1, x, y] == 2) || (map[1, x, y] == 0)))
            {
                user = 0; antiuser = 1;
                maingame(x, y, 1);
                if (!(hirt))
                {
                    user = 1; antiuser = 0;
                    if (ehirt)
                    {
                        x = ex; y = ey;
                        if (where == 0) { if ((x < 9) && ((map[0, x + 1, y] == 0) || (map[0, x + 1, y] == 2))) x++; else { where++; x = ex; y = ey; } }
                        if (where == 1) { if ((x > 0) && ((map[0, x - 1, y] == 0) || (map[0, x - 1, y] == 2))) x--; else { where++; x = ex; y = ey; } }
                        if (where == 2) { if ((y < 9) && ((map[0, x, y + 1] == 0) || (map[0, x, y + 1] == 2))) y++; else { where++; x = ex; y = ey; } }
                        if (where == 3) { if ((y > 0) && ((map[0, x, y - 1] == 0) || (map[0, x, y - 1] == 2))) y--; else { where++; x = ex; y = ey; } }
                        if (where == 4) where = 0;
                    }
                    else
                    {
                        where = 0;
                        if (create)
                            do
                            {
                                x = rand.Next(10); ex = x;
                                y = rand.Next(10); ey = y;
                            }
                            while (!((map[0, x, y] == 0) || (map[0, x, y] == 2)));
                    }
                    maingame(x, y, 0);
                    while (hirt)
                    {
                        if (where == 0) { if ((x < 9) && ((map[0, x + 1, y] == 0) || (map[0, x + 1, y] == 2))) x++; else { where++; x = ex; y = ey; } }
                        if (where == 1) { if ((x > 0) && ((map[0, x - 1, y] == 0) || (map[0, x - 1, y] == 2))) x--; else { where++; x = ex; y = ey; } }
                        if (where == 2) { if ((y < 9) && ((map[0, x, y + 1] == 0) || (map[0, x, y + 1] == 2))) y++; else { where++; x = ex; y = ey; } }
                        if (where == 3) { if ((y > 0) && ((map[0, x, y - 1] == 0) || (map[0, x, y - 1] == 2))) y--; else { where++; x = ex; y = ey; } }
                        if (where == 4) where = 0;
                        maingame(x, y, 0);
                    }
                    where++;
                    if (where == 4) where = 0;
                }
                if (death == 10) window = "end";
                if (edeath == 10) window = "badend";

                Invalidate();
            }

        }

        void maingame(int x, int y, int antiuser)
        {
            hirt = false;
            if (antiuser == 1) score++;
            if (map[antiuser, x, y] == 0) map[antiuser, x, y] = 1;
            if (map[antiuser, x, y] == 2)
            {
                if (antiuser == 0) ehirt = true;
                hirt = true;
                if (check(x, y))
                {
                    if (antiuser == 1) death++;
                    else
                    {
                        ehirt = false; edeath++;
                    }
                    bool single = true;

                    map[antiuser, x, y] = 4; int b = 1;
                    if (((x < 9) && (map[antiuser, x + 1, y] == 3)) || ((x > 0) && (map[antiuser, x - 1, y] == 3)))
                    {
                        single = false;
                        if (y < 9) map[antiuser, x, y + 1] = 1;
                        if (y > 0) map[antiuser, x, y - 1] = 1;
                        while ((x + b < 10) && (map[antiuser, x + b, y] == 3))
                        {
                            if (y < 9) map[antiuser, x + b, y + 1] = 1; if (y > 0) map[antiuser, x + b, y - 1] = 1; map[antiuser, x + b++, y] = 4;
                        }
                        if (x + b <= 9)
                        {
                            if (y < 9) map[antiuser, x + b, y + 1] = 1;
                            if (y > 0) map[antiuser, x + b, y - 1] = 1;
                            map[antiuser, x + b, y] = 1;
                        }
                        b = 1;
                        while ((x - b > -1) && (map[antiuser, x - b, y] == 3))
                        {
                            if (y < 9) map[antiuser, x - b, y + 1] = 1; if (y > 0) map[antiuser, x - b, y - 1] = 1; map[antiuser, x - b++, y] = 4;
                        }
                        if (x - b >= 0)
                        {
                            if (y < 9) map[antiuser, x - b, y + 1] = 1;
                            if (y > 0) map[antiuser, x - b, y - 1] = 1;
                            map[antiuser, x - b, y] = 1;
                        }
                    }
                    b = 1;
                    if (((y < 9) && (map[antiuser, x, y + 1] == 3)) || ((y > 0) && (map[antiuser, x, y - 1] == 3)))
                    {
                        single = false;
                        while ((y + b < 10) && (map[antiuser, x, y + b] == 3))
                        {
                            if (x < 9) map[antiuser, x + 1, y + b] = 1;
                            if (x > 0) map[antiuser, x - 1, y + b] = 1;
                            map[antiuser, x, y + b++] = 4;
                        }
                        if (y + b < 10)
                        {
                            if (x > 0) map[antiuser, x - 1, y + b] = 1;
                            if (x < 9) map[antiuser, x + 1, y + b] = 1;
                            map[antiuser, x, y + b] = 1;
                        }
                        b = 1;
                        while ((y - b > -1) && (map[antiuser, x, y - b] == 3))
                        {
                            if (x < 9) map[antiuser, x + 1, y - b] = 1; if (x > 0) map[antiuser, x - 1, y - b] = 1; map[antiuser, x, y - b++] = 4;
                        }
                        if (y - b >= 0)
                        {
                            if (x > 0) map[antiuser, x - 1, y - b] = 1;
                            if (x < 9) map[antiuser, x + 1, y - b] = 1;
                            map[antiuser, x, y - b] = 1;
                        }
                        if (x > 0) map[antiuser, x - 1, y] = 1;
                        if (x < 9) map[antiuser, x + 1, y] = 1;
                    }
                    if (single)
                    {
                        if (y > 0) map[antiuser, x, y - 1] = 1;
                        if (y < 9) map[antiuser, x, y + 1] = 1;
                        if (x < 9) map[antiuser, x + 1, y] = 1;
                        if (x > 0) map[antiuser, x - 1, y] = 1;
                    }
                    if ((x > 0) && (y > 0)) map[antiuser, x - 1, y - 1] = 1;
                    if ((x > 0) && (y < 9)) map[antiuser, x - 1, y + 1] = 1;
                    if ((x < 9) && (y > 0)) map[antiuser, x + 1, y - 1] = 1;
                    if ((x < 9) && (y < 9)) map[antiuser, x + 1, y + 1] = 1;
                }
                else map[antiuser, x, y] = 3;
            }
        }

        void swap(ref int a, ref int b)
        {
            a = a - b;
            b = a + b;
            a = b - a;
        }

        bool check(int x, int y)
        {
            int b = 1;
            bool c = true;
            if ((((x < 9) && ((map[antiuser, x + 1, y] == 2) || (map[antiuser, x + 1, y] == 3))) || (((x > 0) && ((map[antiuser, x - 1, y] == 2) || (map[antiuser, x - 1, y] == 3))))))
            {
                while ((x + b < 10) && ((map[antiuser, x + b, y] == 2) || (map[antiuser, x + b, y] == 3)))
                {
                    if (map[antiuser, x + b, y] != 3) c = false;
                    b++;
                }
                b = 1;
                while ((x - b > -1) && ((map[antiuser, x - b, y] == 2) || (map[antiuser, x - b, y] == 3)))
                {
                    if (map[antiuser, x - b, y] != 3) c = false;
                    b++;
                }
            }
            b = 1;
            if ((((y < 9) && ((map[antiuser, x, y + 1] == 2) || (map[antiuser, x, y + 1] == 3))) || (((y > 0) && ((map[antiuser, x, y - 1] == 2) || (map[antiuser, x, y - 1] == 3))))))
            {
                while ((y + b < 10) && ((map[antiuser, x, y + b] == 2) || (map[antiuser, x, y + b] == 3)))
                {
                    if (map[antiuser, x, y + b] != 3) c = false;
                    b++;
                }
                b = 1;
                while ((y - b > -1) && ((map[antiuser, x, y - b] == 2) || (map[antiuser, x, y - b] == 3)))
                {
                    if (map[antiuser, x, y - b] != 3) c = false;
                    b++;
                }
            }
            return c;
        }

        bool checkship(int i, int x1, int y1, int x2, int y2)
        {
            float count = 0;
            for (int x3 = x1; x3 <= x2; x3++)
                for (int y3 = y1; y3 <= y2; y3++)
                {
                    if ((x3 < 9) && (map[i, x3 + 1, y3] == 2)) count++;
                    if ((x3 > 0) && (map[i, x3 - 1, y3] == 2)) count++;
                    if ((y3 < 9) && (map[i, x3, y3 + 1] == 2)) count++;
                    if ((y3 > 0) && (map[i, x3, y3 - 1] == 2)) count++;
                    if ((x3 < 9) && (y3 < 9) && (map[i, x3 + 1, y3 + 1] == 2)) count++;
                    if ((x3 > 0) && (y3 > 0) && (map[i, x3 - 1, y3 - 1] == 2)) count++;
                    if ((x3 > 0) && (y3 < 9) && (map[i, x3 - 1, y3 + 1] == 2)) count++;
                    if ((x3 < 9) && (y3 > 0) && (map[i, x3 + 1, y3 - 1] == 2)) count++;
                }
            if (count == 0) return true; else return false;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (window == "end")
            {
                string s = e.KeyCode.ToString();
                if ((s.Length == 1) && (s != " "))
                    name += s;
                if ((s == "Back") && (name.Length > 0))
                    name = name.Substring(0, name.Length - 1);
                if (s == "Return")
                    endgame();
                Invalidate();
            }
            if (window == "badend")
                endgame();
        }

        void save()
        {
            StreamWriter write_stream = new StreamWriter(@"result.txt");
            for (int i = 0; i < 5; i++)
                write_stream.WriteLine(results[0, i] + " " + results[1, i]);
            write_stream.Close();
        }

        void open()
        {
            StreamReader read_stream = new StreamReader(@"result.txt");
            string line;
            for (int i = 0; i < 5; i++)
            {
                line = read_stream.ReadLine();
                results[0, i] = line.Substring(0, line.IndexOf(" "));
                results[1, i] = line.Substring(line.IndexOf(" ") + 1, line.Length - line.IndexOf(" ") - 1);
            }
            read_stream.Close();
        }

        void endgame()
        {
            if (window == "end")
                for (int i = 0; i < 5; i++)
                    if (score < Convert.ToInt32(results[1, i]))
                    {
                        for (int j = 4; j > i; j--)
                        {
                            results[1, j] = results[1, j - 1];
                            results[0, j] = results[0, j - 1];
                        }
                        results[0, i] = name;
                        results[1, i] = score.ToString();
                        save();
                        break;
                    }
            antiuser = 0;
            death = 0;
            user = 1;
            create = false;
            create_x1 = true;
            window = "menu";
            ship = new int[4];
            map = new int[2, 10, 10];
            font = new Font("Arial", 32);
            score = 0;
            name = "";
            esetup();
        }

        void esetup()
        {
            int[] eship = new int[4];
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4 - i; j++)
                {

                    bool b = false;
                    int x = 0, y = 0, where = 0;
                    while (!(b))
                    {
                        where = rand.Next(2);
                        x = rand.Next(10 - i);
                        y = rand.Next(10);
                        if (where == 0) b = checkship(1, x, y, x + i, y); else b = checkship(1, y, x, y, x + i);

                    }
                    for (int l = x; l <= x + i; l++)
                        if (where == 0) map[1, l, y] = 2; else map[1, y, l] = 2;
                }
        }
    }
}