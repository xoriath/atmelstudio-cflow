//------------------------------------------------------------------------------
// <copyright file="CFlow.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.Win32;
using Microsoft.VisualStudio.ExtensionManager;
using System.Linq;
using System.IO;
using Atmel.Studio.Services;
using System.Collections.Generic;
using EnvDTE;

namespace CFlow
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the
    /// IVsPackage interface and uses the registration attributes defined in the framework to
    /// register itself and its components with the shell. These attributes tell the pkgdef creation
    /// utility what data to put into .pkgdef file.
    /// </para>
    /// <para>
    /// To get loaded into VS, the package must be referred by &lt;Asset Type="Microsoft.VisualStudio.VsPackage" ...&gt; in .vsixmanifest file.
    /// </para>
    /// </remarks>
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
    [ProvideAutoLoad(UIContextGuids80.NoSolution)]
    [Guid(PackageGuidString)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideToolWindow(typeof(ReverseFunctionWindow))]
    public sealed class CFlowPackage : Package, IVsUpdateSolutionEvents2
    {
        /// <summary>
        /// CFlow GUID string.
        /// </summary>
        public const string PackageGuidString = "35e5da5c-b7b9-4ca7-b39d-2f52ffc0af10";

        /// <summary>
        /// Initializes a new instance of the <see cref="CFlow"/> class.
        /// </summary>
        public CFlowPackage()
        {
            // Inside this method you can place any initialization code that does not require
            // any Visual Studio service because at this point the package object is created but
            // not sited yet inside Visual Studio environment. The place to do all the other
            // initialization is the Initialize method.
        }

        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            Dte = GetService(typeof(DTE)) as DTE;
            
            solutionBuildManager = ServiceProvider.GlobalProvider.GetService(typeof(SVsSolutionBuildManager)) as IVsSolutionBuildManager2;
            if (solutionBuildManager != null)
                solutionBuildManager.AdviseUpdateSolutionEvents(this, out updateSolutionEventsCookie);
            ReverseFunctionWindowCommand.Initialize(this);

        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (solutionBuildManager != null && updateSolutionEventsCookie != 0)
                solutionBuildManager.UnadviseUpdateSolutionEvents(updateSolutionEventsCookie);
        }

        public DTE Dte;
        private IVsSolutionBuildManager2 solutionBuildManager;
        private uint updateSolutionEventsCookie;

        private string cflow = null;
        public string CFlow
        {
            get { return cflow ?? (cflow = GetCFlowLocation()); }
            set { cflow = value; }
        }

        private string GetCFlowLocation()
        {
            var extensionManager = GetService(typeof(SVsExtensionManager)) as IVsExtensionManager;

            IInstalledExtension self = extensionManager.GetInstalledExtension("CFlow.Morten Engelhardt Olsen.d8f9f542-e05d-4939-a76e-73211eca9cda");
                
            var cflow = self.Content.Where(e => e.ContentTypeName.Equals("CFlow.Executable")).First();

            var fullPath = Path.Combine(self.InstallPath, cflow.RelativePath);

            if (File.Exists(fullPath))
                VSHelper.Log($"Found CFlow at '{ fullPath }'");
            else
                VSHelper.Log($"Failed to find CFlow at '{ fullPath }'");

            return fullPath;
            
        }

        private void UpdateProject(string name)
        {
            VSHelper.Log($"Build finished notification for '{ name }'");
            var projects = VSHelper.GetProjects(Dte);

            var project = projects.Where(p => p.Name.Equals(name)).First();

            UpdateProject(project);
        }

        private void UpdateProject(Project project)
        {
            // TODO: Should get resolved include files as well (to resolve IO headers)
            var files = VSHelper.GetFiles(project);
            files = files.Where(f => Path.GetExtension(f) != ".ld").ToList();

            var reverse_runner = new Runner.ReverseRunner(CFlow, files);
            var tree_runner = new Runner.TreeRunner(CFlow, files);
            var xref_runner = new Runner.XrefRunner(CFlow, files);

            var reverse_output = reverse_runner.Run();
            var tree_output = tree_runner.Run();
            var xref_output = xref_runner.Run();

            VSHelper.Log("############################# Tree  Output ###########################");
            VSHelper.Log(tree_output);
            VSHelper.Log("######################################################################");
            VSHelper.Log("########################### Reverse Output ###########################");
            VSHelper.Log(reverse_output);
            VSHelper.Log("######################################################################");
            VSHelper.Log("########################### XREF    Output ###########################");
            VSHelper.Log(xref_output);
            VSHelper.Log("######################################################################");


            var tree_results = Parser.CFlowReverseParser.Parse(new List<string>(tree_output.Split(new string[] { Environment.NewLine }, StringSplitOptions.None)));
            var reverse_results = Parser.CFlowReverseParser.Parse(new List<string>(reverse_output.Split(new string[] { Environment.NewLine }, StringSplitOptions.None)));
            var xref_result_bound = Parser.CFlowXrefParser.Parse(new List<string>(xref_output.Split(new string[] { Environment.NewLine }, StringSplitOptions.None)));
            var xref_result_unbound = Parser.CFlowXrefParser.ParseUnbound(new List<string>(xref_output.Split(new string[] { Environment.NewLine }, StringSplitOptions.None)));

        }
        
        


        int IVsUpdateSolutionEvents2.UpdateSolution_Begin(ref int pfCancelUpdate)
        {
            return VSConstants.S_OK;
        }

        int IVsUpdateSolutionEvents2.UpdateSolution_Done(int fSucceeded, int fModified, int fCancelCommand)
        {
            return VSConstants.S_OK;
        }

        int IVsUpdateSolutionEvents2.UpdateSolution_StartUpdate(ref int pfCancelUpdate)
        {
            return VSConstants.S_OK;
        }

        int IVsUpdateSolutionEvents2.UpdateSolution_Cancel()
        {
            return VSConstants.S_OK;

        }

        int IVsUpdateSolutionEvents2.OnActiveProjectCfgChange(IVsHierarchy pIVsHierarchy)
        {
            return VSConstants.S_OK;
        }

        int IVsUpdateSolutionEvents2.UpdateProjectCfg_Begin(IVsHierarchy pHierProj, IVsCfg pCfgProj, IVsCfg pCfgSln, uint dwAction, ref int pfCancel)
        {
            return VSConstants.S_OK;
        }

        int IVsUpdateSolutionEvents2.UpdateProjectCfg_Done(IVsHierarchy pHierProj, IVsCfg pCfgProj, IVsCfg pCfgSln, uint dwAction, int fSuccess, int fCancel)
        {
            object o;
            pHierProj.GetProperty((uint)VSConstants.VSITEMID.Root, (int)__VSHPROPID.VSHPROPID_Name, out o);

            var name = o as string;
            if (!string.IsNullOrEmpty(name))
                UpdateProject(name);

            return VSConstants.S_OK;
        }

        int IVsUpdateSolutionEvents.UpdateSolution_Begin(ref int pfCancelUpdate)
        {
            return VSConstants.S_OK;
        }

        int IVsUpdateSolutionEvents.UpdateSolution_Done(int fSucceeded, int fModified, int fCancelCommand)
        {
            return VSConstants.S_OK;
        }

        int IVsUpdateSolutionEvents.UpdateSolution_StartUpdate(ref int pfCancelUpdate)
        {
            return VSConstants.S_OK;
        }

        int IVsUpdateSolutionEvents.UpdateSolution_Cancel()
        {
            return VSConstants.S_OK;
        }

        int IVsUpdateSolutionEvents.OnActiveProjectCfgChange(IVsHierarchy pIVsHierarchy)
        {
            return VSConstants.S_OK;
        }

        #endregion
    }
}
