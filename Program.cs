using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace TextFilesUtilsEEC
{
    public class PathNotFoundException : Exception {
        public PathNotFoundException() {
        }

        public PathNotFoundException(string message) : base(message) {

        }

        public PathNotFoundException(string message, Exception inner) : base(message, inner) {

        }
    } //-- class PathNotFoundException

    public class WriteToFileException : Exception {
        public WriteToFileException() {
        }

        public WriteToFileException(string message) : base(message) {
        }

        public WriteToFileException(string message, Exception inner) : base(message, inner) {
        }
    } //-- class WriteToFileException

    public class FilesUtil {

        private static string DefaultStorageFileName = "MatricesFileEECApp.txt";
        public FilesUtil() {
        }

        // ++For more info: https://stackoverflow.com/questions/15653921/get-current-folder-path
        // Obtiene la ruta (path) desde la que se esta ejecutando la aplicación (.exe). 
        public string GetCurrentExecutablePath() {
            string path = "";
            try {
                path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            }catch(Exception e){
                throw new PathNotFoundException("No fue posible determinar la RUTA (Path) del ejecutable (.exe)", e);
            }
            return path;
        } //-- GetCurrentExecutablePath

        // Método para agregar la cadena <content> al archivo con el nombre <fileName>.
        // Si el archivo NO existe es creado por el método StreamWriter.WriteLine(),
        // el archivo creado se ubica en el mismo directorio en el que se encuentre el 
        // archivo ejecutable (.exe) 
        // * el método StreamWriter.Writeline() también admite la especificación de rutas
        // absolutas o relativas para la ubicación del archivo.
        private void WriteLine(string fileName, string content) {
            if (fileName is null) {
                throw new ArgumentNullException("El nombre del archivo NO puede ser null.");
            } else if (fileName.Trim().Length == 0) {
                throw new ArgumentException("El nombre del archivo NO puede ser una cadena en blanco (vacía).");
            }

            if (content is null) {
                throw new ArgumentNullException("La cadena a escribir en el archivo NO puede ser null.");
            }

            try {
                using (StreamWriter file = new StreamWriter(fileName, true))
                {
                    file.WriteLine(content);
                }
            } catch (Exception e) {
                throw new WriteToFileException($"A ocurido un error al intentar escribir la cadena [{content}] en el archivo [{fileName}].", e);
            }
        } //-- WriteLine

        public void WriteLine(string content) {
            WriteLine(DefaultStorageFileName, content);
        }

    } //-- class FilesUtil

    class Program
    {
        /*
         * How to append text to the end of the file.
         */
        static void Main(string[] args)
        {
            FilesUtil filesUtil = new FilesUtil();
            DateTime currentDateTime = DateTime.Now;
            string[] lines = {"Primera línea ErickEscam", "Segunda línea ErickEscam", "Tercera línea ErickEscam"};
            string path = "";
            try {
                path = filesUtil.GetCurrentExecutablePath();
                Console.WriteLine($"Executable path (mediante FilesUtil custom class): [{path}]");
                // Example #1: Write an array of strings to a file. SOBREESCRIBE EL CONTENIDO DEL ARCHIVO.
                // File.WriteAllLines(path + "\\EECTextFile.txt", lines);
                Console.WriteLine($"Fecha y hora actuales: [{currentDateTime}]");

                // ##-- Ini
                // Example #4: Append new text to an existing file.
                // The using statement automatically flushes AND CLOSES the stream and calls
                // IDisposable.Dispose on the stream object.
                // SOLO AGREGA UNA LÍNEA AL FINAL DEL ARCHIVO.
                // Si el archivo NO existe, lo crea.
                // For more info: https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/file-system/how-to-write-to-a-text-file
                using (StreamWriter file = new StreamWriter(path + "\\NewTxtFileDragonRojo.txt", true)) {
                    file.WriteLine($"Last session: {currentDateTime}");
                }
                // ##++ FIN

                // --Ini : Test_02, solo se especifica el nombre del archivo
                // ¿el archivo se crea en el Directorio actual? 
                // Sí el archivo se crea en el Directorio Actual
                StringBuilder sb = new StringBuilder();
                sb.Append("fila 1").Append("\nfila2").Append("\nfila3");
                using (StreamWriter file = new StreamWriter("ArchivoSimpleEEC.txt", true)) {
                    file.WriteLine(sb.ToString());
                }
                // ++FIN

            }
            catch (Exception e) {
                Console.Error.WriteLine(e.Message);
                Console.Write(e.StackTrace);
            }

            Console.WriteLine("\n\tPresione cualquier tecla para finalizar...");
            Console.ReadKey();
        }
    }
}
