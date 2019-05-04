/*
 * ServerE.h
 *
 *  Created on: Dec 22, 2016
 *      Author: Dao
 */

#ifndef SERVERE_SERVERE_H_
#define SERVERE_SERVERE_H_

#include <iostream>
#include <netinet/in.h>
#include <stdio.h>
#include <unistd.h>
#include <string.h>
#include <pthread.h>
//#include <sys/socket.h>
//#include <arpa/inet.h>
//#include <stdlib.h>
//#include <errno.h>
//#include <sys/types.h>
#include "../Controller/Controller.h"
#include "../others/types.h"
using namespace std;

void SE_Init();
bool SE_SendImage(uint8_t* image, uint16_t size);
void* SE_ListenConnect(void*);
void* SE_ListenCommand(void*);
void SE_CloseConnect();

#endif /* SERVERE_SERVERE_H_ */
