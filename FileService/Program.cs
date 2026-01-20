namespace FileService
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            IFileService service = new FileService();

            while (true)
            {
                Console.Clear();

                Console.WriteLine("=== MENU ===");
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
                        Console.WriteLine("Davom etish uchun birorta tugma bosing...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private static async Task CreateNewFile(IFileService service)
        {
            Console.Clear();

            Console.Write("File nomini kiriting (masalan: note1): ");
            string fileName = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(fileName))
            {
                Console.WriteLine("File nomi bo'sh bo'lmasin!");
                Console.WriteLine("Davom etish uchun tugma bosing...");
                Console.ReadKey();
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

            bool saved = await service.WriteFileAsync(fileName, text);

            if (!saved)
            {

                Console.WriteLine("❗ Bunday fayl allaqachon bor. Boshqa nom kiriting.");
            }
            else
            {
                Console.WriteLine($" Saqlandi: {fileName}");
            }

            Console.WriteLine("Davom etish uchun birorta tugma bosing...");
            Console.ReadKey();
        }

        private static async Task ShowFilesAndRead(IFileService service)
        {
            Console.Clear();

            var files = await service.GetAllFileNamesAsync();

            if (files.Count == 0)
            {
                Console.WriteLine("Hozircha .txt file yo'q.");
                Console.WriteLine("Davom etish uchun birorta tugma bosing...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("--- Filelar ---");
            for (int i = 0; i < files.Count; i++)
            {

                Console.WriteLine($"{i + 1}) {files[i]}");
            }

            Console.Write("Qaysi file ni o'qiymiz? (raqam kiriting): ");
            string input = Console.ReadLine();

            if (!int.TryParse(input, out int index) || index < 1 || index > files.Count)
            {
                Console.WriteLine("Noto'g'ri raqam!");
                Console.WriteLine("Davom etish uchun tugma bosing...");
                Console.ReadKey();
                return;
            }


            string selectedDisplay = files[index - 1];
            string selectedFileName = selectedDisplay.Replace("M: ", "").Trim();

            string content = await service.ReadFileAsync(selectedFileName);

            if (content == null)
            {

                Console.WriteLine("❗ File topilmadi (mavjud emas).");
                Console.WriteLine("Davom etish uchun tugma bosing...");
                Console.ReadKey();
                return;
            }

            Console.Clear();

            Console.WriteLine($"=== {selectedFileName} ichidagi text ===");
            Console.WriteLine(content);


            Console.WriteLine("\nMenyuga qaytish uchun birorta tugma bosing...");
            Console.ReadKey();


        }
    }
}
