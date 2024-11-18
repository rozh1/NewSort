using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace NewSORT
{
    class Program
    {
        static void Main(string[] args)
        {
            //FileSplit("input.txt");
            LinkedList<int> firstFile = GetData("Output_1.txt");
            LinkedList<int> secondFile = GetData("Output_2.txt");
            //Console.WriteLine(firstFile.Find(firstFile.Max()));
            //var item = firstFile.First;
            //while (item != null)
            //{
            //    Console.WriteLine(item.Value);
            //    item = item.Next;
            //}
            List<int> _sortedList = new List<int>();

            List<LinkedList<int>> joinQueues = new List<LinkedList<int>>() ;
            joinQueues.Add(firstFile);
            joinQueues.Add(secondFile);
            List<int> indexes = new List<int> { 0, 1 };
            List<int> workSpace = getWorkSpace(joinQueues, indexes);
            indexes = Algorithm(workSpace, _sortedList);

            workSpace = getWorkSpace(joinQueues, indexes);
            Console.ReadKey();
        }

        //Получение очередей <int>
        private static LinkedList<int> GetData(string inputPath)
        {
            List<int> output = new List<int>();
            string[] data = File.ReadAllLines(inputPath);
            foreach (var line in data)
            {
                string[] lines = line.Split('\t');
                foreach (var number in line.Split('\t'))
                {
                    if (number != "")
                    {
                        output.Add(Convert.ToInt32(number));
                    }
                }
            }
            return new LinkedList<int>(output);
        }
        
        private static List<int> getWorkSpace(List<LinkedList<int>> queues, List<int> indexes)
        {
            List<int> output = new List<int>();

            foreach (var index in indexes)
            {
                output.Add(queues[index].First());
                queues[index].RemoveFirst();
                
            }

            return output;
        }

        private static List<int> Algorithm(List<int> workSpace, List<int> maxElements)
        {
            List<int> indexOfQ = new List<int>();
            int firstMax = workSpace.Max();
            int nextMax = 0;
            indexOfQ.Add(workSpace.IndexOf(firstMax));
            do
            {
                workSpace.Remove(firstMax);
                nextMax = workSpace.Max();

            } while (firstMax == nextMax);

            List<int> maximums = maxElements;
            maximums.Add(firstMax);
            maxElements = maximums;
            return indexOfQ;
        }

        private static void FileSplit(string inputPath)
        {
            bool clear = false;
            List<string> inputData = File.ReadAllLines(inputPath).ToList();
            int lineCount = inputData.Count / 4;
            int linesReady = 0;
            int fileCount = 1;
            for (int i = 0; i < inputData.Count; i++)
            {
                
               
                if (fileCount < 4)
                {
                    if(linesReady == lineCount)
                    {
                        fileCount++;
                        linesReady = 0;
                        clear = false;
                    }
                }

                if (!clear && new FileInfo ("Output_" + fileCount + ".txt").Exists)
                {
                    new FileInfo("Output_" + fileCount + ".txt").Delete();
                    clear = true;
                }                
                File.AppendAllText("Output_" + fileCount + ".txt", inputData[i] + Environment.NewLine);
                linesReady++;

            }
        }
    }
}
