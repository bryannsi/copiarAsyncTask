using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace copiarAsyncTask
{
    public partial class Form5 : Form
    {
        private readonly Dictionary<string, ProgressBar> progressBars; // Diccionario para almacenar las barras de progreso

        public Form5()
        {
            InitializeComponent();
            progressBars = new Dictionary<string, ProgressBar>(); // Inicializar el diccionario
        }

        private async void button1_ClickAsync(object sender, EventArgs e)
        {
            await Trigger();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            label1.Text = (Convert.ToInt32(label1.Text) + 1).ToString();
        }

        private static async Task Trigger()
        {
            var carpetaOrigen = SeleccionarCarpeta("Seleccionar carpeta de origen");
            var carpetaDestino = SeleccionarCarpeta("Seleccionar carpeta de destino");
            if (string.IsNullOrWhiteSpace(carpetaOrigen) || string.IsNullOrWhiteSpace(carpetaDestino))
                return;

            IEnumerable<string>? sourceFiles = ObtenerRutaModulos(carpetaOrigen);
            if (sourceFiles != null)
            {
                Dictionary<string, Task> tasks = new();
                Dictionary<string, string> taskNames = new()
                {
                    {"Thread1", Path.Combine(carpetaDestino, "Destino1")},
                    {"Thread2", Path.Combine(carpetaDestino, "Destino2")},
                    {"Thread3", Path.Combine(carpetaDestino, "Destino3")}
                };

                // Crear el formulario principal
                Form5 mainForm = (Form5)Application.OpenForms["Form5"];

                taskNames.Keys.ToList().ForEach(key => mainForm.RemoveProgressBar(key));

                foreach (var taskName in taskNames)
                {
                    Task task = CopyFiles(taskName.Key, carpetaOrigen, taskName.Value, mainForm);
                    tasks.Add(taskName.Key, task);
                }

                await Task.WhenAll(tasks.Values);
            }
        }

        static async Task CopyFiles(string taskName, string sourcePath, string destinationPath, Form5 mainForm)
        {
            Directory.CreateDirectory(destinationPath);
            string[] files = Directory.GetFiles(sourcePath);

            // Agregar la ProgressBar al formulario principal de forma dinámica
            ProgressBar progressBar = mainForm.AddProgressBar(taskName, files.Length);

            foreach (string file in files)
            {
                string fileName = Path.GetFileName(file);
                // prueba para comprobar progressBar cuando los hilos tienen cantidad de archivos diferentes
                if (taskName == "Thread1" && fileName == "archivo1.exe") break;
                string destinationFilePath = Path.Combine(destinationPath, fileName);
                 await Task.Run(() => File.Copy(file, destinationFilePath, true));
                 mainForm.IncrementProgress(taskName); // Incrementar el progreso de la ProgressBar
            }

            //mainForm.RemoveProgressBar(taskName); // Remover la ProgressBar del formulario principal
        }

        private static string SeleccionarCarpeta(string nombreFolderBrowserDialog = "Seleccionar carpeta de módulos a publicar.")
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
            return string.IsNullOrWhiteSpace(carpetaOrigen) ? null : Directory.GetFiles(carpetaOrigen, "*", SearchOption.AllDirectories).ToList();
        }

        // Método para agregar una ProgressBar al formulario principal de forma dinámica
        public ProgressBar AddProgressBar(string threadName, int totalFiles)
        {
            ProgressBar progressBar = new()
            {
                Minimum = 0,
                Maximum = totalFiles
            };
            progressBars.Add(threadName, progressBar);
            flowLayoutPanel.Controls.Add(progressBar);
            return progressBar;
        }

        // Método para incrementar el progreso de la ProgressBar
        public void IncrementProgress(string threadName)
        {
            if (progressBars.ContainsKey(threadName))
            {
                ProgressBar progressBar = progressBars[threadName];
                if (progressBar.InvokeRequired)
                {
                    progressBar.Invoke(new Action(() => progressBar.Value++));
                }
                else
                {
                    progressBar.Value++;
                }
            }
        }

        // Método para remover la ProgressBar del formulario principal
        public void RemoveProgressBar(string threadName)
        {
            if (progressBars.ContainsKey(threadName))
            {
                ProgressBar progressBar = progressBars[threadName];
                if (progressBar.InvokeRequired)
                {
                    progressBar.Invoke(new Action(() => flowLayoutPanel.Controls.Remove(progressBar)));
                }
                else
                {
                    flowLayoutPanel.Controls.Remove(progressBar);
                }
                progressBars.Remove(threadName);
            }
        }
    }
}
