using System;
using System.IO;
using Microsoft.WindowsAzure.MobileServices;

namespace MobileClientNetStandardTest
{
    public class AppStorageOps : PlatformDetail.IApplicationStorageOps
    {
        public readonly string Prefix;

        public AppStorageOps()
        {
            Prefix = Environment.GetEnvironmentVariable("USERPROFILE") + "\\AppData\\Local\\MobileClientNetStandardTest";
        }

        public FileStream GetFileStream(string path, FileMode mode, FileAccess access)
        {
            FileStream stream = new FileStream(path, mode, access);
            return stream;
        }
    }

    class Program
    {

        static void Nop() { }

        static void Main(string[] args)
        {
            var ops = new AppStorageOps();

            PlatformDetail.AppStorageOps = ops;
            PlatformDetail.LocalApplicationDataDirectory = ops.Prefix;
            MobileServiceClient client = new MobileServiceClient("https://qi.loqu8.net");
            Console.WriteLine("Hello World!");
            Console.Read();
        }
    }
}