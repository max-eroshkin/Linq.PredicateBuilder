using Fallout.Common;
using Fallout.Common.CI.GitHubActions;
using Isotopes;

[GitHubActions(
    "CI",
    GitHubActionsImage.UbuntuLatest,
    OnPullRequestBranches = ["develop", "master", "release/**", "hotfix/**"],
    OnPushBranches = ["develop", "master", "release/**", "hotfix/**", "feature/**"],
    InvokedTargets = [nameof(ITest.Test)])]
class Build : FalloutBuild, IPublish, ITest
{
    public static int Main() => Execute<Build>(x => x.From<ICompile>().Compile);

    T From<T>()
        where T : IFalloutBuild
        => (T)(object)this;
}