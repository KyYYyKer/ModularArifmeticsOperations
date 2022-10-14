using System;

/*
 * У програмній реалізації мають бути такі можливості: 
 *  ~ задавати модуль m, за яким будуть вестись розрахунки
 *  ~ розв’язувати рівняння виду  a mod m = x
 *  ~ розв’язувати рівняння виду a^b mod m = x
 *  ~ розв’язувати рівняння виду  a*x ≡ b mod m
 *  ~ генерувати просте число у діапазоні від A до B.
*/
namespace PowOnMod
{
    class Module
    {
        // метод который представляет целое число в двоичном виде
        static private string ToBinInt(double number) 
        {
            string str = "";
            while (number > 0)
            {
                str = String.Concat(Convert.ToString(number % 2), str);
                number = Math.Truncate(number / 2);
            }
            return str;
        }
        // метод для поднесения числа в степень по модулю
        // также этот метод может выполнять задание 2 (розв’язувати рівняння виду  a mod m = x), только если а > 0
        // для этого в параметр degree нужно вписать 1
        public long PowOnMod(long number, long degree, long module)
        {
            long t, d;  //воспомогательные переменные
            string degree_bin = ToBinInt(degree);

            //в алгоритме идём с конца к началу, по этому последнее значение может быть или 1 или 0 
            //в зависимости от этого числа определяем переменные d, t
            if (degree_bin[degree_bin.Length - 1] == '1')
            {
                d = (1 * number) % module;
                t = (d * d) % module;
            }
            else
            {
                d = 1;
                t = (number * number) % module;
            }

            //начинаем с предпоследнего числа
            for (int i = degree_bin.Length - 2; i >= 0; i--)
            {
                if (degree_bin[i] == '1')
                {
                    d = (d * t) % module;
                    t = (t * t) % module;
                }
                else
                {
                    t = (t * t) % module;
                }
            }
            //Console.WriteLine($"{number} ^ {degree} mod{module} = {d}");
            return d;
        }
        //метод поднисения числа по модулю другого числа
        //метод для 2 задания (розв’язувати рівняння виду  a mod m = x), только а є Z
        public long Mod(long number, long module)
        {
            long res = number;
            if (number < 0)
                for (; res < 0; res += module);
            else
                for (; res > module; res -= module);
            return res;
            
        }
        //метод для проверки числа на простоту (тест Миллера-Рабина)
        public bool testMillerRabin(long m)
        {
            Random rnd = new Random();

            if (m == 2 || m == 3)
                return true;
            if (m % 2 == 0 || m == 1)
                return false;

            long s = 0;
            long t = m - 1;
            long x = 0;
            long r1 = 2;
            long r2 = m - 2;
            long a;
            double r = (Math.Log(m) / Math.Log(2));

            while (t != 0 && t % 2 == 0)
            {
                s++;
                t /= 2; ;
            }

            for (long i = 0; i < r; i++)
            {

                a = r1 + rnd.Next() % (r2 - r1);
                x = PowOnMod(a, t, m);//

                if (x == 1 || x == m - 1)
                    continue;

                for (long j = 0; j < s - 1; j++)
                {
                    x = PowOnMod(x, 2, m);

                    if (x == 1)
                        return false;
                    if (x == m - 1)
                        break;
                }

                if (x == m - 1)
                    continue;

                return false;
            }
            return true;
        }
        //метод для нахождения функции эйлера
        public long Phi(long number)
        {
            long res = number;
            for (long i = 2; i * i <= number; ++i)
                if (number % i == 0)
                {
                    while (number % i == 0)
                        number /= i;
                    res -= res / i;
                }
            if (number > 1)
                res -= res / number;
            return res;
        }
        //метод для нахождения х из уравнения a*x ≡ b mod m
        public long FindComparisonX(long a, long b, long module)
        {
            long res = PowOnMod(a, Phi(module) - 1, module);
            res = Mod(b * res, module);
            return res;
        }

        //метод для генерации сильного простого числа, с помощью алгоритма Гордона
        public long GenStrongPrime()
        {
            int s, t;
            long r, v, p0, p, i, j;
            Random rnd = new Random();

            s = rnd.Next(1000);
            t = rnd.Next(1000);
            if (testMillerRabin(s) == false)
            {
                do
                {
                    s = rnd.Next(1000); 
                } while (testMillerRabin(s) == false);
            }
            if (testMillerRabin(t) == false)
            {
                do
                {
                    t = rnd.Next(1000);
                } while (testMillerRabin(t) == false);
            }

            i = 3;

            do
            {
                r = 2 * i * t + 1;
                i = i + 1;
                if (testMillerRabin(r) == true)
                    break;
            } while (true);

            v = PowOnMod(s, r - 2, r);

            p0 = 2 * v * s - 1;

            j = 5;

            do
            {
                p = p0 + 2 * j * r * s;
                j = j + 1;
                if (testMillerRabin(p) == true)
                    break;
            } while (true);

            return p;
        }
        //самый просто метод генерации простого числа, с помощью класса Random и теста Миллера-Рабина
        public long GenPrimeInRange(int lrange, int rrange)
        {
            Random rnd = new Random();
            long res;
            do
            {
                res = rnd.Next(lrange, rrange);
            } while(testMillerRabin(res) == false);
            return res;
        }
    }
    internal class Program
    {
        
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            Console.InputEncoding = System.Text.Encoding.Unicode;




            Console.WriteLine("Task 1 (задавати модуль m, за яким будуть вестись розрахунки):");
            Console.Write("Plese enter a module: ");
            long module = long.Parse(Console.ReadLine());




            Console.WriteLine("Task 2 (розв’язувати рівняння виду  a mod m = x):");
            Module task2 = new Module();
            string str2 = "";
            do
            {
                Console.Write("Please enter a digit number: ");
                long number = long.Parse(Console.ReadLine());
                Console.WriteLine($"{number} mod{module} = {task2.Mod(number, module)}");
                Console.Write("If you want to stop, type STOP: ");
                str2 = Console.ReadLine();
            } while (str2 != "STOP");




            Console.WriteLine("Task 3 (розв’язувати рівняння виду a^b mod m = x):");
            Module task3 = new Module();
            string str3 = "";
            do
            {
                Console.Write("Please enter a digit number: ");
                long number = long.Parse(Console.ReadLine());
                Console.Write("Please enter a degree: ");
                long degree = long.Parse(Console.ReadLine());
                Console.WriteLine($"{number} ^ {degree} mod{module} = {task3.PowOnMod(number, degree, module)}");
                Console.Write("If you want to stop, type STOP: ");
                str3 = Console.ReadLine();
            } while (str3 != "STOP");
            



            Console.WriteLine("Task 4 (розв’язувати рівняння виду  a*x ≡ b mod m):");
            Console.WriteLine("Please enter an a value and b value:");
            long a = long.Parse(Console.ReadLine());
            long b = long.Parse(Console.ReadLine());
            Module task4 = new Module();
            long x = task4.FindComparisonX(a, b, module);
            Console.WriteLine($"Your comparison {a} * x ≡ {b} mod{module} answer is {x}. ({a} * x = {b} mod{module} ≡ {x})");




            Console.WriteLine("Task 5 (генерувати просте число у діапазоні від A до B):");
            Console.WriteLine("Please enter your left and right range: ");
            int l_range = int.Parse(Console.ReadLine());
            int r_range = int.Parse(Console.ReadLine());
            Module task5 = new Module();
            long prime = task5.GenPrimeInRange(l_range, r_range);
            long strong_prime = task5.GenStrongPrime();
            Console.WriteLine($"Your prime from range [{l_range}..{r_range}] is {prime}. And some strong prime {strong_prime}");

        }
    }
}
