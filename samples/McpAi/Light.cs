// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Device.Gpio;
using System.Diagnostics;
using nanoFramework.WebServer.Mcp;

namespace McpAi
{
    public static class Light
    {
        private static GpioPin _lightPin;
        private static string _location = "kitchen";

        public static void Initialize()
        {
            _lightPin = new GpioController().OpenPin(8, PinMode.Output);
            // Ensure the light is off initially
            _lightPin.Write(PinValue.High);
        }

        [McpServerTool("turn_on", "Turn on the light. Check the location to make sure it's the proper location first. Do not change location of this light to turn it on.")]
        public static void TurnOn()
        {
            if (_lightPin == null)
            {
                Initialize();
            }

            Debug.WriteLine($"Turning on the light at location: {_location}");
            _lightPin.Write(PinValue.Low);
        }

        [McpServerTool("turn_off", "Turn off the light. Check the location to make sure it's the proper location first. Do not change location of this light to turn it off.")]
        public static void TurnOff()
        {
            if (_lightPin == null)
            {
                Initialize();
            }

            Debug.WriteLine($"Turning off the light at location: {_location}");
            _lightPin.Write(PinValue.High);
        }

        [McpServerTool("get_location", "Get the location of the light. Check this before switching a light on or off.")]
        public static string GetLocation()
        {
            Debug.WriteLine($"Getting the location of the light: {_location}");
            return _location;
        }

        [McpServerTool("set_location", "Change the location of the light. Do not change the location of the light unless the user ask you to change the location.")]
        public static void SetLocation(string location)
        {
            if (string.IsNullOrEmpty(location))
            {
                throw new ArgumentException("Location cannot be null or empty.", nameof(location));
            }

            Debug.WriteLine($"Setting the location of the light to: {location}");
            _location = location;
        }
    }
}
