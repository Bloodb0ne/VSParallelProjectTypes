using System;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.VisualStudio.Shell;
using Task = System.Threading.Tasks.Task;
using System.ComponentModel;
using EnvDTE;

namespace ParallelProjectTypes
{

    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [Guid(ParallelProjectTypesPackage.PackageGuidString)]
    [ProvideOptionPage(typeof(ParallelOptionsPage),
    "Parallel Package Options", "General", 0, 0, true)]
    public sealed class ParallelProjectTypesPackage : AsyncPackage
    {
       
        public const string PackageGuidString = "91f55a1e-4049-489f-83bb-9a08183d14cd";

        #region Package Members
        
        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            await this.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
            
        }

        #endregion
    }
}
