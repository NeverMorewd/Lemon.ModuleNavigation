using Avalonia;
using Avalonia.Controls;
using Lemon.Toolkit.Framework;
using Lemon.Toolkit.ViewModels;
using Lemon.Toolkit.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemon.Toolkit.Modules
{
    public class FileInspectorModule : TabModule<SingleFileView, SingleFileViewModel>
    {
        public FileInspectorModule(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {

        }

        public override bool LoadDefault
        {
            get => false;
        }
    }
}
