using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MergeSorter
{
    class ChunkReader : IDisposable
    {
        private readonly SortConfig _config;
        private readonly TextReader _textStream;

        private bool _isEnded = false;

        public ChunkReader(Stream stream, SortConfig config) {
            _config = config;

            _textStream = new StreamReader(stream);
        }

        public void Dispose()
        {
            _textStream.Dispose();
        }

        public Row GetNextRow() {
            if (_isEnded) return null;

            var str = _textStream.ReadLine();
            if (str == null)
            {
                _isEnded = true;
                return null;
            }

            return new Row(str, _config);
        }
    }
}
