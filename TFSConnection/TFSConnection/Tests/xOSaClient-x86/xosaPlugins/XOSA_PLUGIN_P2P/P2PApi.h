#ifndef P2P_API_H
#define P2P_API_H

#ifndef P2P_DISABLE_GCCXML_DEFINE

typedef int int32_t;
typedef unsigned int uint32_t;
typedef unsigned char uint8_t;

#define P2P_API


#define IN  __attribute((gccxml("in")))
#define OUT __attribute((gccxml("out")))
#define INOUT IN OUT
#define POINTER(size) __attribute((gccxml("pointer",#size)))
#define REGISTER_CALLBACK __attribute((gccxml("register_callback")))
#define CALLBACK_FUNCTION __attribute((gccxml("callback_function")))
#define UNREGISTER_CALLBACK(callback) __attribute((gccxml("unregister_callback", #callback)))
#define UNREGISTER_CALLBACK_PK __attribute((gccxml("unregister_callback_pk")))

#else
#if defined(XOSA_WINDOWS) && defined(XOSA_PLUGIN_P2PPlugin_EXPORTS)
#define P2P_API __declspec(dllexport)
#elif defined(XOSA_WINDOWS) && !defined(XOSA_PLUGIN_TenLiraPlugin_EXPORTS)
#define P2P_API
#else
#define P2P_API
#endif

#include <xOSa/PlatformDetection.h>
#include <xOSa/Types.h>

#define IN  
#define OUT 
#define INOUT 
#define POINTER(size) 
#define UNREGISTER_CALLBACK(callback) 
#endif

typedef enum P2P_STATUS
{
	P2P_STATUS_OK,
	P2P_STATUS_ERROR,
	P2P_STATUS_UNSUPPORTED,
	P2P_STATUS_NOT_EXIST,
	P2P_STATUS_INVALID_PARAMETER,
	P2P_STATUS_INSUFFICIENT_BUFFER,
	P2P_STATUS_INVALID_VALUE,
	P2P_STATUS_ACCESS_DENIED,
	P2P_STATUS_TIMEOUT,
	P2P_STATUS_UNREACHABLE
} P2P_API_STATUS;

typedef uint32_t P2P_DLL_STATUS;
#define P2P_DLL_STATUS_OK 0

#pragma pack(1) 
struct IPINFO
{
	char addr4[17];
	char addr6[47];
	char netmask[17];
	char gw[17];
	char mac[18];
	bool dhcp;
};
#pragma pack()

P2P_STATUS P2P_API GetIPConfig(const char* name, struct IPINFO* info, uint32_t descsize, IN POINTER(descsize) char* desc, bool tbt);
P2P_STATUS P2P_API SetIPConfig(const char* name, const char* ipaddress, const char* netmask, const char* gw, bool dhcp, bool tbt);
P2P_STATUS P2P_API Ping(const char* name, const char* sourceip, bool tbt, uint32_t packet_size, uint32_t packets, uint32_t* lost, uint32_t* time);
P2P_STATUS P2P_API GetRemoteIP(const char* name, IN POINTER(17) char* ip4addr, IN POINTER(47) char* ip6addr, bool tbt);
P2P_STATUS P2P_API FileTransfer(bool receive, uint32_t port, const char* dest, const char* filename, bool udp, uint32_t* Bps);
#endif
