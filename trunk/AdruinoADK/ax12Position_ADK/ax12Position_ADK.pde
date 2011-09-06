/*

Move the AX-12 motor to a position using a  
message from an android capable device
current code was tested with Google's DemoKit
example AX-12 position connected to the Servo 1 slider

* Note since this is tested on the elevation motor of 
  the MechWarrior turret, we have constrained the 
  servo position to be within the integer values of 480-870

RTEAM Robotics Club
www.phys-x.org/rbots
Ben Loftin

GPL 3 or greater
*/

#include <Wire.h>
#include <Servo.h>

#include <Max3421e.h>
#include <Usb.h>
#include <AndroidAccessory.h>

byte motorID = 0x02;

int xmtEn = 31;

// AX-12 led on packet
byte ledOn01[8] = {0xff, 0xff, motorID, 0x04, 0x03, 0x19, 0x01, 0xE1};
// AX-12 led off packet
byte ledOff01[8] = {0xff, 0xff, motorID, 0x04, 0x03, 0x19, 0x00, 0xE2};

//Torque Enable Motor
byte tqEnEl[8] = {0xff,0xff,motorID,0x04,0x03,0x18,0x01,0x00};

// Goal Position for the Elevation AX-12 motor
byte goalEl[11] = {0xff, 0xff, motorID, 0x07, 0x03, 0x1E, 0x00, 0x01, 0x00, 0x02, 0x00};


AndroidAccessory acc("RTEAM Robotics Club",
		     "AX-12 Position Control",
		     "AX-12 Position Control Arduino Board",
		     "1.0",
		     "http://www.phys-x.org/rbots/",
		     "000000001");

void setup();
void loop();

byte b1, b2, b3, b4, c;
void setup()
{
  Serial.begin(115200);
  Serial.print("\r\nStart\r\n");

  pinMode(xmtEn, OUTPUT); // output enables half duplex transmit buffer
  Serial3.begin(1000000); // sets 1 Mbps baudrate for uart
  
  //calculating the checksums
  ledOn01[7] = ~(ledOn01[2]+ledOn01[3]+ledOn01[4]+ledOn01[5]+ledOn01[6]);
  ledOff01[7] = ~(ledOff01[2]+ledOff01[3]+ledOff01[4]+ledOff01[5]+ledOff01[6]);
  tqEnEl[7] =  ~(tqEnEl[2]+tqEnEl[3]+tqEnEl[4]+tqEnEl[5]+tqEnEl[6]);
  goalEl[10] = ~(goalEl[2]+goalEl[3]+goalEl[4]+goalEl[5]+goalEl[6] + goalEl[7] + goalEl[8] + goalEl[9]); 
  
  //Enable Elevation motor
  digitalWrite(xmtEn, HIGH); //Enables transmit buffer
    for (int i=0; i<8; i++)
      Serial3.write(tqEnEl[i]); //Sends motor enable packet
  delay(0);                  //Need this 0 delay for the transmission to complete
  digitalWrite(xmtEn, LOW);  //Disables transmit buffer, saw it on the scope
  delay(5);                  //5ms follows the pot smoothly, 100ms is jumpy, emperical
  
  acc.powerOn();
}

void loop()
{
  byte err;
  byte idle;
  static byte count = 0;
  byte msg[3];

  if (acc.isConnected()) {
    int len = acc.read(msg, sizeof(msg), 1);
    if (len > 0) {
      // assumes only one command per packet
      if (msg[0] == 0x3) {
        if (msg[1] == 0x0) {
            digitalWrite(xmtEn, HIGH); // enable transmit buffer
            if( msg[2] == HIGH)
            {
              for(int i = 0; i < 8; i++)
                Serial3.write(ledOn01[i]);
            }
            else
            {
              for(int i = 0; i < 8; i++)
                Serial3.write(ledOff01[i]);
            } 
            delayMicroseconds(25); // need this 0 delay for the transmission to complete
            digitalWrite(xmtEn, LOW); // disables transmit buffer
        }
      }
      else if (msg[0] == 0x2) {
        if (msg[1] == 0x10) {
           int cmdEl = map(msg[2], 0, 255, 480, 870);
           byte goalElPosLow = (byte)(cmdEl & 0xff);
           byte goalElPosHigh = (byte)(cmdEl >> 8);
           goalEl[6] = goalElPosLow;
           goalEl[7] = goalElPosHigh;
           Serial.print("Position Low: ");
           Serial.print(goalElPosLow,DEC);
           Serial.print(", ");
           Serial.print("Position High: ");
           Serial.println(goalElPosHigh,DEC);
                      
           goalEl[10] = ~(goalEl[2]+goalEl[3]+goalEl[4]+goalEl[5]+goalEl[6] + goalEl[7] + goalEl[8] + goalEl[9]); 
         
           digitalWrite(xmtEn, HIGH); //Enables transmit buffer
           for (int i=0; i<11; i++) {   
             Serial3.write(goalEl[i]); //Sends motor packet
              delayMicroseconds(25);
            }
           delayMicroseconds(25);
           digitalWrite(xmtEn, LOW); // disables transmit buffer
         }
      }
    }			
  } 
  else {
      // reset outputs to default values on disconnect
      digitalWrite(xmtEn, HIGH); // enable transmit buffer
      for(int i = 0; i < 8; i++)
        Serial3.write(ledOff01[i]); 
      delayMicroseconds(25); // need this 0 delay for the transmission to complete
      digitalWrite(xmtEn, LOW); // disables transmit buffer
		
  }

  delay(10);
}
