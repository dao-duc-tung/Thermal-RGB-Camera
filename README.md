# Thermal-RGB Camera

This project won a Second Prize in 2nd Vietnam Maker Contest with Intel Galileo 2016 held by Intel Vietnam.

This project is supported by School of Information and Communication Technology - Hanoi University of Science and Technology.

# Overview

A thermal camera is used to show the high-temperature object, and an RGB camera to show the detail of that object.

When the highest temperature point in the thermal image over a threshold, we will combine the thermal image with the RGB image to show which object or which part of the object has the highest temperature.

We use the micro-processor ARM Cortext-M3 ATSAM3X8E to receive the thermal image from thermal camera, and live-stream it directly to an LCD.

Simultaneously, that micro-processor also sends the thermal image to an Intel Edison board.

The Intel Edison board will transfer this thermal image to PC through Wi-Fi.

PC will process that thermal image. If the highest temperature point over the threshold, PC will send a command to Intel Edison board through Wi-Fi, to command the micro-processor capturing an RGB image and send it back to PC.

PC will combine these 2 images into one to show the thermal-RGB image.

# Communication

We use a FLIR thermal camera. This thermal camera communicates with the micro-processor by using SPI and I2C.

The RGB camera communicates with the micro-processor by using UART.

The micro-processor communicates with the Intel Edison board by using UART.

The Intel Edison board communicates with PC by using TCP-based socket connection.

All the command processes and server-client connection processes between the micro-processor, Intel Edison board and PC are run in multi-threading tasks.

# Demo

## Prototype and Final Product

<img src="https://github.com/dao-duc-tung/Thermal-RGB-Camera/raw/master/media/prototype.JPG" alt="drawing" style="transform: scaleY(-1)" width="300"/>

<img src="https://github.com/dao-duc-tung/Thermal-RGB-Camera/raw/master/media/IMG_3422.JPG" alt="drawing" width="300"/>

<img src="https://github.com/dao-duc-tung/Thermal-RGB-Camera/raw/master/media/IMG_3423.JPG" alt="drawing" width="300"/>

<img src="https://github.com/dao-duc-tung/Thermal-RGB-Camera/raw/master/media/IMG_3424.JPG" alt="drawing" width="300"/>

<img src="https://github.com/dao-duc-tung/Thermal-RGB-Camera/raw/master/media/IMG_3425.JPG" alt="drawing" width="300"/>

## Thermal-RGB Image Combination

<img src="https://github.com/dao-duc-tung/Thermal-RGB-Camera/raw/master/media/anh5-fix1.bmp" alt="drawing" width="300"/>

<img src="https://github.com/dao-duc-tung/Thermal-RGB-Camera/raw/master/media/anh9-fix1.bmp" alt="drawing" width="300"/>

## Alert System Desktop App

<img src="https://github.com/dao-duc-tung/Thermal-RGB-Camera/raw/master/media/alert system.png" alt="drawing"/>

## Others

<img src="https://github.com/dao-duc-tung/Thermal-RGB-Camera/raw/master/media/IMG_3430.JPG" alt="drawing" width="300"/>

<img src="https://github.com/dao-duc-tung/Thermal-RGB-Camera/raw/master/media/IMG_3446.JPG" alt="drawing" width="300"/>
