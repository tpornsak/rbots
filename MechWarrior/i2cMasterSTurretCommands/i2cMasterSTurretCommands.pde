// Reads a word (2 bytes) from an atmega I2C/TWI slave device
// then prints the word to serial port

// this is test setup for mechwarrior since the gyro is a 10 bit
// A2D and needs to come over i2c since the uart is being used
// in half duplex mode to communicate with the AX-12 motors

// Created 21 March 2010
// Ben Loftin
// GPL 3.0 or greater

// i2c address for this device (the master device)
#define MASTER_ADDRESS 0x6
// i2c address fot the platform stabilization subsystem
#define STAB_PLATFORM_ADDRESS 0x7

#include <Wire.h>

// int => 2 bytes for the elevation gyro reading 10 bits so 
// fits in an int range range of -32,768 to 32,767 
// start negative an invalid analog reading
int gyroEL = -999;

// command message from host PC to master i2c 
byte cmdByte1;
byte cmdByte2;
byte legActionByte;
byte legSpeedByte;
byte turretAzPos;
byte turretAzSpeed;
struct msgTurret {
  byte turretElPos;
  byte turretElSpeed;
};

msgTurret msgTurretOut = { 0, 0 };

        
void setup()
{
  Wire.begin(MASTER_ADDRESS);  // join i2c bus and assign address 0x6
  Serial.begin(9600);  // start serial for output
  Serial.print('A', BYTE);
  //establishContact();  // send a byte to establish contact until receiver responds 
}

void loop()
{
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
    while(millis() - startTime < timeout && Serial.available() < 6) {
      ; //wait for enough data to be available (14 characters of time string), while doing nothing else
    }
        // read in commands from serial port
        legActionByte   = Serial.read();
        legSpeedByte    = Serial.read();
        turretAzPos     = Serial.read();
        turretAzSpeed   = Serial.read();
        msgTurretOut.turretElPos     = Serial.read();
        msgTurretOut.turretElSpeed   = Serial.read();
        
        // stub for sending leg commands over i2c

        
        // send turret bytes over i2c to the turret subsystem
        Wire.beginTransmission(STAB_PLATFORM_ADDRESS); 
        Wire.send(msgTurretOut.turretElPos);   
        Wire.send(msgTurretOut.turretElSpeed);      
        Wire.endTransmission();
        
        Serial.println(msgTurretOut.turretElPos); 
        //Serial.flush();
        
    }
    else
    {
      Serial.println('E', BYTE); 
    }
            
   
}

void establishContact() {
  while (Serial.available() <= 0) {
    Serial.print('A', BYTE);   // send a capital A
    delay(300);
  }
}
