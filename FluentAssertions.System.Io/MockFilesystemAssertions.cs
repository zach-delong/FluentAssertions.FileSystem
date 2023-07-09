using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;

namespace System.IO.Abstractions.TestingHelpers.FluentAssertions;

public class MockFileSystemAssertions : ReferenceTypeAssertions<MockFileSystem, MockFileSystemAssertions>
{
    protected override string Identifier => "file system";

    public MockFileSystemAssertions(MockFileSystem fileSystem) : base(fileSystem)
    { }

    [CustomAssertion]
    public AndConstraint<MockFileSystemAssertions> Contain(
	string? path,
	string because="",
	params object[] args)
    {

	// you will get a warning that path might be null in the
	// second ForCondition. this isn't a problem, because the
	// first ForCondition catches that case, so I have disabled
	// the warning below, just for this line
#pragma warning disable CS8604
        Execute.Assertion
            .BecauseOf(because, args)
            .ForCondition(!string.IsNullOrWhiteSpace(path))
	    .FailWith("The input path should not be null or empty")
	    .Then
	    .Given(() => Subject.AllNodes)
	    .ForCondition(paths => paths.Any(p => p.Contains(path)))
	    .FailWith("Expected {context:system} to contain {0}{reason}, but found {1}.", _ => path, paths => paths);
#pragma warning restore CS8604


        return new AndConstraint<MockFileSystemAssertions>(this);
    }

    [CustomAssertion]
    public AndConstraint<MockFileSystemAssertions> ContainDirectory(
	string? path,
	string because="",
	params object[] args)

    {
	// you will get a warning that path might be null in the
	// second ForCondition. this isn't a problem, because the
	// first ForCondition catches that case, so I have disabled
	// the warning below, just for this line

#pragma warning disable CS8604
        Execute.Assertion
            .BecauseOf(because, args)
            .ForCondition(!string.IsNullOrWhiteSpace(path))
	    .FailWith("The input path should not be null or empty")
	    .Then
	    .Given(() => Subject.AllDirectories)
	    .ForCondition(paths => paths.Any(p => p.Contains(path)))
	    .FailWith("Expected {context:system} to contain {0}{reason}, but found {1}.", _ => path, paths => paths);

#pragma warning restore CS8604

        return new AndConstraint<MockFileSystemAssertions>(this);
    }

    [CustomAssertion]
    public AndConstraint<MockFileSystemAssertions> NotContainFile(
    string path)
    {
        Subject.FileExists(path).Should().BeFalse($"the file system should not contain {path} but does.");

        return new AndConstraint<MockFileSystemAssertions>(this);
    }

    [CustomAssertion]
    public AndConstraint<MockFileSystemAssertions> FileHasContents(string path, string expectedContents, string becauseReasons = "", params object[] becauseArgs)
    {
        path.Should().NotBeNullOrEmpty("You must provide a file path to check");
        expectedContents.Should().NotBeNull("You must provide contents to compare against (but you may provide the empty string!");

        Subject.GetFile(path)
	    ?.TextContents
	    ?.Should()
	    .NotBeNull()
	    .And
	    .BeEquivalentTo(expectedContents, becauseReasons, becauseArgs);

        return new AndConstraint<MockFileSystemAssertions>(this);
    }

    [CustomAssertion]
    public AndConstraint<MockFileSystemAssertions> FileExistsWithContents(string path,
                                                                          string expectedContents,
                                                                          string becauseReasons = "",
                                                                          params object[] becauseArgs)
    {
        return Contain(path)
	    .And.FileHasContents(path, expectedContents, becauseReasons, becauseArgs);
    }

    [CustomAssertion]
    public AndConstraint<MockFileSystemAssertions> HaveFileCount(int expectedFileCount, string because = "", params object[] becauseArgs)
    {
        Subject.AllFiles.Should().HaveCount(expectedFileCount, because, becauseArgs);
        return new AndConstraint<MockFileSystemAssertions>(this);
    }
}
