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
