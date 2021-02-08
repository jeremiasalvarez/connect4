using GameService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Host
{
    public partial class GameServiceHost : Form
    {
        private ServiceHost Host;
        private bool SuccessfullOpen;
        public GameServiceHost()
        {
            InitializeComponent();
            OpenService();
            label1.Focus();     
        }

        private bool OpenService()
        {
            SuccessfullOpen = false;
            try
            {
                Host = new ServiceHost(typeof(GameService.GameService));
                Host.Open();
                textBox1.Text = "";
                textBox1.AppendText("Estado: Iniciado");
                textBox1.AppendText(Environment.NewLine);
                textBox1.AppendText($"Direccion: {GetServerIp()}");
                textBox1.ForeColor = Color.Green;
                SuccessfullOpen = true;
            }
            catch (AddressAccessDeniedException)
            {
                textBox1.Text = "";
                textBox1.Text = "El sistema no permitio la activacion del servidor";
                textBox1.ForeColor = Color.Red;
                MessageBox.Show("Debes ejecutar el servidor como administrador", "Aviso", MessageBoxButtons.OK,MessageBoxIcon.Error);
                button1.Text = "Iniciar Servidor";
            }
            catch (Exception error)
            {
                textBox1.Text = "";
                textBox1.Text = "Ocurrio un error al iniciar el servidor";
                textBox1.ForeColor = Color.Red;
                MessageBox.Show(error.ToString(),"Aviso",MessageBoxButtons.OK, MessageBoxIcon.Error);
                button1.Text = "Iniciar Servidor";
            }

            return SuccessfullOpen;
        }

        private string GetServerIp()
        {
            return Host.Description.Endpoints[0].Address.ToString();
        }

        private void CloseService()
        {
            if (Host.State == CommunicationState.Opened)
                Host.Close();       
        }

        private void GameServiceHost_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseService();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Host.State == CommunicationState.Opened)
            {
                CloseService();
                button1.Text = "Iniciar Servidor";
                textBox1.Text = "";
                textBox1.Text = "Estado: Desconectado";
                textBox1.ForeColor = Color.Red;
                
            } 
            else
            {
                SuccessfullOpen = OpenService();

                if (SuccessfullOpen)
                     button1.Text = "Detener Servidor";
            }
        }

        private void textBox1_MouseDown(object sender, MouseEventArgs e)
        {
            label1.Focus();
        }
    }
}
