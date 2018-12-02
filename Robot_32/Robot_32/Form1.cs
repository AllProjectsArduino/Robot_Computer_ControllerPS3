using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Robot_32
{
    public partial class Form1 : Form
    {
        private GamePadState controller;
        private bool ctrX = false;
        private bool ctrCircle = false;
        private bool ctrTriangle = false;
        private bool ctrSquare = false;
        private bool ctrDown = false;
        private bool ctrUp = false;
        private bool ctrLeft = false;
        private bool ctrRight = false;
        private bool ctrStart = false;
        private bool ctrSelect = false;
        private bool ctrL1 = false;
        private bool ctrR1 = false;
        private float ctrL2 = 0;
        private float ctrR2 = 0;
        private float ARY = 0;
        private float ARX = 0;
        private float ALY = 0;
        private float ALX = 0;
        private bool connected = false;
        private int count = 0;
        private Client client;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) { }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            Connect();
        }

        private void buttonDisconnect_Click(object sender, EventArgs e)
        {
            Disconnect();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (connected)
                {
                    controller = GamePad.GetState(Microsoft.Xna.Framework.PlayerIndex.One);
                   
                    // X
                    if (controller.Buttons.A == Microsoft.Xna.Framework.Input.ButtonState.Pressed) 
                    {
                        if (!ctrX)
                        {
                            SendMessage("A" + Conversions.Extract_Value(1)); // ARDUINO inData[0] = 65 + inData[3] = 49
                            pictureBoxLedXis.Visible = true;
                            ctrX = !ctrX;
                        }
                    }
                    else
                    {
                        if(ctrX)
                        {
                            SendMessage("A" + Conversions.Extract_Value(0)); // ARDUINO inData[0] == 65 + inData[3] == 48
                            pictureBoxLedXis.Visible = false;
                            ctrX = !ctrX;
                        }
                    }

                    // Circle
                    if (controller.Buttons.B == Microsoft.Xna.Framework.Input.ButtonState.Pressed) 
                    {
                        if (!ctrCircle)
                        {
                            SendMessage("B" + Conversions.Extract_Value(1)); // ARDUINO inData[0] == 66 + inData[3] == 49
                            pictureBoxLedCirculo.Visible = true;
                            ctrCircle = !ctrCircle;
                        }
                    }
                    else
                    {
                        if (ctrCircle)
                        {
                            SendMessage("B" + Conversions.Extract_Value(0)); // ARDUINO inData[0] == 66 + inData[3] == 48
                            pictureBoxLedCirculo.Visible = false;
                            ctrCircle = !ctrCircle;
                        }
                    }

                    // Triangle
                    if (controller.Buttons.Y == Microsoft.Xna.Framework.Input.ButtonState.Pressed) 
                    {
                        if(!ctrTriangle)
                        {
                            SendMessage("C" + Conversions.Extract_Value(1));
                            pictureBoxLedTriangulo.Visible = true;
                            ctrTriangle = !ctrTriangle;
                        }
                    }
                    else
                    {
                        if(ctrTriangle)
                        {
                            SendMessage("C" + Conversions.Extract_Value(0));
                            pictureBoxLedTriangulo.Visible = false;
                            ctrTriangle = !ctrTriangle;
                        }
                    }

                    // Square
                    if (controller.Buttons.X == Microsoft.Xna.Framework.Input.ButtonState.Pressed) 
                    {
                        if(!ctrSquare)
                        {
                            SendMessage("D" + Conversions.Extract_Value(1));
                            pictureBoxLedQuadrado.Visible = true;
                            ctrSquare = !ctrSquare;
                        }  
                    }
                    else
                    {
                        if (ctrSquare)
                        {
                            SendMessage("D" + Conversions.Extract_Value(0));
                            pictureBoxLedQuadrado.Visible = false;
                            ctrSquare = !ctrSquare;
                        }
                    }
              
                    // Down
                    if (controller.DPad.Down == Microsoft.Xna.Framework.Input.ButtonState.Pressed) 
                    {
                        if (!ctrDown)
                        {
                            SendMessage("E" + Conversions.Extract_Value(1));
                            pictureBoxLedBaixo.Visible = true;
                            ctrDown = !ctrDown;
                        }
                    }
                    else
                    {
                        if (ctrDown)
                        {
                            SendMessage("E" + Conversions.Extract_Value(0));
                            pictureBoxLedBaixo.Visible = false;
                            ctrDown = !ctrDown;
                        }
                    }

                    // Up
                    if (controller.DPad.Up == Microsoft.Xna.Framework.Input.ButtonState.Pressed) 
                    {
                        if (!ctrUp)
                        {
                            SendMessage("F" + Conversions.Extract_Value(1));
                            pictureBoxLedCima.Visible = true;
                            ctrUp = !ctrUp;
                        }
                    }
                    else
                    {
                        if (ctrUp)
                        {
                            SendMessage("F" + Conversions.Extract_Value(0));
                            pictureBoxLedCima.Visible = false;
                            ctrUp = !ctrUp;
                        }
                    }

                    // Left
                    if (controller.DPad.Left == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                    {
                        if (!ctrLeft)
                        {
                            SendMessage("G" + Conversions.Extract_Value(1));
                            pictureBoxLedEsquerda.Visible = true;
                            ctrLeft = !ctrLeft;
                        }
                    }
                    else
                    {
                        if (ctrLeft)
                        {
                            SendMessage("G" + Conversions.Extract_Value(0));
                            pictureBoxLedEsquerda.Visible = false;
                            ctrLeft = !ctrLeft;
                        }
                    }

                    // Right
                    if (controller.DPad.Right == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                    {
                        if (!ctrRight)
                        {
                            SendMessage("H" + Conversions.Extract_Value(1));
                            pictureBoxLedDireita.Visible = true;
                            ctrRight = !ctrRight;
                        }
                    }
                    else
                    {
                        if (ctrRight)
                        {
                            SendMessage("H" + Conversions.Extract_Value(0));
                            pictureBoxLedDireita.Visible = false;
                            ctrRight = !ctrRight;
                        }
                    }

                    // Start
                    if (controller.Buttons.Start == Microsoft.Xna.Framework.Input.ButtonState.Pressed) 
                    {
                        if (!ctrStart)
                        {
                            SendMessage("I" + Conversions.Extract_Value(1));
                            pictureBoxLedStart.Visible = true;
                            ctrStart = !ctrStart;
                        }
                    }
                    else
                    {
                        if (ctrStart)
                        {
                            SendMessage("I" + Conversions.Extract_Value(0));
                            pictureBoxLedStart.Visible = false;
                            ctrStart = !ctrStart;
                        }
                    }

                    // Select
                    if (controller.Buttons.Back == Microsoft.Xna.Framework.Input.ButtonState.Pressed) 
                    {
                        if (!ctrSelect)
                        {
                            SendMessage("J" + Conversions.Extract_Value(1));
                            pictureBoxLedSelect.Visible = true;
                            ctrSelect = !ctrSelect;
                        }
                    }
                    else
                    {
                        if (ctrSelect)
                        {
                            SendMessage("J" + Conversions.Extract_Value(0));
                            pictureBoxLedSelect.Visible = false;
                            ctrSelect = !ctrSelect;
                        }
                    }

                    //HOME
                    if (controller.Buttons.BigButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                    {
                        pictureBoxLedHome.Visible = true;
                    }
                    else
                    {
                        pictureBoxLedHome.Visible = false;
                    }

                    // L1
                    if (controller.Buttons.LeftShoulder == Microsoft.Xna.Framework.Input.ButtonState.Pressed) 
                    {
                        if (!ctrL1)
                        {
                            SendMessage("L" + Conversions.Extract_Value(1));
                            pictureBoxLedL1.Visible = true;
                            ctrL1 = !ctrL1;
                        }
                    }
                    else
                    {
                        if (ctrL1)
                        {
                            SendMessage("L" + Conversions.Extract_Value(0));
                            pictureBoxLedL1.Visible = false;
                            ctrL1 = !ctrL1;
                        }
                    }

                    // R1
                    if (controller.Buttons.RightShoulder == Microsoft.Xna.Framework.Input.ButtonState.Pressed) 
                    {
                        if (!ctrR1)
                        {
                            SendMessage("M" + Conversions.Extract_Value(1));
                            pictureBoxLedR1.Visible = true;
                            ctrR1 = !ctrR1;
                        }
                    }
                    else
                    {
                        if (ctrR1)
                        {
                            SendMessage("M" + Conversions.Extract_Value(0));
                            pictureBoxLedR1.Visible = true;
                            ctrR1 = !ctrR1;
                        }
                    }

                    // R2 analog
                    if (controller.Triggers.Right > 0) 
                    {
                        pictureBoxLedR2.Visible = true;
                        if (ctrL2 != controller.Triggers.Right)
                        {
                            SendMessage("4" + Conversions.Extract_Value(controller.Triggers.Right)); // R2 ENVIA 52 + VALOR
                            ctrL2 = controller.Triggers.Right;
                        }
                    }
                    else
                    {
                        pictureBoxLedR2.Visible = false;
                    }

                    // L2 analog
                    if (controller.Triggers.Left > 0) 
                    {
                        pictureBoxLedL2.Visible = true;
                        if (ctrR2 != controller.Triggers.Left)
                        {
                            SendMessage("5" + Conversions.Extract_Value(controller.Triggers.Left)); // L2 ENVIA 53 + VALOR
                            ctrR2 = controller.Triggers.Left;
                        }
                    }
                    else
                    {
                        pictureBoxLedL2.Visible = false;
                    }

                    // ANALOG DIREITA EIXO Y , VALOR VAI DE 0.00 - 10
                    if (ARY != controller.ThumbSticks.Right.Y) 
                    {
                        SendMessage("2" + Conversions.Extract_Value(controller.ThumbSticks.Right.Y * 10)); // RY ENVIA 48 + VALOR
                        ARY = controller.ThumbSticks.Right.Y;
                    }

                    // ANALOG DIREITA EIXO X , VALOR VAI DE 0.00 - 10
                    if (ARX != controller.ThumbSticks.Right.X) 
                    {
                        SendMessage("1" + Conversions.Extract_Value(controller.ThumbSticks.Right.X * 10)); // RX ENVIA 49 + VALOR
                        ARX = controller.ThumbSticks.Right.X;
                    }

                    // ANALOG ESQUERDA EIXO Y , VALOR VAI DE 0.00 - 10 
                    if (ALY != controller.ThumbSticks.Left.Y) 
                    {
                        SendMessage("0" + Conversions.Extract_Value(controller.ThumbSticks.Left.Y * 10));  // LY ENVIA 50 + VALOR
                        ALY = controller.ThumbSticks.Left.Y;
                    }

                    // ANALOG ESQUERDA EIXO X , VALOR VAI DE 0.00 - 10
                    if (ALX != controller.ThumbSticks.Left.X) 
                    {
                        SendMessage("3" + Conversions.Extract_Value(controller.ThumbSticks.Left.X * 10));  // LX ENVIA 51 + VALOR
                        ALX = controller.ThumbSticks.Left.X;
                    }

                    pictureBoxLedPadLeft.Left = 132 + (int)(controller.ThumbSticks.Left.X * 15);
                    pictureBoxLedPadLeft.Top = 280 - (int)(controller.ThumbSticks.Left.Y * 15);

                    if (controller.ThumbSticks.Left.X == 0 && controller.ThumbSticks.Left.Y == 0)
                    {
                        pictureBoxLedPadLeft.Left = 132;
                        pictureBoxLedPadLeft.Top = 280;
                    }

                    pictureBoxLedPadRigth.Left = 234 + (int)(controller.ThumbSticks.Right.X * 15);
                    pictureBoxLedPadRigth.Top = 280 - (int)(controller.ThumbSticks.Right.Y * 15);

                    if (controller.ThumbSticks.Right.X == 0 && controller.ThumbSticks.Right.Y == 0)
                    {
                        pictureBoxLedPadRigth.Left = 234;
                        pictureBoxLedPadRigth.Top = 280;
                    }

                    count += 1;  // Envia sinal para retornar o valor do sensor
                    if (count >= 100)
                    {
                        SendMessage("6"); // ARDUINO inData[0] == 54
                        count = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to Connect.\r\n" + ex.Message);
            }
        }

        private void Connect()
        {
            //Agora pega a conexão inicial das constantes
            //string ipAddr = Constant.enderecoIp;
            string ipAddr = textBoxIP1.Text + "." + textBoxIP2.Text + "." + textBoxIP3.Text + "." + textBoxIP4.Text;
            string port = textBoxPort.Text;

            if (IsValidIPAddress(ipAddr) == true)
            {
                try
                {
                    if (client == null)
                        client = new Client(this);

                    client.Connect(ipAddr, port);
                    //client.Send(Encoding.GetEncoding(Constant.EncodingFormat).GetBytes("c" + '\n'));
                    buttonDisconnect.Enabled = true;
                    buttonConnect.Enabled = false;
                    connected = true;

                    client.OnReceiveData += Receive;
                    //on.Enabled = true;
                }
                catch (SocketException se)
                {
                    MessageBox.Show("Unable to Connect.\r\n" + se.ToString());
                }
            }
            else
            {
                MessageBox.Show("Invaild Ip Adrress", "Invaild Ip Adrress", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void Disconnect()
        {
            buttonConnect.Enabled = true;
            buttonDisconnect.Enabled = false;
            connected = false;
            client.Disconnect();
        }

        private void Receive(object sender, EventArgs e) // Pega os dados do C e joga no form 
        {
            if (client != null)
            {
                BeginInvoke((Action)(() => //Invoke at UI thread
                {
                    textBoxDataIn.Text = client.Value;
                    switch (client.Sensor_ID)
                    {
                        case 0:
                            //progressBar1.Value = Int32.Parse(client.Value) * 2;
                            break;
                    }
                }), null);
            }
        }

        private bool IsValidIPAddress(string ipaddr)//Validate the input IP address
        {
            try
            {
                IPAddress.Parse(ipaddr);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void SendMessage(string mensagem)
        {

            //disconect.PerformClick();
            //connect.PerformClick();
            try
            {
                if (client == null)
                {
                    client = new Client(this);
                }

                client.Send(Encoding.GetEncoding(Constant.EncodingFormat).GetBytes(mensagem + '\n'));
            }
            catch (SocketException se)
            {
                MessageBox.Show("Unable to Connect.\r\n" + se.ToString());
            }
        }

    }
}
