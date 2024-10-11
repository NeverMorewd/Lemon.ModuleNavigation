# Lemon.ModuleNavigation

![Logo](https://github.com/NeverMorewd/Lemon.ModuleNavigation/blob/master/src/Lemon.ModuleNavigation.Sample/Assets/lemon-100.png) 

[![NuGet Badge](https://img.shields.io/badge/NuGet-v1.0.0-blue.svg)](https://www.nuget.org/packages/Lemon.ModuleNavigation/) 

## Introduction

### Lemon.ModuleNavigation
A lightweight module navigation framework built on top of the Microsoft Dependency Injection (MSDI) for AvaloniaUI, WPF and .net-xaml-platforms else.
Support native aot!
### Lemon.ModuleNavigation.AvaloniaUI
Extending **Lemon.ModuleNavigation**, this package provides NavigationContainers specifically designed for AvaloniaUI.

### Lemon.Extensions.ModuleNavigation.Sample.Desktop
This is a sample desktop application for **Lemon.ModuleNavigation** and **Lemon.ModuleNavigation.AvaloniaUI** using AvaloniaUI. It introduces **Lemon.Hosting.AvaloniaUIDesktop** to support the .NET Generic Host, although this is not a strict requirement. The only dependency for **Lemon.ModuleNavigation** is **Microsoft.Extensions.DependencyInjection.Abstractions**.

![sample-show](https://github.com/user-attachments/assets/58690f91-6939-47d7-84d3-113d04c722a7)

#### AOT config:

```xml
<Directives>
	<!-- 
        This file is part of RdXmlLibrary project.
        Visit https://github.com/kant2002/rdxmllibrary for latest version.
        If you have modifications specific to this Nuget package,
        please contribute back. 
    -->
	<Application>
		<!--all assemblies in which Modules are located-->
		<Assembly Name="Lemon.ModuleNavigation.Sample" Dynamic="Required All"/>
		<Assembly Name="Lemon.ModuleNavigation.Avaloniaui" Dynamic="Required All"/>
		<Assembly Name="Lemon.ModuleNavigation" Dynamic="Required All"/>
	</Application>
</Directives>
```

### Lemon.Toolkit
This is a practical application of **Lemon.ModuleNavigation**. It is a collection of toolkits for clients based on **Semi-AvaloniaUI**, **Lemon.Hosting.AvaloniaUIDesktop**, and **Lemon.ModuleNavigation**. As of now, it includes:
- Module navigation based on NTabContainer
- Custom HomeModule
- Custom console log
- File Inspector (ongoing)
- File Comparer (ongoing)
- ...

![image](https://github.com/user-attachments/assets/6298d8cb-a8a0-493b-a3ac-c9b23b9487a7)




