/*
 * SAM.cpp
 *
 *  Created on: Oct 28, 2016
 *      Author: Dao
 */

#include "SAM.h"

mraa_uart_context SerialImage = NULL;
uint16_t* imageFLIR16 = (uint16_t *)malloc(sizeof(uint16_t)* 4800);
uint8_t* imageRGB8;
uint16_t sub, minval, maxval, numBytesFLIR, numBytesRGB;

bool SAM_Init(){
	SerialImage = mraa_uart_init(0);
	mraa_uart_set_baudrate(SerialImage, 115200);
	mraa_uart_set_mode(SerialImage, 8, MRAA_UART_PARITY_NONE, 1);
//	mraa_uart_set_timeout(SerialImage, 1000, 1000, 100);
	mraa_uart_flush(SerialImage);

	if (SerialImage == NULL) {
		fprintf(stderr, "UART failed to setup\n");
		return false;
	}
	else return true;
}
bool SAM_SendCmd(const char* cmd, const char* ack){
	char temp[2], first, second;
	while(mraa_uart_data_available(SerialImage, 0)) mraa_uart_read(SerialImage, temp, 2);
	mraa_uart_write(SerialImage, cmd, 2);
	if(mraa_uart_data_available(SerialImage, 100)){
	  mraa_uart_read(SerialImage, &first, 1);
	  mraa_uart_read(SerialImage, &second, 1);
	  if (!(first == ack[0] && second == ack[1])) return false;
	}
	mraa_uart_flush(SerialImage);
	return true;
}

bool SAM_Capture(int c){
	char tempBuff;
	while(mraa_uart_data_available(SerialImage, 0)) mraa_uart_read(SerialImage, &tempBuff, 1);
	switch(c){
		case THERMAL_IMAGE:{//capture thermal image
			if(!SAM_SendCmd("A#", "A#")) return false;
			if(!SAM_GetImageFromSAM(THERMAL_IMAGE)) return false;
			break;
		}
		case RGB_IMAGE:{//capture rgb image
			if(!SAM_SendCmd("5#", "5#")) return false;
			if(!SAM_GetImageFromSAM(RGB_IMAGE)) return false;
			break;
		}
		case BOTH_IMAGE:{//capture 2 image
			if(!SAM_SendCmd("A5", "A5")) return false;
			if(!SAM_GetImageFromSAM(THERMAL_IMAGE)) return false;
			if(!SAM_GetImageFromSAM(RGB_IMAGE)) return false;
			break;
		}
		default:break;
	}
	return true;
}

bool SAM_GetImageFromSAM(int c){
//	sleep(0.9);//waiting for capturing from cam rgb
	if(mraa_uart_data_available(SerialImage, 5000)){
		int nByte = 0;			//so byte vua doc dc
		int readByte = 0;			//so byte da doc
		uint16_t numBytes = 0;		//so byte anh
		mraa_uart_read(SerialImage, (char*)(&numBytes), 2);
		mraa_uart_flush(SerialImage);
		if(numBytes == 0) numBytes = 9600;
		uint8_t* tempBuff;
		if(c==THERMAL_IMAGE) {
			tempBuff = (uint8_t*)imageFLIR16;
		}
		else if(c==RGB_IMAGE) {
			imageRGB8 = (uint8_t*)malloc(numBytes*sizeof(uint8_t));
			tempBuff = (uint8_t*)imageRGB8;
		}
		nByte = 0; readByte = 0;
		do{
			nByte = mraa_uart_read(SerialImage, (char*)((uint8_t*)tempBuff) + readByte, numBytes - readByte);//printf("%d\n", n);
			readByte += nByte;
		}while(readByte < numBytes);

//		int n = 0;
//		for(int i = 0; i <= 3; i++){
//			n = mraa_uart_read(SerialImage, (char*)((uint8_t*)imageFLIR16) + 2048*i, 2048);//printf("%d\n", n);
//			mraa_uart_flush(SerialImage);
//			if(n!=2048) {
//				n = mraa_uart_read(SerialImage, (char*)((uint8_t*)imageFLIR16 + n + 2048*i), 2048-n);//printf("%d\n", n);
//				mraa_uart_flush(SerialImage);
//			}
//		}
//		n = mraa_uart_read(SerialImage, (char*)((uint8_t*)imageFLIR16 + 2048*4), 1408);printf("%d\n", n);
//		mraa_uart_flush(SerialImage);
//		if(n!=1408) {
//			n = mraa_uart_read(SerialImage, (char*)((uint8_t*)imageFLIR16 + n + 2048*4), 1408-n);//printf("%d\n", n);
//			mraa_uart_flush(SerialImage);
//		}

		if(c==THERMAL_IMAGE) {
			numBytesFLIR = numBytes;
		}
		else if(c==RGB_IMAGE) {
			numBytesRGB = numBytes;
		}
		return true;
	}
	return false;
}

void SAM_PrintImage(){
	printf("P2\n80 60\n");
	printf("%d\n", sub);
	for (int i = 0; i<60; i++) {
		for (int j = 0; j<80; j++) {
			printf("%d\t", imageFLIR16[i*80 + j]);
		}
		printf("\n");
	}
}

void SAM_FreeImageBuff(){
//	free(imageFLIR16);//cap phat 1 lan vao luc dau roi, k dc free
	if(imageRGB8 != NULL) free(imageRGB8);
	mraa_uart_flush(SerialImage);
}

uint16_t SAM_GetSizeFLIR(){
	return numBytesFLIR;
}

uint16_t SAM_GetSizeRGB(){
	return numBytesRGB;
}

uint8_t* SAM_GetImageRGB8(){
	return (uint8_t*)imageRGB8;
}

uint16_t* SAM_GetImageFLIR16(){
	return (uint16_t*)imageFLIR16;
}

uint16_t SAM_GetSubVal(){
	return sub;
}
uint16_t SAM_GetMinVal(){
	return minval;
}
