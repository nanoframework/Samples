文档语言: [English](README.md) | [简体中文](README.zh-cn.md)

# 🌶️ to 🌶️🌶️ - WebSocket示例包

使用WebSockets相关的api[System.Net.WebSockets](http://docs.nanoframework.net/api/System.Net.WebSockets.html).

## 示例

### WebSockets Server 示例

[🌶️🌶️ - Server.RgbSample](./WebSockets.Server.RgbSample) 展示了如何使用Websocket服务器与Webserver托管WebApp控制rgb led。

### WebSockets Client 示例

[🌶️ - Client.Sample](./WebSockets.Client.Sample)展示了如何使用Websocket客户端。

### WebSockets Server 和 Client 示例

[🌶️🌶️ - ServerClient.Sample](./Websockets.ServerClient.Sample) 显示如何配置和启动WebSocket服务器和(ssl)客户端。

## 硬件要求

具有网络功能的硬件设备，运行一个纳米框架映像。

Websocket服务器样本需要一个M5Stack ATOM Lite板，但可以很容易地改变到另一个板连接到rgb led。

## 相关的话题

### 引用

- [nanoFramework.Net.WebSockets](https://github.com/nanoframework/System.Net.WebSockets/blob/main/README.md)
- [System.Net.WebSockets](http://docs.nanoframework.net/api/System.Net.WebSockets.html)

## 构建样本

1. 启动Microsoft Visual Studio 2022 (VS 2019和VS 2017应该也可以)并选择 `File > Open > Project/Solution`.
1. 从解压缩示例/克隆存储库的文件夹开始，转到这个特定示例的子文件夹。双击Visual Studio Solution (.sln)文件。
1. 按 `Ctrl+Shift+B`, 或选择 `Build > Build Solution`.

## 运行示例

接下来的步骤取决于您是只想部署示例，还是想同时部署和运行它。

### 部署示例

- 选择 `Build > Deploy Solution`.

### 部署和运行示例

- 要调试示例并运行它，请按F5或选择 `Debug > Start Debugging`.

> **Important**: 在部署或运行示例之前，请确保您的设备在设备资源管理器中可见。
> **Tip**: 要显示设备资源管理器，请转到Visual Studio菜单: `View > Other Windows > Device Explorer`.
