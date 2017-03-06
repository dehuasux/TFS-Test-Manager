/** @file xOSaSensorApi.h
 *  @brief xOSa API declaration
//--------------------------------------------------------------------------
//
// Copyright (c) Intel Corporation, 2013  All Rights Reserved.
//
//  File:       SensorApi.h
//
//  Contents:   Includes the xOSa Sensor API declaration
//
//--------------------------------------------------------------------------

For using GCC_XML please use it like this: (depends on your location)
gccxml.exe SensorApi.h -fxml=SensorApi.xml -DXOSA_PLUGIN_GENERATOR -I..\..\xOSaService\xOSaClientLib\Include

xOSaPluginUI.exe Sensor SensorApi.h SensorApi.xml
For using Plugin Generator
 */



#ifndef SENSOR_API_H
#define SENSOR_API_H

#ifdef XOSA_PLUGIN_GENERATOR
typedef int int32_t;
typedef unsigned int uint32_t;
typedef unsigned char uint8_t;
typedef unsigned long long uint64_t;
#define IN  __attribute((gccxml("in")))
#define OUT __attribute((gccxml("out")))
#define INOUT IN OUT
#define POINTER(size) __attribute((gccxml("pointer",#size)))
#define REGISTER_CALLBACK __attribute((gccxml("register_callback")))
#define UNION(type) __attribute((gccxml("uniontype",#type)))
#define UNIONLABEL(label) __attribute((gccxml("unionlabel",#label)))
#define CALLBACK_FUNCTION_ __attribute((gccxml("callback_function")))
#define UNREGISTER_CALLBACK(callback) __attribute((gccxml("unregister_callback", #callback)))
#define UNREGISTER_CALLBACK_PK __attribute((gccxml("unregister_callback_pk")))
#else
#define IN 
#define OUT 
#define POINTER(size)
#define REGISTER_CALLBACK
#define UNION(type)
#define UNIONLABEL(label)
#define CALLBACK_FUNCTION_ 
#define UNREGISTER_CALLBACK(callback)
#define UNREGISTER_CALLBACK_PK
#endif
#include <xOSa/PlatformDetection.h>

#if defined(XOSA_WINDOWS) && defined(SENSORAPI_EXPORTS)
	#define SENSOR_API __declspec(dllexport)
#elif defined(XOSA_WINDOWS) && !defined(SENSORAPI_EXPORTS)
	#define SENSOR_API
#else
	#define SENSOR_API
#endif

#ifdef XOSA_GCC
#define __cdecl __attribute__((__cdecl__))
#endif

#include <xOSa/Types.h>




#ifdef __cplusplus
extern "C"{
#endif

#pragma region Sensort Interface

#pragma pack(1)


#define SENSOR_PROPERTY_TYPE_DOUBLE_SIZE		(sizeof(double)) // double size
#define VECTOR_SIZE_LENGTH						(1) // when vector is passed, the number of elemnts in the vector is the first byte in the data array
#define SENSOR_QUATERNION_VECTOR_ELEMENTS_NUM   (4)
#define SENSOR_QUATERNION_VECTOR_SIZE			(SENSOR_QUATERNION_VECTOR_ELEMENTS_NUM * SENSOR_PROPERTY_TYPE_DOUBLE_SIZE)

#define SENSOR_ROTATION_MATRIX_VECTOR_ELEMENTS_NUM   (9)
#define SENSOR_ROTATION_MATRIX_VECTOR_SIZE      (SENSOR_ROTATION_MATRIX_VECTOR_ELEMENTS_NUM * SENSOR_PROPERTY_TYPE_DOUBLE_SIZE)


typedef enum PropertyIds
{
	PROPERTY_ID_0 = 0,
	PROPERTY_ID_1,
	PROPERTY_ID_2,
	PROPERTY_ID_3,
	PROPERTY_ID_MANUFACTURER = PROPERTY_ID_3,
	PROPERTY_ID_4,
	PROPERTY_ID_5,
	PROPERTY_ID_6,
	PROPERTY_ID_FRIENDLY_NAME = PROPERTY_ID_6,
	PROPERTY_ID_7,
	PROPERTY_ID_8,
	PROPERTY_ID_9,
	PROPERTY_ID_MIN_REPORT_INTERVAL = PROPERTY_ID_9,
	PROPERTY_ID_10,
	PROPERTY_ID_11,
	PROPERTY_ID_12,
	PROPERTY_ID_13,
	PROPERTY_ID_14,
	PROPERTY_ID_15,
	PROPERTY_ID_RESOLUTION = PROPERTY_ID_15,
	PROPERTY_ID_16,
	PROPERTY_ID_17,
	PROPERTY_ID_18,
	PROPERTY_ID_19,
	PROPERTY_ID_20,
	PROPERTY_ID_21,
	PROPERTY_ID_22,
	PROPERTY_ID_23,
	PROPERTY_ID_24,
	PROPERTY_ID_25,
	PROPERTY_ID_26,
	PROPERTY_ID_27,
	PROPERTY_ID_28,
	PROPERTY_ID_29,
	PROPERTY_ID_30,
	PROPERTY_ID_31,
	PROPERTY_ID_32,
	PROPERTY_ID_33,
	PROPERTY_ID_34,
	PROPERTY_ID_35,
	PROPERTY_ID_36,
	PROPERTY_ID_37,
	PROPERTY_ID_38,
	PROPERTY_ID_39,
	PROPERTY_ID_40,
	PROPERTY_ID_41,
	PROPERTY_ID_42,
	PROPERTY_ID_43,
	PROPERTY_ID_44,
	PROPERTY_ID_45,
	PROPERTY_ID_46,
	PROPERTY_ID_47,
	PROPERTY_ID_48,
	PROPERTY_ID_49,
	PROPERTY_ID_50,
	PROPERTY_ID_51,
	PROPERTY_ID_52,
	PROPERTY_ID_53,
	PROPERTY_ID_54,
	PROPERTY_ID_55,
	PROPERTY_ID_56,
	PROPERTY_ID_57,
	PROPERTY_ID_58,
	PROPERTY_ID_59,
	PROPERTY_ID_60,
	PROPERTY_ID_61,
	PROPERTY_ID_62,
	PROPERTY_ID_63,
	PROPERTY_ID_PRESSURE = PROPERTY_ID_63,
	PROPERTY_ID_64,
	PROPERTY_ID_65,
	PROPERTY_ID_66,
	PROPERTY_ID_ACCELEROMETER_X = PROPERTY_ID_66,
	PROPERTY_ID_67,
	PROPERTY_ID_ACCELEROMETER_Y = PROPERTY_ID_67,
	PROPERTY_ID_68,
	PROPERTY_ID_ACCELEROMETER_Z = PROPERTY_ID_68,
	PROPERTY_ID_69,
	PROPERTY_ID_ROTATION_X = PROPERTY_ID_69,
	PROPERTY_ID_70,
	PROPERTY_ID_ROTATION_Y = PROPERTY_ID_70,
	PROPERTY_ID_71,
	PROPERTY_ID_ROTATION_Z = PROPERTY_ID_71,
	PROPERTY_ID_72,
	PROPERTY_ID_73,
	PROPERTY_ID_MOTION_STATE = PROPERTY_ID_73,
	PROPERTY_ID_SIGNIFICANT_MOTION = PROPERTY_ID_73,
	PROPERTY_ID_74,
	PROPERTY_ID_GYROSCOPE_FIELD_X = PROPERTY_ID_74,
	PROPERTY_ID_75,
	PROPERTY_ID_GYROSCOPE_FIELD_Y = PROPERTY_ID_75,
	PROPERTY_ID_76,
	PROPERTY_ID_GYROSCOPE_FIELD_Z = PROPERTY_ID_76,
	PROPERTY_ID_77,
	PROPERTY_ID_GAME_ROTATION_X = PROPERTY_ID_77,
	PROPERTY_ID_78,
	PROPERTY_ID_GAME_ROTATION_Y = PROPERTY_ID_78,
	PROPERTY_ID_79,
	PROPERTY_ID_GAME_ROTATION_Z = PROPERTY_ID_79,
	PROPERTY_ID_80,
	PROPERTY_ID_MAGNETIC_FIELD_X = PROPERTY_ID_80,
	PROPERTY_ID_81,
	PROPERTY_ID_MAGNETIC_FIELD_Y = PROPERTY_ID_81,
	PROPERTY_ID_82,
	PROPERTY_ID_MAGNETIC_FIELD_Z = PROPERTY_ID_82,
	PROPERTY_ID_83,
	PROPERTY_ID_84,
	PROPERTY_ID_85,
	PROPERTY_ID_86,
	PROPERTY_ID_87,
	PROPERTY_ID_88,
	PROPERTY_ID_89,
	PROPERTY_ID_90,
	PROPERTY_ID_91,
	PROPERTY_ID_92,
	PROPERTY_ID_93,
	PROPERTY_ID_94,
	PROPERTY_ID_95,
	PROPERTY_ID_96,
	PROPERTY_ID_97,
	PROPERTY_ID_98,
	PROPERTY_ID_99,
	PROPERTY_ID_100,
	PROPERTY_ID_101,
	PROPERTY_ID_102,
	PROPERTY_ID_103,
	PROPERTY_ID_104,
	PROPERTY_ID_105,
	PROPERTY_ID_106,
	PROPERTY_ID_107,
	PROPERTY_ID_LIGHT_PROXIMITY = PROPERTY_ID_107,
	PROPERTY_ID_108,
	PROPERTY_ID_109,
	PROPERTY_ID_LIGHT_LEVEL = PROPERTY_ID_109,
	PROPERTY_ID_110,
	PROPERTY_ID_111,
	PROPERTY_ID_112,
	PROPERTY_ID_113,
	PROPERTY_ID_114,
	PROPERTY_ID_115,
	PROPERTY_ID_116,
	PROPERTY_ID_117,
	PROPERTY_ID_118,
	PROPERTY_ID_119,
	PROPERTY_ID_120,
	PROPERTY_ID_121,
	PROPERTY_ID_122,
	PROPERTY_ID_123,
	PROPERTY_ID_124,
	PROPERTY_ID_125,
	PROPERTY_ID_126,
	PROPERTY_ID_127,
	PROPERTY_ID_128,
	PROPERTY_ID_129,
	PROPERTY_ID_130,
	PROPERTY_ID_131,
	PROPERTY_ID_132,
	PROPERTY_ID_133,
	PROPERTY_ID_134,
	PROPERTY_ID_135,
	PROPERTY_ID_136,
	PROPERTY_ID_137,
	PROPERTY_ID_138,
	PROPERTY_ID_139,
	PROPERTY_ID_140,
	PROPERTY_ID_141,
	PROPERTY_ID_142,
	PROPERTY_ID_143,
	PROPERTY_ID_144,
	PROPERTY_ID_145,
	PROPERTY_ID_146,
	PROPERTY_ID_147,
	PROPERTY_ID_148,
	PROPERTY_ID_149,
	PROPERTY_ID_150,
	PROPERTY_ID_151,
	PROPERTY_ID_152,
	PROPERTY_ID_153,
	PROPERTY_ID_154,
	PROPERTY_ID_155,
	PROPERTY_ID_156,
	PROPERTY_ID_157,
	PROPERTY_ID_158,
	PROPERTY_ID_159,
	PROPERTY_ID_MAX_GUID = PROPERTY_ID_159, // the max property ID of a property that has a GUID (Non Android property)
	PROPERTY_ID_STEPS,
	PROPERTY_ID_ROTATION_SCALLAR,
	PROPERTY_ID_GYROSCOPE_DRIFT_X,
	PROPERTY_ID_GYROSCOPE_DRIFT_Y,
	PROPERTY_ID_GYROSCOPE_DRIFT_Z,
	PROPERTY_ID_STEP_DETECTOR, 
	PROPERTY_ID_MAGNETIC_UNCALIBRATED_IRON_X,
	PROPERTY_ID_MAGNETIC_UNCALIBRATED_IRON_Y,
	PROPERTY_ID_MAGNETIC_UNCALIBRATED_IRON_Z,
	PROPERTY_ID_SLEEP,
	PROPERTY_ID_DEEP_MOTIONLESS,
	PROPERTY_ID_MOTIONLESS,
	PROPERTY_ID_SEMISTATIONARY,
	PROPERTY_ID_CHUB1,
	PROPERTY_ID_CHUB2,
	PROPERTY_ID_MAGNETIC_FIELD,
	PROPERTY_ID_GYROSCOPE,
	PROPERTY_ID_PROXIMITY,
	PROPERTY_ID_GRAVITY,
	PROPERTY_ID_LINEAR_ACCELERATION,
	PROPERTY_ID_GEOMAGNETIC_ROTATION,
	PROPERTY_ID_GAME_ROTATION,
	PROPERTY_ID_ROTATION,
	PROPERTY_ID_SCALAR,
	PROPERTY_ID_GYROSCOPE_UNCLIBRATED,
	PROPERTY_ID_DRIFT_X,
	PROPERTY_ID_DRIFT_Y,
	PROPERTY_ID_DRIFT_Z,
	PROPERTY_ID_MAGNETIC_UNCALIBRATED,
	PROPERTY_ID_IRON_X,
	PROPERTY_ID_IRON_Y,
	PROPERTY_ID_IRON_Z,
	PROPERTY_ID_ACCELEROMETER,
	PROPERTY_ID_X,
	PROPERTY_ID_Y,
	PROPERTY_ID_Z,

	PROPERTY_ID_STRING_TYPE,
	PROPERTY_ID_MAX_REPORT_INTERVAL,
	PROPERTY_ID_STEPS_DURATION,
	PROPERTY_ID_STEP_KIND,
	PROPERTY_ID_ACTIVITY_TYPE,
	PROPERTY_ID_ACTIVITY_CONFIDENCE,
	PROPERTY_ID_MAX_DEFINED,
	PROPERTY_ID_MAX = 255,
	PROPERTY_ID_INVALID = 255,
} PropertyIds_e;



typedef enum SENSOR_STATUS
{
	SENSOR_STATUS_OK,
	SENSOR_STATUS_ERROR,
	SENSOR_STATUS_TYPE_NOT_SUPPORTED,
	SENSOR_STATUS_NOT_EXIST,
	SENSOR_STATUS_INVALID_HANDLE,
	SENSOR_STATUS_INVALID_PARAMETER,
	SENSOR_STATUS_INSUFFICIENT_BUFFER,
	SENSOR_STATUS_INVALID_VALUE,
	SENSOR_STATUS_AGAIN,
	SENSOR_STATUS_TIMEOUT,
	SENSOR_STATUS_ACTION_NOT_SUPPORTED,
	SENSOR_STATUS_PROPERTY_ERROR
} SENSOR_STATUS;

typedef enum SENSOR_TYPE
{
	SENSOR_TYPE_INVALID,
	SENSOR_TYPE_ALL,
	SENSOR_TYPE_ACCELEROMETER,
	SENSOR_TYPE_MAGNETIC_FIELD,
	SENSOR_TYPE_GYROSCOPE,
	SENSOR_TYPE_LIGHT,
	SENSOR_TYPE_PROXIMITY,
	SENSOR_TYPE_GPS,
	SENSOR_TYPE_AMBIENT_TEMPERATURE,
	SENSOR_TYPE_PRESSURE,
	SENSOR_TYPE_RELATIVE_HUMIDITY,
	SENSOR_TYPE_MOTION,
	SENSOR_TYPE_INCLINOMETER,
	SENSOR_TYPE_MAGNETIC_FIELD_2D,
	SENSOR_TYPE_DEVICE_ORIENTATION,
	SENSOR_TYPE_SIMPLE_DEVICE_ORIENTATION,
	SENSOR_TYPE_CUSTOM,
	SENSOR_TYPE_HUMAN_PRESENCE,
	SENSOR_TYPE_UNKNOWN,
	SENSOR_TYPE_LINEAR_ACCELERATION, //for Android
	SENSOR_TYPE_ROTATION_VECTOR, //for Android
	SENSOR_TYPE_GRAVITY, //for Android
	SENSOR_TYPE_MAGNETIC_FIELD_UNCALIBRATED, //for Android
	SENSOR_TYPE_GAME_ROTATION_VECTOR,   //for Android     
	SENSOR_TYPE_GYROSCOPE_UNCALIBRATED,  //for Android    
	SENSOR_TYPE_SIGNIFICANT_MOTION,  //for Android        
	SENSOR_TYPE_STEP_DETECTOR,  //for Android             
	SENSOR_TYPE_STEP_COUNTER,   //for Android             
	SENSOR_TYPE_GEOMAGNETIC_ROTATION_VECTOR,  //for Android
	SENSOR_TYPE_ACCELEROMETER_UNCALIBRATED,   //for Android
	SENSOR_TYPE_MOTION_DETECT,   //for Android
	SENSOR_TYPE_NON_WAKE_UP_PROXIMITY_SENSOR, 
	SENSOR_TYPE_WAKE_UP_ACCELEROMETER = 40, 			// Android L 
	SENSOR_TYPE_WAKE_UP_MAGNETIC_FIELD, 				// Android L 
	SENSOR_TYPE_WAKE_UP_ORIENTATION, 					// Android L 
	SENSOR_TYPE_WAKE_UP_GYROSCOPE, 						// Android L 
	SENSOR_TYPE_WAKE_UP_LIGHT, 							// Android L 
	SENSOR_TYPE_WAKE_UP_PRESSURE, 						// Android L 
	SENSOR_TYPE_WAKE_UP_GRAVITY, 						// Android L 
	SENSOR_TYPE_WAKE_UP_LINEAR_ACCELERATION, 			// Android L 
	SENSOR_TYPE_WAKE_UP_ROTATION_VECTOR, 				// Android L 
	SENSOR_TYPE_WAKE_UP_RELATIVE_HUMIDITY, 				// Android L 
	SENSOR_TYPE_WAKE_UP_AMBIENT_TEMPERATURE, 			// Android L 
	SENSOR_TYPE_WAKE_UP_MAGNETIC_FIELD_UNCALIBRATED, 	// Android L 
	SENSOR_TYPE_WAKE_UP_GAME_ROTATION_VECTOR, 			// Android L 
	SENSOR_TYPE_WAKE_UP_GYROSCOPE_UNCALIBRATED, 		// Android L 
	SENSOR_TYPE_WAKE_UP_STEP_DETECTOR, 					// Android L 
	SENSOR_TYPE_WAKE_UP_STEP_COUNTER, 					// Android L 
	SENSOR_TYPE_WAKE_UP_GEOMAGNETIC_ROTATION_VECTOR, 	// Android L 
	SENSOR_TYPE_WAKE_UP_HEART_RATE, 					// Android L 
	SENSOR_TYPE_WAKE_UP_TILT_DETECTOR,					// Android L 
	SENSOR_TYPE_ACTIVITY,
	SENSOR_TYPE_ALTIMETER,
	SENSOR_TYPE_PEDOMETER

} SENSOR_TYPE;

typedef enum SENSOR_STATE
{
  SENSOR_STATE_READY = 0,
  SENSOR_STATE_NOT_AVAILABLE,
  SENSOR_STATE_NO_DATA,
  SENSOR_STATE_INITIALIZING,
  SENSOR_STATE_ACCESS_DENIED,
  SENSOR_STATE_ERROR,
  SENSOR_STATE_REMOVED = 1000
} SENSOR_STATE;

typedef uint32_t SENSOR_HANDLE;

#define MAX_SENSOR_PROPERTY_STRING 256
#define MAX_SENSOR_PROPERTY_VECTOR_SIZE 80
typedef enum SENSOR_PROPERTY_TYPE
{
	SENSOR_PROPERTY_TYPE_NONE,
	SENSOR_PROPERTY_TYPE_NUMBER,
	SENSOR_PROPERTY_TYPE_STRING,
	SENSOR_PROPERTY_TYPE_NUMBER_VECTOR
} SENSOR_PROPERTY_TYPE;

/**
 *  A struct representig a sensor property with key and data.
 */
typedef struct SensorProperty
{
	unsigned char fatherId;   /* enum representing the father property. Every property ID is maped to a Property GUID, name, Type and size */

	unsigned char sonId;      /* enum representing the son property. Every property ID is maped to a Property GUID, name, Type and size */

	SENSOR_PROPERTY_TYPE type; /*!< type of the data */

	int  sizeDataBuffer;       /* size of dataBuffer */
	
	POINTER(sizeDataBuffer) unsigned char *dataBuffer;  /* data buffer of size sizeDataBuffer which includs the property */
} SensorProperty;

#define MAX_STRINGDATA 260

typedef struct _StringData
{
	char text[MAX_STRINGDATA];
} StringData;

/***
Remarks  regarding Sensor API

1. If you don't know which value provide to the function parameter use:
      - NULL for pointers
      - 0 for others
*/

/**
* Retrieve sensor instance
* @param[in] type the type of the requested sensor
* @param[in] index the index of the requested sensor type
* @param[out] handle xOSa handle to the requested sensor

* @return 0 success, otherwise failure
*/
SENSOR_API SENSOR_STATUS __cdecl GetInstance(SENSOR_TYPE type, uint32_t index, SENSOR_HANDLE *handle, uint32_t *nativeErrorCode);

/**
* Retrieve custom sensors id's through Win10RT framework
* @param[in] list of guids to test
* @param[out] list of available ld's
* @return 0 success, otherwise failure
*/

SENSOR_API SENSOR_STATUS __cdecl GetWin10RtAvailableCustomSensors(POINTER(numtest) StringData *test, uint32_t numtest, POINTER(*requestedNum) StringData *ids, uint32_t *requestedNum);

/**
* Retrieve custom sensor instance through Win10RT framework
* @param[in] guid the guid of the requested sensor
* @param[out] handle xOSa handle to the requested sensor
* @return 0 success, otherwise failure
*/
SENSOR_API SENSOR_STATUS __cdecl GetWin10RtCustomInstance(const char* guid, SENSOR_HANDLE *handle);

/**
* Retrieve sensor instance through WinRT framework
* @param[in] type the type of the requested sensor
* @param[out] handle xOSa handle to the requested sensor
* @return 0 success, otherwise failure
*/
SENSOR_API SENSOR_STATUS __cdecl GetWinRtInstance(SENSOR_TYPE type, SENSOR_HANDLE *handle);

/**
* Retrieve all sensor instances of a specific type
* @param[in] type the type of the requested sensors. It can be all type
* @param[out] handles xOSa handles to the requested sensors
* @param[in,out] requestedNum the number of requested sensors. If 0 then the method returns the number of available sensors
* @param[out] nativeErrorCode native error code
* @return 0 success, otherwise failure
*/
SENSOR_API SENSOR_STATUS __cdecl GetAllInstances(SENSOR_TYPE type, POINTER(*requestedNum) SENSOR_HANDLE *handles, uint32_t *requestedNum, uint32_t *nativeErrorCode);

/**
* Release sensor instance related resources used by xOSa
* @author
* @param[in] handle xOSa handle to a specific sensor as returned by GetInstance method
* @return 0 success, otherwise failure
*/
SENSOR_API SENSOR_STATUS __cdecl ReleaseInstance(SENSOR_HANDLE handle);

/**
* Retrieve sensor type
* @author
* @param[in] handle xOSa handle to a specific sensor as returned by GetInstance method
* @param[out] type the type of the sensor related to handle
* @return 0 success, otherwise failure
*/
SENSOR_API SENSOR_STATUS __cdecl GetType(SENSOR_HANDLE handle, SENSOR_TYPE *type);

/**
* Retrieve sensor name
* @author
* @param[in] handle xOSa handle to a specific sensor as returned by GetInstance method
* @param[out] name the name of the sensor related to handle
* @param[in] bufferSize size of the buffer where the output string will be stored
* @return 0 success, otherwise failure
*/
SENSOR_API SENSOR_STATUS __cdecl GetName(SENSOR_HANDLE handle, POINTER(bufferSize) char *name, uint32_t bufferSize);

/**
* Retrieve sensor vendor
* @author
* @param[in] handle xOSa handle to a specific sensor as returned by GetInstance method
* @param[out] vendor the vendor of the sensor related to handle
* @param[in] bufferSize size of the buffer where the output string will be stored
* @return 0 success, otherwise failure
*/
SENSOR_API SENSOR_STATUS __cdecl GetVendor(SENSOR_HANDLE handle, POINTER(bufferSize) char *vendor, uint32_t bufferSize);

/**
* Retrieve sensor minimum interval
* @author
* @param[in] handle xOSa handle to a specific sensor as returned by GetInstance method
* @param[out] minInterval the minimum interval supported by the sensor in microseconds
* @return 0 success, otherwise failure
*/
SENSOR_API SENSOR_STATUS __cdecl GetMinimumReportInterval(SENSOR_HANDLE handle, uint32_t *minInterval);

/**
* Retrieve sensor interval
* @author
* @param[in] handle xOSa handle to a specific sensor as returned by GetInstance method
* @param[out] interval the current interval of the sensor in microseconds
* @return 0 success, otherwise failure
*/
SENSOR_API SENSOR_STATUS __cdecl GetReportInterval(SENSOR_HANDLE handle, uint32_t *interval);

/**
* Set sensor interval
* @author
* @param[in] handle xOSa handle to a specific sensor as returned by GetInstance method
* @param[in] interval the desired interval for the sensor in microseconds
* @return 0 success, otherwise failure
*/
SENSOR_API SENSOR_STATUS __cdecl SetReportInterval(SENSOR_HANDLE handle, uint32_t interval);

/**
* Set sensor max latency
* @author
* @param[in] handle xOSa handle to a specific sensor as returned by GetInstance method
* @param[in] maxLatency the desired max latency for the sensor in microseconds
* @return 0 success, otherwise failure
*/
SENSOR_API SENSOR_STATUS __cdecl SetMaxLatency(SENSOR_HANDLE handle, uint32_t maxLatency);

/**
* Retrieve sensor resolution
* @author
* @param[in] handle xOSa handle to a specific sensor as returned by GetInstance method
* @param[out] properties array of properties to be filled by the function
* @param[in, out] requestedNum the number of requested properties of the sensor
* @return 0 success, otherwise failure
*/
SENSOR_API SENSOR_STATUS __cdecl GetResolution(SENSOR_HANDLE handle,  POINTER(*requestedNum) SensorProperty *properties, uint32_t *requestedNum);
/**
* Retrieve sensor sensitivity
* @author
* @param[in] handle xOSa handle to a specific sensor as returned by GetInstance method
* @param[out] properties array of properties to be filled by the function
* @param[in, out] requestedNum the number of requested properties of the sensor
* @return 0 success, otherwise failure
*/
SENSOR_API SENSOR_STATUS __cdecl GetSensitivity(SENSOR_HANDLE handle,  POINTER(*requestedNum) SensorProperty *properties, uint32_t *requestedNum);

/**
* Set sensor sensitivity
* @author
* @param[in] handle xOSa handle to a specific sensor as returned by GetInstance method
* @param[in] index axe index
* @param[in] sensitivity the desired sensitivity of the sensor
* @return 0 success, otherwise failure
*/
SENSOR_API SENSOR_STATUS __cdecl SetSensitivity(SENSOR_HANDLE handle, uint8_t index, double sensitivity);

/**
* Retrieve sensor accuracy
* @author
* @param[in] handle xOSa handle to a specific sensor as returned by GetInstance method
* @param[out] properties array of properties to be filled by the function
* @param[in, out] requestedNum the number of requested properties of the sensor
* @return 0 success, otherwise failure
*/
SENSOR_API SENSOR_STATUS __cdecl GetAccuracy(SENSOR_HANDLE handle,  POINTER(*requestedNum) SensorProperty *properties, uint32_t *requestedNum);

/**
* Retrieve sensor maximum range
* @author
* @param[in] handle xOSa handle to a specific sensor as returned by GetInstance method
* @param[out] properties array of properties to be filled by the function
* @param[in, out] requestedNum the number of requested properties of the sensor
* @return 0 success, otherwise failure
*/
SENSOR_API SENSOR_STATUS __cdecl GetMaximumRange(SENSOR_HANDLE handle,  POINTER(*requestedNum) SensorProperty *properties, uint32_t *requestedNum);

/**
*  Retrieve sensor minimum range
* @author
* @param[in] handle xOSa handle to a specific sensor as returned by GetInstance method
* @param[out] properties array of properties to be filled by the function
* @param[in, out] requestedNum the number of requested properties of the sensor
* @return 0 success, otherwise failure
*/
SENSOR_API SENSOR_STATUS __cdecl GetMinimumRange(SENSOR_HANDLE handle,  POINTER(*requestedNum) SensorProperty *properties, uint32_t *requestedNum);

/**
* Retrieve the number of (flatten) properties of a sensor
* @author
* @param[in] handle xOSa handle to a specific sensor as returned by GetInstance method
* @param[out] num the number of (flatten) properties of the sensor
* @return 0 success, otherwise failure
*/
SENSOR_API SENSOR_STATUS __cdecl GetNumOfFlattenProperties(SENSOR_HANDLE handle, uint32_t *num);

/**
* Retrieve last error of a sensor
* @author
* @param[in] handle xOSa handle to a specific sensor as returned by GetInstance method
* @param[out] last error to be filled by the function
* @param[out] buffer size of the error message
* @return 0 success, otherwise failure
*/
SENSOR_API SENSOR_STATUS __cdecl GetLastErrorSensor(SENSOR_HANDLE handle, POINTER(bufferSize) char *error, uint32_t bufferSize);
/**
* Retrieve properties of a sensor
* @author
* @param[in] handle xOSa handle to a specific sensor as returned by GetInstance method
* @param[out] properties array of properties to be filled by the function
* @param[in, out] requestedNum the number of requested properties of the sensor
* @return 0 success, otherwise failure
*/
SENSOR_API SENSOR_STATUS __cdecl GetAllProperties(SENSOR_HANDLE handle, POINTER(*requestedNum) SensorProperty *properties, uint32_t *requestedNum);

/**
* Retrieve property with the specified key from a sensor 
* @author
* @param[in] handle xOSa handle to a specific sensor as returned by GetInstance method
* @param[in] key key of the requested property
* @param[out] property the requested property of the sensor
* @return 0 success, otherwise failure
*/
SENSOR_API SENSOR_STATUS __cdecl GetPropertyByKey(SENSOR_HANDLE handle, char *key, SensorProperty *prop);

/**
* Retrieve state of a sensor 
* @author
* @param[in] handle xOSa handle to a specific sensor as returned by GetInstance method
* @param[out] state state of the sensor
* @return 0 success, otherwise failure
*/
SENSOR_API SENSOR_STATUS __cdecl GetState(SENSOR_HANDLE handle, SENSOR_STATE *state);

/**
* Callback data
*/

#define MAX_SENSOR_PROPERTY_DATA 20
#define MAX_SENSOR_EVENTS 10000

struct PollingData
{
	/// handle registered with StartPolling
	SENSOR_HANDLE handle;
	/// handle type
	enum SENSOR_TYPE type;
	/// data timestamp in nano
	int64_t timestamp;
	/// data os timestamp
	int64_t systemTimestamp;
	// number of properties in data
	int  propertiesNum;
	/// sensor data
	POINTER(propertiesNum) SensorProperty* data;  /* data buffer of size sizeDataBuffer which includs the property */
};

/**
* Callback type for polling operations

* @param[in] handle, registered with AddNewDataCallback
* @param[in] data, sensor data buffer
* @return void
*/
typedef int32_t (*POLLING_CALLBACK)(struct PollingData * data);

/**
* Add callback for new data events

* @param[in] handle to listen
* @param[in] listening delay, FUTURE USE
* @param[in] iterations, FUTURE USE
* @param[in] data ready callback, function to call when event arrives
* @return 0 success, otherwise failure
*/
SENSOR_API SENSOR_STATUS __cdecl AddNewDataCallback(SENSOR_HANDLE handle, size_t polling_delay, size_t iterations, POLLING_CALLBACK cb);
SENSOR_API SENSOR_STATUS __cdecl AddNullCallback(SENSOR_HANDLE handle, char* file );

/**
*  Remove callback for new data events
*
* @param[in]
* @return 0 success, SENSOR_STATUS_AGAIN retry later, otherwise failure
*/
SENSOR_API SENSOR_STATUS __cdecl RemoveNewDataCallback(SENSOR_HANDLE handle, POLLING_CALLBACK cb);


/**
* Polling sensor for events
* @param[in] handle
* @param[in] timeout in milliseconds
* @param[out] sensor data
* @return 0 success, otherwise SENSOR_STATUS_AGAIN or failure
*/
SENSOR_API SENSOR_STATUS __cdecl Poll(SENSOR_HANDLE handle, uint32_t timeout, struct PollingData * data);


/**
* Get last sensor events
* @param[in] handle
* @param[in] num of sensor data requested
* @param[out] num of sensor data returned
* @param[out] sensor data array
* @return 0 success, otherwise SENSOR_STATUS_AGAIN or failure
*/
SENSOR_API SENSOR_STATUS __cdecl GetLastDataEvents(SENSOR_HANDLE handle, int numEvents, int* retEvents, struct PollingData * data);


/**
* free Polling data structure
* @param[in] handle
* @param[in] sensor data
* @return 0 success, otherwise SENSOR_STATUS_AGAIN or failure
*/
SENSOR_API SENSOR_STATUS __cdecl FreePollingData (SENSOR_HANDLE handle, struct PollingData* data, bool freeServerMemory);

/**
* Callback type for added sensor events
* @param[in] handle of the added sensor
* @return void
*/
typedef void (*ADDED_SENSOR_CALLBACK)(SENSOR_HANDLE handle);

/**
* Add callback for added sensor events
* @param[in] callback function to call when state changes
* @return 0 success, otherwise failure
*/
SENSOR_API SENSOR_STATUS __cdecl AddAddedSensorCallback(ADDED_SENSOR_CALLBACK cb);

/**
* Remove callback for added sensor events
* @param[in] callback function to be removed
* @return 0 success, otherwise failure
*/
SENSOR_API SENSOR_STATUS __cdecl RemoveAddedSensorCallback(ADDED_SENSOR_CALLBACK cb);

/**
* Callback type for state change events
* @param[in] handle, registered with AddStateChangeCallback
* @param[in] state, sensor state
* @return void
*/
typedef void (*STATE_CHANGE_CALLBACK)(SENSOR_HANDLE handle, SENSOR_STATE state);

/**
* Add callback for state change events
* @param[in] handle handle to the sensor to listen
* @param[in] callback function to call when state changes
* @return 0 success, otherwise failure
*/
SENSOR_API SENSOR_STATUS __cdecl AddStateChangeCallback(SENSOR_HANDLE handle, STATE_CHANGE_CALLBACK cb);

/**
* Remove callback for state change events
* @param[in] handle handle to the sensor 
* @param[in] callback function to be removed
* @return 0 success, otherwise failure
*/
UNREGISTER_CALLBACK(AddStateChangeCallback) 
SENSOR_API SENSOR_STATUS __cdecl RemoveStateChangeCallback(SENSOR_HANDLE handle, STATE_CHANGE_CALLBACK cb);


/**
* Callback type for shake events
* @param[in] timestamp, when the event happened
* @return void
*/
typedef void (*SHAKE_CALLBACK)(int64_t timestamp);

/**
* Add callback for shake events
* @param[in] callback function to call when there is a shake event
* @return 0 success, otherwise failure
*/
SENSOR_API SENSOR_STATUS __cdecl AddShakeCallback(SHAKE_CALLBACK cb);

/**
* Remove callback for shake events
* @param[in] callback function to be removed
* @return 0 success, otherwise failure
*/
SENSOR_API SENSOR_STATUS __cdecl RemoveShakeCallback(SHAKE_CALLBACK cb);

#pragma pack()

#pragma endregion

#ifdef __cplusplus
	}
#endif

#endif
