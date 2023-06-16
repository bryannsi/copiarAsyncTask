using System.ComponentModel;

namespace copiarAsyncTask;

public partial class Form2 : Form
{
    private List<string>? ListaRutaModulos { get; set; }
    private string CarpetaDestino { get;  set; }
    private int NumeroHilos { get; set; }

    public Form2()
    {
        InitializeComponent();
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

    /// <summary>
    /// Devuleve una lista con la uri de las carpetas de módulos de la ubicación seleccionada
    /// </summary>
    /// <param name="carpetaOrigen">Ruta de la carpeta seleccionada en el FolderBrowserDialog</param>
    /// <returns></returns>
    private static List<string>? ObtenerRutaModulos(string carpetaOrigen)
    {
        return string.IsNullOrWhiteSpace(carpetaOrigen)
            ? null
            : Directory.GetFiles(carpetaOrigen, "*", SearchOption.AllDirectories).ToList();
    }

    private void IniciarCopiadoModulo()
    {
    
    }   


    private void NewMethod()
    {
        // Realizar la copia de archivos en hilos separados
        if (ListaRutaModulos == null) return;
        var archivosPorHilo = (int)Math.Ceiling((double)ListaRutaModulos.Count / NumeroHilos);

        for (var i = 0; i < NumeroHilos; i++)
        {
            var indiceInicio = i * archivosPorHilo;
            var indiceFin = Math.Min((i + 1) * archivosPorHilo, ListaRutaModulos.Count);

            var archivosHilo = ListaRutaModulos.GetRange(indiceInicio, indiceFin - indiceInicio);
            // Realizar la copia de archivos para el hilo actual
            CopyController.CopiarArchivosHiloAsync(archivosHilo, CarpetaDestino).Wait();

            // Informar el progreso al BackgroundWorker
            var progreso = (int)((double)(i + 1) / NumeroHilos * 100);
            bgWorker.ReportProgress(progreso);
        }
    }


    private void Form2_Load(object sender, EventArgs e)
    {
        // Inicializar los valores de la interfaz gráfica
        progressBar.Minimum = 0;
        progressBar.Value = 0;
        CarpetaDestino = "C:\\Users\\bchavarriah\\OneDrive - Grupo Colono S.A\\Escritorio\\CopyAsyncCarpetaDestino";
        NumeroHilos = 2; // Número de hilos determinado por un parámetro
    }

    private void btnIniciarCopia_Click(object sender, EventArgs e)
    {
        var carpetaOrigen = ObtenerCarpetaPublicacion();
        ListaRutaModulos = ObtenerRutaModulos(carpetaOrigen);
        bgWorker.WorkerReportsProgress = true;
        bgWorker.DoWork += Worker_DoWork;
        bgWorker.ProgressChanged += Worker_ProgressChanged;
        bgWorker.RunWorkerCompleted += Worker_RunWorkerCompleted;
        // Iniciar el trabajo en segundo plano
        bgWorker.RunWorkerAsync();
    }

    private void Worker_DoWork(object sender, DoWorkEventArgs e)
    {
        NewMethod();
        // // Realizar la copia de archivos en hilos separados
        // if (ListaRutaModulos == null) return;
        // var archivosPorHilo = (int)Math.Ceiling((double)ListaRutaModulos.Count / NumeroHilos);

        // for (var i = 0; i < NumeroHilos; i++)
        // {
        //     var indiceInicio = i * archivosPorHilo;
        //     var indiceFin = Math.Min((i + 1) * archivosPorHilo, ListaRutaModulos.Count);

        //     var archivosHilo = ListaRutaModulos.GetRange(indiceInicio, indiceFin - indiceInicio);
        //     // Realizar la copia de archivos para el hilo actual
        //     CopyController.CopiarArchivosHiloAsync(archivosHilo, CarpetaDestino).Wait();

        //     // Informar el progreso al BackgroundWorker
        //     var progreso = (int)((double)(i + 1) / NumeroHilos * 100);
        //     bgWorker.ReportProgress(progreso);
        // }
    }
    private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
        // Actualizar la barra de progreso con el valor actualizado
        progressBar.Value = e.ProgressPercentage;
    }

    private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
        // Mostrar un mensaje de finalización al completar el trabajo
        MessageBox.Show("La copia de archivos ha finalizado.", "Finalizado", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void button1_Click(object sender, EventArgs e)
    {
        label1.Text = (Convert.ToInt32(label1.Text) + 1).ToString();
    }
}