using Atmel.Studio.Services;
using EnvDTE;
using System.Collections.Generic;
using System.IO;

namespace CFlow
{
    public class VSHelper
    {
        public static IList<Project> GetProjects(DTE dte)
        {
            Projects projects = dte.Solution.Projects;

            var list = new List<Project>();

            foreach (var item in projects)
            {
                var project = item as Project;
                if (project == null)
                    continue;

                if (project.Kind == EnvDTE80.ProjectKinds.vsProjectKindSolutionFolder)
                    list.AddRange(GetSolutionFolderProjects(project));
                else
                    list.Add(project);
            }

            return list;
        }


        public static IEnumerable<Project> GetSolutionFolderProjects(Project solutionFolder)
        {
            var list = new List<Project>();
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

        public static List<string> GetFiles(ProjectItems items)
        {
            var files = new List<string>();
            foreach (var item in items)
            {
                var file = (ProjectItem)item;
                for (short i = 0; i < file.FileCount; ++i)
                {
                    if (File.Exists(file.FileNames[i]))
                        files.Add(file.FileNames[i]);
                    else if (file.ProjectItems != null && file.ProjectItems.Count != 0)
                        files.AddRange(GetFiles(file.ProjectItems));
                }
                
            }
            return files;
        }

        public static List<string> GetFiles(Project project)
        {
            return GetFiles(project.ProjectItems);
        }

        public static void Log(string message, StatusSeverity severity = StatusSeverity.INFO)
        {
            ATServiceProvider.StatusService.WriteOutputWindow(message, "CFlow", severity);
        }
        public static void Log(object o, StatusSeverity severity = StatusSeverity.INFO)
        {
            Log(o.ToString(), severity);
        }

    }
}
