/*
 * types.h
 *
 *  Created on: Sep 29, 2016
 *      Author: Dao
 */

#ifndef OTHERS_TYPES_H_
#define OTHERS_TYPES_H_

#include "mraa.hpp"

#define THERMAL_IMAGE 	0
#define RGB_IMAGE		1
#define BOTH_IMAGE		2

/* These types must be 16-bit, 32-bit or larger integer */
typedef int			INT;
typedef unsigned int	UINT;

/* These types must be 8-bit integer */
typedef signed char	CHAR;
typedef unsigned char	UCHAR;
typedef unsigned char	BYTE;

/* These types must be 16-bit integer */
typedef short			SHORT;
typedef unsigned short	USHORT;
typedef unsigned short	WORD;
typedef unsigned short	WCHAR;

/* These types must be 32-bit integer */
typedef long			LONG;
typedef unsigned long	ULONG;
typedef unsigned long	DWORD;
typedef long int		SYS_TIME;

/* Boolean type */
//typedef enum { FALSE = 0, TRUE } BOOL;
#include <stdbool.h>
typedef   bool BOOL;
#ifndef FALSE
#define FALSE false
#define TRUE true
#endif

#endif /* OTHERS_TYPES_H_ */
