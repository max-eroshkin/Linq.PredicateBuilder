using System.Linq;
using Bimlab.Nuke.Components;
using Nuke.Common;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.Execution;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

[CheckBuildProjectConfigurations]
[UnsetVisualStudioEnvironmentVariables]
[GitHubActions(
    "CI",
    GitHubActionsImage.Ubuntu1804,
    OnPullRequestBranches = new[] { "develop", "master", "release/**", "hotfix/**" },
    OnPushBranches = new[] { "develop", "master", "release/**", "hotfix/**" },
    InvokedTargets = new[] { nameof(Test) })]
class Build : NukeBuild, IPublish
{
    public static int Main() => Execute<Build>(x => x.From<ICompile>().Compile);

    [Solution("Linq.PredicateBuilder.sln")]
    public readonly Solution Solution;

    Solution IHazSolution.Solution => Solution;

    Target Clean => _ => _
        .Before<IRestore>()
        .Executes(() =>
        {
            GlobDirectories(Solution.Directory, "**/bin", "**/obj")
                .Where(x => !IsDescendantPath(BuildProjectDirectory, x))
                .ForEach(DeleteDirectory);
        });

    Target Test => _ => _
        .DependsOn<ICompile>()
        .Executes(() =>
        {
            DotNetTest(s => s
                .SetProjectFile(Solution)
                .SetConfiguration(From<IHazConfiguration>().Configuration)
                .EnableNoBuild());
        });
    
    T From<T>()
        where T : INukeBuild
        => (T)(object)this;
}