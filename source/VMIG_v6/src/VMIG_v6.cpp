#include <iostream>
#include "Controller/Controller.h"
using namespace std;

int main() {
	CT_Init();

	for (;;) {
//		CT_Process();
		sleep(0.005);
	}

	return 0;
}
