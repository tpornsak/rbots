/*
 * Auto-Generated by NUKE!
 *   http://arbotix.googlecode.com
 */

#include <ax12.h>
#include <BioloidController.h>
#include <Commander.h>
#include "nuke.h"

Commander command = Commander();

void setup(){
    pinMode(0,OUTPUT);
    // setup IK
    setupIK();
    gaitSelect(AMBLE);
    // setup serial
    Serial.begin(38400);

    // wait, then check the voltage (LiPO safety)
    delay (1000);
    float voltage = (ax12GetRegister (1, AX_PRESENT_VOLTAGE, 1)) / 10.0;
    Serial.print ("System Voltage: ");
    Serial.print (voltage);
    Serial.println (" volts.");
    if (voltage < 10.0)
        while(1);

    // stand up slowly
    bioloid.poseSize = 12;
    bioloid.readPose();
    doIK();
    bioloid.interpolateSetup(1000);
    while(bioloid.interpolating > 0){
        bioloid.interpolateStep();
        delay(3);
    }
}

void loop(){
  // update IK if needed
  if(bioloid.interpolating == 0){
    doIK();
    bioloid.interpolateSetup(tranTime);
  }
  // update joints
  bioloid.interpolateStep();
  // take commands
  if(command.ReadMsgs() > 0){
    digitalWrite(0,HIGH-digitalRead(0));
    Xspeed = ((command.walkV));
    if((command.buttons&BUT_LT) > 0)
      Yspeed = (command.walkH);
    else
      Rspeed = -(command.walkH)/250.0;
    bodyRotY = (((float)command.lookV))/250.0;
    if((command.buttons&BUT_RT) > 0)
      bodyRotX = ((float)command.lookH)/250.0;
    else
      bodyRotZ = ((float)command.lookH)/250.0;
  }
}

