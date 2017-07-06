using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using System.IO;

namespace KeyLoggerLocal
{
    public partial class frmVerLog : Form
    {
        public frmVerLog(string filename)
        {
            InitializeComponent();

            try
            {
                // Leemos el fichero de log pasado como parametro a la instancia de la clase
                StreamReader sr = new StreamReader(filename);

                byte[] binary;

                // Transformamos la cadena en base64 a un formato legible para mostrar en el log
                System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
                binary = Convert.FromBase64String(sr.ReadLine());                
                richTextBox1.Text = enc.GetString(binary);

                // Recorremos el fichero hasta el final y vamos decodificando linea a linea
                while (!sr.EndOfStream)
                {
                    binary = Convert.FromBase64String(sr.ReadLine());
                    richTextBox1.AppendText(sr.ReadLine());
                }

                sr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Se ha producido un error al intentar abrir el fichero de log de KeyLogger v1.0:" + ex.Message);
            }
        }
    }
}