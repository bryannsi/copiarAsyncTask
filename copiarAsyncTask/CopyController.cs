using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace copiarAsyncTask
{
    internal class CopyController
    {

        public static async Task CopiarArchivosHiloAsync(List<string> archivosFuente, string carpetaDestino)
        {
            foreach (var archivoFuente in archivosFuente)
            {
                try
                {
                    var nombreArchivo = Path.GetFileName(archivoFuente);
                    var rutaDestino = Path.Combine(carpetaDestino, nombreArchivo);

                    // Realizar la copia del archivo de manera asíncrona
                    await Task.Run(() => File.Copy(archivoFuente, rutaDestino, true));
                }
                catch (Exception ex)
                {
                    // Manejar el error y mostrar un mensaje de error
                    MessageBox.Show($"Error al copiar el archivo {archivoFuente}: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
