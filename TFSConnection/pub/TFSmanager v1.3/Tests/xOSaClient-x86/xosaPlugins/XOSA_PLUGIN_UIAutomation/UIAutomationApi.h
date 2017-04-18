//--------------------------------------------------------------------------
//
// Copyright (c) Intel Corporation, 2009-2014  All Rights Reserved.
//
//  File:       UIAutomationApi.h
//
//  Contents:   xOSaLib: API file definition for UA AUTOMATION library
//
//  Notes:   
//
//--------------------------------------------------------------------------
#ifndef UA_AUTOMATION_API_H
#define UA_AUTOMATION_API_H

#ifndef UIAUTOMATION_DISABLE_GCCXML_DEFINE

typedef int int32_t;
typedef unsigned int uint32_t;

#define UIAUTOMATION_API


#define IN  __attribute((gccxml("in")))
#define OUT __attribute((gccxml("out")))
#define INOUT IN OUT
#define POINTER(size) __attribute((gccxml("pointer",#size)))
#define REGISTER_CALLBACK __attribute((gccxml("register_callback")))
#define CALLBACK_FUNCTION __attribute((gccxml("callback_function")))
#define UNREGISTER_CALLBACK(callback) __attribute((gccxml("unregister_callback", #callback)))
#define UNREGISTER_CALLBACK_PK __attribute((gccxml("unregister_callback_pk")))

#else
#pragma unmanaged
#if defined(XOSA_WINDOWS) && defined(XOSA_PLUGIN_UIAutomationPlugin_EXPORTS)
#define UIAUTOMATION_API __declspec(dllexport)
#elif defined(XOSA_WINDOWS) && !defined(XOSA_PLUGIN_UIAutomationPlugin_EXPORTS)
#define UIAUTOMATION_API
#else
#define UIAUTOMATION_API
#endif

#include <xOSa/PlatformDetection.h>
#include <xOSa/Types.h>

#define IN  
#define OUT 
#define INOUT 
#define POINTER(size) 
#define UNREGISTER_CALLBACK(callback) 


#endif


typedef enum UIAUTOMATION_STATUS
{
	UIAUTOMATION_STATUS_OK,
	UIAUTOMATION_STATUS_ERROR,
	UIAUTOMATION_STATUS_UNSUPPORTED,
	UIAUTOMATION_STATUS_NOT_EXIST,
	UIAUTOMATION_STATUS_INVALID_HANDLE,
	UIAUTOMATION_STATUS_INVALID_PARAMETER,
	UIAUTOMATION_STATUS_INSUFFICIENT_BUFFER,
	UIAUTOMATION_STATUS_INVALID_VALUE,
	UIAUTOMATION_STATUS_AGAIN,
	UIAUTOMATION_STATUS_TIMEOUT,
	UIAUTOMATION_STATUS_NOTCLICKABLE,
} SENSOR_STATUS;


#define AUTOMATION_ELEMENT_INVALID_HANDLE 0

typedef int AutomationElementHandle;

enum UIA_FindType
{
	UIA_FindType_First	= 0x1,
	UIA_FindType_All
};


enum UIA_TreeScope
{
	UIA_TreeScope_Element	= 0x1,
	UIA_TreeScope_Children	= 0x2,
	UIA_TreeScope_Descendants	= 0x4,
	UIA_TreeScope_Parent	= 0x8,
	UIA_TreeScope_Ancestors	= 0x10,
	UIA_TreeScope_Subtree	= ( ( UIA_TreeScope_Element | UIA_TreeScope_Children )  | UIA_TreeScope_Descendants ) 
} ;


enum UIA_InvokePattern
{
	UIA_InvokePattern_Invoke	= 0x1,
	UIA_InvokePattern_Toggle,
	UIA_InvokePattern_Close, 
	UIA_InvokePattern_Select, 
	UIA_InvokePattern_Expand, 
	UIA_InvokePattern_Collapse, 
	UIA_InvokePattern_LeftClick, 
	UIA_InvokePattern_RightClick, 
	UIA_InvokePattern_DoubleClick, 
	UIA_InvokePattern_InvokeModal,
	UIA_InvokePattern_Invalid
} ;


enum UIA_StatePattern
{
	UIA_StatePattern_Selected = 0x1,
	UIA_StatePattern_Enabled, 
	UIA_StatePattern_Toggled, 
	UIA_StatePattern_ReadOnly, 
	UIA_StatePattern_Invalid
};

enum UIA_LegacyProperties
{
	UIA_LegacyName = 0x1,
	UIA_LegacyValue,
	UIA_LegacyDefaultAction,
	UIA_LegacyHelp,
	UIA_LegacyDescription
};

#pragma pack(1)
struct SearchParameters
{
	int32_t scope;
	POINTER(name_size) char * name;
	int32_t name_size;


};
#pragma pack()

/**
* Find fisrt element that mathes search criteria
* @author Reuven, Abliyev
* @param[in] root, handle to start search from
* @param[in] sp, search parameters, when NULL returns next element in the hierarchy  
* @param[out] handle, first handle matches to search parameters
* @return 0 success, otherwise failure 
*/
UIAUTOMATION_API UIAUTOMATION_STATUS __cdecl UIAutomation_Find(enum UIA_FindType type, AutomationElementHandle start_handle, 
	IN struct SearchParameters * sp, int32_t n_handles, POINTER(n_handles) AutomationElementHandle * handles, int32_t * n_handles_required);


/**
* Release resources allocated to handle
* @author Reuven, Abliyev
* @param[in] handle, handle to relase
* @return 0 success, otherwise failure 
*/
UIAUTOMATION_API UIAUTOMATION_STATUS __cdecl UIAutomation_Release(AutomationElementHandle handle);

/**
	

/**
* Call Invoke on "handle" 
* @author Reuven, Abliyev
* @param[in] handle, handle to make invocation on
* @param[in] pattern, invocation pattern
* @return 0 success, otherwise failure 
*/
UIAUTOMATION_API UIAUTOMATION_STATUS __cdecl UIAutomation_Invoke(AutomationElementHandle handle, enum UIA_InvokePattern pattern);


/**
* Get text from handle
* @author Reuven, Abliyev
* @param[in] handle, handle to make invocation on
* @param[out] buffer, buffer to store text
* @param[in] buffer_size, buffer size, if buffer_size less than required XOSA_RES_SMALL_BUFFER returned
* @param[out] required_size, required buffer size, contains required buffer size when XOSA_RES_SMALL_BUFFER returned 
* @return 0 success, otherwise failure 
*/
UIAUTOMATION_API UIAUTOMATION_STATUS __cdecl UIAutomation_GetText(AutomationElementHandle handle, uint32_t text_size, IN POINTER(text_size) wchar_t * text, uint32_t * required_size);



/**
* Get state for contorl
* @author Reuven, Abliyev
* @param[in] handle, handle to make invocation on
* @param[in] pattern, state pattern
* @param[out] result, depends on state pattern
* @return 0 success, otherwise failure 
*/
UIAUTOMATION_API UIAUTOMATION_STATUS __cdecl  UIAutomation_GetState(AutomationElementHandle handle, enum UIA_StatePattern pattern, int32_t * result);

/**
* Set text to handle
* @author Reuven, Abliyev
* @param[in] handle, handle to make invocation on
* @param[in] buffer, buffer with text 
* @param[in] buffer_size, buffer size, 
* @return 0 success, otherwise failure 
*/
UIAUTOMATION_API UIAUTOMATION_STATUS __cdecl UIAutomation_SetText(AutomationElementHandle handle, OUT POINTER(text_size) wchar_t * text, uint32_t text_size);


/**
* Get property from handle
* @author Reuven, Abliyev
* @param[in] handle, handle to make invocation on
* @param[in] property_id, property to retrieve
* @param[out] buffer, buffer to store text
* @param[in] buffer_size, buffer size, if buffer_size less than required XOSA_RES_SMALL_BUFFER returned
* @param[out] required_size, required buffer size, contains required buffer size when XOSA_RES_SMALL_BUFFER returned 
* @return 0 success, otherwise failure 
*/
UIAUTOMATION_API UIAUTOMATION_STATUS __cdecl UIAutomation_GetPropertyValue(AutomationElementHandle handle, int32_t property_id, uint32_t text_size, IN POINTER(text_size) wchar_t * text, uint32_t * required_size);

/**
* Get parent element using TreeWalker.
* @param[in] handle, handle to make invocation on
* @param[out] parent, parent element 
* @return 0 success, otherwise failure
*/
UIAUTOMATION_API UIAUTOMATION_STATUS __cdecl UIAutomation_GetParent(AutomationElementHandle handle, AutomationElementHandle* parent);

/**
* Execute some method of get/set property of parent.
* @param[in] handle, handle to make invocation on
* @param[in] id, pattern id
* @param[in] method or property to invoke
* @param[in] signature of method
* @param[in] parameters of method
* @param[in out] result, buffer to store result
* @param[in] size, buffer size, if size less than required XOSA_RES_SMALL_BUFFER returned
* @param[out] req_size, required buffer size, contains required buffer size when XOSA_RES_SMALL_BUFFER returned
* @return 0 success, otherwise failure
*/
UIAUTOMATION_API UIAUTOMATION_STATUS __cdecl UIAutomation_CallPattern(AutomationElementHandle handle, int id, const wchar_t* method, const wchar_t* signature, const wchar_t* parms, uint32_t size, IN POINTER(size) wchar_t* result, uint32_t* req_size);


UIAUTOMATION_API UIAUTOMATION_STATUS __cdecl UIAutomation_MouseClick(int x, int y);
UIAUTOMATION_API UIAUTOMATION_STATUS __cdecl UIAutomation_SendKey(int code, bool up);
UIAUTOMATION_API UIAUTOMATION_STATUS __cdecl UIAutomation_SetFocus(void* hwnd);
//UIAUTOMATION_API UIAUTOMATION_STATUS __cdecl UIAutomation_ToastClick(uint32_t text_size, IN POINTER(text_size) wchar_t* text);
UIAUTOMATION_API UIAUTOMATION_STATUS __cdecl UIAutomation_ToastClick();
UIAUTOMATION_API UIAUTOMATION_STATUS __cdecl UIAutomation_ClickButtonOnWindow(char* wndname, char* btnname);

#endif
