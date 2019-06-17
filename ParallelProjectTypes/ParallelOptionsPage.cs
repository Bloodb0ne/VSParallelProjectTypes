using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell.Settings;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;

namespace ParallelProjectTypes
{
    [ComVisible(true)]
    public class ParallelOptionsPage : DialogPage
    {
        private BaseOptionsModel<ParallelOptionsPage> _model;


        public ParallelOptionsPage()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            var settingsManager = (IVsSettingsManager)ServiceProvider.GlobalProvider.GetService(typeof(SVsSettingsManager));
            this._model = new BaseOptionsModel<ParallelOptionsPage>(new ShellSettingsManager(settingsManager));
        }
        public override void LoadSettingsFromStorage()
        {
            _model.LoadSettings();
        }

        public override void SaveSettingsToStorage()
        {
            _model.SaveSettings();
        }

        private string libraryPath = "D:\\Projects\\ParallelProgBook\\TBB\\tbb2019\\lib\\ia32\\vc14";
        private string headersPath = "D:\\Projects\\ParallelProgBook\\TBB\\tbb2019\\include";
        private string dllPath = "D:\\Projects\\ParallelProgBook\\TBB\\tbb2019\\bin\\ia32\\vc14";

        [Category("Thread Building Blocks")]
        [DisplayName("Library Path")]
        [Description("Path to the approptiate .lib folder")]
        [DefaultValue("D:\\Projects\\ParallelProgBook\\TBB\\tbb2019\\lib\\ia32\\vc14")]
        [Editor(typeof(System.Windows.Forms.Design.FolderNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string LibraryPath { get => libraryPath; set => libraryPath = value; }

        [Category("Thread Building Blocks")]
        [DisplayName("Headers Path")]
        [Description("Path to the approptiate header folder")]
        [DefaultValue("D:\\Projects\\ParallelProgBook\\TBB\\tbb2019\\include")]
        [Editor(typeof(System.Windows.Forms.Design.FolderNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string HeaderPath { get => headersPath; set => headersPath = value; }

        [Category("Thread Building Blocks")]
        [DisplayName("DLL Path")]
        [Description("Path to the approptiate .dll folder")]
        [DefaultValue("D:\\Projects\\ParallelProgBook\\TBB\\tbb2019\\bin\\ia32\\vc14")]
        [Editor(typeof(System.Windows.Forms.Design.FolderNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string DLLPath { get => dllPath; set => dllPath = value; }
    }

}
