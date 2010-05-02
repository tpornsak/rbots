/*
R-TEAM Robotics Club
GPL License

Communicate using serial to the arduino board (bluetooth or usb/serial) to control the 2-axis servo turret

*/

#include <Servo.h> 
 
#include "WProgram.h"
void setup();
void loop();
void establishContact();
int gunPin =  5;    // Airsoft gun is connected to digital pin 5

// a maximum of eight servo objects can be created
Servo servoAz;  // create servo object to control the azimuth servo 
Servo servoEl;  // create servo object to control the elevation servo
      
int cmdByte = 0;    // variable to store the servo position
int elevation = 110; // keep track of elevation position, start in middle
int azimuth = 120;   // keep track of azimuth position, start in middle

void setup() 
{ 
  
  // initialize the digital pin for the gun as an output:
  pinMode(gunPin, OUTPUT); 
  
  // The two servos to control the turret
  servoAz.attach(9);  // attaches the az servo on pin 9 to the servo object
  servoEl.attach(10);  // attaches the el servo on pin 10 to the servo object
  
  // The BlueSMiRF bluetooth module is currently configured for 115200 kbps
  Serial.begin(115200); 

  // prints a starting string
  Serial.println("Press any key to begin...");
  establishContact(); 
} 

 

void loop() 
{ 
  
  while(true) {
  // if we get a valid byte, read analog ins:
  if (Serial.available() > 0) {
    // get incoming byte:
    cmdByte = Serial.read();
    
    // meant to use the numpad keys to move the servos
    // 4,6 for azimuth control
    // 8,2 for elevation control
    // 5 to fire
    switch(cmdByte) {
     
      // if number 4, decrement the azimuth
      case 52:
        if (azimuth > 10)
          azimuth = azimuth - 10;
        else
          azimuth = 0;
        break;
      // if number is 6, increment the azimuth
      case 54: 
        if (azimuth < 170)
          azimuth = azimuth + 10;
        else
          azimuth = 180;
        break; 
      // if number is 8, decrement the elevation
      case 56: 
        if (elevation > 10)
          elevation = elevation - 10;
        else
          elevation = 0;
        break;
      // if number is 2, increment the elevation
      case 50: 
        if (elevation < 170)
          elevation = elevation + 10;
        else
          elevation = 180;
        break;
      // if number is 5, fire the gun
      case 53:
        digitalWrite(gunPin, HIGH);  // set the gun pin to on
        delay(1200);                 // hold pin high for 2.5 seconds for a complete fire
        digitalWrite(gunPin, LOW);   // turn the gun off
        break; 
                
    } // end switch 
    
    // if nothing else matches, do nothing  so no default statement
    Serial.println("Recieved");
    Serial.println(cmdByte);
    
  }  // end serial avaiable If statement
    
    servoAz.write(azimuth);              // tell servo to go to position in variable 'Azimuth' 
    servoEl.write(elevation);            // tell servo to go to position in variable 'Elevation' 
    delay(15); 
    
  }
} 

// a function that prints 'A' to the serial port until
// we receive a byte
void establishContact() {
  while (Serial.available() <= 0) {
    Serial.print('A', BYTE);   // send a capital A
    delay(300);
  }
}

int main(void)
{
	init();

	setup();
    
	for (;;)
		loop();
        
	return 0;
}

