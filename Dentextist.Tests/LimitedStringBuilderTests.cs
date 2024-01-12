namespace Dentextist.Tests;

public class LimitedStringBuilderTests
{
    [Test]
    public void InterpolatedStringUsage()
    {
        var builder = new LimitedStringBuilder();

        int a = 1;
        int b = 2;
        int c = 3;

        builder.Append($".{a}.{b}|{c}");

        const string expectedString = ".1.2|3";

        AssertStrings(builder, expectedString);
    }

    private static void AssertStrings(LimitedStringBuilder builder, string expected)
    {
        Assert.That(builder.ToString(), Is.EqualTo(expected));
    }
}
