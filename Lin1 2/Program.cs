using System;
using System.Collections.Generic;
using System.Data.Entity;
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
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<CarDb>());
            InsertData();
            //QueryDate();









            //var cars = ProcessCars("fuel.csv");
            //var manufacturers = ProcessManufacturers("manufacturers.csv");

            //var query =
            //    from car in cars
            //    group car by car.Manufacturer into carGroup
            //    select new
            //    {
            //        Name = carGroup.Key,
            //        Max = carGroup.Max(c => c.Combined),
            //        Min = carGroup.Min(c => c.Combined),
            //        Avg = carGroup.Average(c => c.Combined)
            //    } into result
            //    orderby result.Name ascending, result.Max descending
            //    select result;


            //var query2 = cars.GroupBy(c => c.Manufacturer).
            //    Select(g =>
            //    {
            //        var results = g.Aggregate(new CarStatics(),
            //            (acc, c) => acc.Accumulate(c),
            //            acc => acc.Compute()
            //            );
            //        return new
            //        {
            //            Name = g.Key,
            //            Max = results.Max,
            //            Min = results.Min,
            //            Avg = results.Average
            //        };
            //    })
            //    .OrderByDescending(r => r.Max);


            //foreach (var item in query)
            //{
            //    Console.WriteLine($"{item.Name}");
            //    Console.WriteLine($"\t{item.Min}");
            //    Console.WriteLine($"\t{item.Max}");
            //    Console.WriteLine($"\t{item.Avg}");
            //}









            //var query =
            //     from m in manufacturers
            //     join c in cars on m.Name equals c.Manufacturer
            //         into carGroup
            //     select new
            //     {
            //         man = m,
            //         cars = carGroup
            //     } into result
            //     orderby result.man.Headquarters
            //     group result by result.man.Headquarters;


            //var query2 =
            //    manufacturers.GroupJoin(cars, m => m.Name, c => c.Manufacturer,
            //    (m, g) => new
            //    {
            //        man = m,
            //        cars = g
            //    }).GroupBy(m => m.man.Headquarters);


            //foreach (var group in query)
            //{
            //    Console.WriteLine($"{group.Key}");
            //    foreach (var car in group.SelectMany(c=>c.cars)
            //        .OrderByDescending(c=>c.Combined).Take(3))
            //    {
            //        Console.WriteLine($"\t{car.Name} : {car.Combined}");
            //    }
            //}





















            //var query = from manu in manufacturers
            //            join car in cars on manu.Name equals car.Manufacturer
            //            into carGroup
            //            orderby manu.Name
            //            select new
            //            {
            //                Manf = manu,
            //                Carss = carGroup
            //            };
            //var query2 = manufacturers.GroupJoin(cars, m => m.Name, c => c.Manufacturer, (m, g) =>
            //new
            //{
            //    Manf = m,
            //    Carss = g
            //}).OrderBy(m => m.Manf.Name);

            //foreach (var result in query)
            //{
            //    Console.WriteLine($"{result.Manf.Name} : {result.Manf.Headquarters}");
            //    foreach (var car in result.Carss.OrderByDescending(c => c.Combined).Take(2))
            //    {
            //        Console.WriteLine($"\t{car.Name} : {car.Combined}");
            //    }

            //}

            //var query = from car in cars
            //            group car by car.Manufacturer into man
            //            orderby man.Key
            //            select man;
            //var query2 =
            //        cars.GroupBy(c => c.Manufacturer.ToUpper())
            //        .OrderBy(c => c.Key);


            //foreach (var result in query.OrderByDescending(c=>c.Count()))
            //{
            //    Console.WriteLine($"Name : {result.Key} : Count {result.Count()}");

            //}
            //foreach (var result in query)
            //{
            //    Console.WriteLine($"{result.Key} : Count {result.Count()}");
            //    foreach (var car in result.OrderByDescending(c => c.Combined).Take(2))
            //    {
            //        Console.WriteLine($"\t{car.Name} : {car.Combined}");
            //    }

            //}










            //var query = cars.OrderByDescending(c => c.Combined)
            //    .ThenBy(c=>c.Name);
            //where c.Manufacturer == "BMW" && c.Year == 2016

            //var query = from c in cars
            //            join m in manufacturers on new { c.Manufacturer, c.Year } equals new { Manufacturer = m.Name, m.Year }
            //            orderby c.Combined descending, c.Name ascending
            //            select new
            //            {
            //                m.Headquarters,
            //                c.Name,
            //                c.Combined
            //            };

            //var query2 = cars
            //    .Join(manufacturers,
            //    c => c.Manufacturer,
            //    m => m.Name,
            //    (c, m) => new
            //    {
            //        m.Headquarters,
            //        c.Name,
            //        c.Combined
            //    }).OrderByDescending(c => c.Combined)
            //.ThenBy(c => c.Name);

            //foreach (var item in query2.Take(10))
            //{
            //    Console.WriteLine($"{item.Headquarters} {item.Name} {item.Combined}");
            //}



            //var top = cars.OrderByDescending(c => c.Combined)
            //    .ThenBy(c => c.Name)
            //    .Select(c => c)
            //    .First(c => c.Manufacturer == "BMW");
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

        private static void QueryDate()
        {
            throw new NotImplementedException();
        }

        private static void InsertData()
        {
            var cars = ProcessCars("fuel.csv");
            var db = new CarDb();

            if (!db.Cars.Any())
            {
                foreach (var car in cars)
                {
                    db.Cars.Add(car);
                }
                db.SaveChanges();
            }
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

    public class CarStatics
    {
        public int Max { get; set; }
        public int Min { get; set; }
        public double Average { get; set; }
        public int Total { get; set; }
        public int Count { get; set; }
        public CarStatics()
        {
            Max = Int32.MinValue;
            Min = Int32.MaxValue;
        }
        internal CarStatics Accumulate(Car c)
        {
            Total += c.Combined;
            Count++;
            Max = Math.Max(Max, c.Combined);
            Min = Math.Min(Min, c.Combined);
            return this;
        }

        internal CarStatics Compute()
        {
            Average = Total / Count;
            return this;
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
