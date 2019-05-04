// rgb.h

#ifndef _RGB_h
#define _RGB_h

#if defined(ARDUINO) && ARDUINO >= 100
#include "arduino.h"
#else
#include "WProgram.h"
#endif

#include "globaldef.h"

void rgb_init();
void rgb_free();
void rgb_printImage();
void rgb_printDEBUG();

void prepare_snapshot_message();
void prepare_package_request_message();
void clear_serial_buffer(int cam);
void send_snapshot_command(int cam);
void send_package_request_command(int cam);
void receive_image_information(int cam);
void receive_package_data(int package_id, int num_packages, int cam);
void take_one_image_from_cam1();

void rgb_run();

void debug_print(int s);
void debug_print(char* s);
void debug_println(char* s);

#endif

