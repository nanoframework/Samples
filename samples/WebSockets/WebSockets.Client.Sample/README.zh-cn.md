文档语言: [English](README.md) | [简体中文](README.zh-cn.md)

# 🌶️ - WebSocket 客户端示例

使用WebSockets相关的api [System.Net.WebSockets](http://docs.nanoframework.net/api/System.Net.WebSockets.html). 有关Websocket库的文档可以在 [WebSockets Repo](https://github.com/nanoframework/System.Net.WebSockets).

## 示例
这个例子展示了如何使用Websocket客户端。

## 硬件要求

具有网络功能的硬件设备，运行一个纳米框架映像。

## 相关主题

### 参考

- [nanoFramework.Net.WebSockets](https://github.com/nanoframework/System.Net.WebSockets/blob/main/README.md)
- [System.Net.WebSockets](http://docs.nanoframework.net/api/System.Net.WebSockets.html)
- [getting started guide](https://www.feiko.io/posts/2022-01-03-getting-started-with-net-nanoframework)

## 构建示例

1. 启动Microsoft Visual Studio 2022 (VS 2019和VS 2017应该也可以)并选择 `File > Open > Project/Solution`.
2.从解压缩示例/克隆存储库的文件夹开始，转到这个特定示例的子文件夹。双击Visual Studio Solution (.sln)文件。
3.在第14行和第15行更改Wifi Ssid和Password，同时在第41行更改websocket服务器的url。
4. 按 `Ctrl+Shift+B`,或选择 `Build > Build Solution`.

## 运行示例

接下来的步骤取决于您是只想部署示例，还是想同时部署和运行它。

## 部署示例

- 选择 `Build > Deploy Solution`.

### 部署和运行示例

- 要调试示例并运行它，请按F5或选择 `Debug > Start Debugging`.

> [!NOTE]
>
> **Important**: 在部署或运行示例之前，请确保您的设备在设备资源管理器中可见。
>
> **Tip**: 要显示设备资源管理器，请转到Visual Studio菜单: `View > Other Windows > Device Explorer`.
