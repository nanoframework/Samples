# 🌶️🌶️ - Model Context Protocol (MCP) sample

This sample demonstrates how to create an MCP (Model Context Protocol) server on a nanoFramework device that can be controlled by AI agents through natural language commands. The sample implements a smart light controller that can be managed remotely via AI assistants. In this sample, the light is the embedded led.

## How the Sample Works

### Overview

The MCP sample creates a smart IoT device that exposes hardware controls through the Model Context Protocol, allowing AI agents to interact with physical hardware using natural language. The sample consists of:

1. **MCP Server (nanoFramework device)**: Runs on your ESP32 and exposes light control functions
2. **MCP Client (Full .NET application)**: Connects AI agents to your device via Azure OpenAI and Semantic Kernel

### Hardware Control Implementation

The `Light.cs` class demonstrates how to create MCP-enabled hardware controls:

```csharp
[McpServerTool("turn_on", "Turn on the light. Check the location to make sure it's the proper location first.")]
public static void TurnOn()
{
    Debug.WriteLine($"Turning on the light at location: {_location}");
    _lightPin.Write(PinValue.Low);  // GPIO control
}
```

Each method decorated with `[McpServerTool]` becomes available to AI agents with:

- **Function name**: Used by AI to identify the tool
- **Description**: Guides AI on when and how to use the tool
- Optional **description of output**: You can also specify the description of the output
- **Parameters**: Automatically parsed from method signatures

### Server Setup and Tool Discovery

The `Program.cs` shows the complete setup process:

1. **WiFi Connection**: Establishes network connectivity (don't forget to replace your wifi credentials)
2. **Tool Discovery**: Automatically finds all MCP-decorated methods
3. **Web Server**: Starts HTTP server with MCP endpoint at `/mcp`

```csharp
// Discover tools from classes
McpToolRegistry.DiscoverTools(new Type[] { typeof(Light) });

// Start web server with MCP support
using (var server = new WebServer(80, HttpProtocol.Http, new Type[] { typeof(McpServerController) }))
{
    server.Start();
    // Device is now ready to receive MCP requests
}
```

### Real-World Interaction Example

Based on the provided traces, here's how a typical AI conversation works:

**User**: "turn on the light in the main room"

**AI Agent Process**:

1. Gets current location: `get_location()` → "kitchen"
2. Recognizes mismatch between requested location and actual location
3. Asks user for clarification

**Device Trace**:

```text
Getting the location of the light: kitchen
```

**User**: "no, just switch on the one in the kitchen"

**AI Agent Process**:

1. Confirms location is kitchen
2. Calls `turn_on()` to activate the light

**Device Trace**:

```text
Turning on the light at location: kitchen
```

**User**: "I moved the light from the kitchen to the main room"

**AI Agent Process**:

1. Calls `set_location("main room")` to update the device state

**Device Trace**:

```text
Setting the location of the light to: main room
```

### Available MCP Tools

The sample exposes four tools to AI agents:

| Tool | Description | AI Usage |
|------|-------------|----------|
| `get_location` | Returns current light location | Check before any operation |
| `turn_on` | Activates the light (GPIO LOW) | Turn on light at current location |
| `turn_off` | Deactivates the light (GPIO HIGH) | Turn off light at current location |
| `set_location` | Updates location metadata | Change light's logical location |

### Key Features

- **Context Awareness**: AI agents understand location context and ask for clarification
- **State Management**: Device maintains location state across requests
- **Natural Language**: Users can interact using conversational commands
- **Safety Checks**: AI verifies location before performing actions
- **Real-time Communication**: Immediate hardware response to AI commands

## .NET 10 MCP Client with Azure OpenAI

The main WebServer repository also includes a [.NET 10 MCP client example](tests/McpClientTest/) that demonstrates how to connect to your nanoFramework MCP server from a full .NET application using Azure OpenAI and Semantic Kernel. This client example shows:

- **Azure OpenAI integration** using Semantic Kernel
- **MCP client connectivity** to nanoFramework devices
- **Automatic tool discovery** and registration as AI functions
- **Interactive chat interface** that can invoke tools on your embedded device
- **Real-time communication** between AI agents and nanoFramework hardware

The client uses the official ModelContextProtocol NuGet package and can automatically discover and invoke any tools exposed by your nanoFramework MCP server, enabling seamless AI-to-hardware interactions.

```csharp
// Example: Connect .NET client to nanoFramework MCP server
var mcpToolboxClient = await McpClientFactory.CreateAsync(
    new SseClientTransport(new SseClientTransportOptions()
    {
        Endpoint = new Uri("http://192.168.1.139/mcp"), // Your nanoFramework device IP
        TransportMode = HttpTransportMode.StreamableHttp,
    }, new HttpClient()));

// Register discovered tools with Semantic Kernel
var tools = await mcpToolboxClient.ListToolsAsync();
kernel.Plugins.AddFromFunctions("MyDeviceTools", tools.Select(t => t.AsKernelFunction()));
```

This comprehensive MCP support enables your nanoFramework devices to seamlessly integrate with AI systems and language models, opening up new possibilities for intelligent embedded applications.

## Hardware requirements

A hardware device with networking capabilities running a nanoFramework image.
These samples have been tested with ESP32-S3 and ESP32-C3.

For this specific sample, you'll need:

- An LED connected to GPIO pin 8 (or modify the pin number in `Light.cs`). In the case of the ESP32-S3 used, it's the embedded led. Most ESP32 devices have an embedded light you can use.
- A current-limiting resistor (220Ω recommended) if you are not using the embedded led.

## Wiring Diagram

```text
ESP32 GPIO 8 ─── 220Ω Resistor ─── LED Anode
                                    │
ESP32 GND ──────────────────────── LED Cathode
```

## Related topics

### Reference

- See API documentation [here](https://github.com/nanoframework/nanoFramework.WebServer).

## Build the sample

1. Start Microsoft Visual Studio 2022 or Visual Studio 2019 (Visual Studio 2017 should be OK too) and select `File > Open > Project/Solution`.
1. Starting in the folder where you unzipped the samples/cloned the repository, go to the subfolder for this specific sample. Double-click the Visual Studio Solution (.sln) file.
1. Press `Ctrl+Shift+B`, or select `Build > Build Solution`.

## Run the sample

The next steps depend on whether you just want to deploy the sample or you want to both deploy and run it.

### Deploying the sample

- Select `Build > Deploy Solution`.

### Deploying and running the sample

- To debug the sample and then run it, press F5 or select `Debug > Start Debugging`.

> [!NOTE]
>
> **Important**: Before deploying or running the sample, please make sure your device is visible in the Device Explorer.
>
> **Tip**: To display the Device Explorer, go to Visual Studio menus: `View > Other Windows > Device Explorer`.

## Testing the MCP Server

1. Deploy and run the sample on your nanoFramework device
2. Note the IP address assigned to your device (check debug output)
3. Use the MCP client example and adjust the IP address or create your own AI application
4. The AI agent will automatically discover your device's capabilities and provide intelligent responses based on the current state and user requests.
