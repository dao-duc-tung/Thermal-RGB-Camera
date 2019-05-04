#include "ServerE.h"

int listenfd = 0, connfd = 0;
uint8_t cmdBuff[2];
struct sockaddr_in serv_addr;
pthread_t ListenConnectThread, ListenCommandThread;

void SE_Init(){
	listenfd = socket(AF_INET, SOCK_STREAM, 0);
	memset(&serv_addr, '0', sizeof(serv_addr));

	serv_addr.sin_family = AF_INET;
	serv_addr.sin_addr.s_addr = htonl(INADDR_ANY);
	serv_addr.sin_port = htons(8888);

	bind(listenfd, (struct sockaddr*)&serv_addr, sizeof(serv_addr));

	listen(listenfd, 2);

	pthread_create(&ListenConnectThread, 0, SE_ListenConnect, NULL);
	pthread_create(&ListenCommandThread, 0, SE_ListenCommand, NULL);
}

bool SE_SendImage(uint8_t* image, uint16_t size){
	if(connfd == 0) return false;
	if(size == 0) size = 9600;
	uint8_t* SIZE = (uint8_t*)size;
	int ret = write(connfd, &SIZE, 2);
	uint8_t* sendBuff = (uint8_t*)image;
	ret = write(connfd, sendBuff, size);
	if(ret < 0){
		SE_CloseConnect();
		return false;
	}
	else if(ret < size){
		int numBytesSecondSending = size - ret;
		ret = write(connfd, (uint8_t*)(sendBuff + ret), numBytesSecondSending);
		if(ret < numBytesSecondSending){
			SE_CloseConnect();
			return false;
		}
	}

	//check status if client is connected
	int error_code;
	unsigned int error_code_size = sizeof(error_code);
	getsockopt(connfd, SOL_SOCKET, SO_ERROR, &error_code, &error_code_size);
	if(error_code != 0) {
		SE_CloseConnect();
		return false;
	}

	return true;
}

void* SE_ListenConnect(void*){
	while(true){
		if(connfd == 0) {
			connfd = accept(listenfd, (struct sockaddr*)NULL, NULL);
		}
		sleep(0.2);
	}
}

void* SE_ListenCommand(void*){
	while(true){
		if(connfd != 0){
			int n = read(connfd, (uint8_t*)cmdBuff, 2);
			if(n <= 0){
				//error
				SE_CloseConnect();
			}else{
				CT_SetMode(cmdBuff);
				cmdBuff[0] = 0;
				cmdBuff[1] = 0;
			}
		}
		sleep(0.2);
	}
}

void SE_CloseConnect(){
	close(connfd);
	connfd = 0;
}
