// Wifi
#include <WiFi.h>

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
    // Frequencia = frequencia do PWM em Hz - pode ser alterada dependendo da resolucao
    // Resolucao = pode ser alterada de 1 a 16 bits
    // Canal do PWM =  pode utilizar mais de um canal, para mais de um pino, indo de 0 a 15
    // ledcSetup =  Porta - Frequencia - Resolucao

    // Rigth --------------------------------------------------------------
    pinMode(27, OUTPUT);  // Saida PWM RY (Ponte H)
    ledcAttachPin(27, 0); //Atribuimos o pino 34 ao canal 0.
    
    //Atribuimos ao canal 0 a frequencia de 1000 Hz com resolucao de 10 bits.
    ledcSetup(0, 5000, 8); 

    pinMode(14, OUTPUT); // SAIDA DIGITAL ENABLE RY (Ponte H)
    pinMode(12, OUTPUT); // SAIDA DIGITAL ENABLE RY (Ponte H)

    // Left --------------------------------------------------------------
    pinMode(33, OUTPUT);  // Saida PWM LY (Ponte H)
    ledcAttachPin(33, 1); //Atribuimos o pino 33 ao canal 1.

     //Atribuimos ao canal 0 a frequencia de 1000 Hz com resolucao de 10 bits.
    ledcSetup(1, 5000, 8); 
    
    pinMode(25, OUTPUT); // SAIDA DIGITAL ENABLE LY (Ponte H)
    pinMode(26, OUTPUT); // SAIDA DIGITAL ENABLE LY (Ponte H)
    
    // -----------------------------------------------------------------------------------
  
}

void loop(){
  waitIncommingConnection();
}

void waitIncommingConnection() //Função que espera alguém conectar, quando conecta começa  a fazer algo
{
  String inData         = "";    //dados de entrada
  WiFiClient client = server.available(); //cria um objeto client
    
  if (client) //se deu certo na hora de criar o objeto client, entra no if
  {
    while ( client.connected() ) //se houver cliente conectado, entra no while
    {      
      if ( client.available() ) //aqui é pra ver se está disponivel
      { 
        char recieved = client.read(); //se estiver, vai ler os dados na variavel indata
        inData += recieved;   
        if (recieved == '\n')
        { 
          controleAnalogico(inData, 50, 0, 14, 12); // RY entrada, botao, canal, porta1, porta2
          controleAnalogico(inData, 48, 1, 26, 25); // LY entrada, botao, canal, porta1, porta2
          inData = ""; //aqui reinicializa a variavel indata para pegar o próximo comando
        }        
      }
    }
    client.stop();
    Serial.println("Client Disconnected.");   
  }
}

void controleAnalogico(String entrada, int botao, int canal, int porta1, int porta2)
{
  if (entrada[0] == botao )// ANALOGICO R Y inData[0] == 48 RECEBE DE 0.00 - 10
  {                            
    float val = (((entrada[2]-48)*10) + ((entrada[3]-48)*1)); // XX.00  0-10 tanto pra cima quanto pra baixo
    float ptFr1 = (entrada[4]-48);
    float ptFr2 = ((entrada[5]-48));
    float valFra = ((ptFr1 / 10) + (ptFr2 /100));// 0.XX
    float resultPart = val + valFra;
    float result = resultPart * 25.5;
    
    // ANALOGICO R Y entrada[1] == 48 analogico pra cima, 49 analogico pra baixo
    if (entrada[1] == 48 )
    {
      ledcWrite(canal, result);
      digitalWrite(porta1, LOW);
      digitalWrite(porta2, HIGH);
    }       
    else
    {
      ledcWrite(canal, result);
      digitalWrite(porta1, HIGH);
      digitalWrite(porta2, LOW);
    }           
  } 
}
