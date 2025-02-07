//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using Microsoft.Extensions.Logging;
using nanoFramework.Logging;
using nanoFramework.Logging.Debug;
using nanoFramework.Logging.Serial;
using nanoFramework.Logging.Stream;
#if BUIID_FOR_ESP32
using nanoFramework.Hardware.Esp32;
#endif

namespace Logging
{
    public class Program
    {
        private static DebugLogger _logger;

        public static void Main()
        {
            _logger = new DebugLogger("Example");
            _logger.MinLogLevel = LogLevel.Trace;
            _logger.LogInformation("Hello from nanoFramework!");
            _logger.LogTrace("Trace: the Debug Logger is initialized");
            _logger.LogInformation($"Logger name is: {_logger.LoggerName}, you can use that to trace which component is used");
            _logger.LogInformation("The next call to the class will log as well");
            _logger.LogInformation("For this component, we're using the Logger Factory pattern. It will use the debugger as well");
            LogDispatcher.LoggerFactory = new DebugLoggerFactory();
            MyTestComponent test = new MyTestComponent();
            test.DoSomeLogging();
            _logger.LogInformation("Your responsibility is to make sure you set the right level as well as formatting the strings");
            _logger.LogInformation("More examples below. All will display as the log level is Trace.");
            _logger.LogTrace("TRACE {0} {1}", new object[] { "param 1", 42 });
            _logger.LogDebug("DEBUG {0} {1}", new object[] { "param 1", 42 });
            _logger.LogInformation("INFORMATION and nothing else");
            _logger.LogWarning("WARNING {0} {1}", new object[] { "param 1", 42 });
            _logger.LogError(new Exception("Big problem"), "ERROR {0} {1}", new object[] { "param 1", 42 });
            _logger.LogCritical(42, new Exception("Insane problem"), "CRITICAL {0} {1}", new object[] { "param 1", 42 });
            _logger.LogInformation("Now we will adjust the level to Critical, only the Critical message will appear");
            _logger.MinLogLevel = LogLevel.Critical;
            _logger.LogTrace("TRACE {0} {1}", new object[] { "param 1", 42 });
            _logger.LogDebug("DEBUG {0} {1}", new object[] { "param 1", 42 });
            _logger.LogInformation("INFORMATION and nothing else");
            _logger.LogWarning("WARNING {0} {1}", new object[] { "param 1", 42 });
            _logger.LogError(new Exception("Big problem"), "ERROR {0} {1}", new object[] { "param 1", 42 });
            _logger.LogCritical(42, new Exception("Insane problem"), "CRITICAL {0} {1}", new object[] { "param 1", 42 });

            // Now you can as well use the Stream and the Serial logger
            // Uncomment the following lines to use one or the other
            // SerialLogger();

        }

        private static void SerialLogger()
        {
            try
            {
#if BUIID_FOR_ESP32
                ////////////////////////////////////////////////////////////////////////////////////////////////////
                // COM2 in ESP32-WROVER-KIT mapped to free GPIO pins
                // mind to NOT USE pins shared with other devices, like serial flash and PSRAM
                // also it's MANDATORY to set pin funcion to the appropriate COM before instanciating it

                // set GPIO functions for COM2 (this is UART1 on ESP32)
                Configuration.SetPinFunction(Gpio.IO04, DeviceFunction.COM2_TX);
                Configuration.SetPinFunction(Gpio.IO05, DeviceFunction.COM2_RX);

                // open COM2
                LogDispatcher.LoggerFactory = new SerialLoggerFactory("COM2");
#else
                ///////////////////////////////////////////////////////////////////////////////////////////////////
                // COM6 in STM32F769IDiscovery board (Tx, Rx pins exposed in Arduino header CN13: TX->D1, RX->D0)
                // open COM6
                LogDispatcher.LoggerFactory = new SerialLoggerFactory("COM6");
#endif
                // Then you can use the serial logger as expected
                MyTestComponent test = new MyTestComponent();
                test.DoSomeLogging();
            }
            finally
            {
                LogDispatcher.LoggerFactory = null;
            }
        }

        private static void StreamLogger()
        {
            const string logFilePath = "C:\\logFile.txt";
            try
            {
                LogDispatcher.LoggerFactory = new StreamLoggerFactory(logFilePath);
                // This will log into the file
                MyTestComponent test = new MyTestComponent();
                test.DoSomeLogging();
            }
            finally
            {
                LogDispatcher.LoggerFactory = null;
            }
        }
    }
}
