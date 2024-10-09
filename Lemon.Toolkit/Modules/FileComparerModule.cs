using Lemon.Extensions.SlimModule;
using Lemon.Toolkit.ViewModels;
using Lemon.Toolkit.Views;
using System;

namespace Lemon.Toolkit.Modules
{
    public class FileComparerModule : Module<CompareView, CompareViewModel>
    {
        public FileComparerModule(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            
        }

        public override bool LoadOnDemand => true;
        public override string? Alias { get => Key; }
    }
}
