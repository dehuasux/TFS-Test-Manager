#ifndef TENLIRA_API_H
#define TENLIRA_API_H

#ifndef TENLIRA_DISABLE_GCCXML_DEFINE

typedef int int32_t;
typedef unsigned int uint32_t;
typedef unsigned char uint8_t;

#define TENLIRA_API


#define IN  __attribute((gccxml("in")))
#define OUT __attribute((gccxml("out")))
#define INOUT IN OUT
#define POINTER(size) __attribute((gccxml("pointer",#size)))
#define REGISTER_CALLBACK __attribute((gccxml("register_callback")))
#define CALLBACK_FUNCTION __attribute((gccxml("callback_function")))
#define UNREGISTER_CALLBACK(callback) __attribute((gccxml("unregister_callback", #callback)))
#define UNREGISTER_CALLBACK_PK __attribute((gccxml("unregister_callback_pk")))

#else
#if defined(XOSA_WINDOWS) && defined(XOSA_PLUGIN_TenLiraPlugin_EXPORTS)
#define TENLIRA_API __declspec(dllexport)
#elif defined(XOSA_WINDOWS) && !defined(XOSA_PLUGIN_TenLiraPlugin_EXPORTS)
#define TENLIRA_API
#else
#define TENLIRA_API
#endif

#include <xOSa/PlatformDetection.h>
#include <xOSa/Types.h>

#define IN  
#define OUT 
#define INOUT 
#define POINTER(size) 
#define UNREGISTER_CALLBACK(callback) 
#endif

typedef uint32_t TLJTAG_DEVICE_HANDLE;
typedef uint32_t TLCORE_DEVICE_HANDLE;
typedef uint32_t TL_SWITCH_HANDLE;

typedef enum TENLIRA_STATUS
{
	TENLIRA_STATUS_OK,
	TENLIRA_STATUS_ERROR,
	TENLIRA_STATUS_UNSUPPORTED,
	TENLIRA_STATUS_NOT_EXIST,
	TENLIRA_STATUS_INVALID_PARAMETER,
	TENLIRA_STATUS_INSUFFICIENT_BUFFER,
	TENLIRA_STATUS_INVALID_VALUE,
} TENLIRA_API_STATUS;

typedef uint32_t TENLIRA_DLL_STATUS;
#define TENLIRA_DLL_STATUS_OK 0

#define MAX_ROUTE 15
#define MAX_DEVTYPE 10

#pragma pack(1) 
struct Switch
{
	TL_SWITCH_HANDLE handle;
	bool isroot;
	uint8_t depth;
	uint8_t index;
	char route_string[MAX_ROUTE];
	char device_type[MAX_DEVTYPE];
	uint32_t device_id;
};
#pragma pack()

TENLIRA_API TENLIRA_STATUS GetTenliraErrorText(uint32_t text_size, char * text, uint32_t * required_size);
TENLIRA_API TENLIRA_STATUS GetLibraryVersion(uint32_t text_size, char * text, uint32_t * required_size);
TENLIRA_API TENLIRA_STATUS GetSwitches(uint32_t size, POINTER(size) struct Switch * swarray, uint32_t * required_size);
TENLIRA_API TENLIRA_STATUS RefreshDevices();
TENLIRA_API TENLIRA_STATUS Init();
TENLIRA_API TENLIRA_STATUS Uninit();
TENLIRA_API TENLIRA_STATUS WriteEEPROMByte(const char* route_string, uint8_t offset, uint8_t value, uint8_t mask);
TENLIRA_API TENLIRA_STATUS ReadEEPROMByte(const char* route_string, uint8_t offset, uint8_t* value);
TENLIRA_API TENLIRA_STATUS WriteFlashByte(const char* route_string, uint8_t offset, uint8_t value, uint8_t mask);
TENLIRA_API TENLIRA_STATUS ReadFlashByte(const char* route_string, uint8_t offset, uint8_t* value);
#endif
