using EnvDTE;
using Microsoft.VisualStudio.Settings;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell.Settings;
using Microsoft.VisualStudio.TemplateWizard;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace RootTemplatePlaceholder
{
    class TBBProjectWizard : IWizard
    {
        public void BeforeOpeningFile(ProjectItem projectItem)
        {
        }

        public void ProjectFinishedGenerating(Project project)
        {
        }

        public void ProjectItemFinishedGenerating(ProjectItem projectItem)
        {
        }

        public void RunFinished()
        {
        }
        
        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            //Apply all the settings from the options dialog page
            DTE dte = (DTE)automationObject;
            Properties props = dte.get_Properties("Parallel Package Options", "General");
            
            replacementsDictionary.Add("$tbbheaders$", props.Item("HeaderPath").Value.ToString());
            replacementsDictionary.Add("$tbblibs$", props.Item("LibraryPath").Value.ToString());
            replacementsDictionary.Add("$tbbdlls$", props.Item("DLLPath").Value.ToString());
        }

        public bool ShouldAddProjectItem(string filePath)
        {
            return true;
        }
    }
}
