﻿using Lemon.ModuleNavigation.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Lemon.ModuleNavigation;

public abstract class Module<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TView,
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TViewModel>
    : IModule
    where TViewModel : IModuleNavigationAware
    where TView : IView
{
    protected readonly IServiceProvider ServiceProvider;
    public Module(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }
    public virtual void Initialize()
    {
        lock (this)
        {
            if (IsInitialized) return;
            View = ServiceProvider.GetRequiredKeyedService<IView>(Key);
            ViewModel = ServiceProvider.GetRequiredKeyedService<IModuleNavigationAware>(Key);
            View.DataContext = ViewModel;
            IsInitialized = true;
        }
    }
    public string Key => GetType().Name;
    public IView? View
    {
        get;
        protected set;
    }
    public IModuleNavigationAware? ViewModel
    {
        get;
        protected set;
    }
    public Type ViewType => typeof(TView);

    public Type ViewModelType => typeof(TViewModel);

    public abstract bool LoadOnDemand
    {
        get;
    }
    public bool IsInitialized
    {
        get;
        protected set;
    } = false;
    public bool IsActivated
    {
        get;
        set;
    } = false;
    public virtual bool ForceNew => false;
    public virtual bool CanUnload => true;
    public virtual string? Alias => Key;
}
