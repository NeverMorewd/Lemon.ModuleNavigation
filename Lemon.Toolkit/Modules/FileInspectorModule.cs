using Lemon.ModuleNavigation.Avaloniaui;
using Lemon.Toolkit.ViewModels;
using Lemon.Toolkit.Views;
using System;

namespace Lemon.Toolkit.Modules
{
    public class FileInspectorModule : AvaModule<FileInspectorView, FileInspectorViewModel>
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
