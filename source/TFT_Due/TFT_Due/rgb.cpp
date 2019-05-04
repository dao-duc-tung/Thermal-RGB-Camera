// 
// 
// 

#include "rgb.h"

unsigned char snapshotRequest[7] = { 0x55, 0x48, 0x00, 0x32, 0x00, 0x04, 0x23 };
unsigned char packageRequest[6] = { 0x55, 0x45, 0x00, 0x23, 0x00, 0x23 };
unsigned char package_id, old_package_id;
unsigned char dataBuffer[1035];
unsigned char ackBuffer[4];
unsigned char sizeBuffer[10];

int last_package_size, total_img1 = 0, image_size = 0, num_packages, cam1_failed = 0;
int SNAP_SHOT_TIMEOUT = 50;
int IMAGE_TIMEOUT = 100;
int MAX_IMAGE_NUM = 1000;

unsigned char* imageBufferRGB;
uint16_t imageBufferOffset = 0;

void rgb_init() {
	SerialCam1.begin(115200);
}
void rgb_run() {
	take_one_image_from_cam1();
	rgb_printImage();
	//rgb_printDEBUG();
	rgb_free();
}
void rgb_free() {
	free(imageBufferRGB);
	imageBufferOffset = 0;
	image_size = 0;
	SerialCam1.flush();
	SerialImage.flush();
}
void rgb_printImage() {
	//transfer rgb image
	//SerialDEBUG.println(image_size);
	if (image_size <= 0) return;
	SerialImage.write((unsigned char)image_size);
	SerialImage.write((unsigned char)(image_size >> 8));
	//for (int i = 0; i < image_size; i++) {
	//	SerialImage.write(imageBufferRGB[i]);
	//	//SerialImage.print(" ");
	//}
	SerialImage.write(imageBufferRGB, image_size);
}

void rgb_printDEBUG() {
#ifdef DEBUG
	for (int i = 0; i < image_size; i++) {
		SerialDEBUG.print((unsigned char)imageBufferRGB[i]);
		SerialDEBUG.print(' ');
	}
#endif // DEBUG
}

void prepare_snapshot_message()
{
	//Snapshot command
	snapshotRequest[0] = 0x55; //U
	snapshotRequest[1] = 0x48; //H
	snapshotRequest[2] = 0x00; //Camera ID = 0
	snapshotRequest[3] = 0x32; //0x32=320x240, 0x33=640x480 resolution
	snapshotRequest[4] = 0x00;
	snapshotRequest[5] = 0x04;
	snapshotRequest[6] = 0x23; //#
}

void prepare_package_request_message()
{
	packageRequest[0] = 0x55;//U
	packageRequest[1] = 0x45;//E
	packageRequest[2] = 0x00;//Camera ID = 0
	packageRequest[5] = 0x23;//#
}

void clear_serial_buffer(int cam)
{
	//Clear Serial buffer
	if (cam == 1)
	{
		while (SerialCam1.available())
		{
			int inByte = SerialCam1.read();
		}
	}
}

void send_snapshot_command(int cam)
{
	prepare_snapshot_message();

	if (cam == 1)
	{
		SerialCam1.write(snapshotRequest, 7);
		delay(200); //sleep 200 ms waiting for the camera doing snapshotting.
	}
}

void send_package_request_command(int cam)
{
	prepare_package_request_message();

	if (cam == 1)
	{
		if (package_id > old_package_id)
		{
			packageRequest[3] = package_id & 0x00FF;
			packageRequest[4] = package_id >> 8;

			SerialCam1.write(packageRequest, 6);

			old_package_id = package_id;
			delay(20);
		}
	}
}

void receive_image_information(int cam)
{
	if (cam == 1)
	{
		//Continue reading Serial to get number of packages from camera
		memset(sizeBuffer, 0, 10);

		int inByte = SerialCam1.readBytes(sizeBuffer, 10);

		if (sizeBuffer[0] == 'U' && sizeBuffer[1] == 'R')
		{
			int value1 = sizeBuffer[3];
			int value2 = sizeBuffer[4];
			int value3 = sizeBuffer[5];
			int value4 = sizeBuffer[6];

			image_size = value1 + (value2 << 8) + (value3 << 16) + (value4 << 24);
			num_packages = sizeBuffer[7] + (sizeBuffer[8] << 8);
			last_package_size = image_size - 1024 * (num_packages - 1);

			//image_size = (num_packages - 1)*(1024 - 7) + (last_package_size - 7);
			//imageBufferRGB = (unsigned char*)malloc(image_size * sizeof(unsigned char));
			//image_size = image_size;
			imageBufferRGB = (unsigned char*)malloc(image_size * sizeof(unsigned char));
		}
	}
}

void receive_package_data(int package_id, int num_packages, int cam)
{
	if (cam == 1)
	{
		memset(dataBuffer, 0, sizeof(dataBuffer));

		if (package_id < num_packages)
		{
			int inByte = SerialCam1.readBytes(dataBuffer, 1024 + 9);
			//dueFlashStorage.write(FLASH_DATA1 + 8 + 1024 * (package_id - 1), dataBuffer + 7, 1024);
			for (int j = 7; j < 1024 + 7; j++) {
				imageBufferRGB[imageBufferOffset] = (unsigned char)dataBuffer[j];
				imageBufferOffset++;
			}
			delay(5);
		}
		else
		{
			memset(dataBuffer, 0, sizeof(dataBuffer));
			int inByte = SerialCam1.readBytes(dataBuffer, last_package_size + 9);
			//dueFlashStorage.write(FLASH_DATA1 + 8 + 1024 * (package_id - 1), dataBuffer + 7, last_package_size);
			for (int j = 7; j < last_package_size; j++) {
				imageBufferRGB[imageBufferOffset] = (unsigned char)dataBuffer[j];
				imageBufferOffset++;
			}
			delay(5);
		}
	}
}

void take_one_image_from_cam1()
{
	cam1_failed += 1;

	clear_serial_buffer(1);

	package_id = 1;
	old_package_id = 0;
	image_size = -1;

	if (total_img1 > MAX_IMAGE_NUM)
	{
		total_img1 = 0;
	}

	//Host first sends out snapshot command
	send_snapshot_command(1);
	uint32_t t12 = millis();
	while (!SerialCam1.available())
	{
		uint32_t t22 = millis();
		if (t22 - t12 > SNAP_SHOT_TIMEOUT)
		{
			break;
		}

		delay(50);
	}
	//Host then receive the ACK from the Camera
	if (SerialCam1.available())
	{
		memset(ackBuffer, 0, 4);

		int inByte = SerialCam1.readBytes(ackBuffer, 4);

		if (ackBuffer[0] == 'U' && ackBuffer[1] == 'H' && ackBuffer[2] == 0x00 && ackBuffer[3] == '#')
		{
			//if the ACK is OK, receive the image information
			receive_image_information(1);
		}
	}

	/*memset(flash_header, 0, sizeof(flash_header));
	dueFlashStorage.write(FLASH_DATA1, flash_header, sizeof(flash_header));*/

	if (image_size > 0 && image_size < 32768)
	{
		uint32_t t1 = millis();
		while (package_id <= num_packages)
		{
			//Host sends out command of getting the package with desired package ID
			send_package_request_command(1);

			uint32_t t12 = millis();
			while (!SerialCam1.available())
			{
				uint32_t t22 = millis();
				if (t22 - t12 > SNAP_SHOT_TIMEOUT) //timeout
				{
					break;
				}
				delay(50);
			}

			//receive the ACK from Camera
			if (SerialCam1.available())
			{
				int inByte;
				memset(ackBuffer, 0, 4);

				inByte = SerialCam1.readBytes(ackBuffer, 4);//UE 0x00 #

				if (ackBuffer[0] == 'U' && ackBuffer[1] == 'E' && ackBuffer[2] == 0x00 && ackBuffer[3] == '#')
				{
					//receive package data & write to flash memory
					receive_package_data(package_id, num_packages, 1);
					package_id++;
					//delay(50);
				}
				else
				{
					uint32_t t2 = millis();
					if (t2 - t1 > IMAGE_TIMEOUT)
					{
						break;
					}
				}
			}
			else
			{
				uint32_t t2 = millis();
				if (t2 - t1 > IMAGE_TIMEOUT)
				{
					break;
				}
			}
		}


		if (package_id > num_packages)
		{
			total_img1++;
			cam1_failed = 0;
		}
	}
	debug_print(total_img1);
	debug_println(" - captured from CAM1");
}



void debug_print(int s) {
#ifdef DEBUG
	SerialDEBUG.print(s);
#endif // DEBUG
}
void debug_print(char* s) {
#ifdef DEBUG
	SerialDEBUG.print(s);
#endif // DEBUG
}
void debug_println(char* s) {
#ifdef DEBUG
	SerialDEBUG.println(s);
#endif // DEBUG
}
