using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Runtime.CompilerServices;
using System.Transactions;

/*
 Коллекция – двусвязный список.
Поля данных структуры: номер поезда, станция назначения, вре-
мя отправления, время в пути, наличие билетов.
Дополнительные пункты меню:
− вывести номера поездов и время их отправления в определен-
ный город в заданном временном интервале;
− получить информацию о наличии билетов на поезд с опреде-
ленным номером.
*/

namespace lab2_v18
{
    struct Train
    {
        public string number;
        public string station;
        public string go_time;
        public int time_in_a_way;
        public int tickets; 

        public override string ToString()
        {
            return $"|{this.number}\t| {station}\t\t| {go_time}\t\t\t| {time_in_a_way}\t\t| {tickets}";
        }
    }
    class Program
    {
        public static int CompareTrainsByNumber(Train t1, Train t2)
        {
            return String.Compare(t1.number, t2.number);
        }

        static int Menu()
        {
            int choosed = -1;
            Console.WriteLine("Выберите действие");
            Console.WriteLine("1 - Добавить рейс");
            Console.WriteLine("2 - Убрать рейс под номером");
            Console.WriteLine("3 - Вывести все рейсы");
            Console.WriteLine("4 вывести номера поездов и время их отправления в определенный город в заданном временном интервале");
            Console.WriteLine("5 -получить информацию о наличии билетов на поезд с определенным номером");
            Console.WriteLine("6 - Отсортировать список по номерам поездов");
            Console.WriteLine("0 - Выход");
            Int32.TryParse(Console.ReadLine(), out choosed);

            return choosed;
        }

        static void Main(string[] args)
        {
            string filename = "trains.dat";

            LinkedList<Train> trains = new LinkedList<Train>();

            Console.WriteLine("Введите название файла");
            filename = Console.ReadLine();

            //Бинарный ридер
            if (File.Exists(filename))
            {
                using (BinaryReader sr = new BinaryReader(File.Open(filename, FileMode.Open)))
                {
                    while (sr.PeekChar() != -1)
                    {
                        Train train;
                        train.number = sr.ReadString();
                        train.station = sr.ReadString();
                        train.go_time = sr.ReadString();
                        train.time_in_a_way = sr.ReadInt32();
                        train.tickets = sr.ReadInt32();
                        trains.AddLast(train);
                    }
                }
            }


            int choosed = -1, num;
            while (choosed != 0)
            {
                Console.Clear();
                choosed = Menu();
                switch (choosed)
                {
                    case 1:
                        {
                            Console.Clear();
                            Train t;
                            string string_test;
                            Console.WriteLine("Введите номер поезда: ");
                            string_test = Console.ReadLine();
                            if (string_test != null)
                                t.number = string_test;
                            else
                                break;
                            Console.WriteLine("Введите станцию направления: ");
                            string_test = Console.ReadLine();
                            if (string_test != null)
                                t.station = string_test;
                            else
                                break;
                            Console.WriteLine("Введите вермя отправления в формате чч.мм: ");
                            string_test = Console.ReadLine();
                            if (string_test != null)
                                t.go_time = string_test;
                            else
                                break;
                            Console.WriteLine("Введите длительность рейса (кол-во минут): ");
                            int.TryParse(Console.ReadLine(), out num);
                            if (num > 0)
                                t.time_in_a_way = num;
                            else
                                break;
                            Console.WriteLine("Введите кол-во билетов");
                            int.TryParse(Console.ReadLine(), out num);
                            if (num > -1)
                            {
                                t.tickets = num;
                            }
                            else
                                break;
                            Console.WriteLine("Куда будет добавлять?\n1.В конец\n2.В начало\n3.В определенное место");
                            int.TryParse(Console.ReadLine(), out num);

                            switch (num)
                            {
                                case 1:
                                    trains.AddLast(t);
                                    break;

                                case 2:
                                    trains.AddFirst(t);
                                    break;

                                case 3:
                                    Console.WriteLine("После какого елемента? :");
                                    int.TryParse(Console.ReadLine(), out num);

                                    if (num > 0 && num < trains.Count)
                                        trains.AddAfter(trains.Find(trains.ElementAt(num)), t);
                                    break;
                            }
                        }
                        break;

                    case 2:
                        Console.Clear();
                        Console.WriteLine("Введите номер рейса: ");
                        int.TryParse(Console.ReadLine(), out num);
                        trains.Remove(trains.ElementAt(num));
                        break;

                    case 3:
                        Console.Clear();
                        num = 0;
                        Console.WriteLine("№\t|Номер поезда\t|Конечная станция\t|Время отправления\t|Время в пути\t|Наличие билетов\t");
                        foreach(Train t in trains)
                            Console.WriteLine($"{num++}\t{t}");

                        Console.ReadKey();
                        break;

                    case 4:
                        string time;
                        double time_d;
                        int count = 0;

                        Console.WriteLine("Введите город");
                        string station = Console.ReadLine();
                        Console.WriteLine("Введите интервал:\n от");
                        string time_from = Console.ReadLine();
                        if (time_from != null)
                            time_from = time_from.Replace(":", ",");
                        else
                            break;
                        double time_from_d = Convert.ToDouble(time_from);

                        Console.WriteLine("до ");
                        string time_to = Console.ReadLine();
                        if (time_to != null)
                            time_to = time_to.Replace(":", ",");
                        else
                            break;
                        double time_to_d = Convert.ToDouble(time_to);


                        foreach (Train t in trains)
                        {
                            if((station == t.station))
                            {
                                time = t.go_time;
                                time = time.Replace(":", ",");
                                time_d = Convert.ToDouble(time);
                                if (time_d < time_to_d && time_d > time_from_d)
                                {
                                    Console.WriteLine($"Поезд под номером {t.number}" +
                                        $" отправляется в {t.go_time}");
                                    count++;
                                }
                            }
                        }

                        if (count == 0)
                            Console.WriteLine("Рейс не найден");

                        Console.ReadKey();
                        break;

                    case 5:
                        count = 0;
                        Console.WriteLine("Введите номер поезда");
                        string number = Console.ReadLine();
                        foreach(Train t in trains)
                        {
                            if (number == t.number)
                            {
                                Console.WriteLine($"{t.tickets} билетов");
                                num = 0;
                                count++;
                                break;
                            }
                        }
                        if (number == null)
                        {
                            Console.ReadKey();
                            break;
                        }
                        if (count == 0)
                            Console.WriteLine("Поезд не найден");
                        Console.ReadKey();
                        break;

                    case 6:
                        Train[] temp = new Train[trains.Count];
                        trains.CopyTo(temp, 0);
                        Array.Sort(temp, CompareTrainsByNumber);
                        trains = new LinkedList<Train>(temp);

                        num = 0;
                        Console.WriteLine("№\t|Номер поезда\t|Конечная станция\t|Время отправления\t|Время в пути\t|Наличие билетов\t");
                        foreach (Train t in trains)
                            Console.WriteLine($"{num++}\t{t}");

                        Console.ReadKey();
                        break;

                    case 0:
                        Console.WriteLine("Выход...");
                        break;

                    default:
                        Console.WriteLine("Input Error");
                        break;
                }
            }

            using (BinaryWriter sr = new BinaryWriter(File.Open(filename, FileMode.Create)))
            {
                foreach (Train t in trains)
                {
                    sr.Write(t.number);
                    sr.Write(t.station);
                    sr.Write(t.go_time);
                    sr.Write(t.time_in_a_way);
                    sr.Write(t.tickets);
                }
            }
            Console.ReadKey();
        }
    }
}