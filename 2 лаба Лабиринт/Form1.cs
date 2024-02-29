using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace _2_лаба_Лабиринт
{
    public partial class Form1 : Form
    {
        private static int height = 11;
        private static int width = 11;
        private static int[,] labirint = new int[height, width];
        private static int[,] paths = new int[height, width];

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label2.Text = "Выход";
            labirint = new int[height, width];
            paths = new int[height, width];
            dataGridView1.Rows.Clear();
            GenerateLabirint();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SearchShortestPath();
        }

        private void GenerateLabirint()
        {
            dataGridView1.RowCount = height;
            dataGridView1.ColumnCount = width;
            int cellSize = 25;
            dataGridView1.RowTemplate.Height = cellSize;
            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                col.Width = cellSize;
            }
            Random wall = new Random();
            for (int i = 0; i < height; i++)
            {
                for (int k = 0; k < 4; k++)
                {
                    int j = wall.Next(0, width);
                    labirint[i, j] = 1;
                }
            }
            labirint[0, 0] = 0;
            labirint[height - 1, width - 1] = 0;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    dataGridView1.Rows[i].Cells[j].Value = labirint[i, j];
                    if (labirint[i, j] == 1)
                    {
                        dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.Black;
                    }
                }
            }
        }

        private void SearchShortestPath()
        {
            Queue<Point> points = new Queue<Point>();
            bool[,] visitCells = new bool[height, width];

            Point start = new Point(0, 0);
            points.Enqueue(start);
            visitCells[start.X, start.Y] = true;
            while (points.Count > 0)
            {
                Point current = points.Dequeue();
                if (current == new Point(10, 10))
                    break;
                int[] dx = { -1, 1, 0, 0 };
                int[] dy = { 0, 0, -1, 1 };
                for (int i = 0; i < 4; i++)
                {
                    int newX = current.X + dx[i];
                    int newY = current.Y + dy[i];
                    if (newX >= 0 && newX < height && newY >= 0 && newY < width &&
                        labirint[newX, newY] == 0 && !visitCells[newX, newY])
                    {
                        labirint[newX, newY] += 1;
                        points.Enqueue(new Point(newX, newY));
                        visitCells[newX, newY] = true;
                        paths[newX, newY] = paths[current.X, current.Y] + 1;
                    }
                }
            }
            if (!visitCells[10, 10])
            {
                label2.Text = "Нет пути!";
                return;
            }
            Point end = new Point(10, 10);
            int shortPath = paths[end.X, end.Y];
            while (shortPath > 0)
            {
                dataGridView1.Rows[end.X].Cells[end.Y].Style.BackColor = Color.Red;
                int[] dx = { -1, 1, 0, 0 };
                int[] dy = { 0, 0, -1, 1 };
                for (int i = 0; i < 4; i++)
                {
                    int newX = end.X + dx[i];
                    int newY = end.Y + dy[i];
                    if (newX >= 0 && newX < height && newY >= 0 && newY < width &&
                        paths[newX, newY] == shortPath - 1)
                    {
                        end = new Point(newX, newY);
                        shortPath--;
                        break;
                    }
                }
            }


            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    dataGridView1.Rows[i].Cells[j].Value = paths[i, j];
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label2.Text = "Выход";
        }
    }
}
