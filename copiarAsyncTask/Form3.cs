using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace copiarAsyncTask
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            var carpetaOrigen = ObtenerCarpetaPublicacion();

            // Get the source files
            IEnumerable<string>? sourceFiles = ObtenerRutaModulos(carpetaOrigen);
            // Get the destination folder
            string destinationFolder = "C:\\Users\\bchavarriah\\OneDrive - Grupo Colono S.A\\Escritorio\\CopyAsyncCarpetaDestino";

            if (sourceFiles != null)
            {
                // Copy the files
                bool success = await CopyMultipleFilesAsync(sourceFiles, destinationFolder);

                // If the copy operation was successful, show a message box
                if (success)
                {
                    MessageBox.Show("Files copied successfully");
                }
            }
        }

        private static string ObtenerCarpetaPublicacion()
        {
            using var folderBrowser = new FolderBrowserDialog();
            folderBrowser.Description = "Seleccionar carpeta de módulos a publicar.";
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

        public async Task<bool> CopyMultipleFilesAsync(IEnumerable<string> sourceFiles, string destinationFolder, int numThreads = 1)
        {
            // Validate parameters
            if (sourceFiles == null)
            {
                throw new ArgumentNullException("sourceFiles");
            }

            if (destinationFolder == null)
            {
                throw new ArgumentNullException("destinationFolder");
            }

            // Check if the number of threads is valid
            if (numThreads <= 0)
            {
                numThreads = 1;
            }

            // Create a list of tasks to copy the files
            List<Task> tasks = new();
            foreach (string sourceFile in sourceFiles)
            {
                string destinationPath = Path.Combine(destinationFolder, Path.GetFileName(sourceFile));
                Task task = Task.Run(() => CopyFileAsync(sourceFile, destinationPath));
                tasks.Add(task);
            }

            // Use a progress bar to show the user the progress of the copy operation
            //ProgressBar progressBar = new();
            progressBar1.Maximum = sourceFiles.Count();

            // Wait for all tasks to complete
            await Task.WhenAll(tasks);

            // Update the progress bar to show that the copy operation is complete
            //progressBar1.Value = sourceFiles.Count();

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

        private void button2_Click(object sender, EventArgs e)
        {
            label1.Text = (Convert.ToInt32(label1.Text) + 1).ToString();
        }
    }
}
