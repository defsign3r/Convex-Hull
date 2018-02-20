using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace tbb
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Point [] random_point = new Point[10];
        Point [] final_point = new Point[9];
        Random rand = new Random();
        Graphics g;
        ArrayList arr_point = new ArrayList();
        double[] m = new double[10];
        double[] n = new double[10];
        private Pen pen = new Pen(Color.Black, 2);
        private Brush brush = Brushes.Red;
        private void button1_Click(object sender, EventArgs e)
        {
            this.Refresh();
            for (int i = 0; i < random_point.Length; i++)
            {
                random_point [i].X = rand.Next(100, 300);
                random_point [i].Y = rand.Next(100, 300);
                g = this.CreateGraphics();
                g.FillEllipse(brush, random_point [i].X-2, random_point [i].Y-2, 5, 5);
                g.DrawString("" + i, new Font("宋体", 10), Brushes.Blue, new PointF(random_point [i].X + 5, random_point [i].Y + 5));
            }
        }

        public double Polar_angle(Point a, Point b)//定义一个函数，通过两点求极角
        {
            double polar_angle = Math.PI / 2;
            if (a.X != b.X)
            {
                polar_angle = Math.Atan((double)(b.Y-a.Y) / (double)(a.X - b.X));
            }
            return polar_angle; 
        }

        public double Ed(Point a, Point b)//定义一个函数，通过两端点求线段长度
        {
            double ed = System.Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y));
            return ed;
        }

        private  void button2_Click(object sender, EventArgs e)
        {
            int max = 0;
            int thefirstpoint = 0;
            for (int i = 0; i < random_point.Length; i++)//获取初始点
            {
                if (max <= random_point[i].Y)
                {
                    max = random_point[i].Y;
                    thefirstpoint = i;
                }
            }
            for (int i = 0; i < random_point.Length; i++)//获取极角数组
            {
                if (Polar_angle(random_point[i], random_point[thefirstpoint]) > 0)
                {
                    m[i] = Polar_angle(random_point[i], random_point[thefirstpoint]);
                }
                else
                {
                    m[i] = Polar_angle(random_point[i], random_point[thefirstpoint]) + Math.PI;
                }
                n[i] = Polar_angle(random_point[i], random_point[thefirstpoint]);
            }
            int length = 10;
            for (int i = 0; i < m.Length-1; i++)//排序
            {
                for (int j = 0; j < m.Length-1; j++)
                {
                    if (m[j] > m[j + 1])
                    {
                        double k = m[j];
                        m[j] = m[j + 1];
                        m[j + 1] = k;
                    }
                }
                if (m[i] == m[i+1])
                {
                    length--;
                }
            }
            Point[] new_point = new Point[10];
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < random_point.Length; j++)
                {
                    if (m[i] == Polar_angle(random_point[j], random_point[thefirstpoint]))
                    {
                        new_point[i] = random_point[j];
                    }
                    if (m[i] == Polar_angle(random_point[j], random_point[thefirstpoint]) + Math .PI )
                    {
                        new_point[i] = random_point[j];
                    }
                }
            }
            List<Point> list = new_point.ToList();
            for (int i = 0; i < new_point.Length; i++)
            {
                if (new_point[i] == random_point[thefirstpoint])
                {
                    list.RemoveAt(i);
                }
            }
            final_point = list.ToArray();
            g.DrawLine(pen, final_point[8], random_point[thefirstpoint]);
            g.DrawLine(pen, final_point[0], random_point[thefirstpoint]);
            for (int i = 0; i < list.Count-2; i++)
            {
                Point point_a = list [i];
                Point point_b = list [i + 1];
                Point point_c = list [i + 2];
                double topointb = (point_a.X - point_b.X) * (point_c.Y - point_b.Y) - (point_a.Y - point_b.Y) * (point_c.X - point_b.X);
                double tothefirstpoint = (point_a.X - random_point[thefirstpoint].X) * (point_c.Y - random_point[thefirstpoint].Y) - (point_a.Y - random_point[thefirstpoint].Y) * (point_c.X - random_point[thefirstpoint].X);
                if (topointb * tothefirstpoint > 0)//如果在同侧，就将该点从list中去除掉
                {
                    list.RemoveAt(i + 1);
                    i = 0;
                }
            }
            for (int i = 0; i < list.Count-1; i++)//根据list中剩余的点依次画出凸闭包
            {
                g.DrawLine(pen, list[i], list[i+1]);
            }
        }
    }
}
