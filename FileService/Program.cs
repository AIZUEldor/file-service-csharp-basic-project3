namespace FileService
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            IFileService service = new FileService();

            while (true)
            {
                Console.WriteLine("\n=== MENU ===");
                Console.WriteLine("1) Yangi file yaratish va text yozish");
                Console.WriteLine("2) Filelar ro'yxati va ichini ko'rish");
                Console.WriteLine("0) Chiqish");
                Console.Write("Tanlang: ");

                string choice = Console.ReadLine();

                if (choice == "0")
                    break;

                switch (choice)
                {
                    case "1":
                        await CreateNewFile(service);
                        break;

                    case "2":
                        await ShowFilesAndRead(service);
                        break;

                    default:
                        Console.WriteLine("Noto'g'ri tanlov!");
                        break;
                }
            }

            Console.WriteLine("Dastur tugadi.");
        }

        private static async Task CreateNewFile(IFileService service)
        {
            Console.Write("File nomini kiriting (masalan: note1): ");
            string fileName = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(fileName))
            {
                Console.WriteLine("File nomi bo'sh bo'lmasin!");
                return;
            }


            if (!fileName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
                fileName += ".txt";

            Console.WriteLine("Text kiriting (tugatish uchun bo'sh qator bosing):");


            var lines = new List<string>();
            while (true)
            {
                string line = Console.ReadLine();
                if (string.IsNullOrEmpty(line)) break;
                lines.Add(line);
            }

            string text = string.Join(Environment.NewLine, lines);

            await service.WriteFileAsync(fileName, text);
            Console.WriteLine($"Saqlandi: {fileName}");
        }

        private static async Task ShowFilesAndRead(IFileService service)
        {
            var files = await service.GetAllFileNamesAsync();

            if (files.Count == 0)
            {
                Console.WriteLine("Hozircha .txt file yo'q.");
                return;
            }

            Console.WriteLine("\n--- Filelar ---");
            for (int i = 0; i < files.Count; i++)
            {
                Console.WriteLine($"{i + 1}) {files[i]}");
            }

            Console.Write("Qaysi file ni o'qiymiz? (raqam kiriting): ");
            string input = Console.ReadLine();

            if (!int.TryParse(input, out int index) || index < 1 || index > files.Count)
            {
                Console.WriteLine("Noto'g'ri raqam!");
                return;
            }

            string selectedFile = files[index - 1];
            string content = await service.ReadFileAsync(selectedFile);

            Console.WriteLine($"\n=== {selectedFile} ichidagi text ===");
            Console.WriteLine(content);
            Console.WriteLine("=== TAMOM ===");
        }
    }
}
