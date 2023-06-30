using System.Threading.Tasks.Dataflow;

namespace copiarAsyncTask
{
    public partial class Form4 : Form
    {
        public Form4()
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

            IEnumerable<string>? sourceFiles = ObtenerRutaModulos(carpetaOrigen);
            if (sourceFiles != null)
            {
                // Create a progress bar
                progressBar1.Minimum = 0;
                progressBar1.Maximum = sourceFiles.Count();
                progressBar1.Value = 0;

                // Create a dataflow block to copy files
                var copyBlock = new ActionBlock<string>(async sourceFile =>
                {
                    string destinationFile = Path.Combine(carpetaDestino, Path.GetFileName(sourceFile));
                    using (var sourceStream = File.OpenRead(sourceFile))
                    using (var destinationStream = File.Create(destinationFile))
                    {
                        await sourceStream.CopyToAsync(destinationStream, bufferSize: 81920);
                    }

                    progressBar1.Invoke((MethodInvoker)delegate
                    {
                        progressBar1.PerformStep();
                    });
                }, new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = Environment.ProcessorCount });

                // Create a completion task to track overall completion
                var completionTask = copyBlock.Completion.ContinueWith(_ =>
                {
                    progressBar1.Invoke((MethodInvoker)delegate
                    {
                        progressBar1.Value = progressBar1.Maximum;
                    });
                });

                // Post the source files to the copy block
                foreach (var item in (List<string>)new() {"Primer hilo", "Segundo hilo","Tercer hilo"})
                {
                    foreach (string sourceFile in sourceFiles)
                    {
                        copyBlock.Post($"{sourceFile}_{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}");
                    }
                }

                // Signal the copy block that no more data will be posted
                copyBlock.Complete();

                // Wait for the copy block and completion task to complete
                await Task.WhenAll(copyBlock.Completion, completionTask);

                MessageBox.Show("All files copied successfully!");
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

        private void Button2_Click(object sender, EventArgs e)
        {
            label1.Text = (Convert.ToInt32(label1.Text) + 1).ToString();
        }
    }
}
