/*
 * Controller.cpp
 *
 *  Created on: Dec 30, 2016
 *      Author: Dao
 */

#include "Controller.h"

int MODE = 0;
uint8_t* both_image;

void CT_Init(){
	mraa_init();
	SE_Init();
	SAM_Init();
}

void CT_SetMode(uint8_t* cmd){
	if(cmd[0] == 'A' && cmd[1] == '#'){
		MODE = THERMAL_IMAGE;
	}
	else if(cmd[0] == '5' && cmd[1] == '#'){
		MODE = RGB_IMAGE;
	}
	else if(cmd[0] == 'A' && cmd[1] == '5'){
		MODE = BOTH_IMAGE;
	}
	else return;

	CT_Process();
}

bool CT_Process(){
	switch(MODE){
	case THERMAL_IMAGE:{
		if(!SAM_Capture(THERMAL_IMAGE)) return false;
		SE_SendImage((uint8_t*)SAM_GetImageFLIR16(), SAM_GetSizeFLIR());
//		SAM_FreeImageBuff();
		return true;
		break;
	}
	case RGB_IMAGE:{
		if(!SAM_Capture(RGB_IMAGE)) return false;
		SE_SendImage((uint8_t*)SAM_GetImageRGB8(), SAM_GetSizeRGB());
		SAM_FreeImageBuff();
		return true;
		break;
	}
	case BOTH_IMAGE:{
		if(!SAM_Capture(BOTH_IMAGE)) return false;
		both_image = (uint8_t*)malloc((SAM_GetSizeFLIR()+SAM_GetSizeRGB())*sizeof(uint8_t));
		memcpy(both_image, (uint8_t*)SAM_GetImageFLIR16(), SAM_GetSizeFLIR());
		memcpy(both_image+SAM_GetSizeFLIR(), (uint8_t*)SAM_GetImageRGB8(), SAM_GetSizeRGB());

		SE_SendImage(both_image, SAM_GetSizeFLIR()+SAM_GetSizeRGB());

		free(both_image);
		SAM_FreeImageBuff();
		return true;
		break;
	}
	default:break;
	}
	return false;
}

