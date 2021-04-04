using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Globalization;

namespace Lab_2.Models.Collections
{
    public class V5DataOnGrid : V5Data, IEnumerable<DataItem>
    {
        public Grid2D net { get; set; }
        public Vector2[,] mas { get; set; }

        public V5DataOnGrid(string s, DateTime d, Grid2D gr) : base(s, d)
        {
            net = gr;
            mas = new Vector2[net.x_kol, net.y_kol];
        }

        /*public V5DataOnGrid(string filename) // в файле все далее перечисленное на отдельной строке: сначала информация (типа string, для базового класса), время (типа string в формате "MM/dd/yyyy hh:mm", для базового класса), шаг по сетке для оси х (тип float), количество узлов по оси х (тип int), шаг по сетке для оси у (тип float), количество узлов на сетке (тип int), далее на каждой строке значения поля (тип float) 
        {
            try
            {
                using (StreamReader sr = new StreamReader(filename))
                {
                    string constr_information, constr_time_str, x0_step_str, y0_step_str, x0_node_str, y0_node_str, field;
                    constr_information = sr.ReadLine();
                    constr_time_str = sr.ReadLine();
                    x0_step_str = sr.ReadLine();
                    x0_node_str = sr.ReadLine();
                    y0_step_str = sr.ReadLine();
                    y0_node_str = sr.ReadLine();
                    CultureInfo provider = new CultureInfo("en-US"); // сделала явными региональные настройки
                    //CultureInfo provider = CultureInfo.InvariantCulture;
                    DateTime constr_time = DateTime.ParseExact(constr_time_str, "MM/dd/yyyy hh:mm", provider);
                    float x0_step = float.Parse(x0_step_str, provider);
                    int x0_node = int.Parse(x0_node_str, provider);
                    float y0_step = float.Parse(y0_step_str, provider);
                    int y0_node = int.Parse(y0_node_str, provider);
                    Grid1D x0 = new Grid1D(x0_step, x0_node);
                    Grid1D y0 = new Grid1D(y0_step, y0_node);
                    base.information = constr_information;
                    base.time = constr_time;
                    x = x0;
                    y = y0;
                    value = new double[x.node, y.node];
                    for (int i = 0; i < x.node; i++)
                    {
                        for (int j = 0; j < y.node; j++)
                        {
                            field = sr.ReadLine();
                            value[i, j] = float.Parse(field, provider);

                        }
                    }
                }

            }
            catch (ArgumentException)
            {
                Console.WriteLine("Filename is empty string");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File is not found");
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("Directory is not found");
            }
            catch (IOException)
            {
                Console.WriteLine("Unacceptable filename");
            }
            catch (FormatException)
            {
                Console.WriteLine("String could not be parsed");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }*/

        public void InitRandom(float minValue, float maxValue)
        {
            Random rand = new Random(123);
            float rand1, rand2, minv, maxv;
            Vector2 coordinate, value;

            for (int i = 0; i < net.x_kol; i++)
            {
                for (int j = 0; j < net.y_kol; j++)
                {
                    rand1 = (float)rand.NextDouble();
                    rand2 = (float)rand.NextDouble();
                    minv = minValue * rand1 + maxValue * (1 - rand1);
                    maxv = minValue * rand2 + maxValue * (1 - rand2);
                    mas[i, j] = new Vector2(minv, maxv);

                    coordinate.X = i;
                    coordinate.Y = j;
                    value.X = minv;
                    value.Y = maxv;
                }
            }
        }


        public override Vector2[] NearEqual(float eps)
        {
            List<Vector2> list = new List<Vector2>();
            for (int i = 0; i < net.x_kol; i++)
                for (int j = 0; j < net.y_kol; j++)
                    if (Math.Abs(mas[i, j].X - mas[i, j].Y) <= eps)
                        list.Add(mas[i, j]);
            Vector2[] array = list.ToArray();
            return array;
        }

        public override string ToString()
        {
            return "V5DataOnGrid\n" + info + " " + date.ToString() + " " + net.ToString() + "\n";
        }

        public override string ToLongString()
        {
            string str = "V5DataOnGrid\n";
            str += info + " " + date.ToString() + " " + net.ToString() + "\n";
            for (int i = 0; i < net.x_kol; i++)
                for (int j = 0; j < net.x_kol; j++)
                {
                    str += "[" + i + ", " + j + "] " + "(" + mas[i, j].X + ", " + mas[i, j].Y + ")\n";
                }
            str += "\n";
            return str;
        }

        public string ToLongString(string format)
        {
            string str = "V5DataOnGrid(ls):\n" + info + " " + date.ToString(format) + " " + net.ToString(format) + "\n";
            for (int i = 0; i < net.x_kol; i++)
                for (int j = 0; j < net.y_kol; j++)
                {
                    str += "Score for node " + "[" + i + "," + j + "] " + " is " + "(" + mas[i, j].X + "," + mas[i, j].Y + ")\n";
                }

            return str;
        }

        public static explicit operator V5DataCollection(V5DataOnGrid d)
        {
            V5DataCollection Out;
            Vector2 x, x_1;
            Out = new V5DataCollection(d.info, d.date);

            for (int i = 0; i < d.net.x_kol; i++)
                for (int j = 0; j < d.net.y_kol; j++)
                {
                    x = new Vector2(i, j);
                    x_1 = new Vector2(d.mas[i, j].X, d.mas[i, j].Y);
                    Out.dic.Add(x, x_1);
                }
            return Out;
        }

        public IEnumerator<DataItem> GetEnumerator()
        {
            List<DataItem> list = new List<DataItem>();
            DataItem tmp;
            Vector2 coordinate, value;
            for (int i = 0; i < net.x_kol; i++)
                for (int j = 0; j < net.y_kol; j++)
                {
                    coordinate.X = i;
                    coordinate.Y = j;
                    value.X = mas[i, j].X;
                    value.Y = mas[i, j].Y;
                    tmp = new DataItem(coordinate, value);
                    list.Add(tmp);
                }
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            List<DataItem> list = new List<DataItem>();
            DataItem tmp;
            Vector2 coordinate, value;
            for (int i = 0; i < net.x_kol; i++)
                for (int j = 0; j < net.y_kol; j++)
                {
                    coordinate.X = i;
                    coordinate.Y = j;
                    value.X = mas[i, j].X;
                    value.Y = mas[i, j].Y;
                    tmp = new DataItem(coordinate, value);
                    list.Add(tmp);
                }
            return list.GetEnumerator();
        }
    }
}
