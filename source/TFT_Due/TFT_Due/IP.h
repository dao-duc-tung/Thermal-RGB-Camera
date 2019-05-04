// IP.h

#ifndef _IP_h
#define _IP_h

#if defined(ARDUINO) && ARDUINO >= 100
	#include "arduino.h"
#else
	#include "WProgram.h"
#endif

#include <UTFT.h>
#include <stdio.h>
#include <stdlib.h>
#include "param.h"
#include <math.h>
//#include <limits.h>
//#include <string.h>

using namespace std;

#define RGB565CONVERT(red, green, blue) (int) (((red >> 3) << 11) | ((green >> 2) << 5) | (blue >> 3))

void IP_Init();
uint16_t** IP_ZoomIn(uint16_t* imgFLIR16, uint16_t maxVal, uint16_t minVal, uint16_t subVal, UTFT* myGLCD);
void IP_Flush();

void free_char_array2D(uint8_t **the_array, uint16_t length);
void free_short_array2D(uint16_t **the_array, uint16_t length);
void free_char_array1D(uint8_t *the_array);
void free_short_array1D(uint16_t *the_array);
void make_color_table(Pixels *rgb, long color);
uint8_t** zoom_in_with_cubic(uint8_t **image, uint8_t **_zoomImg, uint16_t width, uint16_t height, uint16_t new_width, uint16_t new_height);
float CubicHermite(float A, float B, float C, float D, float t);
//float CubicHermite(uint8_t A, uint8_t B, uint8_t C, uint8_t D, float t);
void convertRGB565(uint8_t **img_in, uint16_t *img_out, Pixels *rgb);

#endif

