namespace copiarAsyncTask
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }
        private class AgrupacionHilo
        {
            public List<Task> TaskList { get; set; } = null!;
            public string NombreHilo { get; set; } = null!;
        }

        private async void Button1_Click(object sender, EventArgs e)
        {
            var carpetaOrigen = ObtenerCarpetaPublicacion("Seleccionar carpeta de origen");
            var carpetaDestino = ObtenerCarpetaPublicacion("Seleccionar carpeta de destino");

            if (string.IsNullOrWhiteSpace(carpetaOrigen) || string.IsNullOrWhiteSpace(carpetaDestino)) return;
            // Get the source files
            IEnumerable<string>? sourceFiles = ObtenerRutaModulos(carpetaOrigen);

            if (sourceFiles != null)
            {
                // Copy the files
                bool success = await CopyMultipleFilesAsync(sourceFiles, carpetaDestino);

                // If the copy operation was successful, show a message box
                if (success)
                {
                    MessageBox.Show("Files copied successfully");
                }
            }
        }

        private static string ObtenerCarpetaPublicacion(string nombreFolderBrowserDialog = "Seleccionar carpeta de módulos a publicar.")
        {
            using var folderBrowser = new FolderBrowserDialog();
            folderBrowser.Description = nombreFolderBrowserDialog;
            folderBrowser.ShowNewFolderButton = true;
            folderBrowser.UseDescriptionForTitle = true;

            var result = folderBrowser.ShowDialog();
            return result == DialogResult.OK ? folderBrowser.SelectedPath : string.Empty;
        }

        private static IEnumerable<string>? ObtenerRutaModulos(string carpetaOrigen)
        {
            return string.IsNullOrWhiteSpace(carpetaOrigen)
                ? null
                : Directory.GetFiles(carpetaOrigen, "*", SearchOption.AllDirectories).ToList();
        }

        public async Task<bool> CopyMultipleFilesAsync(IEnumerable<string> sourceFiles, string destinationFolder, int numThreads = 3)
        {
            // Validate parameters
            if (sourceFiles == null)
            {
                throw new ArgumentNullException(nameof(sourceFiles));
            }

            if (destinationFolder == null)
            {
                throw new ArgumentNullException(nameof(destinationFolder));
            }

            // Check if the number of threads is valid
            if (numThreads <= 0)
            {
                numThreads = 1;
            }

            List<string> listaHilos = new()
            {
                "Primer hilo",
                "Segundo hilo",
                "Tercer hilo"
            };

            // Crear una lista de tareas para copiar los archivos
            List<AgrupacionHilo> todasLasTareas = new();
            AgrupacionHilo agrupacionHilo = new();

            // foreach (var item in listaHilos)
            // {
            //     foreach (var sourceFile in sourceFiles)
            //     {
            //         string destinationPath = Path.Combine(destinationFolder, Path.GetFileName(sourceFile));
            //         Task task = Task.Run(() => CopyFileAsync(sourceFile, destinationPath));
            //         agrupacionHilo.TaskList.Add(task);
            //     }
            //     agrupacionHilo.NombreHilo = item;
            //     todasLasTareas.Add(agrupacionHilo);
            // }

            List<Task> tasks = new();
            foreach (string sourceFile in sourceFiles)
            {
                string destinationPath = Path.Combine(destinationFolder, Path.GetFileName(sourceFile));
                Task task = Task.Run(() => CopyFileAsync(sourceFile, destinationPath));
                tasks.Add(task);
            }

            // Use a progress bar to show the user the progress of the copy operation
            progressBar1.Maximum = sourceFiles.Count();

             await Task.WhenAll(tasks);

            // Wait for all tasks to complete
            // foreach (var item in todasLasTareas)
            // {
            //     await Task.WhenAll(item.TaskList);
            // }

            // Return true if the copy operation was successful
            return true;
        }

        private async Task CopyFileAsync(string sourceFile, string destinationPath)
        {
            // Check if the source file exists
            if (!File.Exists(sourceFile))
            {
                throw new FileNotFoundException("Source file does not exist: " + sourceFile);
            }

            // Check if the destination file already exists
            //if (File.Exists(destinationPath))
            //{
            //    throw new FileExistsException("Destination file already exists: " + destinationPath);
            //}

            // Create a stream to read from the source file
            using (Stream inputStream = File.OpenRead(sourceFile))
            {
                // Create a stream to write to the destination file
                using (Stream outputStream = File.Create(destinationPath))
                {
                    // Copy the data from the source file to the destination file
                    byte[] buffer = new byte[8192];
                    int bytesRead;
                    while ((bytesRead = inputStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        outputStream.Write(buffer, 0, bytesRead);
                        progressBar1.Value = bytesRead;
                    }
                }
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            label1.Text = (Convert.ToInt32(label1.Text) + 1).ToString();
        }
    }
}
