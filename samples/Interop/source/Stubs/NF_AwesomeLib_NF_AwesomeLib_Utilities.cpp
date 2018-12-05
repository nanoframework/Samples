//-----------------------------------------------------------------------------
//
//                   ** WARNING! ** 
//    This file was generated automatically by a tool.
//    Re-running the tool will overwrite this file.
//    You should copy this file to a custom location
//    before adding any customization in the copy to
//    prevent loss of your changes when the tool is
//    re-run.
//
//-----------------------------------------------------------------------------


#include "NF_AwesomeLib.h"
#include "NF_AwesomeLib_NF_AwesomeLib_Utilities.h"

using namespace NF::AwesomeLib;

void Utilities::NativeGetHardwareSerial( CLR_RT_TypedArray_UINT8 param0, HRESULT &hr )
{
    if (param0.GetSize() < 12)
    { 
       hr=CLR_E_BUFFER_TOO_SMALL; 
       return; 
    } 
    
    // Device line                  - Starting address
    // F0, F3                       - 0x1FFFF7AC
    // F1                           - 0x1FFFF7E8
    // F2, F4                       - 0x1FFF7A10
    // F7                           - 0x1FF0F420
    // L0                           - 0x1FF80050
    // L0, L1 Cat.1,Cat.2           - 0x1FF80050
    // L1 Cat.3,Cat.4,Cat.5,Cat.6   - 0x1FF800D0
    memcpy((void*)param0.GetBuffer(), (const void*)0x1FF0F420, 12);  
}

