using System.Configuration;
using System;
using System.ComponentModel;
using System.Diagnostics;

namespace copiarAsyncTask
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            List<string> archivosFuente = new();
            var rutaOrigen = SeleccionarPublicacion();

            if (!string.IsNullOrWhiteSpace(rutaOrigen))
            {
                foreach (var dirPath in Directory.GetFiles(rutaOrigen, "*", SearchOption.AllDirectories))
                {
                    archivosFuente.Add(dirPath);
                }

                string carpetaDestino = "C:\\Users\\bchavarriah\\OneDrive - Grupo Colono S.A\\Escritorio\\CopyAsyncCarpetaDestino";

                int numeroHilos = 2; // Número de hilos determinado por un parámetro

                await CopiarArchivosAsync(archivosFuente, carpetaDestino, numeroHilos);

                Console.WriteLine("Operación de copia completada.");
                Console.ReadLine();
            }
        }

        /// <summary>
        /// Copia múltiples archivos utilizando un número condicional de hilos.
        /// </summary>
        /// <param name="archivosFuente">Lista de archivos fuente a copiar.</param>
        /// <param name="carpetaDestino">Ruta de la carpeta destino.</param>
        /// <param name="numeroHilos">Número de hilos a utilizar.</param>
        static async Task CopiarArchivosAsync(List<string> archivosFuente, string carpetaDestino, int numeroHilos)
        {
            // Crear una barra de progreso
            //ProgressBar barraProgreso = new ProgressBar();
            Form1 form1 = new Form1();
            //ProgressBar barraProgreso = new ProgressBar();
            form1.barraProgreso.Minimum = 0;
            //barraProgreso.Maximum = 99 ;


            // Crear un objeto BackgroundWorker para el seguimiento del progreso en un hilo de fondo
            var worker = form1.segundoPlano;
            worker.WorkerReportsProgress = true;

            // Manejar el evento ProgressChanged para actualizar la barra de progreso en el hilo de la interfaz de usuario
            worker.ProgressChanged += (sender, e) =>
            {
                form1.barraProgreso.Value = e.ProgressPercentage;
            };

            // Lista para almacenar las tareas de copia
            List<Task> tareasCopia = new();

            // Calcular el número de archivos por hilo
            int archivosPorHilo = (int)Math.Ceiling((double)archivosFuente.Count / numeroHilos);

            // Iterar sobre los hilos
            for (int i = 0; i < numeroHilos; i++)
            {
                // Calcular los índices de inicio y fin para los archivos del hilo actual
                int indiceInicio = i * archivosPorHilo;
                int indiceFin = Math.Min((i + 1) * archivosPorHilo, archivosFuente.Count);

                // Obtener la lista de archivos del hilo actual
                List<string> archivosHilo = archivosFuente.GetRange(indiceInicio, indiceFin - indiceInicio);

                // Agregar la tarea de copia a la lista de tareas
                tareasCopia.Add(CopiarArchivosHiloAsync(archivosHilo, carpetaDestino, worker));
            }

            // Esperar a que todas las tareas se completen
            await Task.WhenAll(tareasCopia);

            // Liberar recursos de la barra de progreso y el BackgroundWorker
            form1.barraProgreso.Dispose();
            worker.Dispose();
            MessageBox.Show($"Archivos copiados con éxito");
        }

        /// <summary>
        /// Copia los archivos del hilo actual.
        /// </summary>
        /// <param name="archivosFuente">Lista de archivos fuente a copiar.</param>
        /// <param name="carpetaDestino">Ruta de la carpeta destino.</param>
        /// <param name="worker">Objeto BackgroundWorker para el seguimiento del progreso.</param>
        static async Task CopiarArchivosHiloAsync(List<string> archivosFuente, string carpetaDestino, BackgroundWorker worker)
        {
            foreach (string archivoFuente in archivosFuente)
            {
                try
                {
                    // Obtener el nombre del archivo y la ruta de destino
                    string nombreArchivo = Path.GetFileName(archivoFuente);
                    string rutaDestino = Path.Combine(carpetaDestino, nombreArchivo);

                    // Abrir los flujos de archivo fuente y destino
                    using (FileStream flujoFuente = File.Open(archivoFuente, FileMode.Open))
                    using (FileStream flujoDestino = File.Create(rutaDestino))
                    {
                        // Obtener el tamaño total del archivo
                        long totalBytes = flujoFuente.Length;

                        // Buffer de lectura/escritura
                        byte[] buffer = new byte[4096];

                        // Variables para el seguimiento del progreso
                        int bytesLeidos = 0;
                        long bytesCopiados = 0;

                        // Leer y escribir los datos del archivo en bloques
                        while ((bytesLeidos = await flujoFuente.ReadAsync(buffer)) > 0)
                        {
                            await flujoDestino.WriteAsync(buffer.AsMemory(0, bytesLeidos));
                            bytesCopiados += bytesLeidos;

                            // Calcular el progreso actual en porcentaje
                            double porcentajeProgreso = (double)bytesCopiados / totalBytes * 100;
                            if (porcentajeProgreso > 50) Debugger.Break();
                            // Informar el progreso al objeto BackgroundWorker
                            worker.ReportProgress((int)porcentajeProgreso);
                        }
                    }

                    //MessageBox.Show($"Archivo copiado: {archivoFuente}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al copiar el archivo {archivoFuente}: {ex.Message}");
                }
            }
        }

        private static string SeleccionarPublicacion()
        {
            FolderBrowserDialog folderBrowser = new()
            {
                Description = "Seleccionar carpeta de módulos a publicar.",
                ShowNewFolderButton = true,
                UseDescriptionForTitle = true,
            };

            var result = folderBrowser.ShowDialog();
            if (result != DialogResult.OK) return string.Empty;

            return folderBrowser.SelectedPath;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            var valorActual = Convert.ToInt32(label1.Text);
            valorActual++;
            label1.Text = valorActual.ToString();
        }

        private void segundoPlano_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // Mostrar un mensaje de finalización al completar el trabajo
            MessageBox.Show("La copia de archivos ha finalizado.", "Finalizado", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void segundoPlano_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // Actualizar la barra de progreso con el valor actualizado
            Form1 form1 = new Form1();
            form1.barraProgreso.Value = e.ProgressPercentage;
        }
    }
}