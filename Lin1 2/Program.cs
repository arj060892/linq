using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lin1_2
{
    class Program
    {
        static void Main(string[] args)
        {
            var cars = ProcessCars("fuel.csv");
            var manufacturers = ProcessManufacturers("manufacturers.csv");
            //var query = cars.OrderByDescending(c => c.Combined)
            //    .ThenBy(c=>c.Name);



            //where c.Manufacturer == "BMW" && c.Year == 2016

            var query = from c in cars
                        join m in manufacturers on c.Manufacturer equals m.Name
                        orderby c.Combined descending, c.Name ascending
                        select new
                        {
                            m.Headquarters,
                            c.Name,
                            c.Combined
                        };

            var query2 = cars
                .Join(manufacturers,
                c => c.Manufacturer,
                m => m.Name,
                (c, m) => new
                {
                    m.Headquarters,
                    c.Name,
                    c.Combined
                }).OrderByDescending(c => c.Combined)
            .ThenBy(c => c.Name);

            foreach (var item in query2.Take(10))
            {
                Console.WriteLine($"{item.Headquarters} {item.Name} {item.Combined}");
            }



            var top = cars.OrderByDescending(c => c.Combined)
                .ThenBy(c => c.Name)
                .Select(c => c)
                .First(c => c.Manufacturer == "BMW");
            //Console.WriteLine(top.Name);


            //foreach (var car in query.Take(10))
            //{
            //    Console.WriteLine($"{car.Name} : {car.Year}");
            //}


            //var selectMany = cars.SelectMany(c => c.Manufacturer).OrderBy(c=>c).GroupBy(c=>c);
            //foreach (var item in selectMany)
            //{
            //    Console.WriteLine($"{item.Key} + {item.Count()}");
            //}

        }

        public static List<Manufacturer> ProcessManufacturers(string path)
        {
            return
                File.ReadAllLines(path)
                .Skip(1)
                .Where(m => m.Length > 1)
                .ToManufacturer().ToList();
        }

        public static List<Car> ProcessCars(string path)
        {
            //return File.ReadAllLines(path)
            //    .Skip(1)
            //    .Where(l => l.Length > 1)
            //    .Select(Car.ParseFromCsv).ToList();

            return File.ReadAllLines(path)
               .Skip(1)
               .Where(l => l.Length > 1)
               .ToCar().ToList();

            //return (from l in File.ReadAllLines(path).Skip(1)
            //        where l.Length > 1
            //        select Car.ParseFromCsv(l)).ToList();
        }
    }
    public static class CarExtensions
    {
        public static IEnumerable<Car> ToCar(this IEnumerable<string> source)
        {
            foreach (var line in source)
            {
                var columns = line.Split(',');
                yield return new Car
                {
                    Year = int.Parse(columns[0]),
                    Manufacturer = columns[1],
                    Name = columns[2],
                    Displacement = double.Parse(columns[3]),
                    Cylinders = int.Parse(columns[4]),
                    City = int.Parse(columns[5]),
                    Highway = int.Parse(columns[6]),
                    Combined = int.Parse(columns[7])
                };
            }
        }
        public static IEnumerable<Manufacturer> ToManufacturer(this IEnumerable<string> source)
        {
            foreach (var line in source)
            {
                var columns = line.Split(',');
                yield return new Manufacturer
                {
                    Year = int.Parse(columns[2]),
                    Name = columns[0],
                    Headquarters = columns[1]
                };
            }
        }
    }
}
