using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using System.IO.Abstractions.TestingHelpers;

namespace FluentAssertions.FileSystem;

public class MockFileSystemAssertions : ReferenceTypeAssertions<MockFileSystem, MockFileSystemAssertions>
{
    protected override string Identifier => "file system";

    public MockFileSystemAssertions(MockFileSystem fileSystem) : base(fileSystem) { }

    ///<summary>
    /// Given a path to a file or directory assert that it should exist in the file system.
    ///</summary>
    ///<param name="path">A file system path to a file or directory that should exist.</param>
    ///<param name="because">An explanation of why something might have failed</param>
    ///<param name="args">Arguments to format into the because description</param>
    ///<returns>An Constraint so that other constraints can be chained.</returns>
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

    ///<summary>
    /// Given a path to a particular directory, assert that it should exist in the file system.
    ///</summary>
    ///<param name="path">A file system path to a directory that should exist.</param>
    ///<param name="because">An explanation of why something might have failed</param>
    ///<param name="args">Arguments to format into the because description</param>
    ///<returns>An Constraint so that other constraints can be chained.</returns>
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

    ///<summary>
    /// Given a path to a particular file, assert that it should not exist in the file system.
    ///</summary>
    ///<param name="path">A file system path to a file that should not exist.</param>
    ///<param name="because">An explanation of why something might have failed</param>
    ///<param name="args">Arguments to format into the because description</param>
    ///<returns>An Constraint so that other constraints can be chained.</returns>
    [CustomAssertion]
    public AndConstraint<MockFileSystemAssertions> NotContainFile(
    string path)
    {
        Subject.FileExists(path).Should().BeFalse($"the file system should not contain {path} but does.");

        return new AndConstraint<MockFileSystemAssertions>(this);
    }


    ///<summary>
    /// Given a path to a file, assuming it exists in the file system, assert that it should have specific contents.
    ///</summary>
    ///<param name="path">A file system path to a file assumed to exist.</param>
    ///<param name="expectedContents">A string containing the expected output contents of the file.</param>
    ///<param name="because">An explanation of why something might have failed</param>
    ///<param name="args">Arguments to format into the because description</param>
    ///<returns>An Constraint so that other constraints can be chained.</returns>
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


    ///<summary>
    /// Given a path to a particular file, assert that file should exist and that it should have specific contents.
    ///</summary>
    ///<param name="path">A file system path to a file that should exist.</param>
    ///<param name="expectedContents">A string containing the expected output contents of the file.</param>
    ///<param name="because">An explanation of why something might have failed</param>
    ///<param name="args">Arguments to format into the because description</param>
    ///<returns>An Constraint so that other constraints can be chained.</returns>
    [CustomAssertion]
    public AndConstraint<MockFileSystemAssertions> FileExistsWithContents(string path,
                                                                          string expectedContents,
                                                                          string becauseReasons = "",
                                                                          params object[] becauseArgs)
    {
        return Contain(path)
	    .And.FileHasContents(path, expectedContents, becauseReasons, becauseArgs);
    }

    ///<summary>
    /// Asserts that a specific number of files exist in the mock file system
    ///</summary>
    ///<param name="expectedFileCount">The integer number representing the expected number of files in the file system.</param>
    ///<param name="because">An explanation of why something might have failed</param>
    ///<param name="becauseArgs">Arguments to format into the because description</param>
    ///<returns>An Constraint so that other constraints can be chained.</returns>
    [CustomAssertion]
    public AndConstraint<MockFileSystemAssertions> HaveFileCount(int expectedFileCount, string because = "", params object[] becauseArgs)
    {
        Subject.AllFiles.Should().HaveCount(expectedFileCount, because, becauseArgs);
        return new AndConstraint<MockFileSystemAssertions>(this);
    }
}
