using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace miniGauss
{
    class Program
    {
        static void Main(string[] args)
        {
            int i, j;
            int m, n; // m - строки, n - столбцы
            double[,] Matrix; // матрица коеффициентов
            Program a = new Program();
            Gauss g = new Gauss();
            Console.WriteLine("Введите размерность матрицы");
            Console.Write("Количество строк: ");
            m = Convert.ToInt32(Console.ReadLine());
            Console.Write("Количество столбцов: ");
            n = Convert.ToInt32(Console.ReadLine());
            Matrix = new double[m, n];                                             // определение матрицы
            Console.WriteLine("Введите элементы матрицы через \'пробел\'");
            for (i = 0; i < m; i++)
            {
                j = 0;
                string stroka = Console.ReadLine();
                stroka = stroka.Trim();
                string[] strMass = stroka.Split(' ');
                for (j = 0; j < n; j++)
                {
                    Matrix[i, j] = Convert.ToDouble(strMass[j]);                        // заполнение матрицы через строку
                }
            }
            a.Out(Matrix,"Начальная матрица");
            // ----------------- НАЧАЛО РАБОТЫ -------------------------
            for(int t = 0; t<n-1; t++) // перебираем столбцы
            {
                double[] stroka = new double[n];
                int[] place = g.suprem(Matrix, t);
                for(i = 0; i < n; i++) // заполняем строку
                {
                    stroka[i] = Matrix[place[0], i];
                }
                //a.Out(stroka,"Строка с главным компонентом");
                stroka = g.div(stroka, Matrix[place[0], place[1]]); // делим строку

                for (i = 0; i < n; i++) // изменяем главную строку в массиве
                {
                    Matrix[place[0], i] = stroka[i];
                }
                a.Out(Matrix,"Массив после деления строки с главной компонентой");
                // -------Зануление столбца--------------
                for(i = 0; i < m; i++) //перебираем строки
                {
                    if (i == place[0]) continue;
                    double[] present = new double[n];
                    for (j = 0; j < n; j++) // перебираем столбцы и заполняем текущую строку для изменений
                    {
                        present[j] = Matrix[i, j];
                    }
                    a.Out(present, "Текущая строка для изменения");
                    double[] change = new double[n];
                    for (int s = 0; s < n; s++) change[s] = stroka[s];
                    double pow = 0;
                    if ((present[t] > 0.0 && change[t] > 0.0) || (present[t] < 0.0 && change[t] < 0.0)) pow = -1.0 * present[t];
                    else if (((present[t] > 0.0) && (change[t] < 0.0)) || ((present[t] < 0.0) && (change[t] > 0.0))) pow = Math.Abs(present[t]);
                    change = g.multi(change, pow);
                    a.Out(change, "Строка для вычитания из другой строки");
                    present = g.minus(present, change);
                    a.Out(present, "Строка после вычитания");
                    for (j = 0; j < n; j++) // перебираем столбцы и заполняем текущую строку внутри матрицы
                    {
                        Matrix[i, j] = present[j];
                    }
                    a.Out(Matrix, "Матрица после сложения строк");
                }
            }
            double[] X = new double[m];
            for (i = 0; i < m; i++)
            {
                for (j = 0; j < n - 1; j++)
                {
                    if (Matrix[j, i] == 1.0)
                    {
                        X[i] = Matrix[j, n-1];
                        break;
                    }
                }
            }
            a.Out(X, "ОТВЕТ:");



        }
        public void Out(double[,] Array, string Mess)   // вывод матрицы
        {
            int i, j;
            Console.WriteLine("\n \n" + Mess);
            for (i = 0; i < Array.GetLength(0); i++)
            {
                for (j = 0; j < Array.GetLength(1); j++)
                {
                    Console.Write(Array[i, j]);
                    Console.Write("\t");
                }
                Console.WriteLine();
            }
            Console.WriteLine("Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }
        public void Out(double[] Array, string Mess)   // вывод строки
        {
            int i;
            Console.WriteLine("\n \n" + Mess);
            for (i = 0; i < Array.GetLength(0); i++)
            {
                    Console.Write(Array[i]);
                    Console.Write("\t");
            }
            Console.WriteLine("\n Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }
    }
    class Gauss
    {
        int i = 0;
        public int[] suprem(double[,] Matrix, int n) // возвращает адрес максимального элемента в столбце № n
        {
            int[] place = new int[2];
            double max = 0;
            for ( int i = 0; i< Matrix.GetLength(0); i++)
            {
                if (Math.Abs(Matrix[i,n])>Math.Abs(max))
                {
                    max = Matrix[i, n];
                    place[0] = i; place[1] = n;
                }
            }
            return place;
        }
        public double[] minus(double[] a, double[] b) // вычитает из первой строки вторую
        {
            double[] c = new double[a.Length];
            for (i = 0; i < a.Length; i++)
            {
                c[i] = a[i] + b[i];
            }
            return c;
        }
        public double[] div(double[] pole, double denominator) //делит все элементы строки на число
        {
            for (i = 0; i < pole.Length; i++)
            {
                pole[i] /= denominator;
            }
            return pole;
        }
        public double[] multi(double[] pole, double multipler) //умножает все элементы строки на число
        {
            for (i = 0; i < pole.Length; i++)
            {
                pole[i] *= multipler;
            }
            return pole;
        }
    }
}
