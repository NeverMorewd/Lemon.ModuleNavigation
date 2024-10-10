using Lemon.ModuleNavigation;
using Lemon.ModuleNavigation.Avaloniaui;
using Lemon.Toolkit.ViewModels;
using System;

namespace Lemon.Toolkit.Modules
{
    public class TestModule : AvaModule<TestView, TestViewModel>
    {
        public TestModule(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override bool LoadOnDemand => true;
    }
}
