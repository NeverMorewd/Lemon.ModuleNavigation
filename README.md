# Lemon.ModuleNavigation
<p align="center">
    <img src="https://github.com/NeverMorewd/Lemon.ModuleNavigation/blob/master/src/Lemon.ModuleNavigation.Sample/Assets/lemon-100.png" />
</p>

[![NuGet](https://img.shields.io/nuget/v/Lemon.ModuleNavigation.svg?style=flat-square&label=NuGet)](https://www.nuget.org/packages/Lemon.ModuleNavigation/)

## Language

- [English](#english)
- [中文](#中文)

---

### English

## Introduction

### Lemon.ModuleNavigation
A lightweight module navigation framework built on top of the Microsoft Dependency Injection (MSDI) for AvaloniaUI, WPF and .net-xaml-platforms else.
Support native aot (Avaloniaui)
### Lemon.ModuleNavigation.AvaloniaUI
Extending `Lemon.ModuleNavigation`, this package provides handy NavigationContainers specifically designed for AvaloniaUI.

### Key Advantages:
- **Lightweight**: Minimal performance overhead.
- **Commonly module options are provided**:LoadonDemand; Unload; Multi-Instance.
- **Few dependencies**: Only relies on `Microsoft.Extensions.DependencyInjection.Abstractions`.
- **Framework Agnostic**: Does not enforce any specific MVVM framework, allowing developers to use the MVVM framework of their choice.
- **Highly Extensible**: Easily customizable to suit specific needs.
- **Simple to Use**.

---
![sample-show](https://github.com/user-attachments/assets/58690f91-6939-47d7-84d3-113d04c722a7)

### Usage:
#### Create module with View and ViewModel
##### Module.cs
```csharp
using System;
using Lemon.ModuleNavigation.Avaloniaui;

namespace Lemon.ModuleNavigation.Sample.ModuleAs
{
    public class ModuleA : AvaModule<ViewA, ViewModelA>
    {
        public ModuleA(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }

        /// <summary>
        /// Specifies whether this module needs to be loaded on demand
        /// 指定该模块是否按需加载
        /// Default value is True
        /// </summary>
        public override bool LoadOnDemand => false;

        /// <summary>
        /// Alias of module for displaying usually
        /// 模块的别名，一般用于显示
        /// Default value is class name of Module
        /// </summary>
        public override string? Alias => base.Alias;

        /// <summary>
        /// Specifies whether this module allow multiple instances
        /// If true,every navigation to this module will generate a new instance.
        /// 指定该模块是否支持多实例，如果设置为true，那每次导航到该模块都会创建新的实例。
        /// Default value is false.
        /// </summary>
        public override bool AllowMultiple => base.AllowMultiple;

        /// <summary>
        /// Specifies whether this module can be unloaded.
        /// 指定该模块是否支持卸载
        /// Default value is false.
        /// </summary>
        public override bool CanUnload => base.CanUnload;

        /// <summary>
        /// Initialize
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
        }
    }
}

```
##### View.cs
```csharp
using Avalonia.Controls;
using Lemon.ModuleNavigation.Abstracts;

namespace Lemon.ModuleNavigation.Sample.ModuleAs;

public partial class ViewA : UserControl, IView
{
    public ViewA()
    {
        InitializeComponent();
    }

    public void SetDataContext(IViewModel viewModel)
    {
        DataContext = viewModel;
    }
}
```
##### ViewModel.cs
```csharp
using Lemon.ModuleNavigation.Abstracts;
using Lemon.ModuleNavigation.Sample.ViewModels;
using System;

namespace Lemon.ModuleNavigation.Sample.ModuleAs
{
    public class ViewModelA :  IViewModel
    {
        public void Dispose()
        {
	        
        }
    }
}

```

#### In MainView.axaml or MainWindow.axaml
##### NContainer
NContainer is an implementation with ContentControl for displaying View of Module.
```xaml
<lm:NContainer xmlns:lm="https://github.com/NeverMorewd/Lemon.ModuleNavigation" Grid.Column="1" NavigationContext="{Binding NavigationContext}" />
```
##### NTabContainer
NTabContainer is an implementation with TabControl for displaying View of Module.
```xaml
<lm:NTabContainer xmlns:lm="https://github.com/NeverMorewd/Lemon.ModuleNavigation" Grid.Column="1" NavigationContext="{Binding NavigationContext}" >
   <lm:NTabContainer.ItemTemplate>
    <DataTemplate>
        <StackPanel Orientation="Horizontal" Spacing="2">
            <TextBlock Text="{Binding Alias}" />
             <!-- Adding lm:NTabContainerBehaviors.CanUnload to a Button to make this moule can be unloaded with click event.  -->
            <Button lm:NTabContainerBehaviors.CanUnload="{Binding CanUnload}"/>
        </StackPanel>
    </DataTemplate>
   </lm:NTabContainer.ItemTemplate>
   <lm:NTabContainer.ContentTemplate>
    <DataTemplate>
	<ContentControl Content="{Binding View}" />
    </DataTemplate>
   </lm:NTabContainer.ContentTemplate>
</NTabContainer>
```
#### MainViewModel.cs
```csharp
public class MainViewModel : ViewModelBase, INavigationContextProvider
{
    public readonly NavigationService _navigationService;
    public MainViewModel(NavigationContext navigationContext,
        IEnumerable<IModule> modules,
        NavigationService navigationService)
    {
        _navigationService = navigationService;
        NavigationContext = navigationContext;
        Modules = new ObservableCollection<IModule>(modules);
    }

    /// <summary>
    /// Navigation is triggered based on the timing of the customization
    /// </summary>
    public void OnSelectedModuleChanged(IModule? selectedModule)
    {
       _navigationService.NavigateTo(selectedModule!);
    }
    /// <summary>
    /// For binding
    /// </summary>
    public ObservableCollection<IModule> Modules
    {
        get;
        set;
    }
    
    public NavigationContext NavigationContext
    {
        get;
    }
}

```
### With generic host. Using `Lemon.Hosting.AvaloniaUIDesktop`
#### Program.cs in `Lemon.ModuleNavigation.Sample.DesktopHosting`
```csharp
class Program
{
    [STAThread]
    [SupportedOSPlatform("windows")]
    [SupportedOSPlatform("linux")]
    [SupportedOSPlatform("macos")]
    public static void Main(string[] args)
    {
        var hostBuilder = Host.CreateApplicationBuilder();

        // module navigation
        hostBuilder.Services.AddNavigationContext();
        // modules
        hostBuilder.Services.AddModule<ModuleA>();

        hostBuilder.Services.AddAvaloniauiDesktopApplication<App>(BuildAvaloniaApp);
        hostBuilder.Services.AddMainWindow<MainWindow, MainViewModel>();
        var appHost = hostBuilder.Build();
        appHost.RunAvaloniauiApplication<MainWindow>(args);
    }

    public static AppBuilder BuildAvaloniaApp(AppBuilder appBuilder)
    {
        return appBuilder
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace()
            .UseReactiveUI();
    }
}
```

### Without generic host
#### AppWithDI.xaml.cs in `Lemon.ModuleNavigation.Sample`
```csharp
public partial class AppWithDI : Application
{
    private IServiceProvider? _serviceProvider;
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var services = new ServiceCollection();
        services.AddNavigationContext()
                .AddModule<ModuleA>()
                .AddSingleton<MainWindow>()
                .AddSingleton<MainView>()
                .AddSingleton<MainViewModel>();

        _serviceProvider = services.BuildServiceProvider();

        var viewModel = _serviceProvider.GetRequiredService<MainViewModel>();

        switch (ApplicationLifetime)
        {
            case IClassicDesktopStyleApplicationLifetime desktop:
                var window = _serviceProvider.GetRequiredService<MainWindow>();
                window.DataContext = viewModel;
                desktop.MainWindow = window;
                break;
            case ISingleViewApplicationLifetime singleView:
                var view = _serviceProvider.GetRequiredService<MainView>();
                view.DataContext = viewModel;
                singleView.MainView = view;
                break;
        }
        base.OnFrameworkInitializationCompleted();
    }
}

```

### Lemon.Extensions.ModuleNavigation.Sample.Desktop
A sample desktop application for `Lemon.ModuleNavigation` and `Lemon.ModuleNavigation.AvaloniaUI`.

### Lemon.Extensions.ModuleNavigation.Sample.DesktopHosting
A sample desktop application for `Lemon.ModuleNavigation` and `Lemon.ModuleNavigation.AvaloniaUI` using AvaloniaUI. 
It introduces `Lemon.Hosting.AvaloniaUIDesktop` to support .NET Generic Host.

#### AOT config:
Update .csproj
```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    ...
    <PublishAot>true</PublishAot>
    <IncludeNativeLibrariesForSelfExtract>true</IncludeNativeLibrariesForSelfExtract>
    ...
  </PropertyGroup>

  <PropertyGroup Condition="$([MSBuild]::IsTargetFrameworkCompatible('$(TargetFramework)', 'net8.0'))">
	<IsTrimmable>true</IsTrimmable>
	<PublishTrimmed>true</PublishTrimmed>
  </PropertyGroup>

  <ItemGroup>
	<RdXmlFile Include="rd.xml" />
  </ItemGroup>
</Project>

```

Add rd.xml
```xml
<Directives>
	<Application>
		<!--all assemblies in which Modules are located-->
		<Assembly Name="Lemon.ModuleNavigation.Sample" Dynamic="Required All"/>
		<Assembly Name="Lemon.ModuleNavigation.Avaloniaui" Dynamic="Required All"/>
		<Assembly Name="Lemon.ModuleNavigation" Dynamic="Required All"/>
	</Application>
</Directives>
```

### Lemon.Toolkit
This is a practical application of `Lemon.ModuleNavigation`. It is a collection of toolkits for clients based on `Semi-Avaloniaui`, `Lemon.Hosting.AvaloniaUIDesktop`, and `Lemon.ModuleNavigation`. As of now, it includes:
- Module navigation based on NTabContainer
- Custom HomeModule
- Custom console log
- File Inspector (ongoing)
- File Comparer (ongoing)
- ...

![image](https://github.com/user-attachments/assets/6298d8cb-a8a0-493b-a3ac-c9b23b9487a7)

---

### 中文

## 简介

### Lemon.ModuleNavigation
**Lemon.ModuleNavigation** 是一个基于微软依赖注入（MSDI）的轻量级模块导航框架，专为 AvaloniaUI、WPF 和其他 .NET XAML 平台设计。它支持 **Native AOT**，为 AvaloniaUI 提供高效的导航，性能开销极低。

### Lemon.ModuleNavigation.AvaloniaUI
**Lemon.ModuleNavigation** 的扩展包，专为 AvaloniaUI 提供特定的 **NavigationContainers**，使得模块导航更加无缝且灵活。

### 主要优势：
- **轻量级**：最小的性能开销。
- **依赖少**：只依赖 `Microsoft.Extensions.DependencyInjection.Abstractions`。
- **框架无关**：不强制使用任何特定的 MVVM 框架，允许开发者选择自己喜欢的 MVVM 框架。
- **高度可扩展**：可以轻松定制以满足特定需求。
- **简单易用**。

---






