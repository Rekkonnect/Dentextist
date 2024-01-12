namespace Dentextist;

public sealed class CSharpCodeBuilder(char indentationCharacter, int indentationSize)
    : IndentedStringBuilder(indentationCharacter, indentationSize)
{
    public BracketBlock EnterBracketBlock(char open = '{', char close = '}')
    {
        return new(this, open, close);
    }

    public readonly struct BracketBlock
        : IDisposable
    {
        private readonly CSharpCodeBuilder _builder;

        private readonly char _close;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        [Obsolete(message: "Do not use the parameterless constructor", error: true)]
        public BracketBlock() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public BracketBlock(CSharpCodeBuilder builder, char open, char close)
        {
            _builder = builder;
            _close = close;
            _builder.AppendLine(open);
            _builder.NestingLevel++;
        }

        public void Dispose()
        {
            _builder.NestingLevel--;
            _builder.AppendLine(_close);
        }
    }
}
