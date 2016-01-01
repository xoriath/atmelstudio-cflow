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

            statusService = ATServiceProvider.StatusService;
            dte = GetService(typeof(DTE)) as DTE;
            
            solutionBuildManager = ServiceProvider.GlobalProvider.GetService(typeof(SVsSolutionBuildManager)) as IVsSolutionBuildManager2;
            if (solutionBuildManager != null)
                solutionBuildManager.AdviseUpdateSolutionEvents(this, out updateSolutionEventsCookie);

        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (solutionBuildManager != null && updateSolutionEventsCookie != 0)
                solutionBuildManager.UnadviseUpdateSolutionEvents(updateSolutionEventsCookie);
        }

        private IStatusReportService statusService;
        private DTE dte;
        private IVsSolutionBuildManager2 solutionBuildManager;
        private uint updateSolutionEventsCookie;

        private string cflow = null;
        public string CFlow
        {
            get { return cflow ?? (cflow = GetCFlowLocation()); }
            set { cflow = value; }
        }

        private void Log(string message)
        {
            statusService.WriteOutputWindow(message, "CFlow", StatusSeverity.INFO);
        }
        private void Log(object o)
        {
            statusService.WriteOutputWindow(o.ToString(), "CFlow", StatusSeverity.INFO);
        }

        private string GetCFlowLocation()
        {
            var extensionManager = GetService(typeof(SVsExtensionManager)) as IVsExtensionManager;

            IInstalledExtension self = extensionManager.GetInstalledExtension("CFlow.Morten Engelhardt Olsen.d8f9f542-e05d-4939-a76e-73211eca9cda");
                
            var cflow = self.Content.Where(e => e.ContentTypeName.Equals("CFlow.Executable")).First();

            var fullPath = Path.Combine(self.InstallPath, cflow.RelativePath);

            if (File.Exists(fullPath))
                Log($"Found CFlow at '{ fullPath }'");
            else
                Log($"Failed to find CFlow at '{ fullPath }'");

            return fullPath;
            
        }

        private void UpdateProject(string name)
        {
            Log($"Build finished notification for '{ name }'");
            var projects = Projects();

            var project = projects.Where(p => p.Name.Equals(name)).First();

            UpdateProject(project);
        }

        private void UpdateProject(Project project)
        {
            var files = GetFiles(project.ProjectItems);
            files = files.Where(f => Path.GetExtension(f) != ".ld").ToList();

            var runner = new Runner.ReverseRunner(CFlow, files);
            var output = runner.Run();

            Log("###########################");
            Log(output);
            Log("###########################");

            var results = Parser.CFlowReverseParser.Parse(new List<string>(output.Split(new string[] { Environment.NewLine }, StringSplitOptions.None)));
            foreach(var f in results)
            {
                Log(f.Function);
            }
        }

        private static List<string> GetFiles(ProjectItems items)
        {
            var files = new List<string>();
            foreach (var item in items)
            {
                var file = (ProjectItem)item;
                if (File.Exists(file.FileNames[0]))
                    files.Add(file.FileNames[0]);
                else if (file.ProjectItems != null && file.ProjectItems.Count != 0)
                    files.AddRange(GetFiles(file.ProjectItems));
            }
            return files;
        }

        public IList<EnvDTE.Project> Projects()
        {
            EnvDTE.Projects projects = dte.Solution.Projects;

            var list = new List<EnvDTE.Project>();
            
            foreach(var item in projects)
            {
                var project = item as EnvDTE.Project;
                if (project == null)
                    continue;

                if (project.Kind == EnvDTE80.ProjectKinds.vsProjectKindSolutionFolder)
                    list.AddRange(GetSolutionFolderProjects(project));
                else
                    list.Add(project);
            }

            return list;
        }

        private static IEnumerable<EnvDTE.Project> GetSolutionFolderProjects(EnvDTE.Project solutionFolder)
        {
            var list = new List<EnvDTE.Project>();
            for (var i = 1; i <= solutionFolder.ProjectItems.Count; i++)
            {
                var subProject = solutionFolder.ProjectItems.Item(i).SubProject;
                if (subProject == null)
                    continue;

                // If this is another solution folder, do a recursive call, otherwise add
                if (subProject.Kind == EnvDTE80.ProjectKinds.vsProjectKindSolutionFolder)
                    list.AddRange(GetSolutionFolderProjects(subProject));
                else
                    list.Add(subProject);
            }

            return list;
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
