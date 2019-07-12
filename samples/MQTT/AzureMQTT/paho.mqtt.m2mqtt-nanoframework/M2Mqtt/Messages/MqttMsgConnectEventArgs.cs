﻿/*
Copyright (c) 2013, 2014 Paolo Patierno

All rights reserved. This program and the accompanying materials
are made available under the terms of the Eclipse Public License v1.0
and Eclipse Distribution License v1.0 which accompany this distribution. 

The Eclipse Public License is available at 
   http://www.eclipse.org/legal/epl-v10.html
and the Eclipse Distribution License is available at 
   http://www.eclipse.org/org/documents/edl-v10.php.

Contributors:
   Paolo Patierno - initial API and implementation and/or initial documentation
*/

#if (NANOFRAMEWORK_1_0)
using nanoFramework.Runtime.Events;
#elif (!MF_FRAMEWORK_VERSION_V4_2 && !MF_FRAMEWORK_VERSION_V4_3)
using System;
#else
using Microsoft.SPOT;
#endif

namespace uPLibrary.Networking.M2Mqtt.Messages
{
    /// <summary>
    /// Event Args class for CONNECT message received from client
    /// </summary>
    public class MqttMsgConnectEventArgs : EventArgs
    {
        /// <summary>
        /// Message received from client
        /// </summary>
        public MqttMsgConnect Message { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="msg">CONNECT message received from client</param>
        public MqttMsgConnectEventArgs(MqttMsgConnect connect)
        {
            this.Message = connect;
        }
    }
} 
