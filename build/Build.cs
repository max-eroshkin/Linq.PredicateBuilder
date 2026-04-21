using System;
using System.Linq;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

class Build : NukeBuild
{
    public static int Main() => Execute<Build>(x => x.Compile);

    [Solution] [Required] public readonly Solution Solution;

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    Target Restore => _ => _
        .Executes(() =>
        {
            DotNetRestore(cfg => cfg
                .Apply(_ => _.SetProjectFile(Solution)));
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetBuild(cfg => cfg
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration));
        });

    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
            Solution.Directory.GlobDirectories("**/bin", "**/obj", "output", "artifacts")
                .Where(x => !BuildProjectDirectory.Contains(x))
                .ForEach(x => x.DeleteDirectory());
        });

    Target Test => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            DotNetTest(s => s
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .EnableNoBuild());
        });

    T From<T>()
        where T : INukeBuild
        => (T)(object)this;
}