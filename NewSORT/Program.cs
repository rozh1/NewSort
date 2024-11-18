using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Linq.Expressions;

namespace NewSORT
{
    class Program
    {
        static void Main(string[] args)
        {
            FileSplit("input_big.txt");
            
            LinkedList<int> firstFile = GetData("Output_1.txt");
            LinkedList<int> secondFile = GetData("Output_2.txt");
            LinkedList<int> thirdFile = GetData("Output_3.txt");
            LinkedList<int> fourthFile = GetData("Output_4.txt");
            

            List<LinkedList<int>> joinQueues = new List<LinkedList<int>>() ;
            joinQueues.Add(firstFile);
            joinQueues.Add(secondFile);
            joinQueues.Add(thirdFile);
            joinQueues.Add(fourthFile);

            List<int> result = SortThis(joinQueues);

            string line = "";
            foreach (int number in result)
            {
                line = line + number + '\t';
            }
            File.WriteAllText("result.txt", line);

           
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
            output.Sort();
            output.Reverse();
            return new LinkedList<int>(output);
        }
        
        private static List<int> SortThis(List<LinkedList<int>> queues)
        {
            List<int> sortedResult = new List<int>();
            List<LinkedListNode<int>> workSpace = new List<LinkedListNode<int>>();
            for (int i = 0; i < queues.Count; i++)
            {
                workSpace.Add(queues[i].First);
            }
            sortedResult = SortingViaMax(workSpace);
            return sortedResult;
        }

        private static List<int> SortingViaMax(List<LinkedListNode<int>> workSpace)
        {
            List<int> sortedValue = new List<int>();

            while (!isWorkSapceEmpty(workSpace))
            {
                LinkedListNode<int> maxNode = FindMaxNode(workSpace);
                sortedValue.Add(maxNode.Value);
                workSpace[workSpace.IndexOf(maxNode)] = maxNode.Next;
            }

            return sortedValue;
        }

        private static LinkedListNode<int> FindMaxNode(List<LinkedListNode<int>> workSpace)
        {
            workSpace = DeleteEmptyNodes(workSpace);
            LinkedListNode<int> result = workSpace.First();
            if (workSpace.First() != null)
            {
                int maxValue = workSpace.First().Value;
                foreach (LinkedListNode<int> node in workSpace)
                {
                    if (node.Value > maxValue)
                    {
                        result = node;
                        maxValue = node.Value;
                    }
                }
            }

            return result;
        }

        private static List<LinkedListNode<int>> DeleteEmptyNodes(List<LinkedListNode<int>> workSpace)
        {
            List<LinkedListNode<int>> result = new List<LinkedListNode<int>>();
            foreach (var node in workSpace)
            {
                if (node != null)
                {
                    result.Add(node);
                }
            }

            return result;
        }

        private static bool isWorkSapceEmpty(List<LinkedListNode<int>> workSpace)
        {
            bool result = true;
            foreach (var node in workSpace)     
            {
                if (node != null)
                {
                    result = false;
                }
            }

            return result;
        }
        
        private static void printList(List<int> list, string name)
        {
            Console.WriteLine(Environment.NewLine + name + ":");
            for (var index = 0; index < list.Count; index++)
            {
                var i = list[index];
                Console.Write(i + "\t");
                if (index !=0 && index % 15 == 0)
                {
                    //Console.Write(Environment.NewLine);
                }
            }
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
