// Control Program for the MechWarrior's platform stabilization subsytem
// Test setup to move the elevation motor at a constant rate between
// 60 - 240 degrees
// and sending the gyro rate output to the master i2c device

// Communicates using the Atmega's UART with the 2 AX-12 motors 
// with half-duplex circuitry for Azimuth/Elevation control

// Uses i2c (as slave) to send data to a master i2c device

// gyro data is from the 10 bit A2D, sends 2 bytes over i2c

// Created 21 March 2010
// Ben Loftin
// GPL 3.0 or greater
#include <Bounce.h>

// i2c address fot the platform stabilization subsystem
#define STAB_PLATFORM_ADDRESS 0x7

#include <Wire.h>

// Analog input pin that the rate gyro is attached
const int analogGyroElPin = 0; 

// elevation gyro data to read from sensor and send
// to i2c master device 
int gyroEl = 0;

// enable pin for half-duplex communication with the AX-12 motors
int xmtEn = 7;

// MechWarrior's gun fire pin
int gunPin = 3;

// digital input pin for the gun sensor which
// lets us know when the gun has finished firing
int gunSensor = 2;
// Instantiate a Bounce object with a 5 millisecond debounce time
// for the gun sensor signal
Bounce bouncer = Bounce( gunSensor,100 ); 
int stateFiring = 0;

// Hopper motor control pin
int hopperPin = 8;

// MechWarrior's Laser pointer pin
int laserPin = 5;

// flag to fire gun, received via i2c
byte gunFire = 0x00;
// led on packet Azimuth motor
byte ledOnEl[8] = {0xff, 0xff, 0x02, 0x04, 0x03, 0x19, 0x01, 0x00};
// led off packet Azimuth motor
byte ledOffEl[8] = {0xff, 0xff, 0x02, 0x04, 0x03, 0x19, 0x00, 0x00};

// 180 goal position with max speed ~113 RPM  1.9 deg/s
byte goal240[11] = {0xff, 0xff, 0x03, 0x07, 0x03, 0x1E, 0x00, 0x02, 0x00, 0x02, 0x00};
// 180 goal position with max speed ~113 RPM  1.9 deg/s
byte goal60[11] = {0xff, 0xff, 0x02, 0x07, 0x03, 0x1E, 0x00, 0x01, 0x00, 0x02, 0x00};

//Torque Enable Azimuth Motor
byte tqEnEl[8] = {0xff,0xff,0x02,0x04,0x03,0x18,0x01,0x00};
byte tqEnAz[8] = {0xff,0xff,0x03,0x04,0x03,0x18,0x01,0x00};

/* Removing continuous motion for the AX-12 motors is done by setting 
values for the CW Angle Limit an the CCW Angle Limit 

0x06:  CW Angle Limit (L)
0x07:  CW Angle Limit (H)
0x08:  CCW Angle Limit (L)
0x09:  CCW Angle Limit (H)

Set continuous mode address through 06-09
*/
//Non-Continuous (Endless Turn) Mode on motor ID 01, set CW and CCW to full 
// 300 degree limit by passing 0x3ff, high byte FF, low byte 03
byte continuous01[11] = {0xff,0xff,0x03,0x07,0x03,0x06,0xFF,0x03,0xFF,0x03,0x00};

//Continuous (Endless Turn) Mode on right motor
byte continuous02[11] = {0xff,0xff,0x02,0x07,0x03,0x06,0xFF,0x03,0xFF,0x03,0x00};

void setup()
{
  pinMode(xmtEn, OUTPUT);  // output enables half duplex transmit buffer 
  pinMode(gunPin, OUTPUT);  
  pinMode(hopperPin, OUTPUT); 
  pinMode(laserPin, OUTPUT); 
  pinMode(gunSensor, INPUT);
  
  digitalWrite(gunPin, LOW);   // turn the gun off
  digitalWrite(laserPin, LOW);
  digitalWrite(hopperPin, LOW);  
  
  //attachInterrupt(gunIntPin, interruptGunFiringDone, RISING);
  
  Serial.begin(1000000);    // sets 1 Mbps baudrate for uart
 //Serial.begin(9600); 
  //calculate checksum
  ledOnEl[7] = ~(ledOnEl[2]+ledOnEl[3]+ledOnEl[4]+ledOnEl[5]+ledOnEl[6]);
  ledOffEl[7] = ~(ledOffEl[2]+ledOffEl[3]+ledOffEl[4]+ledOffEl[5]+ledOffEl[6]);
  tqEnEl[7] =  ~(tqEnEl[2]+tqEnEl[3]+tqEnEl[4]+tqEnEl[5]+tqEnEl[6]);
  tqEnAz[7] =  ~(tqEnAz[2]+tqEnAz[3]+tqEnAz[4]+tqEnAz[5]+tqEnAz[6]);
  goal240[10] = ~(goal240[2]+goal240[3]+goal240[4]+goal240[5]+goal240[6] + goal240[7] + goal240[8] + goal240[9]);
  goal60[10] = ~(goal60[2]+goal60[3]+goal60[4]+goal60[5]+goal60[6] + goal60[7] + goal60[8] + goal60[9]); 
   continuous01[10] = ~(continuous01[2] + continuous01[3] + continuous01[4] + 
                                continuous01[5] + continuous01[6] + continuous01[7] +
                                continuous01[8] + continuous01[9]);

  continuous02[10] = ~(continuous02[2] + continuous02[3] + continuous02[4] + 
                                continuous02[5] + continuous02[6] + continuous02[7] +
                                continuous02[8] + continuous02[9]); 
 
  
  //Enable motor 1
  digitalWrite(xmtEn, HIGH); //Enables transmit buffer
    for (int i=0; i<8; i++)
      Serial.write(tqEnEl[i]); //Sends motor enable packet
  delay(0);                  //Need this 0 delay for the transmission to complete
  digitalWrite(xmtEn, LOW);  //Disables transmit buffer, saw it on the scope
  delay(5);                  //5ms follows the pot smoothly, 100ms is jumpy, emperical
  
  //Enable motor 1
  digitalWrite(xmtEn, HIGH); //Enables transmit buffer
    for (int i=0; i<8; i++)
      Serial.write(tqEnAz[i]); //Sends motor enable packet
  delay(0);                  //Need this 0 delay for the transmission to complete
  digitalWrite(xmtEn, LOW);  //Disables transmit buffer, saw it on the scope
  delay(5);                  //5ms follows the pot smoothly, 100ms is jumpy, emperical
  
    // set continous rotation motor 1
  digitalWrite(xmtEn, HIGH);  // enable transmit buffer
  for(int i = 0; i < 11; i++)
    Serial.write(continuous01[i]);
  delay(0);                   // need this 0 delay for the transmission to complete
  digitalWrite(xmtEn, LOW);   // disables transmit buffer
  delay(5);                  //5ms follows the pot smoothly, 100ms is jumpy, emperical
  
  // set continous rotation motor 2
  digitalWrite(xmtEn, HIGH);  // enable transmit buffer
  for(int i = 0; i < 11; i++)
    Serial.write(continuous02[i]);
  delay(0);                   // need this 0 delay for the transmission to complete
  digitalWrite(xmtEn, LOW);   // disables transmit buffer
  delay(5);                  //5ms follows the pot smoothly, 100ms is jumpy, emperical
  
  
  Wire.begin(STAB_PLATFORM_ADDRESS);  // join i2c bus with address 0x7
  //Wire.onRequest(requestEvent); // register event
  Wire.onReceive(receiveEvent); // register event
}

void loop()
{
  // Update the debouncer
  bouncer.update ( );
  // Get the update value
  int valueFall = bouncer.fallingEdge();
  if ( valueFall == HIGH) {
    stateFiring = 1;
  }
  int valueRise = bouncer.risingEdge();
  
  // Turn off gun
  if ( valueRise == HIGH && stateFiring == 1) {
      digitalWrite(gunPin, LOW);  // set the gun pin to on
      digitalWrite(hopperPin, LOW);
      gunFire = 0x00;  
      stateFiring = 0;
  } 

  if (gunFire == 0x01)
  {
      //digitalWrite(laserPin, LOW);
      digitalWrite(gunPin, HIGH);  // set the gun pin to on
      digitalWrite(hopperPin, HIGH);
      gunFire = 0x00;     
  }
  
   //digitalWrite(hopperPin, HIGH);
   //delay(2000);
   //digitalWrite(hopperPin, LOW);
  
  //delayMicroseconds(50);   
  // read the analog in value:
  //gyroEl = analogRead(analogGyroElPin); 
  // wait 20 milliseconds before the next loop
  // might be able to go down to 10
  // for the analog-to-digital converter to settle
  // after the last reading:
  
  // set goal position to 240
  //Enable motor 1
//  digitalWrite(xmtEn, HIGH); //Enables transmit buffer
//    for (int i=0; i<8; i++)
//      Serial.write(tqEnEl[i]); //Sends motor enable packet
//  delay(0);                  //Need this 0 delay for the transmission to complete
//  digitalWrite(xmtEn, LOW);  //Disables transmit buffer, saw it on the scope
//  delay(5);                  //5ms follows the pot smoothly, 100ms is jumpy, emperical
//  
//  digitalWrite(xmtEn, HIGH); //Enables transmit buffer
//    for (int i=0; i<11; i++)
//      Serial.write(goal60[i]); //Sends motor enable packet
//  delay(0);                  //Need this 0 delay for the transmission to complete
//  digitalWrite(xmtEn, LOW);  //Disables transmit buffer, saw it on the scope
//  delay(5);                  //5ms follows the pot smoothly, 100ms is jumpy, emperical
//  
  // set goal position to 
  //delay(2000);
//  delay(20);
//  digitalWrite(xmtEn, HIGH); //Enables transmit buffer
//    for (int i=0; i<11; i++)
//      Serial.write(goal240[i]); //Sends motor enable packet
//  delay(0);                  //Need this 0 delay for the transmission to complete
//  digitalWrite(xmtEn, LOW);  //Disables transmit buffer, saw it on the scope
//  delay(5);                  //5ms follows the pot smoothly, 100ms is jumpy, emperical
  
  //delay(2000);
  //delay(2000);
}

// function that executes whenever data is to be sent to master
// this function is registered as an event, see setup()
//void requestEvent()
//{
//  // respond with message of 2 bytes - 2 bytes int
//  // as expected by master
//  Wire.send((uint8_t *)&gyroEl, sizeof(gyroEl));
//  //gyroEl = 56;
//  //byte tx[2];
//  //tx[0] = 0x23;
//  //tx[1] = 0xCD;
//  // Wire.send(tx);
//  //Wire.send(0xAC);
//  //Wire.send(gyroEl >> 8);
//  //Wire.send((int)(gyroEl & 0xFF )); 
//  
//}

// function that executes whenever data is received from master
// this function is registered as an event, see setup()
void receiveEvent(int howMany)
{
  //if(Wire.available()) // loop through all but the last
  //{
  byte cmdElPosLow = Wire.receive(); // receive byte as a character
  byte cmdElPosHigh = Wire.receive();
  byte cmdElSpeedLow = Wire.receive(); 
  byte cmdElSpeedHigh = Wire.receive();
  byte cmdAzPosLow = Wire.receive(); 
  byte cmdAzPosHigh = Wire.receive();
  byte cmdAzSpeedLow = Wire.receive(); 
  byte cmdAzSpeedHigh = Wire.receive();
  gunFire = Wire.receive();
    //cmdElHigh = Wire.receive(); // receive byte as a character
    //cmdElHigh = map(int(cmdElHigh), 0, 126, 1, 2);
    //cmdAzHigh = map(int(cmdAzHigh), 0, 126, 0, 2);
    //cmdAzLow = map(int(cmdAzLow), 0, 126, 33, 204);
    //cmdAzHigh = 0x00;
    //cmdAzLow = 0x00;
    
    //cmdAzHigh = map(int(cmdAzHigh), 0, 126, 0, 2);
    
    //if (cmdElHigh <= 0x02 && cmdElHigh <= 0x01 ) {
      goal60[6] = cmdElPosLow;
      goal60[7] = cmdElPosHigh;
      goal60[8] = cmdElSpeedLow;
      goal60[9] = cmdElSpeedHigh;
      goal60[10] = ~(goal60[2]+goal60[3]+goal60[4]+goal60[5]+goal60[6] + goal60[7] + goal60[8] + goal60[9]); 
      goal240[6] = cmdAzPosLow;
      goal240[7] = cmdAzPosHigh;
      goal240[8] = cmdAzSpeedLow;
      goal240[9] = cmdAzSpeedHigh;
      goal240[10] = ~(goal240[2]+goal240[3]+goal240[4]+goal240[5]+goal240[6] + goal240[7] + goal240[8] + goal240[9]); 
    //}
    
      // set goal position to 240
  //Enable motor 1
//  digitalWrite(xmtEn, HIGH); //Enables transmit buffer
//    for (int i=0; i<8; i++)
//      Serial.write(tqEnEl[i]); //Sends motor enable packet
//  delay(0);                  //Need this 0 delay for the transmission to complete
//  digitalWrite(xmtEn, LOW);  //Disables transmit buffer, saw it on the scope
//  delay(5);                  //5ms follows the pot smoothly, 100ms is jumpy, emperical
  
  digitalWrite(xmtEn, HIGH); //Enables transmit buffer
    for (int i=0; i<11; i++) {
      
      Serial.write(goal60[i]); //Sends motor enable packet
      delayMicroseconds(25);
  }
  digitalWrite(xmtEn, LOW);  //Disables transmit buffer, saw it on the scope
  
  digitalWrite(xmtEn, HIGH); //Enables transmit buffer
    for (int i=0; i<11; i++) {
      
      Serial.write(goal240[i]); //Sends motor enable packet
      delayMicroseconds(25);
  }
  digitalWrite(xmtEn, LOW);  //Disables transmit buffer, saw it on the scope
  
  //}
//  goal60[6] = goal240[6];
//  goal60[7] = goal240[7];
  //Serial.println(cmdElLow,DEC);
  //Serial.println(cmdElHigh,DEC);
}

void interruptGunFiringDone()  {
  //digitalWrite(gunPin, LOW);   // turn the gun off
  //digitalWrite(hopperPin, LOW);
  gunFire = 0x00;  
  
}

 
