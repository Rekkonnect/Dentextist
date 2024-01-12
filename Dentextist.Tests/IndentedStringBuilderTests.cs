namespace Dentextist.Tests;

public class IndentedStringBuilderTests : BaseIndentedStringBuilderTests
{
    [Test]
    public void RegularNestingUsage()
    {
        var builder = new IndentedStringBuilder(' ', 4);

        builder.AppendSingleLineContent("Header");
        builder.AppendLine("file");
        using (var _ = builder.IncrementNestingLevel())
        {
            builder.AppendLine("start0");
            builder.AppendLine("middle0");
            using (var _1 = builder.IncrementNestingLevel())
            {
                builder.AppendLine("start1");
                builder.AppendLine("middle1");
                builder.AppendLine("end1");
            }
            builder.AppendLine("end0");
        }

        const string expectedString = """
            Headerfile
                start0
                middle0
                    start1
                    middle1
                    end1
                end0

            """;

        AssertStrings(builder, expectedString);
    }

    [Test]
    public void AppendSingleLineContent_WithMultilineString()
    {
        var builder = new IndentedStringBuilder(' ', 4);

        const string multilineString = """
            abcd
            efgh
            ijk
            """;

        builder.AppendLine("Header");
        using (var _ = builder.IncrementNestingLevel())
        {
            builder.AppendSingleLineContent(multilineString);
            builder.AppendLine("another line");
        }

        const string expectedString = """
            Header
                abcd
            efgh
            ijkanother line

            """;

        AssertStrings(builder, expectedString);
    }
}
