/*
 * IMAGE.h
 *
 *  Created on: Sep 13, 2016
 *      Author: Dao
 */

#ifndef IMAGE_H_
#define IMAGE_H_

#include "mraa.hpp"
#include "types.h"

const uint8_t* IMAGE_GetImage(int n = 0);
BYTE* IMAGE_GetArr(int n = 0);

#endif /* IMAGE_H_ */
