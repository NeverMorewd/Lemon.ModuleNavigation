# Lemon.ModuleNavigation

![Logo](https://github.com/NeverMorewd/Lemon.ModuleNavigation/blob/master/src/Lemon.ModuleNavigation.Sample/Assets/lemon-100.png) 

[![NuGet Badge](https://img.shields.io/badge/NuGet-v1.0.0-blue.svg)](https://www.nuget.org/packages/Lemon.ModuleNavigation/) 

## Introduction

### Lemon.ModuleNavigation
A lightweight module navigation framework built on top of the Microsoft Dependency Injection (MSDI) for AvaloniaUI, WPF and .net-xaml-platforms else.
Support native aot (Avaloniaui)
### Lemon.ModuleNavigation.AvaloniaUI
Extending **Lemon.ModuleNavigation**, this package provides handy NavigationContainers specifically designed for AvaloniaUI.

![sample-show](https://github.com/user-attachments/assets/58690f91-6939-47d7-84d3-113d04c722a7)

Usage:
#### Create module with View and ViewModel
##### Module.cs
```c#
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
        /// Default value is True
        /// </summary>
        public override bool LoadOnDemand => false;

        /// <summary>
        /// Alias of module for displaying usually
        /// Default value is class name of Module
        /// </summary>
        public override string? Alias => base.Alias;

        /// <summary>
        /// Specifies whether this module allow multiple instances
        /// If true,every navigation to this module will generate a new instance.
        /// Default value is false.
        /// </summary>
        public override bool AllowMultiple => base.AllowMultiple;

        /// <summary>
        /// Specifies whether this module can be unloaded.
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
```c#
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
```
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
```c#
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
#### Program.cs or App.axaml.cs
##### Program.cs with Lemon.Hosting.AvaloniauiDesktop
```c#
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
#### App.axaml.cs

```c#
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Lemon.ModuleNavigation.Sample.ModuleAs;
using Lemon.ModuleNavigation.Sample.ViewModels;
using Lemon.ModuleNavigation.Sample.Views;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Lemon.ModuleNavigation.Sample;

public partial class App : Application
{
    private readonly IServiceCollection _services;
    private readonly IServiceProvider _serviceProvider;
    public App()
    {
        _services = new ServiceCollection();
        _services.AddNavigationContext()
                 .AddModule<ModuleA>()
                 .AddSingleton<MainView>()
                 .AddSingleton<MainWindow>()
                 .AddSingleton<MainViewModel>();
        _serviceProvider = _services.BuildServiceProvider();
    }
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        base.OnFrameworkInitializationCompleted();
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime 
            desktopLifetime)
        {
            var window = _serviceProvider.GetRequiredService<MainWindow>();
            var viewModel = _serviceProvider.GetRequiredService<MainViewModel>();
            window.DataContext = viewModel;
            desktopLifetime.MainWindow = window;
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime
            singleViewLifetime)
        {
            var view = _serviceProvider.GetRequiredService<MainView>();
            var viewModel = _serviceProvider.GetRequiredService<MainViewModel>();
            view.DataContext = viewModel;
        }
    }
}

```

### Lemon.Extensions.ModuleNavigation.Sample.Desktop
This is a sample desktop application for **Lemon.ModuleNavigation** and **Lemon.ModuleNavigation.AvaloniaUI** using AvaloniaUI. It introduces **Lemon.Hosting.AvaloniaUIDesktop** to support the .NET Generic Host, although this is not a strict requirement. The only dependency for **Lemon.ModuleNavigation** is **Microsoft.Extensions.DependencyInjection.Abstractions**.

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




