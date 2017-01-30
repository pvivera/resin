﻿using System;
using System.IO;

namespace Resin.IO
{
    public class DocumentWriter : IDisposable
    {
        private readonly StreamWriter _writer;

        public DocumentWriter(StreamWriter writer)
        {
            _writer = writer;
        }

        public void Write(Document doc)
        {
            var bytes = Serialize(doc);

            if (bytes.Length == 0) throw new Exception();

            var base64 = Convert.ToBase64String(bytes);

            _writer.WriteLine("{0}:{1}", doc.Id, base64);
        }

        private byte[] Serialize(Document item)
        {
            using (var stream = new MemoryStream())
            {
                FileBase.Serializer.Serialize(stream, item);
                return stream.ToArray();
            }
        }

        public void Dispose()
        {
            if (_writer != null)
            {
                _writer.Flush();
                _writer.Close();
                _writer.Dispose();
            }
        }
    }
}