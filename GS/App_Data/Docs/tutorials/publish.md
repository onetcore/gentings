---
title: 发布
---

# 发布

在开发完成后，进行发布才能够部署到服务器中，这里主要介绍文件发布，其他如docker等发布可以参考微软的文档。

## 发布文件夹

我们约定一个文件夹用于发布，一般是项目下面的`publisher`文件夹，所以在配置路径时候输入如下：

```sh
../publisher
```

> [!warning]
> 需要安装`winrar`进行自动压缩发布后的文件夹。

## 安装压缩脚本

可以把下面的代码复制到`install.cmd`中，具体代码如下：

```sh
@ECHO OFF 
SET RARLINE="%ProgramFiles%\WinRAR\Winrar.exe"
%RARLINE% a -m1 -r -o+ -s -x%0 ./install.rar ./publisher/app_data
%RARLINE% a -m1 -r -o+ -s -x%0 ./install.rar ./publisher/runtimes
%RARLINE% a -m1 -r -o+ -s -x%0 ./install.rar ./publisher/Areas
%RARLINE% a -m1 -r -o+ -s -x%0 ./install.rar ./publisher/wwwroot/favicon.ico
%RARLINE% a -m1 -r -o+ -s -x%0 ./install.rar ./publisher/wwwroot/*.styles.css
%RARLINE% a -m1 -r -o+ -s -x%0 ./install.rar ./publisher/wwwroot/js/*.min.js
%RARLINE% a -m1 -r -o+ -s -x%0 ./install.rar ./publisher/wwwroot/css/*.min.css
%RARLINE% a -m1 -r -o+ -s -x%0 ./install.rar ./publisher/wwwroot/images
%RARLINE% a -m1 -r -o+ -s -x%0 ./install.rar ./publisher/wwwroot/_content
%RARLINE% a -m1 -r -o+ -s -x%0 ./install.rar ./publisher/wwwroot/lib/*.min.js
%RARLINE% a -m1 -r -o+ -s -x%0 ./install.rar ./publisher/wwwroot/lib/*.min.css
%RARLINE% a -m1 -r -o+ -s -x%0 ./install.rar ./publisher/wwwroot/lib/*.woff
%RARLINE% a -m1 -r -o+ -s -x%0 ./install.rar ./publisher/wwwroot/lib/*.woff2
%RARLINE% a -m1 -r -o+ -s -x%0 ./install.rar ./publisher/wwwroot/lib/*.svg
%RARLINE% a -m1 -r -o+ -s -x%0 ./install.rar ./publisher/*.dll
%RARLINE% a -m1 -r -o+ -s -x%0 ./install.rar ./publisher/*.deps.json
%RARLINE% a -m1 -r -o+ -s -x%0 ./install.rar ./publisher/appsettings.json
%RARLINE% a -m1 -r -o+ -s -x%0 ./install.rar ./publisher/*.runtimeconfig.json
%RARLINE% a -m1 -r -o+ -s -x%0 ./install.rar ./publisher/*.xml
%RARLINE% a -m1 -r -o+ -s -x%0 ./install.rar ./publisher/*.exe
%RARLINE% a -m1 -r -o+ -s -x%0 ./install.rar ./publisher/web.config
```

## 更新压缩脚本

可以把下面的代码复制到`update.cmd`中，具体代码如下：

```sh
@echo off
SET RARLINE="%ProgramFiles%\WinRAR\Winrar.exe"
%RARLINE% a -m1 -r -o+ -s -x%0 ./update.rar ./publisher/app_data
%RARLINE% a -m1 -r -o+ -s -x%0 ./update.rar ./publisher/runtimes
%RARLINE% a -m1 -r -o+ -s -x%0 ./update.rar ./publisher/Areas
%RARLINE% a -m1 -r -o+ -s -x%0 ./update.rar ./publisher/wwwroot/favicon.ico
%RARLINE% a -m1 -r -o+ -s -x%0 ./update.rar ./publisher/wwwroot/*.styles.css
%RARLINE% a -m1 -r -o+ -s -x%0 ./update.rar ./publisher/wwwroot/js/*.min.js
%RARLINE% a -m1 -r -o+ -s -x%0 ./update.rar ./publisher/wwwroot/css/*.min.css
%RARLINE% a -m1 -r -o+ -s -x%0 ./update.rar ./publisher/wwwroot/images
%RARLINE% a -m1 -r -o+ -s -x%0 ./update.rar ./publisher/wwwroot/_content
%RARLINE% a -m1 -r -o+ -s -x%0 ./update.rar ./publisher/wwwroot/lib/*.min.js
%RARLINE% a -m1 -r -o+ -s -x%0 ./update.rar ./publisher/wwwroot/lib/*.min.css
%RARLINE% a -m1 -r -o+ -s -x%0 ./update.rar ./publisher/wwwroot/lib/*.woff
%RARLINE% a -m1 -r -o+ -s -x%0 ./update.rar ./publisher/wwwroot/lib/*.woff2
%RARLINE% a -m1 -r -o+ -s -x%0 ./update.rar ./publisher/wwwroot/lib/*.svg
%RARLINE% a -m1 -r -o+ -s -x%0 ./update.rar ./publisher/*.dll
%RARLINE% a -m1 -r -o+ -s -x%0 ./update.rar ./publisher/*.deps.json
%RARLINE% a -m1 -r -o+ -s -x%0 ./update.rar ./publisher/*.runtimeconfig.json
%RARLINE% a -m1 -r -o+ -s -x%0 ./update.rar ./publisher/*.xml
%RARLINE% a -m1 -r -o+ -s -x%0 ./update.rar ./publisher/*.exe
```

从代码中就可以看到，只是少了`appsettings.json`和`web.config`文件。

## 配置数据库

打开`appsettings.json`，然后增加"Data"和"StorageDir"，当然如果要部署到IIS中，数据库需要添加SQL用户。

```json
{
  "StorageDir": "../storages",
  "Data": {
    "Connectionstring": "Data Source=.;Initial Catalog=database;User ID=user;Password=password"
  },
  "DetailedErrors": true,
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

打开`web.config`文件，启用日志，并且将日志文件夹也改到“StorageDir”配置的路径中。

```xml
<aspNetCore processPath="dotnet" arguments=".\Project.dll" stdoutLogEnabled="true" stdoutLogFile="..\storages\logs\stdout" hostingModel="inprocess" />
```

需要修改的就是：`stdoutLogEnabled="**true**" stdoutLogFile="**..\storages\logs\stdout**"`，然后就可以发布到IIS中。