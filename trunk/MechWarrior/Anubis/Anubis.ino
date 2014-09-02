#include "Arduino-compatibles.h"

// Custom RobotAnubis Header Files
#include "DimensionsAnubis.h"
#include "KinematicsAnubis.h"
#include "GateAnubis.h" 

// Leg Definitions: #define LegLocation Dynamixel#
#define L11 1      // Leg 1, Joint 1 ID 
#define L12 2      // Leg 1, Joint 2 ID
#define L13 3      // Leg 1, Joint 3 ID
#define L21 4      // Leg 2, Joint 1 ID
#define L22 5      // Leg 2, Joint 2 ID
#define L23 6      // Leg 2, Joint 3 ID
#define L31 7      // Leg 3, Joint 1 ID
#define L32 8      // Leg 3, Joint 2 ID
#define L33 9      // Leg 3, Joint 3 ID
#define L41 10      // Leg 4, Joint 1 ID
#define L42 11      // Leg 4, Joint 2 ID
#define L43 12      // Leg 4, Joint 3 ID

// Timer Interrupt Period (usec)
#define TimerMS5 1000000                       // 1 Hz
#define TimerMS4 25000                         // 40 Hz
#define TimerMS3 33333                         // 30 Hz
#define TimerMS2 33333                         // 30 Hz
#define TimerMS1 80000                         // 21.5 Hz

HardwareTimer Timer(1);

boolean ISRState = false;                      // ISR Interrupt Flag


// Function Protoypes
void DynamixelPosTx();
void HostComm();

/*
Anubis Motor limits ID: min, max
1: 207, 807
2: 307, 656
3: 107, 531
4: 207, 807
5: 307, 656
6: 107, 531
7: 285, 807
8: 307, 656
9: 107, 531
10: 207, 807
11: 307, 656
12: 107, 531
*/

//Dynamixel Dxl(1); //Dynamixel on Serial1(USART1)

byte motorID = 9;
byte torqEn = 0;
word currPosition = 0;
volatile int nCount=0;


void setup() {
   // Set up the built-in LED pin as an output:
   pinMode(BOARD_LED_PIN, OUTPUT);
   
   // Pause the timer while we're configuring it
   Timer.pause();
    
   //USB Serial initialize
   SerialUSB.begin();
   
   // XBEE setup on uart 2
   Serial2.begin(38400);
   
   //Initialize dynamixel bus as 1Mbps baud rate
   Dxl.begin(1);  
  
   // Set goal speed
   //Dxl.writeWord( 1, P_GOAL_SPEED_L, 0 );
   // Set goal position
   //Dxl.writeWord( 1, P_GOAL_POSITION_L, 512 );
  
   // set all the motor limits
   Dxl.writeWord(L11, P_CW_ANGLE_LIMIT_L, 207);
   Dxl.writeWord(L11, P_CCW_ANGLE_LIMIT_L, 807);  
   Dxl.writeWord(L12, P_CW_ANGLE_LIMIT_L, 307);
   Dxl.writeWord(L12, P_CCW_ANGLE_LIMIT_L, 656);  
   Dxl.writeWord(L13, P_CW_ANGLE_LIMIT_L, 107);
   Dxl.writeWord(L13, P_CCW_ANGLE_LIMIT_L, 531); 
   Dxl.writeWord(L21, P_CW_ANGLE_LIMIT_L, 207);
   Dxl.writeWord(L21, P_CCW_ANGLE_LIMIT_L, 807); 
   Dxl.writeWord(L22, P_CW_ANGLE_LIMIT_L, 307);
   Dxl.writeWord(L22, P_CCW_ANGLE_LIMIT_L, 656); 
   Dxl.writeWord(L23, P_CW_ANGLE_LIMIT_L, 107);
   Dxl.writeWord(L23, P_CCW_ANGLE_LIMIT_L, 531); 
   Dxl.writeWord(L31, P_CW_ANGLE_LIMIT_L, 285);
   Dxl.writeWord(L31, P_CCW_ANGLE_LIMIT_L, 807); 
   Dxl.writeWord(L32, P_CW_ANGLE_LIMIT_L, 307);
   Dxl.writeWord(L32, P_CCW_ANGLE_LIMIT_L, 656); 
   Dxl.writeWord(L33, P_CW_ANGLE_LIMIT_L, 107);
   Dxl.writeWord(L33, P_CCW_ANGLE_LIMIT_L, 531); 
   Dxl.writeWord(L41, P_CW_ANGLE_LIMIT_L, 207);
   Dxl.writeWord(L41, P_CCW_ANGLE_LIMIT_L, 807); 
   Dxl.writeWord(L42, P_CW_ANGLE_LIMIT_L, 307);
   Dxl.writeWord(L42, P_CCW_ANGLE_LIMIT_L, 656); 
   Dxl.writeWord(L43, P_CW_ANGLE_LIMIT_L, 107);
   Dxl.writeWord(L43, P_CCW_ANGLE_LIMIT_L, 531); 

   delay(100);
   Timer.setPeriod(TimerMS2);         // initialize timer1, and set a 100K usecond period

   // Set up an interrupt on channel 1
    Timer.setMode(TIMER_CH1, TIMER_OUTPUT_COMPARE);
    Timer.setCompare(TIMER_CH1, 1);  // Interrupt 1 count after each update
    Timer.attachInterrupt(TIMER_CH1, ISRTimer1);

    // Refresh the timer's count, prescale, and overflow
    Timer.refresh();

    // Start the timer counting
    Timer.resume();
    
    
}

void loop() {
   // Service and Comm Commands from DFrobot Wireless Controller
  HostComm();
  
  // Service the ISR
  if (ISRState) {
    toggleLED();
    // Determine Gate (Step Positions)
    Gate();
    //Please refer ROBOTIS support page
    currPosition = Dxl.readWord( motorID, 36);
    SerialUSB.println("Current Position:");
    SerialUSB.println(currPosition);
    digitalWrite(BOARD_LED_PIN, HIGH);
    currPosition = Dxl.readWord( motorID, 36);
    SerialUSB.println("Current Position:");
    SerialUSB.println(currPosition);
    torqEn = Dxl.readByte( motorID, 24);
    SerialUSB.println("Torque Enabled:");
    SerialUSB.println(torqEn);
    // Send Position Commands to Dynamixels
    DynamixelPosTx();
  
   // Resetr ISR Flag
    ISRState = false;
  }
}

/*********************************************************************************************
 * Timer Interrupt Service Routine
 *********************************************************************************************/
void ISRTimer1() {
//void ISRTimer5(){ // Uncomment for Atmega2560
  ISRState = true;
}
