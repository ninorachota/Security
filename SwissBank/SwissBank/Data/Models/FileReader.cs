using System.IO;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace SwissBank.Data.Models
{
    public class FileReader
    {
        private readonly string _startupPath;

        public FileReader()
        {
            _startupPath = System.IO.Directory.GetCurrentDirectory();
        }

        public ContentResult ReadHtml(string path)
        {
            return new ContentResult
            {
                ContentType = "text/html",
                StatusCode = (int)HttpStatusCode.OK,
                Content = File.ReadAllText(_startupPath + path)
            };
        }
    }
}
