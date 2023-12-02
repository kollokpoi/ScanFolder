

using System.Text;
using Timer =  System.Timers.Timer;
internal class Program
{
    static Timer timer = new Timer();
    static List<FileInfo> lastFilesData = new List<FileInfo>();
    private static void Main(string[] args)
    {
        const string basePath = "D:\\SharedFolder\\";

        
        timer.Enabled = true;
        timer.Interval = 6000;
        timer.Elapsed += Timer_Elapsed;
        timer.Start();

        lastFilesData = ScanFolder(basePath);
        Console.WriteLine("start");

        string path = $"{basePath}test\\text\\testFile.txt";

        var folders = path.Split("\\");

        string currentPath = "";
        for (int i = 0; i < folders.Length-1; i++)
        {
            currentPath = Path.Combine(currentPath, folders[i]);
            if (!Directory.Exists(currentPath))
            {
                Directory.CreateDirectory(currentPath);
            }
        }

        var file = File.Create(path);

        var text = "testestasta";
        var bytes = Encoding.UTF8.GetBytes(text);

        file.Write(bytes, 0, bytes.Length);
        file.Close();

        void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            var filesData = ScanFolder(basePath);

            foreach (var item in filesData)
            {
                bool contained = false;
                foreach (var lastFileData in lastFilesData)
                {
                    if (lastFileData.FullName == item.FullName)
                    {
                        if (lastFileData.Length != item.Length)
                        {
                            Console.WriteLine(item.Name + "Изменен");
                        }
                        contained = true;
                        break;
                    }
                }
                if (!contained)
                {
                    Console.WriteLine(item.Name + "Добавлен");
                }

            }
            foreach (var lastFileData in lastFilesData)
            {
                bool contained = false;
                foreach (var item in filesData)
                {
                    if (lastFileData.FullName == item.FullName)
                    {
                        contained = true;
                        break;
                    }
                }
                if (!contained)
                {
                    Console.WriteLine(lastFileData.Name + "Удален");
                }

            }
            lastFilesData = filesData;
        }

        Console.ReadLine();

        List<FileInfo> ScanFolder(string path)
        {
            List<FileInfo> filesList = new List<FileInfo>();
            var directories = Directory.GetDirectories(path);
            if (directories.Length > 0)
            {
                foreach (var item in directories)
                {
                    filesList.AddRange(ScanFolder(item));
                }
            }
            var files = Directory.GetFiles(path);
            foreach (var item in files)
            {
                var fileInfo = new FileInfo(item);
                filesList.Add(fileInfo);
            }
            return filesList;
        }
    }
}