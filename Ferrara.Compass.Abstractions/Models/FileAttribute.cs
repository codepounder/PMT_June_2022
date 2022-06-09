using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Ferrara.Compass.Abstractions.Models
{
    [Serializable]
    public class FileAttribute
    {
        public string FileName { get; set; }
        public int PackagingComponentItemId { get; set; }
        public string DisplayFileName { get; set; }
        public string FileUrl { get; set; }
        public string FilePath { get; set; }
        public string DocType { get; set; }
        public byte[] FileContent { get; set; }
        public long FileContentLength { get; set; }
        public DateTime ModifiedDateTime { get; set; }
        public Stream FileStream { get; set; }
    }
}
