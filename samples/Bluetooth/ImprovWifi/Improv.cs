//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Text;
using nanoFramework.Device.Bluetooth;
using nanoFramework.Device.Bluetooth.GenericAttributeProfile;
using System.Net.NetworkInformation;
using System.Device.Wifi;
using nanoFramework.Networking;

namespace ImprovWifi
{
    public class Improv
    {
        /// <summary>
        /// Improv error states.
        /// </summary>
        public enum ImprovError
        {
            /// <summary>
            /// This shows there is no current error state.
            /// </summary>
            noError,
            /// <summary>
            /// RPC packet was malformed/invalid.
            /// </summary>
            invalidRpcPacket,
            /// <summary>
            /// The command sent is unknown.
            /// </summary>
            unknownRpcPacket,
            /// <summary>
            /// The credentials have been received and an attempt to connect to the network has failed.
            /// </summary>
            unableConnect,
            /// <summary>
            /// Credentials were sent via RPC but the Improv service is not authorized.
            /// </summary>
            notAuthorised,
            /// <summary>
            /// Unknown error
            /// </summary>
            unknownError
        };

        /// <summary>
        /// Improv provisioning state
        /// </summary>
        public enum ImprovState
        {
            /// <summary>
            /// Awaiting authorization via physical interaction.
            /// </summary>
            authorizationRequired = 1,
            /// <summary>
            /// Ready to accept credentials.
            /// </summary>
            authorized = 2,
            /// <summary>
            /// Credentials received, attempt to connect.
            /// </summary>
            provisioning = 3,
            /// <summary>
            /// Connection successful.
            /// </summary>
            provisioned = 4
        };

        private GattServiceProvider _serviceProvider;
        private GattLocalCharacteristic _characteristicCurrentState;
        private GattLocalCharacteristic _characteristicErrorState;
        private GattLocalCharacteristic _characteristicRpcCommand;
        private GattLocalCharacteristic _characteristicRpcResult;

        public delegate void OnIdentifyEventDelegate(object sender, EventArgs e);
        public delegate void OnProvisionedEventDelegate(object sender, ProvisionedEventArgs e);
        public delegate void OnProvisioningCompleteEventDelegate(object sender, EventArgs e);
        
        public event OnIdentifyEventDelegate OnIdentify;
        public event OnProvisionedEventDelegate OnProvisioned;
        public event OnProvisioningCompleteEventDelegate OnProvisioningComplete;

        private bool _started = false;
        private ImprovState _currentState;
        private ImprovError _errorState;
        private string _rpcResult = "";

        /// <summary>
        /// Constructor for IMPROV service.
        /// </summary>
        public Improv()
        {
            Initialise();
        }

        /// <summary>
        /// Start the Improv service.
        /// </summary>
        /// <param name="deviceName">Name of device in Bluetooth advert.</param>
        public void Start(string deviceName)
        {
            if (!_started)
            {
                _serviceProvider.StartAdvertising(new GattServiceProviderAdvertisingParameters() { DeviceName = deviceName, IsConnectable = true, IsDiscoverable = true });
                _started = true;
            }
        }

        /// <summary>
        /// Stop the Improv service.
        /// </summary>
        public void Stop()
        {
            if (_started)
            {
                _serviceProvider.StopAdvertising();
                _started = false;
            }
        }

        /// <summary>
        /// Authorise/UnAuthorise the Improv service.
        /// </summary>
        /// <param name="auth">True to Authorise</param>
        public void Authorise(bool auth)
        {
            if (auth)
            {
                if (CurrentState == ImprovState.authorizationRequired)
                {
                    CurrentState = ImprovState.authorized;
                }
            }
            else
            {
                if (CurrentState == ImprovState.authorized)
                {
                    CurrentState = ImprovState.authorizationRequired;
                }
            }
        }

        #region Properties

        /// <summary>
        /// Get current state.
        /// </summary>
        public ImprovState CurrentState
        {
            get => _currentState;
            private set
            {
                _currentState = value;
                // Notify change in value
                if (_characteristicCurrentState != null)
                {
                    //Console.WriteLine($"Notify current state {_currentState}");
                    _characteristicCurrentState.NotifyValue(GetByteBuffer((byte)_currentState));
                }
            }
        }

        /// <summary>
        /// Get the current error state.
        /// </summary>
        public ImprovError ErrorState
        {
            get => _errorState;
            set
            {
                _errorState = value;
                // Notify change in value
                if (_characteristicErrorState != null)
                {
                    //Console.WriteLine($"Notify error state {_errorState}");
                    _characteristicErrorState.NotifyValue(GetByteBuffer((byte)_errorState));
                }
            }
        }

        /// <summary>
        /// Get/Set URL to send provision device when provisioning is complete.
        /// </summary>
        public string RedirectUrl { get => _rpcResult; set => _rpcResult = value; }

        #endregion

        /// <summary>
        /// Set up the Bluetooth Characteristics for Improv service.
        /// </summary>
        /// <returns>0 if no error</returns>
        protected BluetoothError Initialise()
        {
            Guid improvSeviceUuid = new ("00467768-6228-2272-4663-277478268000");
            Guid improvChrCurrentStateUuid = new ("00467768-6228-2272-4663-277478268001");
            Guid improvChrErrorStateUuid = new ("00467768-6228-2272-4663-277478268002");
            Guid improvChrRpcCommandUuid = new ("00467768-6228-2272-4663-277478268003");
            Guid improvChrRpcResultUuid = new ("00467768-6228-2272-4663-277478268004");
            Guid improvChrCapsUuid = new ("00467768-6228-2272-4663-277478268005");

            CurrentState = ImprovState.authorizationRequired;
            ErrorState = ImprovError.noError;

            GattServiceProviderResult result = GattServiceProvider.Create(improvSeviceUuid);
            if (result.Error != BluetoothError.Success)
            {
                return result.Error;
            }

            _serviceProvider = result.ServiceProvider;

            // Current State Characteristic
            GattLocalCharacteristicParameters CSParameters = new ()
            {
                CharacteristicProperties = (GattCharacteristicProperties.Read | GattCharacteristicProperties.Notify),
                UserDescription = "Current State"
            };

            GattLocalCharacteristicResult characteristicResult = _serviceProvider.Service.CreateCharacteristic(improvChrCurrentStateUuid, CSParameters);
            if (characteristicResult.Error != BluetoothError.Success)
            {
                // An error occurred.
                return characteristicResult.Error;
            }

            _characteristicCurrentState = characteristicResult.Characteristic;
            _characteristicCurrentState.ReadRequested += CharacteristicCurrentState_ReadRequested;

            // Error State Characteristic
            GattLocalCharacteristicParameters ESParameters = new()
            {
                CharacteristicProperties = (GattCharacteristicProperties.Read | GattCharacteristicProperties.Notify),
                UserDescription = "Error State"
            };

            characteristicResult = _serviceProvider.Service.CreateCharacteristic(improvChrErrorStateUuid, ESParameters);
            if (characteristicResult.Error != BluetoothError.Success)
            {
                // An error occurred.
                return characteristicResult.Error;
            }

            _characteristicErrorState = characteristicResult.Characteristic;
            _characteristicErrorState.ReadRequested += CharacteristicErrorState_ReadRequested;

            // Rpc command Characteristic
            GattLocalCharacteristicParameters RPCParameters = new()
            {
                CharacteristicProperties = (GattCharacteristicProperties.Write),
                UserDescription = "Rpc command"
            };

            characteristicResult = _serviceProvider.Service.CreateCharacteristic(improvChrRpcCommandUuid, RPCParameters);
            if (characteristicResult.Error != BluetoothError.Success)
            {
                // An error occurred.
                return characteristicResult.Error;
            }

            _characteristicRpcCommand = characteristicResult.Characteristic;
            _characteristicRpcCommand.WriteRequested += CharacteristicRpcCommand_WriteRequested;

            // Rpc result Characteristic
            GattLocalCharacteristicParameters RPCResultParameters = new()
            {
                CharacteristicProperties = (GattCharacteristicProperties.Read | GattCharacteristicProperties.Notify),
                UserDescription = "Rpc result"
            };

            characteristicResult = _serviceProvider.Service.CreateCharacteristic(improvChrRpcResultUuid, RPCResultParameters);
            if (characteristicResult.Error != BluetoothError.Success)
            {
                // An error occurred.
                return characteristicResult.Error;
            }

            _characteristicRpcResult = characteristicResult.Characteristic;
            _characteristicRpcResult.ReadRequested += CharacteristicRpcResult_ReadRequested;

            // Capabilities Characteristic
            GattLocalCharacteristicParameters CapsParameters = new()
            {
                CharacteristicProperties = (GattCharacteristicProperties.Read),
                UserDescription = "Capabilities",
                StaticValue = GetByteBuffer(0x01)
            };

            characteristicResult = _serviceProvider.Service.CreateCharacteristic(improvChrCapsUuid, CapsParameters);
            if (characteristicResult.Error != BluetoothError.Success)
            {
                // An error occurred.
                return characteristicResult.Error;
            }

            return BluetoothError.Success;
        }

        #region Characteristic event handlers

        private Buffer GetByteBuffer(byte value)
        {
            DataWriter dw = new();
            dw.WriteByte(value);
            return dw.DetachBuffer();
        }

        /// <summary>
        /// Handler for reading Current State
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ReadRequestEventArgs"></param>
        private void CharacteristicCurrentState_ReadRequested(GattLocalCharacteristic sender, GattReadRequestedEventArgs ReadRequestEventArgs)
        {
            //Console.WriteLine($"CurrentState_ReadRequested {_currentState}");
            ReadRequestEventArgs.GetRequest().RespondWithValue(GetByteBuffer((byte)_currentState));
        }

        /// <summary>
        /// Handler for reading error state
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ReadRequestEventArgs"></param>
        private void CharacteristicErrorState_ReadRequested(GattLocalCharacteristic sender, GattReadRequestedEventArgs ReadRequestEventArgs)
        {
            //Console.WriteLine($"ErrorState_ReadRequested {_errorState}");
            ReadRequestEventArgs.GetRequest().RespondWithValue(GetByteBuffer((byte)_errorState));
        }

        /// <summary>
        /// Handler for RCP commands
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="WriteRequestEventArgs"></param>
        private void CharacteristicRpcCommand_WriteRequested(GattLocalCharacteristic sender, GattWriteRequestedEventArgs WriteRequestEventArgs)
        {
            GattWriteRequest request = WriteRequestEventArgs.GetRequest();

            //Console.WriteLine($"RpcCommand_WriteRequested");

            // Check expected data length
            if (request.Value.Length < 2)
            {
                request.RespondWithProtocolError((byte)BluetoothError.NotSupported);
                return;
            }

            // Read data from buffer of required format
            DataReader rdr = DataReader.FromBuffer(request.Value);
            byte command = rdr.ReadByte();
            byte length = rdr.ReadByte();

            // Do something with received data
            //Console.WriteLine($"Rpc command {command} length:{length}");

            switch (command)
            {
                case 1:  //  Send WiFi settings
                    if (CurrentState != ImprovState.authorized)
                    {
                        ErrorState = ImprovError.notAuthorised;
                    }
                    else
                    {
                        byte ssidLength = rdr.ReadByte();
                        byte[] bssid = new byte[ssidLength];
                        rdr.ReadBytes(bssid);

                        byte passwordLength = rdr.ReadByte();
                        byte[] bpassword = new byte[passwordLength];
                        rdr.ReadBytes(bpassword);

                        byte csum = rdr.ReadByte();

                        ErrorState = ImprovError.noError;

                        string ssid = UTF8Encoding.UTF8.GetString(bssid, 0, bssid.Length);
                        string password = UTF8Encoding.UTF8.GetString(bpassword, 0, bpassword.Length);

                        //Console.WriteLine($"Rpc Send Wifi SSID:{ssid} Password:{password}");

                        // Start provisioning
                        CurrentState = ImprovState.provisioning;

                        // User handling provisioning ?
                        if (OnProvisioned == null)
                        {
                            // No OnProvisioned user event so try to automatically connect to wifi
                            if (ConnectWiFi(ssid, password))
                            {
                                CurrentState = ImprovState.provisioned;
                            }
                            else
                            {
                                // Unable to connect, go back to authorised state so it can be retried
                                ErrorState = ImprovError.unableConnect;
                                CurrentState = ImprovState.authorized;
                            }
                        }
                        else
                        {
                            // User provisioning, call event
                            OnProvisioned.Invoke(this, new ProvisionedEventArgs(ssid, password));

                            if (ErrorState == ImprovError.noError)
                            {
                                CurrentState = ImprovState.provisioned;
                            }
                        }

                        if (CurrentState == ImprovState.provisioned)
                        {
                            ErrorState = ImprovError.noError;

                            if (OnProvisioningComplete != null)
                            {
                                // Call OnProvisioningComplete to give user a chance to set the Provisioning URL before we notify result
                                OnProvisioningComplete.Invoke(this, EventArgs.Empty);
                            }

                            // Notify of current result
                            NotifyRpcResult();
                        }
                    }
                    break;

                case 2:  //  Identify
                    FireOnIdentify();
                    ErrorState = ImprovError.noError;
                    break;

                default: // Invalid
                    ErrorState = ImprovError.unknownRpcPacket;
                    break;
            }

            // Respond if Write requires response
            if (request.Option == GattWriteOption.WriteWithResponse)
            {
                request.Respond();
            }
        }

        private Buffer SetupRpcResult()
        {
            byte cs = 0;

            byte[] resultBytes = System.Text.UTF8Encoding.UTF8.GetBytes(_rpcResult);

            DataWriter dw = new();
            dw.WriteByte(0); // command

            byte dataLength = (byte)(resultBytes.Length + 1);
            dw.WriteByte(dataLength); // data length
            cs += dataLength;

            dw.WriteByte((byte)resultBytes.Length); // string 1 length
            cs += (byte)resultBytes.Length;

            dw.WriteBytes(resultBytes);
            CheckSum(ref cs, resultBytes);

            dw.WriteByte(cs);

            return dw.DetachBuffer();
        }

        private void NotifyRpcResult()
        {
            // Notify change in value
            if (_characteristicRpcResult != null)
            {
                //Console.WriteLine($"Notify rpc result:{_rpcResult}");
                _characteristicRpcResult.NotifyValue(SetupRpcResult());
            }
        }

        private void CharacteristicRpcResult_ReadRequested(GattLocalCharacteristic sender, GattReadRequestedEventArgs ReadRequestEventArgs)
        {
            //Console.WriteLine($"RpcResult_ReadRequested {_rpcResult}");
            ReadRequestEventArgs.GetRequest().RespondWithValue(SetupRpcResult());
        }

        #endregion

        private void CheckSum(ref byte cs, byte[] bytes)
        {
            foreach (byte b in bytes)
            {
                cs += b;
            }
        }

        /// <summary>
        /// Connect to the Wifi
        /// </summary>
        /// <param name="ssid">SSID to connect to</param>
        /// <param name="password">password for connection</param>
        /// <returns></returns>
        public bool ConnectWiFi(string ssid, string password)
        {
            // Make sure we are disconnected before we start connecting otherwise
            // ConnectDhcp will just return success instead of reconnecting.
            WifiAdapter wa = WifiAdapter.FindAllAdapters()[0];
            wa.Disconnect();

            System.Threading.CancellationTokenSource cs = new(30000);
            Console.WriteLine("ConnectDHCP");
            bool success =  WifiNetworkHelper.ConnectDhcp(ssid, password, requiresDateTime: true, token: cs.Token);
            Console.WriteLine($"ConnectDHCP exit {success}");
            cs.Cancel();
            return success;
        }

        /// <summary>
        /// Get current IP address. Only valid if successfully provisioned and connected
        /// </summary>
        /// <returns>IP address string</returns>
        public string GetCurrentIPAddress()
        {
            NetworkInterface ni  = NetworkInterface.GetAllNetworkInterfaces()[0];

            // get first NI ( Wifi on ESP32 )
            return ni.IPv4Address.ToString();
        }

        private void FireOnIdentify()
        {
            if (OnIdentify != null)
            {
                OnIdentify.Invoke(this, EventArgs.Empty);
            }
        }
    }

    /// <summary>
    /// Event Args for user provisioning.
    /// </summary>
    public class ProvisionedEventArgs : EventArgs
    {
        public ProvisionedEventArgs(string ssid, string password)
        {
            Ssid = ssid;
            Password = password;
        }

        public string Ssid { get; set; }
        public string Password { get; set; }
    }
}
