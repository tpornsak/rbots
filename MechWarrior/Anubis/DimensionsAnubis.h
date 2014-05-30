#ifndef DimensionsAnubis_h
#define DimensionsAnubis_h

// Comment/Uncomment Body Configuration to s
//#define ConfigBody1
#define ConfigBody2

// Select Configuration
#ifdef ConfigBody1
  // Body Dimensions
  #define L0 1.07044                 // Shoulder Fixed Length        (in)
  #define L1 2.60275                 // Upper Leg (Femur) Length     (in)
  #define L2 4.11417                 // Lower Leg (Tibia) Length     (in)
  
  #define Phi 1.0083                 // Shoulder Fixed Angle         (rad)
  #define RSH 3.500                  // Shoulder Joint Radius        (in)
  #define HSH 3.7795                 // Shoulder Joint Height        (in)
  #define LFT 3.1102                 // Foot Radius - Joint Radius   (in)      

  // Offsets used in calculation of Dynamixel angle commands  
  #define offsetT11  -67.5000        // Theta1 Offset - Leg1         (deg)             
  #define offsetT21 -157.5000        // Theta1 Offset - Leg2         (deg)
  #define offsetT31  112.5000        // Theta1 Offset - Leg3         (deg)
  #define offsetT41   22.5000        // Theta1 Offset - Leg4         (deg)
  
  #define offsetT2   12.6699         // Theta2 Offest - All Legs     (deg) 
  #define offsetT3   77.3302         // Theta3 Offset - All Legs     (deg)  
#else if ConfigBody2   
   // Body Dimensions
  #define L0 0.86614173              // Shoulder Fixed Length        (in) = 22 mm
  #define L1 2.5984252               // Upper Leg (Femur) Length     (in) = 66 mm
  #define L2 4.05511811              // Lower Leg (Tibia) Length     (in) = 103 mm
  
  #define Phi 0.0000                 // Shoulder Fixed Angle         (rad)
  #define RSH 1.50                   // Shoulder Joint Radius        (in) - NA
  #define HSH 4.72440945             // Shoulder Joint Height        (in) = 120 mm
  #define LFT 2.95275591             // Foot Radius - Joint Radius   (in) = 75 mm     

  // Offsets used in calculation of Dynamixel angle commands  
  #define offsetT11  0.0           // Theta1 Offset - Leg1         (deg)             
  #define offsetT21  -135.0          // Theta1 Offset - Leg2         (deg)
  #define offsetT31  135.0           // Theta1 Offset - Leg3         (deg)
  #define offsetT41  45.0            // Theta1 Offset - Leg4         (deg)
  
  #define offsetT2   12.6699         // Theta2 Offest - All Legs     (deg) 
  #define offsetT3   75.0            // Theta3 Offset - All Legs     (deg)
#endif /*ConfigBody*/

// Shoulder "Home" x,y,z Locations [Body Center] (in)
float XSH[4] = {.7071 * RSH, -.7071 * RSH, -.7071 * RSH,  .7071 * RSH};
float YSH[4] = {.7071 * RSH,  .7071 * RSH, -.7071 * RSH, -.7071 * RSH};
float ZSH[4] = {HSH,  HSH,  HSH,  HSH};
  
// Foot "Home" x,y,z Locations [Body Center] (in)
float XFT[4] = {.7071 * (RSH + LFT), -.7071 * (RSH + LFT), -.7071 * (RSH + LFT),  .7071 * (RSH + LFT)};
float YFT[4] = {.7071 * (RSH + LFT),  .7071 * (RSH + LFT), -.7071 * (RSH + LFT), -.7071 * (RSH + LFT)};
float ZFT[4] = {0, 0, 0, 0};

#endif /*DimensionsAnubis_h*/
