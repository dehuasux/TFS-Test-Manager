#ifndef TBTSIM_API_H
#define TBTSIM_API_H

#ifndef TBTSIM_DISABLE_GCCXML_DEFINE

typedef int int32_t;
typedef unsigned int uint32_t;
typedef unsigned char uint8_t;

#define TBTSIM_API


#define IN  __attribute((gccxml("in")))
#define OUT __attribute((gccxml("out")))
#define INOUT IN OUT
#define POINTER(size) __attribute((gccxml("pointer",#size)))
#define REGISTER_CALLBACK __attribute((gccxml("register_callback")))
#define CALLBACK_FUNCTION __attribute((gccxml("callback_function")))
#define UNREGISTER_CALLBACK(callback) __attribute((gccxml("unregister_callback", #callback)))
#define UNREGISTER_CALLBACK_PK __attribute((gccxml("unregister_callback_pk")))

#else
#if defined(XOSA_WINDOWS) && defined(XOSA_PLUGIN_TbtSimPlugin_EXPORTS)
#define TBTSIM_API __declspec(dllexport)
#elif defined(XOSA_WINDOWS) && !defined(XOSA_PLUGIN_TbtSimPlugin_EXPORTS)
#define TBTSIM_API
#else
#define TBTSIM_API
#endif

#include <xOSa/PlatformDetection.h>
#include <xOSa/Types.h>

#define IN  
#define OUT 
#define INOUT 
#define POINTER(size) 
#define UNREGISTER_CALLBACK(callback) 
#endif

typedef enum TBTSIM_STATUS
{
	TBTSIM_STATUS_OK,
	TBTSIM_STATUS_ERROR,
	TBTSIM_STATUS_UNSUPPORTED,
	TBTSIM_STATUS_NOT_EXIST,
	TBTSIM_STATUS_INVALID_PARAMETER,
	TBTSIM_STATUS_INSUFFICIENT_BUFFER,
	TBTSIM_STATUS_INVALID_VALUE,
	TBTSIM_STATUS_ACCESS_DENIED,
	TBTSIM_STATUS_TIMEOUT,
	TBTSIM_STATUS_UNREACHABLE
} TBTSIM_API_STATUS;

typedef uint32_t TBTSIM_DLL_STATUS;
#define TBTSIM_DLL_STATUS_OK 0

TBTSIM_STATUS TBTSIM_API PerformCall(const char* indata, uint32_t out_size, IN POINTER(out_size) char * outdata);
#endif
