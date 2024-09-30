using Lemon.Hosting.Modularization.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleA.ViewModels
{
    public class ViewModelA : IViewModel
    {
        public void Dispose()
        {
            
        }
        public string Greeting => nameof(ViewModelA);
    }
}
