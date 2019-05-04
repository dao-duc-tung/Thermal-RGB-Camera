/*
 * Controller.h
 *
 *  Created on: Dec 30, 2016
 *      Author: Dao
 */

#ifndef CONTROLLER_CONTROLLER_H_
#define CONTROLLER_CONTROLLER_H_

#include "mraa.hpp"
#include "../SAM/SAM.h"
#include "../ServerE/ServerE.h"
#include "../others/types.h"

void CT_Init();
void CT_SetMode(uint8_t* cmd);
bool CT_Process();

#endif /* CONTROLLER_CONTROLLER_H_ */
