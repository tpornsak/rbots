/* 
  ArbotiX Test Program for use with PyPose 0013
  Copyright (c) 2008-2010 Michael E. Ferguson.  All right reserved.

  This library is free software; you can redistribute it and/or
  modify it under the terms of the GNU Lesser General Public
  License as published by the Free Software Foundation; either
  version 2.1 of the License, or (at your option) any later version.

  This library is distributed in the hope that it will be useful,
  but WITHOUT ANY WARRANTY; without even the implied warranty of
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
  Lesser General Public License for more details.

  You should have received a copy of the GNU Lesser General Public
  License along with this library; if not, write to the Free Software
  Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
*/
 
//#include <ax12.h>
//#include <BioloidController.h>
//#include <Motors2.h>

int ax12GetRegister(int id, int regstart, int length);
int ax12ReadPacket(int length);

//BioloidController bioloid = BioloidController(1000000);
//Motors2 drive = Motors2();

#define ARB_SIZE_POSE   7  // also initializes
#define ARB_LOAD_POSE   8
#define ARB_LOAD_SEQ    9
#define ARB_PLAY_SEQ    10
#define ARB_LOOP_SEQ    11
#define ARB_TEST        25
#define LED_STATUS      13

// START AX-12 defines
#define AX12_MAX_SERVOS             18
#define AX12_BUFFER_SIZE            32

/** EEPROM AREA **/
#define AX_MODEL_NUMBER_L           0
#define AX_MODEL_NUMBER_H           1
#define AX_VERSION                  2
#define AX_ID                       3
#define AX_BAUD_RATE                4
#define AX_RETURN_DELAY_TIME        5
#define AX_CW_ANGLE_LIMIT_L         6
#define AX_CW_ANGLE_LIMIT_H         7
#define AX_CCW_ANGLE_LIMIT_L        8
#define AX_CCW_ANGLE_LIMIT_H        9
#define AX_SYSTEM_DATA2             10
#define AX_LIMIT_TEMPERATURE        11
#define AX_DOWN_LIMIT_VOLTAGE       12
#define AX_UP_LIMIT_VOLTAGE         13
#define AX_MAX_TORQUE_L             14
#define AX_MAX_TORQUE_H             15
#define AX_RETURN_LEVEL             16
#define AX_ALARM_LED                17
#define AX_ALARM_SHUTDOWN           18
#define AX_OPERATING_MODE           19
#define AX_DOWN_CALIBRATION_L       20
#define AX_DOWN_CALIBRATION_H       21
#define AX_UP_CALIBRATION_L         22
#define AX_UP_CALIBRATION_H         23
/** RAM AREA **/
#define AX_TORQUE_ENABLE            24
#define AX_LED                      25
#define AX_CW_COMPLIANCE_MARGIN     26
#define AX_CCW_COMPLIANCE_MARGIN    27
#define AX_CW_COMPLIANCE_SLOPE      28
#define AX_CCW_COMPLIANCE_SLOPE     29
#define AX_GOAL_POSITION_L          30
#define AX_GOAL_POSITION_H          31
#define AX_GOAL_SPEED_L             32
#define AX_GOAL_SPEED_H             33
#define AX_TORQUE_LIMIT_L           34
#define AX_TORQUE_LIMIT_H           35
#define AX_PRESENT_POSITION_L       36
#define AX_PRESENT_POSITION_H       37
#define AX_PRESENT_SPEED_L          38
#define AX_PRESENT_SPEED_H          39
#define AX_PRESENT_LOAD_L           40
#define AX_PRESENT_LOAD_H           41
#define AX_PRESENT_VOLTAGE          42
#define AX_PRESENT_TEMPERATURE      43
#define AX_REGISTERED_INSTRUCTION   44
#define AX_PAUSE_TIME               45
#define AX_MOVING                   46
#define AX_LOCK                     47
#define AX_PUNCH_L                  48
#define AX_PUNCH_H                  49
/** Status Return Levels **/
#define AX_RETURN_NONE              0
#define AX_RETURN_READ              1
#define AX_RETURN_ALL               2
/** Instruction Set **/
#define AX_PING                     1
#define AX_READ_DATA                2
#define AX_WRITE_DATA               3
#define AX_REG_WRITE                4
#define AX_ACTION                   5
#define AX_RESET                    6
#define AX_SYNC_WRITE               131

/** AX-S1 **/
#define AX_LEFT_IR_DATA             26
#define AX_CENTER_IR_DATA           27
#define AX_RIGHT_IR_DATA            28
#define AX_LEFT_LUMINOSITY          29
#define AX_CENTER_LUMINOSITY        30
#define AX_RIGHT_LUMINOSITY         31
#define AX_OBSTACLE_DETECTION       32
#define AX_BUZZER_INDEX             40

//unsigned char ax_rx_buffer[AX12_BUFFER_SIZE];
/// END AX-12 defines

/** read back the error code for our latest packet read */
int ax12Error;
/** > 0 = success */

int mode = 0;              // where we are in the frame
int xmtEn = 31;

unsigned char id = 0;      // id of this frame
unsigned char length = 0;  // length of this frame
unsigned char ins = 0;     // instruction of this frame

unsigned char params[50];  // parameters
unsigned char index = 0;   // index in param buffer

int checksum;              // checksum

typedef struct{
    unsigned char pose;    // index of pose to transition to 
    int time;              // time for transition
} sp_trans_t;

//  pose and sequence storage
int poses[30][30];         // poses [index][servo_id-1]
sp_trans_t sequence[50];   // sequence
int seqPos;                // step in current sequence

void setup(){
    Serial.begin(38400);   
    Serial3.begin(1000000);  // serial port to talk to AX-12 
    pinMode(LED_STATUS,OUTPUT);     // status LED
    pinMode(xmtEn, OUTPUT); // output enables half duplex transmit buffer
}

/* 
 * packet: ff ff id length ins params checksum
 *   same as ax-12 table, except, we define new instructions for Arbotix
 *
 * ID = 253 for these special commands!
 * Pose Size = 7, followed by single param: size of pose
 * Load Pose = 8, followed by index, then pose positions (# of param = 2*pose_size)
 * Seq Size = 9, followed by single param: size of seq
 * Load Seq = A, followed by index/times (# of parameters = 3*seq_size) 
 * Play Seq = B, no params
 * Loop Seq = C, 
 */

void loop(){
    int i;
    
    // process messages
    while(Serial.available() > 0){
        // We need to 0xFF at start of packet
        if(mode == 0){         // start of new packet
            if(Serial.read() == 0xff){
                mode = 2;
                digitalWrite(LED_STATUS,HIGH-digitalRead(LED_STATUS));
            }
        //}else if(mode == 1){   // another start byte
        //    if(Serial.read() == 0xff)
        //        mode = 2;
        //    else
        //        mode = 0;
        }else if(mode == 2){   // next byte is index of servo
            id = Serial.read();    
            if(id != 0xff)
                mode = 3;
        }else if(mode == 3){   // next byte is length
            length = Serial.read();
            checksum = id + length;
            mode = 4;
        }else if(mode == 4){   // next byte is instruction
            ins = Serial.read();
            checksum += ins;
            index = 0;
            mode = 5;
        }else if(mode == 5){   // read data in 
            params[index] = Serial.read();
            checksum += (int) params[index];
            index++;
            if(index + 1 == length){  // we've read params & checksum
                mode = 0;
                if((checksum%256) != 255){ 
                    // return a packet: FF FF id Len Err params=None check
                    Serial.print(0xff,BYTE);
                    Serial.print(0xff,BYTE);
                    Serial.print(id,BYTE);
                    Serial.print(2,BYTE);
                    Serial.print(64,BYTE);
                    Serial.print(255-((66+id)%256),BYTE);
                }else{
                    if(id == 253){
                        // return a packet: FF FF id Len Err params=None check
                        Serial.print(0xff,BYTE);
                        Serial.print(0xff,BYTE);
                        Serial.print(id,BYTE);
                        Serial.print(2,BYTE);
                        Serial.print(0,BYTE);
                        Serial.print(255-((2+id)%256),BYTE);
                        // special ArbotiX instructions
                        // Pose Size = 7, followed by single param: size of pose
                        // Load Pose = 8, followed by index, then pose positions (# of param = 2*pose_size)
                        // Load Seq = 9, followed by index/times (# of parameters = 3*seq_size) 
                        // Play Seq = A, no params
                        if(ins == ARB_SIZE_POSE){
                            //BEN bioloid.poseSize = params[0];
                            //BEN bioloid.readPose();    
                            //Serial.println(bioloid.poseSize);
                            int i = 0;
                        }else if(ins == ARB_LOAD_POSE){
                            int i;    
                            Serial.print("New Pose:");
                           // BEN for(i=0; i<bioloid.poseSize; i++){
                           // BEN     poses[params[0]][i] = params[(2*i)+1]+(params[(2*i)+2]<<8); 
                                //Serial.print(poses[params[0]][i]);
                                //Serial.print(",");     
                           // BEN  } 
                            Serial.println("");
                        }else if(ins == ARB_LOAD_SEQ){
                            int i;
                            for(i=0;i<(length-2)/3;i++){
                                sequence[i].pose = params[(i*3)];
                                sequence[i].time = params[(i*3)+1] + (params[(i*3)+2]<<8);
                                //Serial.print("New Transition:");
                                //Serial.print((int)sequence[i].pose);
                                //Serial.print(" in ");
                                //Serial.println(sequence[i].time);      
                            }
                        }else if(ins == ARB_PLAY_SEQ){
                            seqPos = 0;
                            while(sequence[seqPos].pose != 0xff){
                                int i;
                                int p = sequence[seqPos].pose;
                                // are we HALT?
                                if(Serial.read() == 'H') return;
                                // load pose
                                // BEN  for(i=0; i<bioloid.poseSize; i++){
                                // BEN     bioloid.setNextPose(i+1,poses[p][i]);
                                // BEN} 
                                // interpolate
                               // BEN  bioloid.interpolateSetup(sequence[seqPos].time);
                               // BEN  while(bioloid.interpolating)
                               // BEN      bioloid.interpolateStep();
                                // next transition
                                seqPos++;
                            }
                        }else if(ins == ARB_LOOP_SEQ){
                            while(1){
                                seqPos = 0;
                                while(sequence[seqPos].pose != 0xff){
                                    int i;
                                    int p = sequence[seqPos].pose;
                                    // are we HALT?
                                    if(Serial.read() == 'H') return;
                                    // load pose
                                   // BEN  for(i=0; i<bioloid.poseSize; i++){
                                   // BEN      bioloid.setNextPose(i+1,poses[p][i]);
                                   // BEN  } 
                                    // interpolate
                                   // BEN  bioloid.interpolateSetup(sequence[seqPos].time);
                                   // BEN  while(bioloid.interpolating)
                                   // BEN      bioloid.interpolateStep();
                                    // next transition
                                    seqPos++;
                                }
                            }
                        }else if(ins == ARB_TEST){
                            int i;
                            // Test Digital I/O
                            for(i=0;i<8;i++){
                                // test digital
                                pinMode(i,OUTPUT);
                                digitalWrite(i,HIGH);  
                                // test analog
                                pinMode(31-i,OUTPUT);
                                digitalWrite(31-i,HIGH);
                                
                                delay(500);
                                digitalWrite(i,LOW);
                                digitalWrite(31-i,LOW);
                            }
                            // Test Ax-12
                            for(i=452;i<552;i+=20){
                            // BEN     SetPosition(1,i);
                                delay(200);
                            }
                            // Test Motors
                            // BEN drive.set(-255,-255);
                            delay(500);
                            // BEN drive.set(0,0);
                            delay(1500);
                           // BEN  drive.set(255,255);
                            delay(500);
                            // BEN drive.set(0,0);
                            delay(1500);
                            // Test Analog I/O
                            for(i=0;i<8;i++){
                                // test digital
                                pinMode(i,OUTPUT);
                                digitalWrite(i,HIGH);  
                                // test analog
                                pinMode(31-i,OUTPUT);
                                digitalWrite(31-i,HIGH);
                                
                                delay(500);
                                digitalWrite(i,LOW);
                                digitalWrite(31-i,LOW);
                            }
                        }   
                    }else{
                        int i;
                        // pass thru
                       if(ins == AX_READ_DATA){
                            int i;
                            ax12GetRegister(id, params[0], params[1]);
                            // return a packet: FF FF id Len Err params check
                            // BENif(ax_rx_buffer[3] > 0){
                            // BENfor(i=0;i<ax_rx_buffer[3]+4;i++)
                            // BEN    Serial.print(ax_rx_buffer[i],BYTE);
                            // BEN}
                            // BENax_rx_buffer[3] = 0;
                        }else if(ins == AX_WRITE_DATA){
                            if(length == 4){
                            // BEN     ax12SetRegister(id, params[0], params[1]);
                              length = 4;  // BEN do nothing here
                            }else{
                                int x = params[1] + (params[2]<<8);
                            // BEN     ax12SetRegister2(id, params[0], x);
                            }
                            // return a packet: FF FF id Len Err params check
                            Serial.print(0xff,BYTE);
                            Serial.print(0xff,BYTE);
                            Serial.print(id,BYTE);
                            Serial.print(2,BYTE);
                            Serial.print(0,BYTE);
                            Serial.print(255-((2+id)%256),BYTE);
                        }
                    }
                }
            }
        }
    }
    
    // update joints
    // BEN bioloid.interpolateStep();
}

/** Read register value(s) */
int ax12GetRegister(int id, int regstart, int length){  
    //setTX(id);
    // 0xFF 0xFF ID LENGTH INSTRUCTION PARAM... CHECKSUM    
    int checksum = ~((id + 6 + regstart + length)%256);
    digitalWrite(xmtEn, HIGH); // enable transmit buffer
    Serial3.write(0xFF); // ax12writeB(0xFF);
    Serial3.write(0xFF); //ax12writeB(0xFF);
    Serial3.write(id); // ax12writeB(id);
    Serial3.write(4);  //ax12writeB(4);    // length
    Serial3.write(AX_READ_DATA); // ax12writeB(AX_READ_DATA);
    Serial3.write(regstart);  //ax12writeB(regstart);
    Serial3.write(length);  //ax12writeB(length);
    Serial3.write(checksum);  // ax12writeB(checksum);  
    digitalWrite(xmtEn, LOW); // enable receive buffer
    //setRX(id);    
    if(ax12ReadPacket(length + 6) > 0){
        ax12Error = ax_rx_buffer[4];
        if(length == 1)
            return ax_rx_buffer[5];
        else
            return ax_rx_buffer[5] + (ax_rx_buffer[6]<<8);
    }else{
        return -1;
    }
}

int ax12ReadPacket(int length){
    unsigned long ulCounter;
    unsigned char offset, blength, checksum, timeout;
    unsigned char volatile bcount; 

    offset = 0;
    timeout = 0;
    bcount = 0;
    while(bcount < length){
        ulCounter = 0;
        while((bcount + offset) == ax_rx_int_Pointer){
            if(ulCounter++ > 1000L){ // was 3000
                timeout = 1;
                break;
            }
        }
        if(timeout) break;
        ax_rx_buffer[bcount] = ax_rx_int_buffer[bcount + offset];
        if((bcount == 0) && (ax_rx_buffer[0] != 0xff))
            offset++;
        else
            bcount++;
    }

    blength = bcount;
    checksum = 0;
    for(offset=2;offset<bcount;offset++)
        checksum += ax_rx_buffer[offset];
    if((checksum%256) != 255){
        return 0;
    }else{
        return 1;
    }
}
