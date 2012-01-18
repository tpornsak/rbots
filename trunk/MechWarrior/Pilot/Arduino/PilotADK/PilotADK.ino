// Arduino Sketch to go with MechWarrior RA II Pilot Software
// It is meant to be run on an Arduino ADK connected to a PC
// either thru usb or over bluetooth 

// Created 13 Jan 2012
// Ben Loftin
// GPL 3.0 or greater

// int => 2 bytes for the elevation gyro reading 10 bits so 
// fits in an int range range of -32,768 to 32,767 
// start negative an invalid analog reading
int gyroEL = -999;

// target sensor input pin, used as interrupt 
// to read which target sensor was hit
int targetSensor = 0;

// hitReport is a status counter to report hits
int hitReport;
// hitEvent is a status counter to time hits
int hitEvent;

// low battery input pin, needed to not kill the liPoly
int lowBatteryPin = 8;

//Volatile: Specifically, it directs the compiler to load the variable from RAM 
//and not from a storage register, which is a temporary memory 
//location where program variables are stored and manipulated. 
//Under certain conditions, the value for a variable stored in registers can be inaccurate.
// maybe a speed improvement also
// 1:
// 2:
// 3:
// 4:
byte hitPlate = 0x00;
int hitPlateInt = 0;
// number of times it has been hit
byte hitCount = 0x00;
// calculates the length of the interrupt pulse which corresponds to which target plate was hit
unsigned long hitTime;

// command message from host PC to master i2c 
byte cmdByte1;
byte cmdByte2;
byte legActionByte;
byte legSpeedByte;
byte turretElPosLow;
byte turretElPosHigh;
byte turretElSpeedLow;
byte turretElSpeedHigh;
byte turretAzPosLow;
byte turretAzPosHigh;
byte turretAzSpeedLow;
byte turretAzSpeedHigh;
byte gunFire;

byte motorAzID = 0x02;
byte motorElID = 0x01;

int xmtEn = 2;

// led on packet
byte ledOn01[8] = {0xff, 0xff, motorAzID, 0x04, 0x03, 0x19, 0x01, 0xE1};
// led off packet
byte ledOff01[8] = {0xff, 0xff, motorAzID, 0x04, 0x03, 0x19, 0x00, 0xE2};

// led on packet
byte ledOn02[8] = {0xff, 0xff, motorElID, 0x04, 0x03, 0x19, 0x01, 0xE1};
// led off packet
byte ledOff02[8] = {0xff, 0xff, motorElID, 0x04, 0x03, 0x19, 0x00, 0xE2};

//Torque Enable Motor Elevation
byte tqEnEl[8] = {0xff,0xff,motorElID,0x04,0x03,0x18,0x01,0x00};

// Goal Position for the Elevation AX-12 motor
byte goalEl[11] = {0xff, 0xff, motorElID, 0x07, 0x03, 0x1E, 0x00, 0x01, 0x00, 0x02, 0x00};
        
//Torque Enable Motor Elevation
byte tqEnAz[8] = {0xff,0xff,motorAzID,0x04,0x03,0x18,0x01,0x00};

// Goal Position for the Elevation AX-12 motor
byte goalAz[11] = {0xff, 0xff, motorAzID, 0x07, 0x03, 0x1E, 0x00, 0x01, 0x00, 0x02, 0x00};

void setup()
{
  //Wire.begin(MASTER_ADDRESS);  // join i2c bus and assign address 0x6
  Serial.begin(115200);  // start serial for output
  //Serial.begin(9600);  // start serial for output
  //pinMode(lowBatteryPin, INPUT);
  
  pinMode(xmtEn, OUTPUT); // output enables half duplex transmit buffer
  Serial3.begin(1000000); // sets 1 Mbps baudrate for uart
  
  //attachInterrupt(targetSensor, interruptTargetHit, CHANGE);
  //hitCount = 20;
  hitReport = 0;
  hitEvent = 0;
  //Serial.print('A', BYTE);
  //establishContact();  // send a byte to establish contact until receiver responds 
  
   //Enable Elevation motor
  digitalWrite(xmtEn, HIGH); //Enables transmit buffer
    for (int i=0; i<8; i++)
      Serial3.write(tqEnEl[i]); //Sends motor enable packet
  delay(0);                  //Need this 0 delay for the transmission to complete
  digitalWrite(xmtEn, LOW);  //Disables transmit buffer, saw it on the scope
  delay(5);                  //5ms follows the pot smoothly, 100ms is jumpy, emperical

   //Enable Elevation motor
  digitalWrite(xmtEn, HIGH); //Enables transmit buffer
    for (int i=0; i<8; i++)
      Serial3.write(tqEnAz[i]); //Sends motor enable packet
  delay(0);                  //Need this 0 delay for the transmission to complete
  digitalWrite(xmtEn, LOW);  //Disables transmit buffer, saw it on the scope
  delay(5);                  //5ms follows the pot smoothly, 100ms is jumpy, emperical
  
}

void loop()
{
   //int readLowBatt = digitalRead(lowBatteryPin);
    // only do something if the liPoly is above 10v
   //if ( readLowBatt == LOW)
   //{
     
     Serial.flush();
  // request 2 bytes (a word) from stab subsystem
  //Wire.requestFrom(STAB_PLATFORM_ADDRESS, 2);   
  // if we get a valid byte, read analog ins:
  byte inByte = '\0'; // declare and initialize a byte to read in serial data
  long startTime = millis();//makes the start time = to now
  int timeout = 1000; // timeout after one second
  while(millis() - startTime < timeout && inByte != '*') {
    inByte = Serial.read(); // read data and wait for an asterisk character
  }
  
  if (inByte == '*') { // if we got the correct start character (instead of a timeout)
  
    long startTime = millis();//makes the start time = to now
    int timeout = 1000; // timeout after one second
    while(millis() - startTime < timeout && Serial.available() < 12) {
      ; //wait for enough data to be available (11 characters of cmd string), while doing nothing else
    }
        // read in commands from serial port
        legActionByte   = Serial.read();
        legSpeedByte    = Serial.read();
        turretElPosLow    = Serial.read();
        turretElPosHigh   = Serial.read();
        turretElSpeedLow    = Serial.read();
        turretElSpeedHigh   = Serial.read();
        turretAzPosLow     = Serial.read();
        turretAzPosHigh   = Serial.read();
        turretAzSpeedLow     = Serial.read();
        turretAzSpeedHigh   = Serial.read();
        gunFire   = Serial.read();
        
        
        // move Elevation motor
        goalAz[6] = turretAzPosLow;
        goalAz[7] = turretAzPosHigh;
        goalAz[8] = turretAzSpeedLow;
        goalAz[9] = turretAzSpeedHigh;
        goalAz[10] = ~(goalAz[2]+goalAz[3]+goalAz[4]+goalAz[5]+goalAz[6] + goalAz[7] + goalAz[8] + goalAz[9]); 
      
        // send commands to AX-12 Elevation
        digitalWrite(xmtEn, HIGH); //Enables transmit buffer
        for (int i=0; i<11; i++) {   
          Serial3.write(goalAz[i]); //Sends motor packet
          delayMicroseconds(25);
        }  
        delayMicroseconds(25);
        digitalWrite(xmtEn, LOW); // disables transmit buffer
        
        // move Elevation motor
        goalEl[6] = turretElPosLow;
        goalEl[7] = turretElPosHigh;
        goalEl[8] = turretElSpeedLow;
        goalEl[9] = turretElSpeedHigh;
        goalEl[10] = ~(goalEl[2]+goalEl[3]+goalEl[4]+goalEl[5]+goalEl[6] + goalEl[7] + goalEl[8] + goalEl[9]); 
      
        // send commands to AX-12 Elevation
        digitalWrite(xmtEn, HIGH); //Enables transmit buffer
        for (int i=0; i<11; i++) {   
          Serial3.write(goalEl[i]); //Sends motor packet
          delayMicroseconds(25);
        }  
        delayMicroseconds(25);
        digitalWrite(xmtEn, LOW); // disables transmit buffer
        // stub for sending leg commands over i2c

        // send led test byte over i2c to the walking subsystem
        //Wire.beginTransmission(WALKING_BASE_ADDRESS); 
        //Wire.send(legActionByte);    
        //Wire.endTransmission();
        
       // delay(25);
        
        // send turret bytes over i2c to the turret subsystem
        //Wire.beginTransmission(STAB_PLATFORM_ADDRESS); 
        //Wire.send(turretElPosLow);   
        //Wire.send(turretElPosHigh);  
        //Wire.send(turretElSpeedLow);   
        //Wire.send(turretElSpeedHigh);   
        //Wire.send(turretAzPosLow);   
        //Wire.send(turretAzPosHigh);    
        //Wire.send(turretAzSpeedLow);   
        //Wire.send(turretAzSpeedHigh); 
        //Wire.send(gunFire); 
        //Wire.endTransmission();
        
        //if (hitReport > 0)  {

        //    Serial.print(hitCount,BYTE);
        //    Serial.println(hitPlate,BYTE);
        //    hitReport = 0;
        //}

//        Serial.print(turretElPosLow,DEC); 
//        Serial.print(','); 
//        Serial.print(turretElPosHigh,DEC); 
//        Serial.print(','); 
//        Serial.print(turretElSpeedLow,DEC); 
//        Serial.print(','); 
//        Serial.print(turretElSpeedHigh,DEC); 
//        Serial.print(','); 
//        Serial.print(turretAzPosLow,DEC); 
//        Serial.print(','); 
//        Serial.print(turretAzPosHigh,DEC); 
//        Serial.print(','); 
//        Serial.print(turretAzSpeedLow,DEC); 
//        Serial.print(','); 
//        Serial.println(turretAzSpeedHigh,DEC); 
//        Serial.print(','); 
//        Serial.print(gunFire,DEC); 
        //Serial.flush();
        
    }
    else
    {
     ;// Serial.println('-1'); 
    }      
     
   //} 
}

void interruptTargetHit()  {
  if(hitEvent < 1)  {  // line went high, this is a hit
    hitTime = millis();
    hitCount++;
    hitEvent = 1;  } 
    else  {
    hitTime = millis() - hitTime;
    hitPlateInt = hitTime/50;
    // let the main loop know to report a hit
    hitReport = 1;
    switch (hitPlateInt)
    {
      case 1:
        hitPlate = 0x01;
        break;
      case 2:
        hitPlate = 0x02;
        break;
      case 3:
        hitPlate = 0x03;
        break;
      case 4:
        hitPlate = 0x04;
        break;
      default:
        hitReport = 0;
      
    }
    // reset to check for signal to go high
    hitEvent = 0;
    
    }
}

void establishContact() {
  while (Serial.available() <= 0) {
    Serial.print('A');   // send a capital A
    delay(300);
  }
}
