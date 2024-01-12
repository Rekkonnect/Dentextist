﻿namespace Dentextist.Tests;

public class CSharpStringBuilderTests : BaseIndentedStringBuilderTests
{
    [Test]
    public void SampleGeneratedCode()
    {
        var builder = new CSharpCodeBuilder(' ', 4);

        builder.AppendSingleLineContent("// Copyright (c) 2024");
        builder.AppendLine();
        builder.AppendLine("file class Test");
        using (var _ = builder.EnterBracketBlock())
        {
            builder.AppendLine("public static int Method()");
            using (var _1 = builder.EnterBracketBlock())
            {
                builder.AppendSingleLineContent("return int.MaxValue");
                builder.AppendLine(" - 432;");
            }
            builder.AppendLine("public enum TestEnum");
            using (var _1 = builder.EnterBracketBlock())
            {
                builder.Append("A");
                builder.AppendSingleLineContent(",");
                builder.AppendLine();
                builder.AppendLine("B,");
                builder.AppendLine("C,");
            }
        }

        const string expectedString = """
            // Copyright (c) 2024
            file class Test
            {
                public static int Method()
                {
                    return int.MaxValue - 432;
                }
                public enum TestEnum
                {
                    A,
                    B,
                    C,
                }
            }

            """;

        AssertStrings(builder, expectedString);
    }
}
