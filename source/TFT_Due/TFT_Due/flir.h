// flir.h

#ifndef _FLIR_h
#define _FLIR_h

#if defined(ARDUINO) && ARDUINO >= 100
#include "arduino.h"
#else
#include "WProgram.h"
#endif

//#include <Wire.h>
#include <UTFT.h>
#include "IP.h"
#include <SPI.h>
#include <Reset.h>
#include "globaldef.h"

void flir_init();
void flir_run();

void openSPI(void);
void closeSPI(void);
void readFrame(void);

//swaps the endianess of the frame data. Lepton data is big-endian
void endianSwap(void);
//Sends a lepton frame (16 bit endian swapped) over UART serial (without packet ID and CRC).
void sendLeptonFrame(void);

void printImage(void);
void printDEBUG();

#endif

