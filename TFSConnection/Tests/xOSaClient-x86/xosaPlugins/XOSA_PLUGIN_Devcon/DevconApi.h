//--------------------------------------------------------------------------
//
// Copyright (c) Intel Corporation, 2014-2016  All Rights Reserved.
//
//  File:       DevconApi.h
//
//  Contents:   DevconPlugin: C++ Devcon plugin API 
//
//  Notes:
//
//--------------------------------------------------------------------------

#ifndef DEVCON_API_H
#define DEVCON_API_H

#ifndef DEVCON_DISABLE_GCCXML_DEFINE

typedef int int32_t;
typedef unsigned long long uint64_t;
typedef unsigned int uint32_t;
typedef unsigned char uint8_t;

#define DEVCON_API


#define IN  __attribute((gccxml("in")))
#define OUT __attribute((gccxml("out")))
#define INOUT IN OUT
#define POINTER(size) __attribute((gccxml("pointer",#size)))
#define REGISTER_CALLBACK __attribute((gccxml("register_callback")))
#define CALLBACK_FUNCTION __attribute((gccxml("callback_function")))
#define UNREGISTER_CALLBACK(callback) __attribute((gccxml("unregister_callback", #callback)))
#define UNREGISTER_CALLBACK_PK __attribute((gccxml("unregister_callback_pk")))

// enum used by Windows
typedef enum _DRIVER_STATUS
{
	STARTED,
	NOTSTARTED,
	DISABLED,
	PHANTOM,
	PROBLEM
} DRIVER_STATUS;

#else
#if defined(XOSA_WINDOWS) && defined(XOSA_PLUGIN_DevconPlugin_EXPORTS)
#define DEVCON_API __declspec(dllexport)
#elif defined(XOSA_WINDOWS) && !defined(XOSA_PLUGIN_DevconPlugin_EXPORTS)
#define DEVCON_API
#else
#define DEVCON_API
#endif

#include <xOSa/PlatformDetection.h>
#include <xOSa/xOSaClientApiDriver.h>
#include <xOSa/Types.h>

#define IN  
#define OUT 
#define INOUT 
#define POINTER(size) 
#define UNREGISTER_CALLBACK(callback) 
#endif


/*typedef enum DEVICE_STATUS
{
	STARTED,
	NOTSTARTED,
	DISABLED,
	PHANTOM,
	PROBLEM
} DEVCON_DEVICE_STATUS;*/

typedef enum DEVCON_STATUS
{
	DEVCON_STATUS_OK,
	DEVCON_STATUS_ERROR,
	DEVCON_STATUS_UNSUPPORTED,
	DEVCON_STATUS_NOT_EXIST,
	DEVCON_STATUS_INVALID_PARAMETER,
	DEVCON_STATUS_INSUFFICIENT_BUFFER,
	DEVCON_STATUS_INVALID_VALUE,
	DEVCON_STATUS_REROOT_REQUIRED,
	DEVCON_STATUS_NO_RESOURCES,
	DEVCON_STATUS_NO_RESERVED_RESOURCES
} DEVCON_API_STATUS;

typedef uint32_t DEVCON_DLL_STATUS;
#define DEVCON_DLL_STATUS_OK 0

#define MAX_STRINGDATA 512
#define MAX_HWID 260

#pragma pack(1)
struct MemoryResource
{
	uint64_t Base;
	uint64_t End;
#ifdef DEVCON_DISABLE_GCCXML_DEFINE
	MemoryResource() { Base = 0, End = 0; }
	MemoryResource(uint64_t base, uint64_t end) { Base = base; End = end; }
#endif
};

struct DriverResources
{
	char name[256];
	int memSize;
	int ioSize;
	int DmaSize;
	int IrqSize;
	POINTER(memSize) struct MemoryResource* MemResources;
	POINTER(ioSize) struct MemoryResource* IoResources;
	POINTER(DmaSize) uint32_t* DmaResources;
	POINTER(IrqSize) uint32_t* IrqResources;
#ifdef DEVCON_DISABLE_GCCXML_DEFINE
	~DriverResources()
	{
		if (memSize > 0)
			delete[] MemResources;
		if (ioSize > 0)
			delete[] IoResources;
		if (DmaSize > 0)
			delete[] DmaResources;
		if (IrqSize > 0)
			delete[] IrqResources;
	}
	DriverResources(){}

	DriverResources(DriverResources& dr)
	{
		strncpy(name, dr.name, 255);
		memSize = dr.memSize;
		ioSize = dr.ioSize;
		DmaSize = dr.DmaSize;
		IrqSize = dr.IrqSize;
		if(dr.memSize>0)
		{
			MemResources = new MemoryResource[dr.memSize];
			memcpy(MemResources, dr.MemResources, dr.memSize*sizeof(MemoryResource));
		}
		if (dr.ioSize>0)
		{
			IoResources = new MemoryResource[dr.ioSize];
			memcpy(IoResources, dr.IoResources, dr.ioSize*sizeof(MemoryResource));
		}
		if (dr.DmaSize>0)
		{
			DmaResources = new uint32_t[dr.DmaSize];
			memcpy(DmaResources, dr.DmaResources, dr.DmaSize*sizeof(uint32_t));
		}
		if (dr.IrqSize>0)
		{
			IrqResources = new uint32_t[dr.IrqSize];
			memcpy(IrqResources, dr.IrqResources, dr.IrqSize*sizeof(uint32_t));
		}
	}

	static void copy(DriverResources& dst, const DriverResources& dr)
	{
		strncpy(dst.name, dr.name, 255);
		dst.memSize = dr.memSize;
		dst.ioSize = dr.ioSize;
		dst.DmaSize = dr.DmaSize;
		dst.IrqSize = dr.IrqSize;
		if (dr.memSize>0)
		{
			dst.MemResources = new MemoryResource[dr.memSize];
			memcpy(dst.MemResources, dr.MemResources, dr.memSize*sizeof(MemoryResource));
		}
		if (dr.ioSize>0)
		{
			dst.IoResources = new MemoryResource[dr.ioSize];
			memcpy(dst.IoResources, dr.IoResources, dr.ioSize*sizeof(MemoryResource));
		}
		if (dr.DmaSize>0)
		{
			dst.DmaResources = new uint32_t[dr.DmaSize];
			memcpy(dst.DmaResources, dr.DmaResources, dr.DmaSize*sizeof(uint32_t));
		}
		if (dr.IrqSize>0)
		{
			dst.IrqResources = new uint32_t[dr.IrqSize];
			memcpy(dst.IrqResources, dr.IrqResources, dr.IrqSize*sizeof(uint32_t));
		}
	}
#endif
};

struct DriverNode
{
	char InfFile[260];
	char DrvDate[260];
	char DrvVersion[50];
	char Manufacturer[256];
	char Provider[256];
	char Description[256];
};

struct StringData
{
	char text[MAX_STRINGDATA];
};

struct StatusData
{
	char hwid[MAX_HWID];
	DRIVER_STATUS status;
};

typedef struct _StatusDataEx
{
	char hwid[MAX_HWID];
	int CR;
	unsigned long CM;
	unsigned long DN;
} StatusDataEx;
#pragma pack()

DEVCON_API DEVCON_STATUS GetAllClasses(uint32_t size, POINTER(size) StringData* classes, uint32_t* required_size);
DEVCON_API DEVCON_STATUS GetClassDevices(const char* name, uint32_t size, POINTER(size) StringData* devices, uint32_t* required_size);
DEVCON_API DEVCON_STATUS Find(const char* name, uint32_t size, POINTER(size) StringData* devices, uint32_t* required_size);
DEVCON_API DEVCON_STATUS FindAll(const char* name, uint32_t size, POINTER(size) StringData* devices, uint32_t* required_size);
DEVCON_API DEVCON_STATUS GetStatusEx(const char* name, uint32_t size, POINTER(size)StatusDataEx* statuses, uint32_t* required_size);
DEVCON_API DEVCON_STATUS GetStatus(const char* name, uint32_t size, POINTER(size) StatusData* statuses, uint32_t* required_size);
DEVCON_API DEVCON_STATUS GetDriverFiles(const char* name, uint32_t size, POINTER(size) StringData* files, uint32_t* required_size);
DEVCON_API DEVCON_STATUS GetDriverNodes(const char* name, uint32_t size, POINTER(size) struct DriverNode * swarray, uint32_t * required_size);
DEVCON_API DEVCON_STATUS GetDriverResources(const char* name, uint32_t size, POINTER(size) struct DriverResources * swarray, uint32_t * required_size);
DEVCON_API DEVCON_STATUS GetDeviceHwids(const char* name, uint32_t size, POINTER(size) struct StringData * ids, uint32_t * required_size);
DEVCON_API DEVCON_STATUS EnableDevice(const char* name);
DEVCON_API DEVCON_STATUS DisableDevice(const char* name);
DEVCON_API DEVCON_STATUS RestartDevice(const char* name);
DEVCON_API DEVCON_STATUS UpdateDriver(const char* inf, const char* device);
DEVCON_API DEVCON_STATUS UpdateDriverNI(const char* inf, const char* device);
DEVCON_API DEVCON_STATUS InstallDriver(const char* inf, const char* device);
DEVCON_API DEVCON_STATUS RemoveDriver(const char* device, bool deleteDP);
DEVCON_API DEVCON_STATUS Rescan();
DEVCON_API DEVCON_STATUS AddDP(const char* inf);
DEVCON_API DEVCON_STATUS RemoveDP(const char* inf);
DEVCON_API DEVCON_STATUS GetDevconError(uint32_t size, POINTER(size) char* errorMsg, uint32_t* errorCode);

#endif
