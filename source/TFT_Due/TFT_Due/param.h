/*
* param.h
*
*  Created on: Nov 22, 2016
*      Author: Dao
*/

#ifndef IMAGEPROCESSING_PARAM_H_
#define IMAGEPROCESSING_PARAM_H_

#pragma once
#define MAX_NAME_LENGTH 80
#define ROWS 100
#define COLS 100
#define GRAY_LEVELS 255
#define PREWITT 1
#define PEAK_SPACE 50
#define PEAKS 30
#define KIRSCH 2
#define SOBEL 3
#define STACK_SIZE 40000
#define STACK_FILE_LENGTH 500
#define FORGET_IT -50
#define STACK_FILE "c:stack"
#define OTHERC 1
union short_char_union
{
	short s_num;
	char s_alpha[2];
};
union int_char_union
{
	int i_num;
	char i_alpha[2];
};
union  long_char_union
{
	long l_num;
	char l_alpha[4];
};
union float_char_union
{
	float f_num;
	char f_alpha[4];
};
union ushort_char_union
{
	short s_num;
	char s_alpha[2];
};
union uint_char_union
{
	int i_num;
	char i_alpha[2];
};
union ulong_char_union
{
	long l_num;
	char l_alpha[4];
};
struct  bmpfileheader
{
	unsigned short int filetype;
	unsigned int filesize;
	unsigned short int reserved1;
	unsigned short int reserved2;
	unsigned int bitmapoffset;
};
struct bitmapheader
{
	unsigned int size;
	int width;
	int height;
	unsigned short int planes;
	unsigned short int bitsperpixel;
	unsigned int compression;
	unsigned int sizeofbitmap;
	int horzres;
	int vertres;
	unsigned int colorsused;
	unsigned int colorsimp;
};
typedef struct ctstruct
{
	unsigned char blue;
	unsigned char green;
	unsigned char red;
}Pixels;
struct pmgheader
{
	char type[2];
	int width;
	int height;
	int gray;
};

#endif /* IMAGEPROCESSING_PARAM_H_ */
