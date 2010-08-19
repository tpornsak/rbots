/*

Blink the LED on the AX-12 motors

RTEAM Robotics Club
Ben Loftin
John Harkey
*/

// led on packet
byte led_on_1[8] = {0xff, 0xff, 0xfe, 0x04, 0x03, 0x19, 0x01, 0xE1};
// led off packet
byte led_off_1[8] = {0xff, 0xff, 0xfe, 0x04, 0x03, 0x19, 0x00, 0xE2};

int xmtEn = 2;

void setup()
{
  pinMode(xmtEn, OUTPUT);  // output enables half duplex transmit buffer 
  Serial.begin(1000000);    // sets 1 Mbps baudrate for uart
  
  //try calculating checksum
  led_on_1[7] = ~(led_on_1[2]+led_on_1[3]+led_on_1[4]+led_on_1[5]+led_on_1[6]);
  led_off_1[7] = ~(led_off_1[2]+led_off_1[3]+led_off_1[4]+led_off_1[5]+led_off_1[6]);
}

void loop()
{
  digitalWrite(xmtEn, HIGH);  // enable transmit buffer
  for(int i = 0; i < 8; i++)
    Serial.write(led_on_1[i]);
  delay(0);                   // need this 0 delay for the transmission to complete
  digitalWrite(xmtEn, LOW);   // disables transmit buffer
  
  delay(1000);   // blink freq
  
  digitalWrite(xmtEn, HIGH);  // enable transmit buffer
  for(int i = 0; i < 8; i++)
    Serial.write(led_off_1[i]);
  delay(0);                   // need this 0 delay for the transmission to complete
  digitalWrite(xmtEn, LOW);   // disables transmit buffer
  
  delay(1000);   // blink freq
  
}
