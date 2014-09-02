

// Dynamixel Tx/Rx
#define paramN 62                             // Dynamixel Parameter Number
byte bParam[paramN];                 // Transmission Parameters
byte bTxDynamixel[5 + paramN + 1];

// Host Tx/Rx
#define sizeRx 13
byte bRxHost[sizeRx];
int  iChcksumHost;
byte bChcksumHost;

// Function Protoypes
void DynamixelPosTx();
void HostComm();
float JoystickReshape(int x);

/*********************************************************************************************
 * Dynamixel Transmit Function
 *********************************************************************************************/
void DynamixelPosTx() {
  #define dynaL 4                              // Dynamixel Data Length
  #define dynaN 12                             // Number of Dynamixels

  #define Header1 255                          // Header #1
  #define Header2 255                          // Header #2
  //#define ID 254                               // Broadcast ID

  //#define Length (dynaL+1)*dynaN+4           // Transmission Length
  #define Length 64                            // Transmission Length
  #define Instr 131;                           // Instruction [131 = Synch Write] 
  unsigned int uSpeed   = 1023;                // Dynamixel Speed Command

  unsigned int uChksum;                        // Integer Checksum
  byte bChksum;                                // Byte Checksum
  
  byte  CommStatus;

  bParam[0]      = 30;                         // Startinmg Command Address [30 = Goal Position]
  bParam[1]      = 4;                          // Command Word Length
  word  GoalPos = 600;
  // communication with Dynamixel motors via OpenCM library
  // Make syncwrite packet
  Dxl.setTxPacketId(254);
  Dxl.setTxPacketInstruction(INST_SYNC_WRITE);
  Dxl.setTxPacketParameter(0, P_GOAL_POSITION_L);
  Dxl.setTxPacketParameter(1, 2);
  
  Dxl.setTxPacketParameter(2, L11);
  Dxl.setTxPacketParameter(3, Dxl.getLowByte(uT1[0]));
  Dxl.setTxPacketParameter(4, Dxl.getHighByte(uT1[0]));
  Dxl.setTxPacketParameter(5, L12);
  Dxl.setTxPacketParameter(6, Dxl.getLowByte(uT1[1]));
  Dxl.setTxPacketParameter(7, Dxl.getHighByte(uT1[1]));
  Dxl.setTxPacketParameter(8, L13);
  Dxl.setTxPacketParameter(9, Dxl.getLowByte(uT1[2]));
  Dxl.setTxPacketParameter(10, Dxl.getHighByte(uT1[2]));
  
  Dxl.setTxPacketParameter(11, L21);
  Dxl.setTxPacketParameter(12, Dxl.getLowByte(uT2[0]));
  Dxl.setTxPacketParameter(13, Dxl.getHighByte(uT2[0]));
  Dxl.setTxPacketParameter(14, L22);
  Dxl.setTxPacketParameter(15, Dxl.getLowByte(uT2[1]));
  Dxl.setTxPacketParameter(16, Dxl.getHighByte(uT2[1]));
  Dxl.setTxPacketParameter(17, L23);
  Dxl.setTxPacketParameter(18, Dxl.getLowByte(uT2[2]));
  Dxl.setTxPacketParameter(19, Dxl.getHighByte(uT2[2]));
  
  Dxl.setTxPacketParameter(20, L31);
  Dxl.setTxPacketParameter(21, Dxl.getLowByte(uT3[0]));
  Dxl.setTxPacketParameter(22, Dxl.getHighByte(uT3[0]));
  Dxl.setTxPacketParameter(23, L32);
  Dxl.setTxPacketParameter(24, Dxl.getLowByte(uT3[1]));
  Dxl.setTxPacketParameter(25, Dxl.getHighByte(uT3[1]));
  Dxl.setTxPacketParameter(26, L33);
  Dxl.setTxPacketParameter(27, Dxl.getLowByte(uT3[2]));
  Dxl.setTxPacketParameter(28, Dxl.getHighByte(uT3[2]));
  
  Dxl.setTxPacketParameter(29, L41);
  Dxl.setTxPacketParameter(30, Dxl.getLowByte(uT4[0]));
  Dxl.setTxPacketParameter(31, Dxl.getHighByte(uT4[0]));
  Dxl.setTxPacketParameter(32, L42);
  Dxl.setTxPacketParameter(33, Dxl.getLowByte(uT4[1]));
  Dxl.setTxPacketParameter(34, Dxl.getHighByte(uT4[1]));
  Dxl.setTxPacketParameter(35, L43);
  Dxl.setTxPacketParameter(36, Dxl.getLowByte(uT4[2]));
  Dxl.setTxPacketParameter(37, Dxl.getHighByte(uT4[2]));
  
  
  //Dxl.setTxPacketParameter(2+3*i, id[i]);
  //  Dxl.setTxPacketParameter(2+3*i+1, Dxl.getLowByte(GoalPos));
  //  Dxl.setTxPacketParameter(2+3*i+2, Dxl.getHighByte(GoalPos));
  SerialUSB.println(uT1[1]);
  SerialUSB.println(uT2[1]);
  SerialUSB.println(uT3[1]);
  SerialUSB.println(uT4[1]);
  // Leg 1 Commands
  bParam[2]      = L11;                        // Joint ID
  bParam[3]      = lowByte(uT1[0]);            // Position Low Byte
  bParam[4]      = highByte(uT1[0]);           // Position High Byte
  bParam[5]      = lowByte(uSpeed);            // Speed Low Byte
  bParam[6]      = highByte(uSpeed);           // Speed High Byte
  bParam[7]      = L12;                        // Joint ID
  bParam[8]      = lowByte(uT1[1]);            // Position Low Byte
  bParam[9]      = highByte(uT1[1]);           // Position High Byte
  bParam[10]     = lowByte(uSpeed);            // Speed Low Byte
  bParam[11]     = highByte(uSpeed);           // Speed High Byte
  bParam[12]     = L13;                        // Joint ID
  bParam[13]     = lowByte(uT1[2]);            // Position Low Byte
  bParam[14]     = highByte(uT1[2]);           // Position High Byte
  bParam[15]     = lowByte(uSpeed);            // Speed Low Byte
  bParam[16]     = highByte(uSpeed);           // Speed High Byte

  // Leg 2 Commands
  bParam[17]     = L21;                        // Joint ID
  bParam[18]     = lowByte(uT2[0]);            // Position Low Byte
  bParam[19]     = highByte(uT2[0]);           // Position High Byte
  bParam[20]     = lowByte(uSpeed);            // Speed Low Byte
  bParam[21]     = highByte(uSpeed);           // Speed High Byte
  bParam[22]     = L22;                        // Joint ID
  bParam[23]     = lowByte(uT2[1]);            // Position Low Byte
  bParam[24]     = highByte(uT2[1]);           // Position High Byte
  bParam[25]     = lowByte(uSpeed);            // Speed Low Byte
  bParam[26]     = highByte(uSpeed);           // Speed High Byte
  bParam[27]     = L23;                        // Joint ID
  bParam[28]     = lowByte(uT2[2]);            // Position Low Byte
  bParam[29]     = highByte(uT2[2]);           // Position High Byte
  bParam[30]     = lowByte(uSpeed);            // Speed Low Byte
  bParam[31]     = highByte(uSpeed);           // Speed High Byte

  // Leg 3 Commands
  bParam[32]     = L31;                        // Joint ID
  bParam[33]     = lowByte(uT3[0]);            // Position Low Byte
  bParam[34]     = highByte(uT3[0]);           // Position High Byte
  bParam[35]     = lowByte(uSpeed);            // Speed Low Byte
  bParam[36]     = highByte(uSpeed);           // Speed High Byte
  bParam[37]     = L32;                        // Joint ID
  bParam[38]     = lowByte(uT3[1]);            // Position Low Byte
  bParam[39]     = highByte(uT3[1]);           // Position High Byte
  bParam[40]     = lowByte(uSpeed);            // Speed Low Byte
  bParam[41]     = highByte(uSpeed);           // Speed High Byte
  bParam[42]     = L33;                        // Joint ID
  bParam[43]     = lowByte(uT3[2]);            // Position Low Byte
  bParam[44]     = highByte(uT3[2]);           // Position High Byte
  bParam[45]     = lowByte(uSpeed);            // Speed Low Byte
  bParam[46]     = highByte(uSpeed);           // Speed High Byte

  // Leg 4 Commands
  bParam[47]     = L41;                        // Joint ID
  bParam[48]     = lowByte(uT4[0]);            // Position Low Byte
  bParam[49]     = highByte(uT4[0]);           // Position High Byte
  bParam[50]     = lowByte(uSpeed);            // Speed Low Byte
  bParam[51]     = highByte(uSpeed);           // Speed High Byte
  bParam[52]     = L42;                        // Joint ID
  bParam[53]     = lowByte(uT4[1]);            // Position Low Byte
  bParam[54]     = highByte(uT4[1]);           // Position High Byte
  bParam[55]     = lowByte(uSpeed);            // Speed Low Byte
  bParam[56]     = highByte(uSpeed);           // Speed High Byte
  bParam[57]     = L43;                        // Joint ID
  bParam[58]     = lowByte(uT4[2]);            // Position Low Byte
  bParam[59]     = highByte(uT4[2]);           // Position High Byte
  bParam[60]     = lowByte(uSpeed);            // Speed Low Byte
  bParam[61]     = highByte(uSpeed);           // Speed High Byte

  uChksum        = ID + Length + Instr;
  bTxDynamixel[0]         = Header1;
  bTxDynamixel[1]         = Header2;
  bTxDynamixel[2]         = 254;
  bTxDynamixel[3]         = Length;
  bTxDynamixel[4]         = Instr;

  for (int j = 0; j < paramN; j++) {
    uChksum      = uChksum + bParam[j];
    bTxDynamixel[j + 5]     = bParam[j];
  }
  bChksum        = 255 - lowByte(uChksum);
  bTxDynamixel[5 + paramN]  = bChksum;

  // Serial1.write(bTxDynamixel, 5 + paramN + 1); // Uncomment for Atmega2560
  //DynamixelSerial.write(bTxDynamixel, 5 + paramN + 1);
  
  Dxl.setTxPacketLength((2+1)*dynaN+4);
  Dxl.txrxPacket();

  CommStatus = Dxl.getResult();
  
  if( CommStatus == COMM_RXSUCCESS ){
     SerialUSB.println("COMM_RXSUCCESS");
  }
  else
  {
      SerialUSB.println("COMM_ERROR");
      SerialUSB.println(uT1[0]);
  }
}
/*
// temp function until we get xbee and controller setup
void HostComm() {
  
  // go forward slowly
  // 511 center up max 1023, up min 0
  // Step Commands in x-direction
  fdelx   = JoystickReshape(1023) * delxMult * float(uStepSize);
  fdely   = -JoystickReshape(1023) * delyMult * float(uStepSize);;
}
*/
/*********************************************************************************************
 * Host Tx/Rx Function - From DFrobot Wireless Controller
 *********************************************************************************************/
void HostComm() {
  unsigned Buttons1;
  unsigned Buttons2;
  unsigned LeftJoystickLRLOW;
  unsigned LeftJoystickLRHIGH;
  unsigned LeftJoystickUDLOW;
  unsigned LeftJoystickUDHIGH;
  unsigned RightJoystickLRLOW;
  unsigned RightJoystickLRHIGH;
  unsigned RightJoystickUDLOW;
  unsigned RightJoystickUDHIGH;

  if (Serial2.available() > sizeRx - 1) {
    for (int i = 0; i <= sizeRx - 1; i++) {
      bRxHost[i] = Serial2.read();
    }
    iChcksumHost = 0;
    for (int i = 2; i <= sizeRx - 2; i++) {
      iChcksumHost = iChcksumHost + int(bRxHost[i]);
    }
    bChcksumHost = 255 - lowByte(iChcksumHost);
    if (bRxHost[0] == 255 && bRxHost[1] == 255 && bChcksumHost == bRxHost[sizeRx - 1]) {
      // Parse Buttons
      Buttons1          = unsigned(bRxHost[2]);
      Buttons2          = unsigned(bRxHost[3]); 
      byte buttonJ2     = bitRead(Buttons1, 0); // J2      Right Joystick Z-Axis (Pressed = 0, Unpressed = 1023)
      byte buttonJ1     = bitRead(Buttons1, 1); // J1      Left Joystick Z-Axis (Pressed = 0, Unpressed = 1023)
      byte buttonSelect = bitRead(Buttons1, 2); // S1      Select Button
      byte buttonStart  = bitRead(Buttons1, 3); // S2      Start Button
      byte buttonUp     = bitRead(Buttons1, 4); // UP      Left Button Up
      byte buttonLeft   = bitRead(Buttons1, 5); // LEFT    Left Button Left
      byte buttonDown   = bitRead(Buttons1, 6); // DOWN    Left Button Down
      byte buttonRight  = bitRead(Buttons1, 7); // RIGHT   Left Button Right

      byte button1      = bitRead(Buttons2, 0); // 1       Right Button 1
      byte button4      = bitRead(Buttons2, 1); // 4       Right Button 4
      byte button2      = bitRead(Buttons2, 2); // 2       Right Button 2
      byte button3      = bitRead(Buttons2, 3); // 3       Right Button 3
      byte buttonRZ1    = bitRead(Buttons2, 4); // RZ1     Right Z1 Button (Upper)
      byte buttonRZ2    = bitRead(Buttons2, 5); // RZ2     Right Z2 Button (Lower)
      byte buttonLZ1    = bitRead(Buttons2, 6); // LZ1     Left Z1 Button  (Upper)
      byte buttonLZ2    = bitRead(Buttons2, 7); // LZ2     Left Z2 Button  (Lower)

      // Parse Joysticks
      LeftJoystickLRLOW   = unsigned(bRxHost[4]);  // Left Joystick Left/Right  (Left = 1023, Center = 511, Right = 0)
      LeftJoystickLRHIGH  = unsigned(bRxHost[5]);  // Left Joystick Left/Right  (Left = 1023, Center = 511, Right = 0)
      LeftJoystickUDLOW   = unsigned(bRxHost[6]);  // Left Joystick Up/Down     (Up = 1023,   Center = 511, Down = 0)
      LeftJoystickUDHIGH  = unsigned(bRxHost[7]);  // Left Joystick Up/Down     (Up = 1023,   Center = 511, Down = 0)
      RightJoystickLRLOW  = unsigned(bRxHost[8]);  // Right Joystick Left/Right (Left = 1023, Center = 511, Right = 0)
      RightJoystickLRHIGH = unsigned(bRxHost[9]);  // Right Joystick Left/Right (Left = 1023, Center = 511, Right = 0)
      RightJoystickUDLOW  = unsigned(bRxHost[10]); // Right Joystick Up/Down    (Up = 1023,   Center = 511, Down = 0)
      RightJoystickUDHIGH = unsigned(bRxHost[11]); // Right Joystick Up/Down    (Up = 1023,   Center = 511, Down = 0)

      // Set Gate Interrupt Update Speed (How Fast Gate Steps are Executed)
      // Set Step Size Multiplier
      // Set Step Height Multiplier
      /*
      if (button1 == 1) {
        Timer1.initialize(TimerMS1);
        uStepSize  = 2;
      }
      else if (button2 == 1) {
        Timer1.initialize(TimerMS2);
        uStepSize  = 3;
      }
      else if (button3 == 1) {
        Timer1.initialize(TimerMS3);
        uStepSize  = 4;
      }
      if (button4 == 1) {
        Timer1.initialize(TimerMS4);
        uStepSize  = 5;
      }
      */
      // Step Commands in x-direction
      fdelx   = JoystickReshape(int(LeftJoystickUDLOW)  + int(LeftJoystickUDHIGH << 8)) * delxMult * float(uStepSize);

      // Step Commands in y-direction
      // Step Commands for yaw
      if (buttonJ1 == 1) {
        fdelyaw = 0;
        fdely   = -JoystickReshape(int(LeftJoystickLRLOW)  + int(LeftJoystickLRHIGH << 8)) * delyMult * float(uStepSize);;
      }
      else
      {
        fdelyaw = -JoystickReshape(int(LeftJoystickLRLOW)  + int(LeftJoystickLRHIGH << 8)) * delyawMult * float(uStepSize);
        fdely   = 0;
      }
      /*
      // Turn On/Off Laser
      if (buttonRZ1 == 1) {
        digitalWrite(laserPin, LOW);
      }
      if (buttonRZ2 == 1) {
        digitalWrite(laserPin, HIGH);
      }
      */
      // Body Pitch Adjustment
      float RightJoystickUD = float(int(RightJoystickUDLOW)  + int(RightJoystickUDHIGH << 8) - 512) / 512;
      if ((RightJoystickUD > .25) && (fPitch < PitchMax))
      {
        fPitch = fPitch + PitchRate;
      }
      if ((RightJoystickUD < -.25) && (fPitch > PitchMin))
      {
        fPitch = fPitch - PitchRate;
      }

      // Body Yaw Adjustment
      float RightJoystickLR = float(int(RightJoystickLRLOW)  + int(RightJoystickLRHIGH << 8) - 512) / 512;
      if ((RightJoystickLR > .25) && (fYaw > YawMin))
      {
        fYaw = fYaw - YawRate;
      }
      if ((RightJoystickLR < -.25) && (fYaw < YawMax))
      {
        fYaw = fYaw + YawRate;
      }

      // Body Height Adjustment
      if ((buttonUp == 1) && (fHeight < HeightMax)) {
        fHeight =  fHeight + HeightRate;
      }
      if ((buttonDown == 1) && ((fHeight > HeightMin))) {
        fHeight =  fHeight - HeightRate;
      }

      // Step Height Adjustment
      if ((buttonLZ1 == 1) && (StepHeight < StepHeightMax)) {
        StepHeight =  StepHeight + StepHeightRate;
      }
      if ((buttonLZ2 == 1) && ((StepHeight > StepHeightMin))) {
        StepHeight =  StepHeight - StepHeightRate;
      }

      // Reset to Home Conditions
      if (buttonSelect == 1)
      {
        fHeight    = 0;
        fPitch     = 0;
        fYaw       = 0;
        StepHeight = StepHeightDefault;
      }

    }
    else {
      Serial2.flush();
    }
  }
}

/*********************************************************************************************
 * Reshape Analog Joystick for Deadband at Zero
 *********************************************************************************************/
float JoystickReshape(int x) {
  float y;
  y = pow((float(x) - 512) / 512, 3);
  if (abs(y) <= .0025) {
    y = 0;
  }
  return y;
}

