// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Device.Spi;
using System.Device.Spi.Drivers;
using System.Threading;

namespace Iot.Device.Samples
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Mcp3008!");

            // This sample implements two different ways of accessing the MCP3008.
            // The SPI option is enabled in the sample by default, but you can switch
            // to the GPIO bit-banging option by switching which one is commented out.
            // The sample uses local functions to make it easier to switch between
            // the two implementations

            // SPI implementation
            Mcp3008 GetMCP3008WithSPI()
            {
                var connection = new SpiConnectionSettings(0,0);
                connection.ClockFrequency = 1000000;
                connection.Mode = SpiMode.Mode0;
                var spi = new UnixSpiDevice(connection);
                var mcp3008 = new Mcp3008(spi);
                return mcp3008;
            }


            // the GPIO (via bit banging) implementation
            Mcp3008 GetMCP3008WithGPIO()
            {
                var mcp3008 = new Mcp3008(18, 23, 24, 25);
                return mcp3008;
            }

            Mcp3008 mcp = GetMCP3008WithSPI();
            // Uncomment next line to use Gpio instead.
            //Mcp3008 mcp = GetMCP3008WithGPIO();

            // Using SPI implementation
            using (mcp)
            {
                while (true)
                {
                    double value = mcp.Read(0);
                    value = value / 10.24;
                    value = Math.Round(value);
                    Console.WriteLine(value);
                    Thread.Sleep(500);
                }
            }
        }
    }
}
