using Lemon.ModuleNavigation;
using Lemon.Toolkit.ViewModels;
using Lemon.Toolkit.Views;
using System;

namespace Lemon.Toolkit.Modules
{
    public class FileInspectorModule : Module<FileInspectorView, FileInspectorViewModel>
    {
        public FileInspectorModule(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {

        }

        public override bool LoadOnDemand
        {
            get => true;
        }
        public override bool AllowMultiple => true;
    }
}
