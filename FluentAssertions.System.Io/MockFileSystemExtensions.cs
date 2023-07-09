using System.IO.Abstractions.TestingHelpers;

namespace System.IO.Abstractions.TestingHelpers.FluentAssertions;

public static class MockFileSystemExtensions
{
    public static MockFileSystemAssertions Should(this MockFileSystem system)
    {
	return new MockFileSystemAssertions(system);
    }
}
