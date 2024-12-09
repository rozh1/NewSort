using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NewSORT
{
    class Program
    {

        static void Main(string[] args)
        {
            LinkedList<RowStructure> firstFile = GetData("First1", 1);
            LinkedList<RowStructure> secondFile = GetData("First2", 1);
            LinkedList<RowStructure> thirdFile = GetData("Last1_O", 3);
            LinkedList<RowStructure> fourthFile = GetData("Last2_O", 3);


            List<LinkedList<RowStructure>> joinQueues = new List<LinkedList<RowStructure>>
            {
                //firstFile,
                //secondFile,
                thirdFile,
                fourthFile
            };
            SortTableFiles(joinQueues);

            //Console.WriteLine("Выберите необходимый тест: \n\t 0 - LINQ \n\t 1 - 1x файл \n\t 2 - 2x файл");
            //SortTableFilesTest(Console.ReadLine());
        }

        private static void SortTableFiles(List<LinkedList<RowStructure>> joinQueues)
        {
            List<RowStructure> result = new List<RowStructure>();
            result = SortThis(joinQueues, true);
            string line = string.Empty;
            foreach (RowStructure row in result)
            {
                Console.WriteLine(row.GetRow());
            }

        }

        private static void SortTableFilesTest(string str)
        {
            LinkedList<RowStructure> firstFile = GetData("First1", 1);
            LinkedList<RowStructure> secondFile = GetData("First2", 1);
            LinkedList<RowStructure> thirdFile = GetData("Last1_O", 3);
            LinkedList<RowStructure> fourthFile = GetData("Last2_O", 3);


            List<LinkedList<RowStructure>> joinQueues = new List<LinkedList<RowStructure>>
            {
                firstFile,
                secondFile,
                //thirdFile,
                //fourthFile
            };
            DateTime startTime = DateTime.Now;
            List<RowStructure> result = new List<RowStructure>();

            switch (str)
            {
                case "0":
                    {
                        List<RowStructure> bigFileList = new List<RowStructure>();
                        bigFileList = GetData("FirstB", 1).ToList();
                        startTime = DateTime.Now;
                        bigFileList.Sort();
                        Console.WriteLine(String.Format("Время сортировки 1х ={0} LINQ", (DateTime.Now - startTime).Milliseconds));

                        break;
                    }
                case "1":
                    {

                        LinkedList<RowStructure> bigFile = new LinkedList<RowStructure>();
                        bigFile = GetData("FirstB", 1);
                        joinQueues = new List<LinkedList<RowStructure>> { bigFile };
                        startTime = DateTime.Now;
                        result = SortThis(joinQueues, true);
                        Console.WriteLine(String.Format("Время сортировки 1х ={0}", (DateTime.Now - startTime).Milliseconds));

                        break;
                    }
                case "2":
                    {
                        startTime = DateTime.Now;
                        result = SortThis(joinQueues, true);
                        Console.WriteLine(String.Format("Время сортировки 2х ={0}", (DateTime.Now - startTime).Milliseconds));
                        break;
                    }
            }

            Console.ReadKey();
        }
        private static void SortIntFiles()
        {
            FileSplit("input_big.txt");

            LinkedList<int> firstFile = GetData("Output_1.txt");
            LinkedList<int> secondFile = GetData("Output_2.txt");
            LinkedList<int> thirdFile = GetData("Output_3.txt");
            LinkedList<int> fourthFile = GetData("Output_4.txt");


            List<LinkedList<int>> joinQueues = new List<LinkedList<int>>
            {
                firstFile,
                secondFile,
                thirdFile,
                fourthFile
            };

            List<int> result = SortThis(joinQueues);

            string line = string.Empty;
            foreach (int number in result)
            {
                line = line + number + '\t';
            }
            File.WriteAllText("result.txt", line);
        }

        private static void PrintList(List<int> list, string name)
        {
            Console.WriteLine(Environment.NewLine + name + ":");
            for (var index = 0; index < list.Count; index++)
            {
                var i = list[index];
                Console.Write(i + "\t");
                if (index != 0 && index % 15 == 0)
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
                    if (linesReady == lineCount)
                    {
                        fileCount++;
                        linesReady = 0;
                        clear = false;
                    }
                }

                if (!clear && new FileInfo("Output_" + fileCount + ".txt").Exists)
                {
                    new FileInfo("Output_" + fileCount + ".txt").Delete();
                    clear = true;
                }
                File.AppendAllText("Output_" + fileCount + ".txt", inputData[i] + Environment.NewLine);
                linesReady++;

            }

        }


        private static LinkedList<int> GetData(string inputPath)
        {
            List<int> output = new List<int>();
            string[] data = File.ReadAllLines(inputPath);
            foreach (var line in data)
            {
                string[] lines = line.Split('\t');
                foreach (var number in line.Split('\t'))
                {
                    if (number != string.Empty)
                    {
                        output.Add(Convert.ToInt32(number));
                    }
                }
            }
            output.Sort();
            output.Reverse();
            return new LinkedList<int>(output);
        }   //Получение очередей <int>

        private static LinkedList<RowStructure> GetData(string inputPath, int sortingColumnNumber)
        {

            List<RowStructure> output = new List<RowStructure>();
            string[] data = File.ReadAllLines(inputPath);
            foreach (var line in data)
            {
                output.Add(GetRow(line, sortingColumnNumber));
            }
            return new LinkedList<RowStructure>(output);

        }   //Получение очередей строк из одного файла

        private static RowStructure GetRow(string inputRow, int sortColumnNumber)
        {
            if (sortColumnNumber == 0)
            {
                Environment.FailFast("Номер столбца должен быть >0");
                return null;
            }

            RowStructure outRowStructure = null;
            int index = 1;
            string beforeData = null;
            string afterData = null;
            string column = null;
            int sortingColumn = new int();


            foreach (char c in inputRow)
            {
                if (c != '|')
                {
                    if (index < sortColumnNumber)
                    {
                        beforeData += c;
                    }
                    else
                    {
                        if (index > sortColumnNumber)
                        {
                            afterData += c;
                        }
                        else
                        {
                            column += c;
                        }
                    }

                }
                else
                {
                    if (index == sortColumnNumber)
                    {
                        column = column.Remove(0, 1);
                        column = column.Remove(column.Length - 1, 1);
                        sortingColumn = Convert.ToInt32(column);
                    }
                    else if (index > sortColumnNumber) { afterData += '|'; }
                    else { beforeData += '|'; }
                    index++;

                }
            }

            outRowStructure = new RowStructure(beforeData, sortingColumn, afterData);
            return outRowStructure;

        }   //Получение СТРОКИ из текстовой строки

        #region сортировка СТРОК
        private static List<RowStructure> SortThis(List<LinkedList<RowStructure>> queues, bool ascending)
        {
            List<LinkedListNode<RowStructure>> workSpace = new List<LinkedListNode<RowStructure>>();  //создание рабочей области и добавление первых элементов очередей
            for (int i = 0; i < queues.Count; i++)                                  //подразумевается, что очереди не пустые
            {
                workSpace.Add(queues[i].First);
            }
            var sortedResult = new List<RowStructure>();
            if (!ascending)
            {
                sortedResult = SortingViaMax(workSpace);
            }
            else
            {
                sortedResult = SortingViaMin(workSpace);
            }

            return sortedResult;
        }
        private static List<RowStructure> SortingViaMax(List<LinkedListNode<RowStructure>> workSpace)
        {
            List<RowStructure> sortedValue = new List<RowStructure>();

            while (!IsWorkSapceEmpty(workSpace))                                    //проверка, что ещё есть элементы в очередях
            {
                LinkedListNode<RowStructure> maxNode = FindMaxNode(workSpace);               //поиск максимального элемента в рабочей области
                sortedValue.Add(maxNode.Value);                                     //добавление значения в результат
                workSpace[workSpace.IndexOf(maxNode)] = maxNode.Next;               //продвежение очереди с максимальным элементом(первым) !наверное можно оптимизировать продвижение нескольких очередей
                //Console.WriteLine(maxNode.Value.GetRow());
            }

            return sortedValue;
        }

        private static List<RowStructure> SortingViaMin(List<LinkedListNode<RowStructure>> workSpace)
        {
            List<RowStructure> sortedValue = new List<RowStructure>();

            while (!IsWorkSapceEmpty(workSpace))                                    //проверка, что ещё есть элементы в очередях
            {
                LinkedListNode<RowStructure> minNode = FindMinNode(workSpace);               //поиск максимального элемента в рабочей области
                sortedValue.Add(minNode.Value);                                     //добавление значения в результат
                workSpace[workSpace.IndexOf(minNode)] = minNode.Next;               //продвежение очереди с максимальным элементом(первым) !наверное можно оптимизировать продвижение нескольких очередей
                //Console.WriteLine(maxNode.Value.GetRow());
            }

            return sortedValue;
        }
        private static bool IsWorkSapceEmpty(List<LinkedListNode<RowStructure>> workSpace)
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
        private static LinkedListNode<RowStructure> FindMaxNode(List<LinkedListNode<RowStructure>> workSpace)
        {
            workSpace = DeleteEmptyNodes(workSpace);
            LinkedListNode<RowStructure> result = workSpace.First();
            if (workSpace.First() != null)
            {
                int maxValue = workSpace.First().Value.SortingColumn;
                foreach (LinkedListNode<RowStructure> node in workSpace)
                {
                    if (node.Value.SortingColumn > maxValue)
                    {
                        result = node;
                        maxValue = node.Value.SortingColumn;
                    }
                }
            }

            return result;
        }

        private static LinkedListNode<RowStructure> FindMinNode(List<LinkedListNode<RowStructure>> workSpace)
        {
            workSpace = DeleteEmptyNodes(workSpace);
            LinkedListNode<RowStructure> result = workSpace.First();
            if (workSpace.First() != null)
            {
                int minValue = workSpace.First().Value.SortingColumn;
                foreach (LinkedListNode<RowStructure> node in workSpace)
                {
                    if (node.Value.SortingColumn < minValue)
                    {
                        result = node;
                        minValue = node.Value.SortingColumn;
                    }
                }
            }

            return result;
        }
        private static List<LinkedListNode<RowStructure>> DeleteEmptyNodes(List<LinkedListNode<RowStructure>> workSpace)
        {
            //удаление пустых нод(соответсвенно очередей) из рабочей области
            List<LinkedListNode<RowStructure>> result = new List<LinkedListNode<RowStructure>>();
            foreach (var node in workSpace)
            {
                if (node != null)
                {
                    result.Add(node);
                }
            }

            return result;
        }

        #endregion

        #region сортировка INT
        private static List<int> SortThis(List<LinkedList<int>> queues)
        {
            List<LinkedListNode<int>> workSpace = new List<LinkedListNode<int>>();  //создание рабочей области и добавление первых элементов очередей
            for (int i = 0; i < queues.Count; i++)                                  //подразумевается, что очереди не пустые
            {
                workSpace.Add(queues[i].First);
            }
            var sortedResult = SortingViaMax(workSpace);
            return sortedResult;
        }
        private static List<int> SortingViaMax(List<LinkedListNode<int>> workSpace)
        {
            List<int> sortedValue = new List<int>();

            while (!IsWorkSapceEmpty(workSpace))                                    //проверка, что ещё есть элементы в очередях
            {
                LinkedListNode<int> maxNode = FindMaxNode(workSpace);               //поиск максимального элемента в рабочей области
                sortedValue.Add(maxNode.Value);                                     //добавление значения в результат
                workSpace[workSpace.IndexOf(maxNode)] = maxNode.Next;               //продвежение очереди с максимальным элементом(первым) !наверное можно оптимизировать продвижение нескольких очередей
            }

            return sortedValue;
        }
        private static bool IsWorkSapceEmpty(List<LinkedListNode<int>> workSpace)
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
            //удаление пустых нод(соответсвенно очередей) из рабочей области
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

        #endregion
    }
}
