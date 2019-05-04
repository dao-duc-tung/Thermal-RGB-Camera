/*
 * SAM.h
 *
 *  Created on: Oct 28, 2016
 *      Author: Dao
 */

#ifndef SAM_H_
#define SAM_H_

#include "mraa.hpp"
#include "../others/types.h"
#include "../others/delay.h"
#include <sys/time.h>
#include <stdlib.h>

bool SAM_Init();
bool SAM_SendCmd(char* cmd, char* ack);
bool SAM_Capture(int c);
void SAM_PrintImage();

bool SAM_GetImageFromSAM(int c);

uint8_t* SAM_GetImageRGB8();
uint16_t* SAM_GetImageFLIR16();

uint16_t SAM_GetSizeFLIR();
uint16_t SAM_GetSizeRGB();

void SAM_FreeImageBuff();
uint16_t SAM_GetSubVal();
uint16_t SAM_GetMinVal();

#endif /* SAM_H_ */
