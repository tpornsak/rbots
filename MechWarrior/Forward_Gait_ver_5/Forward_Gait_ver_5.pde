/*
  Transmits serial data to all 4 legs to walk forward,
 These joint angles use the more vertical leg configuration
 to prevent the weight from back driving the motors and uses less battery current
 
 The circuit:
 * pin 4 turns on transmission when pulled high.
 
 Created 4 Jan 2011
 By John Harkey
 
 */

int xmtEn = 2;
int inPin  =  4;// weight on foot sensor
int val = 0;
int i = 0;
int led1 =  16;    // LED connected to digital pin 16
int led2 =  17;    // LED connected to digital pin 17
int led3 =  18;    // LED connected to digital pin 18
int led4 =  19;    // LED connected to digital pin 19
//Byte declarations
byte ID2 = 0x02;
byte ID3 = 0x03;
byte ID4 = 0x04;
byte ID5 = 0x05;
byte ID6 = 0x06;
byte ID7 = 0x07;
byte ID8 = 0x08;
byte ID9 = 0x09;
byte IDa = 0x0a;
byte IDb = 0x0b;
byte IDc = 0x0c;
byte IDd = 0x0d;
byte Lgth4 = 0x04;
byte Lgth5 = 0x05;
byte Wr_Cmd = 0x03;
byte Pos_Addr = 0x1E;
byte TqEn_Addr = 0x18;
byte Speed_Addr = 0x20;
//Array declarations

// The setup() method runs once, when the sketch starts
void setup()   {                
  // initialize the digital pin as an output:
  pinMode(inPin,  INPUT);
  pinMode(xmtEn,  OUTPUT);
  pinMode(led1, OUTPUT);  
  pinMode(led2, OUTPUT);
  pinMode(led3, OUTPUT);  
  pinMode(led4, OUTPUT);
  Serial.begin(1000000);

  Leg_TqEn();  //Calls function to enable Left Front Leg motors to produce torque
  Leg_speed(0x1f,0x00);  //Calls function to set speed of Left Front Leg motors
  //7f=1/8 speed, ff=1/4 speed, 1ff=1/2 speed, 3ff=full speed

  //Calls functions to set initial stance
  delay(1000);
  LF_leg_pos(0xa9,0x02,0x0a,0x01,0x0a,0x01);  //hip forward, knee down, ankle up, stance
  LB_leg_pos(0x55,0x01,0xf5,0x02,0xf5,0x02);  //hip back, knee down, ankle up, stance
  RF_leg_pos(0x55,0x01,0xf5,0x02,0xf5,0x02);  //hip forward, knee down, ankle up, stance
  RB_leg_pos(0xa9,0x02,0x0a,0x01,0x0a,0x01);  //hip back, knee down, ankle up, stance
  delay(7000);
  Cylon_LEDs();  //Calls function to scan LEDs in a Cylon pattern

  LF_leg_pos(0xa9,0x02,0x0a,0x01,0x0a,0x01);  //hip forward, knee down, ankle up, stance
  LB_leg_pos(0x55,0x01,0xf5,0x02,0xf5,0x02);  //hip back, knee down, ankle up, stance
  RF_leg_pos(0x55,0x01,0xf5,0x02,0xf5,0x02);  //hip forward, knee down, ankle up, stance
  RB_leg_pos(0xa9,0x02,0x0a,0x01,0x0a,0x01);  //hip back, knee down, ankle up, stance
  delay(500);
  
  Leg_speed(0xff,0x00);  //Calls function to set speed of Left Front Leg motors
  //7f=1/8 speed, ff=1/4 speed, 1ff=1/2 speed, 3ff=full speed
}

void loop()                     
{
  val = digitalRead(inPin);
  if (val != 0)
  { 
    //Calls functions to set positions for forward walk
    //cycle 1
        
    LF_leg_pos(0xd2,0x01,0xee,0x00,0x0a,0x01);  //hip back, knee down, ankle up
    LB_leg_pos(0x2c,0x02,0x66,0x02,0x88,0x02);  //hip forward, knee up 288, ankle down
    RF_leg_pos(0x55,0x01,0x10,0x03,0xf5,0x02);  //hip forward, knee down, ankle up, stance
    RB_leg_pos(0xa9,0x02,0xee,0x00,0x0a,0x01);  //hip back, knee down, ankle up, stance

    delay(500);
    //cycle 2
    LF_leg_pos(0xd2,0x01,0xee,0x00,0x0a,0x01);  //hip back, knee down, ankle up
    LB_leg_pos(0x2c,0x02,0x10,0x03,0xf5,0x02);  //hip forward, knee down, ankle up
    RF_leg_pos(0x55,0x01,0x10,0x03,0xf5,0x02);  //hip forward, knee down, ankle up, stance
    RB_leg_pos(0xa9,0x02,0xee,0x00,0x0a,0x01);  //hip back, knee down, ankle up, stance

    delay(500);
    //cycle 3
    LF_leg_pos(0xa9,0x02,0x55,0x01,0x77,0x01);  //hip forward, knee up 177, ankle down
    LB_leg_pos(0x2c,0x02,0x10,0x03,0xf5,0x02);  //hip forward, knee down, ankle up
    RF_leg_pos(0x55,0x01,0x10,0x03,0xf5,0x02);  //hip forward, knee down, ankle up, stance
    RB_leg_pos(0xa9,0x02,0xee,0x00,0x0a,0x01);  //hip back, knee down, ankle up, stance

    delay(500);
    //cycle 4
    LF_leg_pos(0xa9,0x02,0xee,0x00,0x0a,0x01);  //hip forward, knee down, ankle up, stance
    LB_leg_pos(0x55,0x01,0x10,0x03,0xf5,0x02);  //hip back, knee down, ankle up, stance
    RF_leg_pos(0x2c,0x02,0x10,0x03,0xf5,0x02);  //hip back, knee down, ankle up
    RB_leg_pos(0xd2,0x01,0x55,0x01,0x77,0x01);  //hip forward, knee up 177, ankle down

    delay(500);
    //cycle 5
    LF_leg_pos(0xa9,0x02,0xee,0x00,0x0a,0x01);  //hip forward, knee down, ankle up, stance
    LB_leg_pos(0x55,0x01,0x10,0x03,0xf5,0x02);  //hip back, knee down, ankle up, stance
    RF_leg_pos(0x2c,0x02,0x10,0x03,0xf5,0x02);  //hip back, knee down, ankle up
    RB_leg_pos(0xd2,0x01,0xee,0x00,0x0a,0x01);  //hip forward, knee down, ankle up

    delay(500);
    //cycle 6
    LF_leg_pos(0xa9,0x02,0xee,0x00,0x0a,0x01);  //hip forward, knee down, ankle up, stance
    LB_leg_pos(0x55,0x01,0x10,0x03,0xf5,0x02);  //hip back, knee down, ankle up, stance
    RF_leg_pos(0x55,0x01,0x66,0x02,0x88,0x02);  //hip forward, knee up 288, ankle down
    RB_leg_pos(0xd2,0x01,0xee,0x00,0x0a,0x01);  //hip forward, knee down, ankle up

    delay(500);
  }
  else
  {
    delay(500);                // wait for a half second
  }
}

//Function, sends position commands to Left Front Leg motors; LF_HIP, LF_KNEE, LF_ANKLE
void LF_leg_pos(byte poslo2, byte poshi2, byte poslo3, byte poshi3, byte poslo4, byte poshi4)
{
  //Position motor 2, 3 & 4 command
  byte CS_Pos_m2 = ~(ID2 + Lgth5 + Wr_Cmd + Pos_Addr + poslo2 + poshi2);
  byte CS_Pos_m3 = ~(ID3 + Lgth5 + Wr_Cmd + Pos_Addr + poslo3 + poshi3);
  byte CS_Pos_m4 = ~(ID4 + Lgth5 + Wr_Cmd + Pos_Addr + poslo4 + poshi4);
  byte Pos_m2[9] = {
    0xff,0xff,ID2,Lgth5,Wr_Cmd,Pos_Addr,poslo2,poshi2,CS_Pos_m2                          }; // position command array
  byte Pos_m3[9] = {
    0xff,0xff,ID3,Lgth5,Wr_Cmd,Pos_Addr,poslo3,poshi3,CS_Pos_m3                          }; // position command array
  byte Pos_m4[9] = {
    0xff,0xff,ID4,Lgth5,Wr_Cmd,Pos_Addr,poslo4,poshi4,CS_Pos_m4                          }; // position command array
  digitalWrite(xmtEn, HIGH);    //Enables transmit buffer
  for (int i=0; i<9; i++)
    Serial.write(Pos_m2[i]);    //Sends position command to servo motor
  for (int i=0; i<9; i++)
    Serial.write(Pos_m3[i]);    //Sends position command to servo motor
  for (int i=0; i<9; i++)
    Serial.write(Pos_m4[i]);    //Sends position command to servo motor
  return;
}

//Function, sends position commands to Left back Leg motors; LB_HIP, LB_KNEE, LB_ANKLE
void LB_leg_pos(byte poslo5, byte poshi5, byte poslo6, byte poshi6, byte poslo7, byte poshi7)
{
  //Position motor 5, 6 & 7 command
  byte CS_Pos_m5 = ~(ID5 + Lgth5 + Wr_Cmd + Pos_Addr + poslo5 + poshi5);
  byte CS_Pos_m6 = ~(ID6 + Lgth5 + Wr_Cmd + Pos_Addr + poslo6 + poshi6);
  byte CS_Pos_m7 = ~(ID7 + Lgth5 + Wr_Cmd + Pos_Addr + poslo7 + poshi7);
  byte Pos_m5[9] = {
    0xff,0xff,ID5,Lgth5,Wr_Cmd,Pos_Addr,poslo5,poshi5,CS_Pos_m5                  }; // position command array
  byte Pos_m6[9] = {
    0xff,0xff,ID6,Lgth5,Wr_Cmd,Pos_Addr,poslo6,poshi6,CS_Pos_m6                  }; // position command array
  byte Pos_m7[9] = {
    0xff,0xff,ID7,Lgth5,Wr_Cmd,Pos_Addr,poslo7,poshi7,CS_Pos_m7                  }; // position command array
  digitalWrite(xmtEn, HIGH);    //Enables transmit buffer
  for (int i=0; i<9; i++)
    Serial.write(Pos_m5[i]);    //Sends position command to servo motor
  for (int i=0; i<9; i++)
    Serial.write(Pos_m6[i]);    //Sends position command to servo motor
  for (int i=0; i<9; i++)
    Serial.write(Pos_m7[i]);    //Sends position command to servo motor
  return;
}

//Function, sends position commands to Right Front Leg motors; RF_HIP, RF_KNEE, RF_ANKLE
void RB_leg_pos(byte poslo8, byte poshi8, byte poslo9, byte poshi9, byte posloa, byte poshia)
{
  //Position motor 8, 9 & a command
  byte CS_Pos_m8 = ~(ID8 + Lgth5 + Wr_Cmd + Pos_Addr + poslo8 + poshi8);
  byte CS_Pos_m9 = ~(ID9 + Lgth5 + Wr_Cmd + Pos_Addr + poslo9 + poshi9);
  byte CS_Pos_ma = ~(IDa + Lgth5 + Wr_Cmd + Pos_Addr + posloa + poshia);
  byte Pos_m8[9] = {
    0xff,0xff,ID8,Lgth5,Wr_Cmd,Pos_Addr,poslo8,poshi8,CS_Pos_m8                          }; // position command array
  byte Pos_m9[9] = {
    0xff,0xff,ID9,Lgth5,Wr_Cmd,Pos_Addr,poslo9,poshi9,CS_Pos_m9                          }; // position command array
  byte Pos_ma[9] = {
    0xff,0xff,IDa,Lgth5,Wr_Cmd,Pos_Addr,posloa,poshia,CS_Pos_ma                          }; // position command array
  digitalWrite(xmtEn, HIGH);    //Enables transmit buffer
  for (int i=0; i<9; i++)
    Serial.write(Pos_m8[i]);    //Sends position command to servo motor
  for (int i=0; i<9; i++)
    Serial.write(Pos_m9[i]);    //Sends position command to servo motor
  for (int i=0; i<9; i++)
    Serial.write(Pos_ma[i]);    //Sends position command to servo motor
  return;
}

//Function, sends position commands to Right Back Leg motors; RB_HIP, RB_KNEE, RB_ANKLE
void RF_leg_pos(byte poslob, byte poshib, byte posloc, byte poshic, byte poslod, byte poshid)
{
  //Position motor b, c & d command
  byte CS_Pos_mb = ~(IDb + Lgth5 + Wr_Cmd + Pos_Addr + poslob + poshib);
  byte CS_Pos_mc = ~(IDc + Lgth5 + Wr_Cmd + Pos_Addr + posloc + poshic);
  byte CS_Pos_md = ~(IDd + Lgth5 + Wr_Cmd + Pos_Addr + poslod + poshid);
  byte Pos_mb[9] = {
    0xff,0xff,IDb,Lgth5,Wr_Cmd,Pos_Addr,poslob,poshib,CS_Pos_mb                          }; // position command array
  byte Pos_mc[9] = {
    0xff,0xff,IDc,Lgth5,Wr_Cmd,Pos_Addr,posloc,poshic,CS_Pos_mc                          }; // position command array
  byte Pos_md[9] = {
    0xff,0xff,IDd,Lgth5,Wr_Cmd,Pos_Addr,poslod,poshid,CS_Pos_md                          }; // position command array
  digitalWrite(xmtEn, HIGH);    //Enables transmit buffer
  for (int i=0; i<9; i++)
    Serial.write(Pos_mb[i]);    //Sends position command to servo motor
  for (int i=0; i<9; i++)
    Serial.write(Pos_mc[i]);    //Sends position command to servo motor
  for (int i=0; i<9; i++)
    Serial.write(Pos_md[i]);    //Sends position command to servo motor
  return;
}

void Leg_TqEn()  //Function, sends torque enable commands to all leg motors
{
  //Torque enable all motors
  byte CS_TqEn_m2 = ~(ID2 + Lgth4 + Wr_Cmd + TqEn_Addr + 0x01);
  byte CS_TqEn_m3 = ~(ID3 + Lgth4 + Wr_Cmd + TqEn_Addr + 0x01);
  byte CS_TqEn_m4 = ~(ID4 + Lgth4 + Wr_Cmd + TqEn_Addr + 0x01);
  byte CS_TqEn_m5 = ~(ID5 + Lgth4 + Wr_Cmd + TqEn_Addr + 0x01);
  byte CS_TqEn_m6 = ~(ID6 + Lgth4 + Wr_Cmd + TqEn_Addr + 0x01);
  byte CS_TqEn_m7 = ~(ID7 + Lgth4 + Wr_Cmd + TqEn_Addr + 0x01);
  byte CS_TqEn_m8 = ~(ID8 + Lgth4 + Wr_Cmd + TqEn_Addr + 0x01);
  byte CS_TqEn_m9 = ~(ID9 + Lgth4 + Wr_Cmd + TqEn_Addr + 0x01);
  byte CS_TqEn_ma = ~(IDa + Lgth4 + Wr_Cmd + TqEn_Addr + 0x01);
  byte CS_TqEn_mb = ~(IDb + Lgth4 + Wr_Cmd + TqEn_Addr + 0x01);
  byte CS_TqEn_mc = ~(IDc + Lgth4 + Wr_Cmd + TqEn_Addr + 0x01);
  byte CS_TqEn_md = ~(IDd + Lgth4 + Wr_Cmd + TqEn_Addr + 0x01);
  byte TqEn_m2[8] = {
    0xff,0xff,ID2,Lgth4,Wr_Cmd,TqEn_Addr,0x01,CS_TqEn_m2                          };  //Torque Enable Motor 2 packet
  byte TqEn_m3[8] = {
    0xff,0xff,ID3,Lgth4,Wr_Cmd,TqEn_Addr,0x01,CS_TqEn_m3                          };  //Torque Enable Motor 3 packet  
  byte TqEn_m4[8] = {
    0xff,0xff,ID4,Lgth4,Wr_Cmd,TqEn_Addr,0x01,CS_TqEn_m4                          };  //Torque Enable Motor 4 packet
  byte TqEn_m5[8] = {
    0xff,0xff,ID5,Lgth4,Wr_Cmd,TqEn_Addr,0x01,CS_TqEn_m5                          };  //Torque Enable Motor 5 packet
  byte TqEn_m6[8] = {
    0xff,0xff,ID6,Lgth4,Wr_Cmd,TqEn_Addr,0x01,CS_TqEn_m6                          };  //Torque Enable Motor 6 packet  
  byte TqEn_m7[8] = {
    0xff,0xff,ID7,Lgth4,Wr_Cmd,TqEn_Addr,0x01,CS_TqEn_m7                          };  //Torque Enable Motor 7 packet 
  byte TqEn_m8[8] = {
    0xff,0xff,ID8,Lgth4,Wr_Cmd,TqEn_Addr,0x01,CS_TqEn_m8                          };  //Torque Enable Motor 8 packet
  byte TqEn_m9[8] = {
    0xff,0xff,ID9,Lgth4,Wr_Cmd,TqEn_Addr,0x01,CS_TqEn_m9                          };  //Torque Enable Motor 9 packet  
  byte TqEn_ma[8] = {
    0xff,0xff,IDa,Lgth4,Wr_Cmd,TqEn_Addr,0x01,CS_TqEn_ma                          };  //Torque Enable Motor a packet
  byte TqEn_mb[8] = {
    0xff,0xff,IDb,Lgth4,Wr_Cmd,TqEn_Addr,0x01,CS_TqEn_mb                          };  //Torque Enable Motor b packet
  byte TqEn_mc[8] = {
    0xff,0xff,IDc,Lgth4,Wr_Cmd,TqEn_Addr,0x01,CS_TqEn_mc                          };  //Torque Enable Motor c packet  
  byte TqEn_md[8] = {
    0xff,0xff,IDd,Lgth4,Wr_Cmd,TqEn_Addr,0x01,CS_TqEn_md                          };  //Torque Enable Motor d packet
  //Enable all motors
  digitalWrite(xmtEn, HIGH); //Enables transmit buffer
  for (int i=0; i<8; i++)
    Serial.write(TqEn_m2[i]); //Sends motor enable packet
  for (int i=0; i<8; i++)
    Serial.write(TqEn_m3[i]); //Sends motor enable packet
  for (int i=0; i<8; i++)
    Serial.write(TqEn_m4[i]); //Sends motor enable packet
  for (int i=0; i<8; i++)
    Serial.write(TqEn_m5[i]); //Sends motor enable packet
  for (int i=0; i<8; i++)
    Serial.write(TqEn_m6[i]); //Sends motor enable packet
  for (int i=0; i<8; i++)
    Serial.write(TqEn_m7[i]); //Sends motor enable packet
  for (int i=0; i<8; i++)
    Serial.write(TqEn_m8[i]); //Sends motor enable packet
  for (int i=0; i<8; i++)
    Serial.write(TqEn_m9[i]); //Sends motor enable packet
  for (int i=0; i<8; i++)
    Serial.write(TqEn_ma[i]); //Sends motor enable packet
  for (int i=0; i<8; i++)
    Serial.write(TqEn_mb[i]); //Sends motor enable packet
  for (int i=0; i<8; i++)
    Serial.write(TqEn_mc[i]); //Sends motor enable packet
  for (int i=0; i<8; i++)
    Serial.write(TqEn_md[i]); //Sends motor enable packet
  delayMicroseconds(20);        //Need hold time on TqEn
  digitalWrite(xmtEn, LOW);     //Disables transmit buffer
  return;
}

void Leg_speed(byte spd_lo, byte spd_hi)  //Function, sends speed commands to all Leg motors
{
  //Speed of all leg motor commands
  byte CS_Spd_m2 = ~(ID2 + Lgth5 + Wr_Cmd + Speed_Addr + spd_lo + spd_hi);
  byte CS_Spd_m3 = ~(ID3 + Lgth5 + Wr_Cmd + Speed_Addr + spd_lo + spd_hi);
  byte CS_Spd_m4 = ~(ID4 + Lgth5 + Wr_Cmd + Speed_Addr + spd_lo + spd_hi);
  byte CS_Spd_m5 = ~(ID5 + Lgth5 + Wr_Cmd + Speed_Addr + spd_lo + spd_hi);
  byte CS_Spd_m6 = ~(ID6 + Lgth5 + Wr_Cmd + Speed_Addr + spd_lo + spd_hi);
  byte CS_Spd_m7 = ~(ID7 + Lgth5 + Wr_Cmd + Speed_Addr + spd_lo + spd_hi);
  byte CS_Spd_m8 = ~(ID8 + Lgth5 + Wr_Cmd + Speed_Addr + spd_lo + spd_hi);
  byte CS_Spd_m9 = ~(ID9 + Lgth5 + Wr_Cmd + Speed_Addr + spd_lo + spd_hi);
  byte CS_Spd_ma = ~(IDa + Lgth5 + Wr_Cmd + Speed_Addr + spd_lo + spd_hi);
  byte CS_Spd_mb = ~(IDb + Lgth5 + Wr_Cmd + Speed_Addr + spd_lo + spd_hi);
  byte CS_Spd_mc = ~(IDc + Lgth5 + Wr_Cmd + Speed_Addr + spd_lo + spd_hi);
  byte CS_Spd_md = ~(IDd + Lgth5 + Wr_Cmd + Speed_Addr + spd_lo + spd_hi);
  byte Spd_m2[9] = {
    0xff,0xff,ID2,Lgth5,Wr_Cmd,Speed_Addr,spd_lo,spd_hi,CS_Spd_m2                      }; // speed command array
  byte Spd_m3[9] = {
    0xff,0xff,ID3,Lgth5,Wr_Cmd,Speed_Addr,spd_lo,spd_hi,CS_Spd_m3                      }; // speed command array
  byte Spd_m4[9] = {
    0xff,0xff,ID4,Lgth5,Wr_Cmd,Speed_Addr,spd_lo,spd_hi,CS_Spd_m4                      }; // speed command array
  byte Spd_m5[9] = {
    0xff,0xff,ID5,Lgth5,Wr_Cmd,Speed_Addr,spd_lo,spd_hi,CS_Spd_m5                      }; // speed command array
  byte Spd_m6[9] = {
    0xff,0xff,ID6,Lgth5,Wr_Cmd,Speed_Addr,spd_lo,spd_hi,CS_Spd_m6                      }; // speed command array
  byte Spd_m7[9] = {
    0xff,0xff,ID7,Lgth5,Wr_Cmd,Speed_Addr,spd_lo,spd_hi,CS_Spd_m7                      }; // speed command array
  byte Spd_m8[9] = {
    0xff,0xff,ID8,Lgth5,Wr_Cmd,Speed_Addr,spd_lo,spd_hi,CS_Spd_m8                      }; // speed command array
  byte Spd_m9[9] = {
    0xff,0xff,ID9,Lgth5,Wr_Cmd,Speed_Addr,spd_lo,spd_hi,CS_Spd_m9                      }; // speed command array
  byte Spd_ma[9] = {
    0xff,0xff,IDa,Lgth5,Wr_Cmd,Speed_Addr,spd_lo,spd_hi,CS_Spd_ma                      }; // speed command array
  byte Spd_mb[9] = {
    0xff,0xff,IDb,Lgth5,Wr_Cmd,Speed_Addr,spd_lo,spd_hi,CS_Spd_mb                      }; // speed command array
  byte Spd_mc[9] = {
    0xff,0xff,IDc,Lgth5,Wr_Cmd,Speed_Addr,spd_lo,spd_hi,CS_Spd_mc                      }; // speed command array
  byte Spd_md[9] = {
    0xff,0xff,IDd,Lgth5,Wr_Cmd,Speed_Addr,spd_lo,spd_hi,CS_Spd_md                      }; // speed command array  
  digitalWrite(xmtEn, HIGH);    //Enables transmit buffer
  for (int i=0; i<9; i++)
    Serial.write(Spd_m2[i]);    //Sends speed command to servo motor
  for (int i=0; i<9; i++)
    Serial.write(Spd_m3[i]);    //Sends speed command to servo motor
  for (int i=0; i<9; i++)
    Serial.write(Spd_m4[i]);    //Sends speed command to servo motor
  for (int i=0; i<9; i++)
    Serial.write(Spd_m5[i]);    //Sends speed command to servo motor
  for (int i=0; i<9; i++)
    Serial.write(Spd_m6[i]);    //Sends speed command to servo motor
  for (int i=0; i<9; i++)
    Serial.write(Spd_m7[i]);    //Sends speed command to servo motor
  for (int i=0; i<9; i++)
    Serial.write(Spd_m8[i]);    //Sends speed command to servo motor
  for (int i=0; i<9; i++)
    Serial.write(Spd_m9[i]);    //Sends speed command to servo motor
  for (int i=0; i<9; i++)
    Serial.write(Spd_ma[i]);    //Sends speed command to servo motor
  for (int i=0; i<9; i++)
    Serial.write(Spd_mb[i]);    //Sends speed command to servo motor
  for (int i=0; i<9; i++)
    Serial.write(Spd_mc[i]);    //Sends speed command to servo motor
  for (int i=0; i<9; i++)
    Serial.write(Spd_md[i]);    //Sends speed command to servo motor 
  return;
}

void Cylon_LEDs()  //Function, cylon LED pattern
{
  //scans LEDs
  digitalWrite(led1, HIGH);   
  delay(100);                  
  digitalWrite(led1, LOW);    
  digitalWrite(led2, HIGH);   
  delay(100);                 
  digitalWrite(led2, LOW);   
  digitalWrite(led3, HIGH);
  delay(100); 
  digitalWrite(led3, LOW);
  digitalWrite(led4, HIGH);
  delay(100);
  digitalWrite(led3, HIGH);
  digitalWrite(led4, LOW);
  delay(100);                  
  digitalWrite(led2, HIGH);    
  digitalWrite(led3, LOW);
  delay(100);                  
  digitalWrite(led1, HIGH);    
  digitalWrite(led2, LOW);   
  delay(100);                 
  digitalWrite(led1, LOW);    
  delay(100);                 
  return;
}











