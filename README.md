# FluentAssertions.FileSystem

This is a small extension for the popular [FluentAssertions](https://fluentassertions.com/) library that provides some simple assertions for use with the `MockFileSystem` type provided by [System.IO.Abstractions](https://github.com/TestableIO/System.IO.Abstractions) project.

This package provides a new namespace `FluentAssertions.FileSystem` that provides an extension method `.Should()` on the `MockFilesystem` type provided by System.IO.Abstractions.

The Should method returns an assertions object that has several helpful assertions which can analyze and assert about the `MockFilesystem` in question.

## Example 

```csharp
using System.IO.Abstractions.TestingHelpers;
using System.IO.Abstractions;

public class ContainDirectoryTests 
{
    public void foo()
    {
        var mockFileSystem = new MockFileSystem();

        mockFileSystem
	    .AddDirectory("example_directory");

        mockFileSystem
            .Should()
			.ContainDirectory("example_directory");
    }
```
See the provided unit tests for more examples, and the comments in `MockFileSystemAssertions` for full details about what assertions are available and how to use them.
