using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Robot_32
{
    class Client
    {
        //TAM_PROTOCOL 7
        #region Constants
        int TAM_ID_SENSOR = 2;
        int TAM_VALUE = 4;
        int INDEX_VALUE = 2;
        int INDEX_ID_SENSOR = 0;
        #endregion

        private Socket client;
        private Thread clientListener;
        private Form1 form1;

        public Client(Form1 f)
        {
            form1 = f;
        }

        public void Connect(string ipAddr, string port)
        {
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipe = new IPEndPoint(IPAddress.Parse(ipAddr), Convert.ToInt32(port));
            client.Connect(ipe);

            clientListener = new Thread(OnDataReceived);
            clientListener.Start();
        }

        public void Disconnect()
        {
            if (client != null)
            {
                client.Close();
                client = null;
                clientListener.Abort();
            }
        }

        private void OnDataReceived()//For Client Mode
        {
            try
            {
                while (true)
                {
                    byte[] receiveData = new byte[client.ReceiveBufferSize];

                    int iRx = client.Receive(receiveData);
                    string szData = "";

                    if (iRx != 0)
                    {

                        if (iRx < receiveData.Length)
                        {
                            byte[] tempData = new byte[iRx];
                            for (int i = 0; i < iRx; i++)
                            {

                                tempData[i] = receiveData[i];
                            }
                            receiveData = tempData;
                        }

                        for (int i = 0; i < receiveData.Length; i++)
                        {
                            szData += char.ConvertFromUtf32(receiveData[i]).ToString();

                        }

                        if (szData.Length > 0)
                        {
                            if (szData == "x") // Recebe X do C para desconectar
                            {
                                Disconnect();
                            }

                            if (szData.Length > 4)
                            {
                                szData = szData.Replace('\r', '-');
                                DadoRecebido = szData.Replace('\n', '-');
                                DadoRecebido = DadoRecebido.Split('-')[0];
                                Sensor_ID = Convert.ToInt32(DadoRecebido.Substring(INDEX_ID_SENSOR, TAM_ID_SENSOR));
                                Value = DadoRecebido.Substring(INDEX_VALUE, TAM_VALUE);
                            }
                            else
                                DadoRecebido = szData;

                            if (OnReceiveData != null)
                                OnReceiveData(this, new EventArgs());
                        }

                    }
                    System.Threading.Thread.Sleep(500);
                }
            }
            catch (Exception)
            {//Socket has beenn closed

            }
        }

        public void Send(byte[] data)
        {
            if (client != null && data.Length > 0)
            {
                try
                {
                    client.Send(data);
                }
                catch (Exception)
                {
                    MessageBox.Show("Invaild Password or Username, please press Disconnect button!", "Invaild Password or Username, please press Disconnect button!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Disconnect();
                }
            }
            else
            {

            }
        }

        public event EventHandler<EventArgs> OnReceiveData;
    
        public string DadoRecebido { get; set; }
        public string Value { get; set; }
        public int Sensor_ID { get; set; }
    }
}
