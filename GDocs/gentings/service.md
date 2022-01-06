---
title: Gentings服务注册
---

# Gentings服务注册

在.NET（本文档提到的所有.NET都表示.NET6+）开发中，所有接口服务都是基于容器存在，在Gentings中，使用.NET自带服务容器进行注册。为了程序的解耦，Gentings提供了一系列的借口，凡是基于Gentings开发的，都将按照接口进行自动的服务注册。

## IService系列接口

所有实现了`IService`接口的接口都会自动进行服务注册，根据.NET服务生命周期，可以分为普通服务，单例服务，以及生命周期内单例服务，所以在`IService`接口上又延伸了一些接口。

1. `ISingletonService`：单例服务接口，继承此接口的接口，将被自动注册为单例模式服务，在整个程序域内只有一个对象存在，也是整个服务中优先级最高的服务。
2. `IScopedService`：生命周期内单例服务接口，继承此接口的接口，将被自动注册为生命周期内单例存在，在AspNetCore程序中，最直观的就有一个`HttpContext`，这个实例就是在HTTP请求中只有单例存在。
3. `IService`：普通服务，每次需要的时候都会重新实例化一个对象。

## IServices系列接口

凡是带有复数的接口，一般表示某一个接口所有实现对象列表实例，根据不同的模式，在调用的时候需要在构造函数中使用`IEnumerable<TService>`模式进行实例调用。其他的使用方法以及服务的实例化方式和`IService`系列接口一样，包含`IServices`,`IScopedServices`,以及`ISingletonServices`相关服务。

## SuppressAttribute特性

为了后期更好的重写已有的服务，我们可以使用`SuppressAttribute`特性，此特性就是会告诉自动注册服务类型**替代**原有的实现类型，在重写服务时候尤为重要。

## IServiceConfigurer和IServiceBuilder接口

在开发过程中，有些接口为了抽象或者可以更有效的被重写，我们在开发过程中需要手动进行服务注册，这个也是为了能够在需要的时候在手动添加相关服务组件模块。所以提供了`IServiceConfigurer`接口，这个可以更有效的进行服务注册，其中我们对应用程序的配置，和服务集合进行了封装，主要也是为了区别系统的`IServiceConllection`。

实现了`IServiceConfigurer`接口的对象，将在自动服务注册之初就调用了`ConfigureServices`方法进行注册，注意这个方法不会对标注`SuppressAttribute`特性的实例有任何影响。

> [!NOTE]
> 注：Gentings服务容器注册是本框架的灵魂，本框架会对引用的程序集或者UI库进行自动注册，实现了业务逻辑的解耦性质，所以每个业务逻辑直接不会有很高的耦合性，能够将项目进行组件化开发，特别是使用RazorUI库，不仅业务逻辑解耦，UI也可以相应的解耦合。