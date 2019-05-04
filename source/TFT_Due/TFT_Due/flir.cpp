// 
// 
// 

#include "flir.h"

#define PACKET_SIZE (164)
#define PACKETS_PER_FRAME 60

uint8_t frame_buffer[PACKET_SIZE * PACKETS_PER_FRAME] = { 0 };
uint8_t *p;
uint16_t *frame16;
uint16_t *outImg = (uint16_t*)malloc(4800 * sizeof(uint16_t));
// Declare which fonts we will be using
extern uint8_t SmallFont[];
UTFT myGLCD(ITDB32S, 38, 39, 40, 41);

void flir_init()
{
	IP_Init();
	openSPI();
	p = frame_buffer;
	// Setup the LCD
	myGLCD.InitLCD();
	myGLCD.setFont(SmallFont);
	//p = (uint8_t*)malloc(PACKETS_PER_FRAME * PACKET_SIZE * sizeof(uint8_t));
	//outImg = (uint16_t*)malloc(4800 * sizeof(uint16_t));
	readFrame();
}

void flir_run()
{
	readFrame();			//Reads a frame from the Lepton over SPI, endian swap
	sendLeptonFrame();		//send to edison
	printImage();			//print to LCD
	//printDEBUG();
}

//Sends a lepton frame (16 bit endian swapped) over UART SerialImage (without packet ID and CRC).
void sendLeptonFrame(void)
{
	SerialImage.write((unsigned char)(9600));
	SerialImage.write((unsigned char)(9600 >> 8));
	for (int i = 0; i< 60; i++)
	{
		for (int j = 0; j< 80; j++)
		{
			SerialImage.write((unsigned char)(outImg[i * 80 + j]));
			SerialImage.write((unsigned char)(outImg[i * 80 + j] >> 8));
		}
	}
}

void printImage(void)
{
	//free(p); 
	uint16_t maxVal = 0, minVal = 100000, subVal, temp;
	for (int i = 0; i < 60; i++) {
		for (int j = 0; j < 80; j++) {
			temp = outImg[i * 80 + j];
			if (temp > maxVal)maxVal = temp;
			if (temp < minVal)minVal = temp;
		}
	}
	subVal = maxVal - minVal;
	IP_ZoomIn(outImg, maxVal, minVal, subVal, &myGLCD);
}

void endianSwap(void)
{
	int n = 0;
	// frame consists of 60 packets, each 82*2bytes long
	for (int row = 0; row<(PACKETS_PER_FRAME); row++)
	{
		for (int col = 2; col<(PACKET_SIZE / 2); col++)
		{
			int temp1 = *(frame16 + row*(PACKET_SIZE / 2) + col) << 8;
			int temp2 = *(frame16 + row*(PACKET_SIZE / 2) + col) >> 8;
			//*(frame16 + row*(PACKET_SIZE / 2) + col) = temp1 | temp2;
			outImg[n++] = temp1 | temp2;
		}
	}
}

void openSPI(void)
{
	SPI.begin(10);
	SPI.setClockDivider(10, 8);    //Due 84MHz / 8 ~= 10MHz SPI clk
	SPI.setBitOrder(MSBFIRST);
	SPI.setDataMode(SPI_MODE3);
}

void closeSPI(void)
{
	SPI.end(10);
}

void readFrame(void)
{
	int packetID = 0;
	int resets = 0;

	for (int j = 0; j < PACKETS_PER_FRAME; j++)
	{
		//read 164 byte packet directly into frame buffer

		for (int k = 0; k < (PACKET_SIZE); k++)
		{
			if (k < (PACKET_SIZE - 1))  //first 59 packets
			{
				*(p + j*PACKET_SIZE + k) = SPI.transfer(10, 0x00, SPI_CONTINUE);     // use SPI_CONTINUE to keep CS low for entire 164 byte packet
			}
			else                     //last packet
			{
				*(p + j*PACKET_SIZE + k) = SPI.transfer(10, 0x00);
			}
		}

		packetID = *(p + j*PACKET_SIZE + 1);

		if (packetID != j)    //if discard packet reset j, make j=-1 so 0 on next loop
		{
			j = -1;
			resets += 1;
			delayMicroseconds(1000);
			if (resets == 750)
			{
				closeSPI();
				delay(750);
				openSPI();
			}
		}
	}
	frame16 = (uint16_t *)&p + 2;   //convert array of 8-bit pixel values to 16-bit pixel values
	endianSwap();
}

void printDEBUG() {
#ifdef DEBUG
	for (int i = 0; i< 60; i++)
	{
		for (int j = 0; j< 80; j++)
		{
			SerialDEBUG.print(outImg[i * 80 + j]);
			SerialDEBUG.print(" ");
		}
		SerialDEBUG.println();
	}
	SerialDEBUG.println();
	SerialDEBUG.println();
	SerialDEBUG.flush();
#endif // DEBUG
}




