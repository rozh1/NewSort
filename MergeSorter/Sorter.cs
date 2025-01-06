using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MergeSorter
{
    class Sorter
    {
        private readonly SortConfig _config;

        public Sorter(SortConfig config)
        {
            _config = config;
        }

        public void SortFile(string filePath)
        {
            var rows = new List<Row>();

            using (var reader = new StreamReader(filePath)) {
                while (!reader.EndOfStream) { 
                    var str = reader.ReadLine();
                    if (str != null)
                    {
                        rows.Add(new Row(str, _config));
                    }
                }
            }

            rows.Sort((a, b) => a.CompareTo(b));

            using (var writer = new StreamWriter(filePath)) {
                foreach (var row in rows) { 
                    writer.WriteLine(row.RowData);
                }
            }
        }

        public void MergeFiles(List<string> inputPaths, string outputPath)
        {
            var fileWriter = new FileStream(outputPath, FileMode.Create);
            var textWriter = new StreamWriter(fileWriter);
            var fileReaders = inputPaths.Select(x => new FileStream(x, FileMode.Open, FileAccess.Read, FileShare.Read)).ToArray();
            var chunkReaders = fileReaders.Select(x => new ChunkReader(x, _config)).ToArray();
            var workspace = new Row[inputPaths.Count];

            for (int i = 0; i < inputPaths.Count; i++)
            {
                workspace[i] = chunkReaders[i].GetNextRow();
            }

            int selectedIndex = 0;
            Row selectedRow = null;

            for (int i = 0; i < workspace.Length; i++)
            {
                if (workspace[i] != null)
                {
                    selectedIndex = i;
                    selectedRow = workspace[i];
                    break;
                }
            }

            while (!workspace.All(x => x == null))
            {
                for (int i = 0; i < workspace.Length; i++)
                {
                    if (workspace[i] == null) continue;
                    if (i == selectedIndex) continue;

                    if (selectedRow.CompareTo(workspace[i]) == 1)
                    {
                        selectedIndex = i;
                        selectedRow = workspace[i];
                    }
                }

                textWriter.WriteLine(selectedRow.RowData);
                workspace[selectedIndex] = chunkReaders[selectedIndex].GetNextRow();

                for (int i = 0; i < workspace.Length; i++)
                {
                    if (workspace[i] != null)
                    {
                        selectedIndex = i;
                        selectedRow = workspace[i];
                        break;
                    }
                }
            }

            foreach (var chunkReader in chunkReaders)
            {
                chunkReader.Dispose();
            }

            foreach (var fileReader in fileReaders)
            {
                fileReader.Dispose();
            }

            textWriter.Dispose();
            fileWriter.Dispose();
        }


        public void MergeImMemory(List<string> inputPaths, string outputPath)
        {
            var fileWriter = new FileStream(outputPath, FileMode.Create);
            var textWriter = new StreamWriter(fileWriter);
            var chunks = inputPaths.Select(x => File.ReadAllBytes(x)).ToArray();
            var streams = chunks.Select(x => new MemoryStream(x)).ToArray();
            var chunkReaders = streams.Select(x => new ChunkReader(x, _config)).ToArray();
            var workspace = new Row[inputPaths.Count];

            for (int i = 0; i < inputPaths.Count; i++)
            {
                workspace[i] = chunkReaders[i].GetNextRow();
            }

            int selectedIndex = 0;
            Row selectedRow = null;

            for (int i = 0; i < workspace.Length; i++)
            {
                if (workspace[i] != null)
                {
                    selectedIndex = i;
                    selectedRow = workspace[i];
                    break;
                }
            }

            while (!workspace.All(x => x == null))
            {
                for (int i = 0; i < workspace.Length; i++)
                {
                    if (workspace[i] == null) continue;
                    if (i == selectedIndex) continue;

                    if (selectedRow.CompareTo(workspace[i]) == 1)
                    {
                        selectedIndex = i;
                        selectedRow = workspace[i];
                    }
                }

                textWriter.WriteLine(selectedRow.RowData);
                workspace[selectedIndex] = chunkReaders[selectedIndex].GetNextRow();

                for (int i = 0; i < workspace.Length; i++)
                {
                    if (workspace[i] != null)
                    {
                        selectedIndex = i;
                        selectedRow = workspace[i];
                        break;
                    }
                }
            }

            foreach (var chunkReader in chunkReaders)
            {
                chunkReader.Dispose();
            }

            foreach (var stream in streams)
            {
                stream.Dispose();
            }

            textWriter.Dispose();
            fileWriter.Dispose();
        }
    }
}
