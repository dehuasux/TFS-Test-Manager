#ifndef OSGENERAL_API_H
#define OSGENERAL_API_H

#ifndef OSGENERAL_DISABLE_GCCXML_DEFINE

typedef int int32_t;
typedef unsigned int uint32_t;
typedef unsigned char uint8_t;

#define OSGENERAL_API


#define IN  __attribute((gccxml("in")))
#define OUT __attribute((gccxml("out")))
#define INOUT IN OUT
#define POINTER(size) __attribute((gccxml("pointer",#size)))
#define REGISTER_CALLBACK __attribute((gccxml("register_callback")))
#define CALLBACK_FUNCTION __attribute((gccxml("callback_function")))
#define UNREGISTER_CALLBACK(callback) __attribute((gccxml("unregister_callback", #callback)))
#define UNREGISTER_CALLBACK_PK __attribute((gccxml("unregister_callback_pk")))

#else
#if defined(XOSA_WINDOWS) && defined(XOSA_PLUGIN_OSGENERAL_EXPORTS)
#define OSGENERAL_API __declspec(dllexport)
#elif defined(XOSA_WINDOWS) && !defined(XOSA_PLUGIN_OSGENERAL_EXPORTS)
#define OSGENERAL_API
#else
#define OSGENERAL_API
#endif

#include <xOSa/PlatformDetection.h>
#include <xOSa/Types.h>

#define IN  
#define OUT 
#define INOUT 
#define POINTER(size) 
#define UNREGISTER_CALLBACK(callback) 
#endif

typedef enum OSGENERAL_STATUS
{
	OSGENERAL_STATUS_OK,
	OSGENERAL_STATUS_ERROR,
	OSGENERAL_STATUS_UNSUPPORTED,
	OSGENERAL_STATUS_NOT_EXIST,
	OSGENERAL_STATUS_INVALID_PARAMETER,
	OSGENERAL_STATUS_INSUFFICIENT_BUFFER,
	OSGENERAL_STATUS_INVALID_VALUE,
	OSGENERAL_STATUS_ACCESS_DENIED,
	OSGENERAL_STATUS_TIMEOUT
} OSGENERAL_API_STATUS;

typedef uint32_t OSGENERAL_DLL_STATUS;
#define OSGENERAL_DLL_STATUS_OK 0

OSGENERAL_STATUS OSGENERAL_API DoQuery(uint32_t bufsize, IN POINTER(bufsize) char* buf, uint32_t* reqsize, const char* objName /*query in WMI*/, const char* objPath, const char* prop, const char* iface);
OSGENERAL_STATUS OSGENERAL_API GetOrientation(uint32_t* orientation);
OSGENERAL_STATUS OSGENERAL_API SetOrientation(uint32_t orientation);
OSGENERAL_STATUS OSGENERAL_API SetAutoRotate(bool enable);
#endif
