using System;
using System.IO;

namespace FligServer
{
    public class FileService : IFileService
    {
        public void CreateFile(string filename, string content)
        {
            var array = new string[1];
            array[0] = content;
            File.WriteAllLines(filename, array);
        }
    }
}