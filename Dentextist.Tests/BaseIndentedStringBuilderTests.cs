namespace Dentextist.Tests;

public abstract class BaseIndentedStringBuilderTests
{
    protected static void AssertStrings(IndentedStringBuilder builder, string expected)
    {
        Assert.That(builder.ToString(), Is.EqualTo(expected));
    }
}
