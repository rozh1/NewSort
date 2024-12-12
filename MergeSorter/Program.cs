using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MergeSorter
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 3)
            {
                Console.WriteLine("Пример запуска:");
                Console.Write($"{AppDomain.CurrentDomain.FriendlyName} ");
                Console.WriteLine("orders_1.csv orders_2.csv orders_3.csv orders_result.csv 5:string:desc 2:string:desc 4:string:desc 0:int:desc");
                Console.Read();
                return;
            }

            var config = new SortConfig();
            var files = new List<string>();

            foreach (string arg in args)
            {
                if (arg.Contains(":"))
                {
                    var parts = arg.Split(':');
                    var index = int.Parse(parts[0]);
                    var type = ColumnType.Default;
                    var ascSort = parts[2].ToLower() == "asc";
                    switch (parts[1].ToLower())
                    {
                        case "string":
                            type = ColumnType.String;
                            break;
                        case "int":
                            type = ColumnType.Int;
                            break;
                        case "float":
                            type = ColumnType.Float;
                            break;
                        case "double":
                            type = ColumnType.Double;
                            break;
                        case "decimal":
                            type = ColumnType.Decimal;
                            break;
                    }
                    config.Fields.Add(new SortConfigField() { Index = index, Type = type, IsAscSort = ascSort });
                }
                else
                {
                    files.Add(arg);
                }
            }

            var inputFiles = files.Take(files.Count - 1).ToList();
            var outputFile = files.Last();

            var sorter = new Sorter(config);

            var sw = new Stopwatch();
            // нужно только теста сортировки. для мержа нет нужды сортировать файлы заново
            //sw.Start();
            //Parallel.For(0, inputFiles.Count, (i) =>
            //{
            //    sorter.SortFile(inputFiles[i]);
            //});
            //sw.Stop();
            //Console.WriteLine(sw.Elapsed.ToString());

            sw.Restart();
            sorter.MergeFiles(inputFiles, outputFile);
            sw.Stop();
            Console.WriteLine(sw.Elapsed.ToString());
        }
    }
}
