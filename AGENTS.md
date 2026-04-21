# AGENTS.md

This file provides guidance to WARP (warp.dev) when working with code in this repository.

## Existing agent/rule docs
- No existing `AGENTS.md`, `WARP.md`, `CLAUDE.md`, `.cursorrules`, `.cursor/rules/`, or `.github/copilot-instructions.md` were found.

## Development commands
- Restore dependencies:
  - `dotnet restore Linq.PredicateBuilder.sln`
- Build (library + tests; analyzers run as part of build):
  - `dotnet build Linq.PredicateBuilder.sln -c Debug`
- Run all tests in solution:
  - `dotnet test Linq.PredicateBuilder.sln -c Debug`
- Run tests from the main test project only:
  - `dotnet test tests/BuilderTests/BuilderTests.csproj -c Debug`
- Run a single test by fully-qualified name:
  - `dotnet test tests/BuilderTests/BuilderTests.csproj -c Debug --filter "FullyQualifiedName=BuilderTests.Samples.SampleTests.Predicate"`
- Run tests by class:
  - `dotnet test tests/BuilderTests/BuilderTests.csproj -c Debug --filter "FullyQualifiedName~BuilderTests.Samples.SampleTests"`
- CI-equivalent entrypoint (NUKE):
  - Linux/macOS: `./build.sh Test`
  - Windows: `build.cmd Test` (or `./build.ps1 Test`)

## Project structure (high level)
- `source/Linq.PredicateBuilder`: main library (`netstandard2.0`) implementing fluent predicate construction over LINQ expression trees.
- `tests/BuilderTests`: primary xUnit test suite (`net6.0`) that documents expected operator behavior and query output.
- `build/Build.cs` + `build/_build.csproj`: NUKE build orchestration used by CI (`Test` target depends on compile).

## Architecture: how predicates are built
1. Entry points:
   - `QueryableBuilderExtensions.Build(...)` applies a built predicate to `IQueryable<T>`.
   - `QueryableBuilderExtensions.BuildPredicate(...)` returns `Expression<Func<T,bool>>?` directly.
2. Builder bootstrap:
   - `BuildPredicate` creates `BuilderInit<T>` (initial operator state) with an `IOperationStrategy`.
3. Fluent chain execution model:
   - Extension methods in `source/Linq.PredicateBuilder/Extensions/` (`Equals`, `Contains`, `In`, `Any`, `Where`, `Brackets`, `Conditional`) each:
     - ask the strategy to create one segment expression (or `null` if ignored),
     - combine it through the current operator,
     - return a `Result<T>` that exposes `.And` / `.Or` for the next segment.
4. Logical composition:
   - `AndOperator`, `OrOperator`, and `NotOperator` in `Operators/` delegate to `PredicateExtensions.And/Or/Not`.
   - Parameter unification is handled via `ParameterRebinder` when composing expressions.
5. Segment ignore behavior:
   - `OperationStrategy` controls preprocessing and ignore rules (`BuilderOptions.Default = IgnoreCase | IgnoreDefaultInputs | Trim`).
   - `SegmentIgnored` skips null/default/empty inputs.
   - `Conditional(...)` wraps the next operation with `IgnogreOperator`, which forwards `null` when condition is false.
6. Precedence and grouping:
   - Mixed logical precedence is explicit; use `Brackets(...)` to nest sub-builders and force grouping.
7. Nested collections:
   - `Any(...)` builds nested predicates by running a child builder for collection elements, then wrapping with `Enumerable.Any`.

## Important implementation notes
- There is no separate lint command; StyleCop analyzers are wired through project references/rulesets and are enforced during build/test.
- `tests/ResearchTests/ResearchTests.csproj` exists but is not part of `Linq.PredicateBuilder.sln` and references a missing `Research/Research.csproj`; use `tests/BuilderTests` for normal validation.
