/*
Name:		SAM_2CAM.ino
Created:	12/12/2016 19:47:45
Author:	Dao
*/

// the setup function runs once when you press reset or power the board
#include <UTFT.h>
#include "IP.h"
#include <SPI.h>
#include "flir.h"
#include "rgb.h"
#include "globaldef.h"

void setup() {
	delay(500);
	flir_init();
	rgb_init();
	SerialDEBUG.begin(115200);
	SerialImage.begin(115200);
	while (SerialImage.available()) SerialImage.read();
	//SerialDEBUG.println("DONE");
}

bool t = false;
bool h = false;
void loop() {
	if (t) {
		flir_run();
		SerialImage.flush();
		t = false;
	}
	if (h) {
		rgb_run();
		SerialImage.flush();
		h = false;
	}
	delay(5);
}

void serialEventRun() {
	if (SerialImage.available()) {
		char c1 = SerialImage.read();
		char c2 = SerialImage.read();
		if (c1 == 'A' && c2 == '#') {
			//SerialDEBUG.println("A#");
			SerialImage.print("A#");
			t = true;
		}
		else if (c1 == '5' && c2 == '#') {
			//SerialDEBUG.println("5#");
			SerialImage.print("5#");
			h = true;
		}
		else if (c1 == 'A' && c2 == '5') {
			//SerialDEBUG.println("A5");
			SerialImage.print("A5");
			t = true;
			h = true;
		}
	}
}
