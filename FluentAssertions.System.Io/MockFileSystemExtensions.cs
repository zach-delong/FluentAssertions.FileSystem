using System.IO.Abstractions.TestingHelpers;

namespace FluentAssertions.FileSystem;

public static class MockFileSystemExtensions
{
    public static MockFileSystemAssertions Should(this MockFileSystem system)
    {
	return new MockFileSystemAssertions(system);
    }
}
