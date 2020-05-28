using System;
using System.IO;
using Microsoft.Extensions.FileProviders;

namespace Hostly.AppSettings
{
    internal sealed class EmbeddedFileInfo : IFileInfo
    {
        private readonly Stream _fileStream;

        public EmbeddedFileInfo(string name, Stream fileStream)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (fileStream == null) throw new ArgumentNullException(nameof(fileStream));

            _fileStream = fileStream;

            Exists = true;
            IsDirectory = false;
            Length = fileStream.Length;
            Name = name;
            PhysicalPath = name;
            LastModified = DateTimeOffset.Now;
        }

        public Stream CreateReadStream()
        {
            return _fileStream;
        }

        public bool Exists { get; }
        public bool IsDirectory { get; }
        public long Length { get; }
        public string Name { get; }
        public string PhysicalPath { get; }
        public DateTimeOffset LastModified { get; }
    }
}
