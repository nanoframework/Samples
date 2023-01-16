文档语言: [English](README.md) | [简体中文](README.zh-cn.md)

# 🌶️🌶️ - WebSocket ServerClient Sample

显示如何使用[System.Net.WebSockets](http://docs.nanoframework.net/api/System.Net.WebSockets.html)中的WebSockets相关api。关于Websocket库的文档可以在[WebSockets Repo](https://github.com/nanoframework/System.Net.WebSockets)中找到。

## 示例

这个例子展示了如何使用Websocket服务器与一个Webserver托管一个WebApp控制rgb led。

## 硬件要求

这个示例是为[M5Stack ATOM Lite](https://shop.m5stack.com/products/atom-lite-esp32-development-kit)设备编写的，但可以很容易地更改为连接到rgb led的另一块板。

## 相关的话题

### 引用

- [nanoFramework.Net.WebSockets](https://github.com/nanoframework/System.Net.WebSockets/blob/develop/README.md)
- [System.Net.WebSockets](http://docs.nanoframework.net/api/System.Net.WebSockets.html)
- [入门指南](https://www.feiko.io/posts/2022-01-03-getting-started-with-net-nanoframework)

##构建样本

1. 启动Microsoft Visual Studio 2022 (VS 2019和VS 2017应该也可以)，选择`文件&gt;开放比;项目/解决方案`。

2. 从解压缩示例/克隆存储库的文件夹开始，转到这个特定示例的子文件夹。双击Visual Studio Solution (.sln)文件。

3.修改第22和23行的Wifi Ssid和Password。

4. 按`Ctrl+Shift+B`，或选择`Build &gt;构建解决方案`。

## 运行示例

接下来的步骤取决于您是只想部署示例，还是想同时部署和运行它。

### 部署示例

- 选择 `Build > Deploy Solution`.

### 部署和运行示例

- 调试样本，然后运行它，按F5或选择`debug &gt;开始调试`。
- 调试输出将显示WebApp的url。请确保您连接到与设备相同的网络，并在浏览器中打开此url。
- 通过使用WebApp改变rgb led的颜色来设置情绪。

> **Important**:在部署或运行示例之前，请确保您的设备在设备资源管理器中可见。

> **Tip**:要显示设备资源管理器，请转到Visual Studio菜单:`View >  Other Windows >  Device Explorer`.