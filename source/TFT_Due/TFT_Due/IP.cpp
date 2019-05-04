// 
// 
// 

#include "IP.h"

Pixels *rgb;	//NO FREE

uint8_t **raw_to_bmp;
//uint8_t **scaleimg;
uint16_t *rgb565;

#define NEW_HEIGHT  60
#define NEW_WIDTH  80
#define LCD_SCALE  4

void IP_Init() {
	rgb = (Pixels*)malloc(sizeof(Pixels) * 256);
	make_color_table(rgb, 256);
	rgb565 = (uint16_t *)malloc(sizeof(uint16_t) * NEW_HEIGHT * NEW_WIDTH);

	raw_to_bmp = (uint8_t **)malloc(sizeof(uint8_t*) * 60);
	for (int i = 0; i < 60; i++)
	{
		raw_to_bmp[i] = (uint8_t *)malloc(sizeof(uint8_t) * 80);
	}

	//scaleimg = (uint8_t **)malloc(sizeof(uint8_t*) * NEW_HEIGHT);
	//for (int i = 0; i < NEW_HEIGHT; i++)
	//{
	//	scaleimg[i] = (uint8_t *)malloc(sizeof(uint8_t) * NEW_WIDTH);
	//}
}

void IP_Flush() {
	
}
uint16_t** IP_ZoomIn(uint16_t* imgFLIR16, uint16_t maxVal, uint16_t minVal, uint16_t subVal, UTFT* myGLCD) {
	uint16_t *img_arr_pgm;
	img_arr_pgm = imgFLIR16;

	float scale = 255.0 / ((float)(subVal));
	for (int i = 0; i < 60; i++)
	{
		for (int j = 0; j < 80; j++)
		{
			raw_to_bmp[i][j] = (uint8_t)((img_arr_pgm[i * 80 + j] - minVal) * scale);
		}
	}
	//free_short_array1D(img_arr_pgm);
	//zoom_in_with_cubic(raw_to_bmp, scaleimg, 80, 60, NEW_WIDTH, NEW_HEIGHT);
	//free_char_array2D(raw_to_bmp, 60);

	//convert to rgb565 and fill color
	convertRGB565(raw_to_bmp, rgb565, rgb);
	//free_char_array2D(scaleimg, NEW_HEIGHT);
	myGLCD->drawBitmap(0, 0, NEW_WIDTH, NEW_HEIGHT, rgb565, LCD_SCALE);
	//free_short_array1D(rgb565);
	return NULL;
}
void free_char_array2D(uint8_t **the_array, uint16_t length)
{
	if (the_array != NULL) {
		for (int i = 0; i<length; i++)
			free(the_array[i]);
		if (the_array != NULL) free(the_array);
	}
}
void free_short_array2D(uint16_t **the_array, uint16_t length)
{
	if (the_array != NULL) {
		for (int i = 0; i<length; i++)
			free(the_array[i]);
		if (the_array != NULL) free(the_array);
	}
}
void free_char_array1D(uint8_t *the_array)
{
	free(the_array);
}
void free_short_array1D(uint16_t *the_array)
{
	free(the_array);
}

void make_color_table(Pixels *rgb, long color)
{
	for (int i = 0; i < 51; i++)
	{
		//white to yellow
		rgb[color - i - 1].red = 255;
		rgb[color - i - 1].green = 255 - i / 2;
		rgb[color - i - 1].blue = 255 - i * 5;
		//yellow to orange
		rgb[color - (i + 51)].red = 255 - i / 3;
		rgb[color - (i + 51)].green = 255 - i * 4;
		rgb[color - (i + 51)].blue = i / 3;
		////orange to viloet
		rgb[color - (i + 102)].red = 255 - i * 2;
		rgb[color - (i + 102)].green = 55 - i;
		rgb[color - (i + 102)].blue = i * 3 / 2;
		////violet to blue
		rgb[color - (i + 153)].red = 153 - i * 2;
		rgb[color - (i + 153)].green = 0;
		rgb[color - (i + 153)].blue = 55 + i;
		////blue to black
		rgb[color - (i + 204)].red = 55 - i * 2 / 3;
		rgb[color - (i + 204)].green = 0;
		rgb[color - (i + 204)].blue = 100 - i * 2 / 3;
	}
	rgb[0].red = 0;
	rgb[0].green = 0;
	rgb[0].blue = 0;
}
uint8_t** zoom_in_with_cubic(uint8_t **image, uint8_t **_zoomImg, uint16_t width, uint16_t height, uint16_t new_width, uint16_t new_height)
{
	uint16_t x = 0, y = 0, m = 0, n = 0, ox, oy, value;
	float dx, dy, iy, v, ix, s;
	float t[4];
	dx = (float)width / (float)(new_width - 1);
	dy = (float)height / (float)(new_height - 1);
	for (y = 0; y < new_height; y++)
	{
		//Serial.println(y);
		iy = (float)y * dy - 0.5;
		v = iy - floor(iy);
		oy = (int)iy;
		if (oy - 1 < 0)
		{
			oy = 1;
		}
		if ((oy + 2) > height)
		{
			oy = height - 4;
		}
		else if (((oy + 1) <= (height - 1)) && ((oy + 2) > (height - 1)))
		{
			oy = height - 3;
		}
		for (x = 0; x < new_width; x++)
		{
			ix = (float)x * dx - 0.5;
			s = ix - floor(ix);
			ox = (int)ix;
			if (ox - 1 < 0)
			{
				ox = 1;
			}
			if ((ox + 2) > width)
			{
				ox = width - 4;
			}
			else if (((ox + 1) <= (width - 1)) && ((ox + 2) > (width - 1)))
			{
				ox = width - 3;
			}

			t[0] = CubicHermite(image[oy - 1][ox - 1], image[oy - 1][ox], image[oy - 1][ox + 1], image[oy - 1][ox + 2], s);
			t[1] = CubicHermite(image[oy][ox - 1], image[oy][ox], image[oy][ox + 1], image[oy][ox + 2], s);
			t[2] = CubicHermite(image[oy + 1][ox - 1], image[oy + 1][ox], image[oy + 1][ox + 1], image[oy + 1][ox + 2], s);
			t[3] = CubicHermite(image[oy + 2][ox - 1], image[oy + 2][ox], image[oy + 2][ox + 1], image[oy + 2][ox + 2], s);
			value = (int)CubicHermite(t[0], t[1], t[2], t[3], v);

			//_zoomImg[y][x] = clamp(value, 0, 255);
			_zoomImg[y][x] = (value < 0) ? 0 : ((value > 255) ? 255 : value);
		}
	}
	return _zoomImg;
}
float CubicHermite(float A, float B, float C, float D, float t)
{
	float a = -A / 2.0 + 3.0 * B / 2.0 - 3.0 * C / 2.0 + D / 2.0;
	float b = A - 5.0 * B / 2.0 + 2.0 * C - D / 2.0;
	float c = -A / 2.0 + C / 2.0;
	float d = B;
	return  a*t*t*t + b*t*t + c*t + d;
}
//float CubicHermite(uint8_t A, uint8_t B, uint8_t C, uint8_t D, float t)
//{
//	float a = -(float)A / 2.0 + 3.0 * (float)B / 2.0 - 3.0 * (float)C / 2.0 + (float)D / 2.0;
//	float b = (float)A - 5.0 * (float)B / 2.0 + 2.0 * (float)C - (float)D / 2.0;
//	float c = -(float)A / 2.0 + (float)C / 2.0;
//	float d = (float)B;
//	return  a*t*t*t + b*t*t + c*t + d;
//}
void convertRGB565(uint8_t **img_in, uint16_t *img_out, Pixels *rgb)
{
	for (int i = 0; i < NEW_HEIGHT; i++)
	{
		for (int j = 0; j < NEW_WIDTH; j++)
		{
			img_out[i * NEW_WIDTH + j] = ((rgb[img_in[i][j]].red >> 3) << 11) | ((rgb[img_in[i][j]].green >> 2) << 5) | rgb[img_in[i][j]].blue >> 3;
		}
	}
}