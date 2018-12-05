#include <WiFi.h> // Wifi

const char* ssid     = "Robotica";
const char* password = "12344321";
unsigned char Dados[8]; // 2 digitos para identificação do sensor + 4 digitos para o valor entre 0 e 1024 + 1 digito para sinalizar o fim('Z')

WiFiServer server(80); //Cria o objeto servidor na porta 80

void setup()
{
  Serial.begin(115200);//Habilita a comm serial

  // Wifi ------------------------------------------------------------------------------
  WiFi.mode(WIFI_AP);//Define o WiFi como Acess_Point.
  WiFi.softAP(ssid, password);//Cria a rede de Acess_Point.
  Serial.println(WiFi.softAPIP());
  server.begin();//Inicia o servidor TCP na porta declarada no começo.

  // ----------------------------------------------------------------------------------

  // Portas tracao ---------------------------------------------------------------------
  // Frequencia do PWM = De 1000Hz a 40000Hz - Calculo da frequencia maxima com a resolucao escolhida, (clock/resolucao) 80.000.000/8 = 10.000.000(Maximo)
  // Resolucao = De 1 a 16 bits
  // Canal do PWM =  Pode utilizar mais de um canal, para mais de um pino, indo de 0 a 15

  // ledcAttachPin(Porta, Canal);
  // ledcSetup(Porta, Frequencia(Hz), Resolucao);

  // Rigth --------------------------------------------------------------
  pinMode(27, OUTPUT);     // Saida PWM ENABLE RY (Ponte H)
  ledcAttachPin(27, 0);    // Atribuimos o pino 27 ao canal 0
  ledcSetup(0, 200000, 8); //Atribuimos ao canal 0 a frequencia de 200.000 Hz com resolucao de 8 bits.

  pinMode(14, OUTPUT); // Saida digital direcao RY (Ponte H)
  pinMode(12, OUTPUT); // Saida digital direcao RY (Ponte H)

  // Left --------------------------------------------------------------
  pinMode(33, OUTPUT);  // Saida PWM ENABLE LY (Ponte H)
  ledcAttachPin(33, 1); //Atribuimos o pino 33 ao canal 1
  ledcSetup(1, 5000, 8); //Atribuimos ao canal 1 a frequencia de 200.000 Hz com resolucao de 8 bits.

  pinMode(25, OUTPUT); // Saida digital direcao LY (Ponte H)
  pinMode(26, OUTPUT); // Saida digital direcao LY (Ponte H)

  // -----------------------------------------------------------------------------------
}

void loop() {
  waitIncommingConnection();
}

void waitIncommingConnection() // Função que espera alguém conectar, quando conecta começa  a fazer algo
{
  String inData         = "";    // Dados de entrada
  WiFiClient client = server.available(); // Cria um objeto client

  if (client) // Se deu certo na hora de criar o objeto client, entra no if
  {
    while ( client.connected() ) // Se houver cliente conectado, entra no while
    {
      if ( client.available() ) // Aqui é pra ver se está disponivel
      {
        char recieved = client.read(); // Se estiver, vai ler os dados na variavel indata
        inData += recieved;
        if (recieved == '\n')
        {
          controleAnalogico(inData, 50, 0, 14, 12); // RY protocol, joystick_button, channel_PWM, pin_in_a, pin_in_b
          controleAnalogico(inData, 48, 1, 26, 25); // LY protocol, joystick_button, channel_PWM, pin_in_a, pin_in_b
          inData = ""; // Aqui reinicializa a variavel indata para pegar o próximo comando
        }
      }
    }
    client.stop();
    Serial.println("Client Disconnected.");
  }
}

void controleAnalogico(String protocol, int joystick_button, int channel_PWM, int pin_in_a, int pin_in_b)
{
  if (protocol[0] == joystick_button )// ANALOGICO R Y inData[0] == 48 RECEBE DE 0.00 - 10
  {
    float val = (((protocol[2] - 48) * 10) + ((protocol[3] - 48) * 1)); // XX.00  0-10 tanto pra cima quanto pra baixo
    float ptFr1 = (protocol[4] - 48);
    float ptFr2 = ((protocol[5] - 48));
    float valFra = ((ptFr1 / 10) + (ptFr2 / 100)); // 0.XX
    float resultPart = val + valFra;
    float value_PWM = resultPart * 25.5;

    // ANALOGICO R Y entrada[1] == 48 analogico pra cima, 49 analogico pra baixo
    if (protocol[1] == 48 )
    {
      digitalWrite(pin_in_a, LOW);
      digitalWrite(pin_in_b, HIGH);
    }
    else
    {
      digitalWrite(pin_in_a, HIGH);
      digitalWrite(pin_in_b, LOW);
    }
    ledcWrite(channel_PWM, value_PWM);
  }
}
