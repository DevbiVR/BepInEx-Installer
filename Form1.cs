using System.Net;
using System.IO.Compression;
using System.Security.Policy;
using System.Diagnostics;


namespace bepInexinstaller
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Installing BepInEx from Github...");
            string tempZipPath = Path.GetTempFileName();
            string default_steam_path = @"C:\Program Files (x86)\Steam\steamapps\common\Gorilla Tag";
            string default_oculus_path = @"C:\Program Files\Oculus\Software\Software\Gorilla Tag";
            using (var client = new WebClient())
            {
                client.DownloadFile("https://github.com/BepInEx/BepInEx/releases/download/v5.4.23.3/BepInEx_win_x64_5.4.23.3.zip", tempZipPath);
            }

            Console.WriteLine("Type the file path in console:");
            Console.WriteLine("Go to Steam, open your library, click on Gorilla Tag, click the gear icon hover over Manage and then click browse local files. thats where your Gorilla Tag file path will be.");
            Console.WriteLine("Example: C:\\Program Files (x86)\\Steam\\steamapps\\common\\Gorilla Tag\\");

            string filePath = Console.ReadLine();
            string bepinex_check_path = Path.Combine(filePath, "BepInEx");
            string bepinex_plugins_path = Path.Combine(filePath, "BepInEx", "plugins");
            if (string.IsNullOrWhiteSpace(filePath) || filePath.IndexOf('\0') >= 0 || !Directory.Exists(filePath))
            {
                Console.WriteLine("Invalid file path. Aborting installation.");
                return;
            }

            if (Directory.Exists(bepinex_check_path))
            {
                Console.WriteLine("BepInEx already exists in the specified directory. Aborting installation.");
                return;
            }

            Console.WriteLine("Are you sure you want to inject BepInEx and its recommended dependencies to Gorilla Tag?");
            string answer = Console.ReadLine();

            if (answer == "yes")
            {
                Console.WriteLine("Extracting BepInEx...");
                ZipFile.ExtractToDirectory(tempZipPath, filePath, true);
                string root = @"C:\Temp";
                // If directory does not exist, don't even try   
                if (Directory.Exists(bepinex_check_path))
                {
                    Console.WriteLine("BepInEx installed.");
                    using (var client = new WebClient())
                    {
                        Console.WriteLine("Run Gorilla Tag and then type and enter something random in the console. This is because the application needs a plugins folder to add dependencies.");
                        Console.ReadLine();
                        client.DownloadFile("https://github.com/Auros/Bepinject/releases/download/1.0.1/Bepinject-Auros.zip", tempZipPath);
                        ZipFile.ExtractToDirectory(tempZipPath, bepinex_plugins_path, true);
                        client.DownloadFile("https://github.com/Auros/Bepinject/releases/download/1.0.1/Extenject.zip", tempZipPath);
                        if (new FileInfo(tempZipPath).Length == 0)
                        {
                            Console.WriteLine("Plugins folder does not exist or download has failed.");
                            Console.WriteLine("Try running Gorilla Tag before downloading dependencies.");
                            return;
                        }
                        ZipFile.ExtractToDirectory(tempZipPath, bepinex_plugins_path, true);
                        Console.WriteLine("Bepinject and Extenject installed.");
                        Console.WriteLine("BepInEx and mod dependencies have been successfully installed into Gorilla Tag!");
                    }

                }
            }
            else
            {
                Console.WriteLine("Aborting installation.");
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Please enter your Gorilla Tag file path.");
            string filePath = Console.ReadLine();
            string bepinex_file_path = Path.Combine(filePath, "BepInEx", "plugins");
            if (!Directory.Exists(bepinex_file_path))
            {
                Console.WriteLine("BepInEx plugins folder does not exist. Use Install BepInEx first. ");
                return;
            }

            Console.WriteLine("Installing Utilla from Github...");

            // Remove Zone.Identifier if it exists
            string zoneIdentifier = filePath + ":Zone.Identifier";
            if (File.Exists(zoneIdentifier))
            {
                File.Delete(zoneIdentifier);
            }

            using (var client = new WebClient())
            {
                client.DownloadFile("https://github.com/developer9998/Utilla/releases/download/March3/Utilla.dll", bepinex_file_path);
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            Console.WriteLine("Enter your Gorilla Tag file path.");
            string filePath = Console.ReadLine();
            string bepinex_file_path = Path.Combine(filePath, "BepInEx", "plugins");
            if (!Directory.Exists(bepinex_file_path))
            {
                Console.WriteLine("BepInEx plugins folder does not exist. Use Install BepInEx first. ");
                return;
            }
            Console.WriteLine("Enter the direct download github link of the mod you want to install.");
            string modLink = Console.ReadLine();
            using (var client = new WebClient())
            {

                string fileName = Path.GetFileName(modLink);
                string destinationPath = Path.Combine(bepinex_file_path, fileName);
                if (File.Exists(destinationPath))
                {
                    Console.WriteLine($"File {fileName} already exists. Overwriting...");
                    File.Delete(destinationPath);
                }
                client.DownloadFile(modLink, destinationPath);
                Console.WriteLine($"Mod downloaded to Gorilla Tag successfully!");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Example: Open a URL in the default browser
            string url = "https://www.youtube.com/watch?v=HEGxgVpVdQ0";
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                };
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to open URL: " + ex.Message);
            }
        }
    }
}
