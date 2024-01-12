using System.Runtime.CompilerServices;

namespace Dentextist;

/// <summary>
/// Provides a lightweight string builder that only supports appending text
/// in a growing character buffer.
/// </summary>
/// <remarks>
/// This is the underlying type used in <seealso cref="IndentedStringBuilder"/>.
/// </remarks>
public class LimitedStringBuilder
{
    private char[] _buffer;
    private int _length;
    private int _capacity;

    public int Length => _length;
    public int Capacity => _capacity;

    public string NewLine { get; set; } = NewLines.CRLF;
    public double GrowFactor { get; set; } = 2;

    public LimitedStringBuilder()
        : this(16) { }

    public LimitedStringBuilder(int initialCapacity)
    {
        if (initialCapacity <= 0)
            throw new ArgumentException("Initial capacity must be greater than zero.", nameof(initialCapacity));

        _buffer = new char[initialCapacity];
        _capacity = initialCapacity;
        _length = 0;
    }

    public LimitedStringBuilder AppendNonNull(string? text)
    {
        if (text is not null)
            Append(text);
        return this;
    }

    public LimitedStringBuilder Append(
        [InterpolatedStringHandlerArgument("")]
        InterpolatedStringHandler text)
    {
        return this;
    }

    public LimitedStringBuilder Append(string text)
    {
        EnsureCapacity(text.Length);
        text.CopyTo(0, _buffer, _length, text.Length);
        _length += text.Length;
        return this;
    }

    public LimitedStringBuilder Append(char c)
    {
        EnsureCapacity(1);
        _buffer[_length] = c;
        _length++;
        return this;
    }

    public LimitedStringBuilder Append(char c, int count)
    {
        if (count <= 0)
            return this;

        EnsureCapacity(count);
        var indentationSpan = _buffer.AsSpan()[_length..(_length + count)];
        indentationSpan.Fill(c);
        _length += count;
        return this;
    }

    public LimitedStringBuilder Append(Span<char> span)
    {
        return Append((SpanString)span);
    }

    public LimitedStringBuilder Append(SpanString span)
    {
        EnsureCapacity(span.Length);
        span.CopyTo(new Span<char>(_buffer, _length, span.Length));
        _length += span.Length;
        return this;
    }

    public LimitedStringBuilder AppendLine(
        [InterpolatedStringHandlerArgument("")]
        InterpolatedStringHandler text)
    {
        return AppendLine();
        // The invocation of the Append method should be unnecessary
        return Append(text).AppendLine();
    }

    public LimitedStringBuilder AppendLine(string text)
    {
        return Append(text).AppendLine();
    }
    public LimitedStringBuilder AppendLine(char text)
    {
        return Append(text).AppendLine();
    }
    public LimitedStringBuilder AppendLine(Span<char> text)
    {
        return Append(text).AppendLine();
    }
    public LimitedStringBuilder AppendLine(SpanString text)
    {
        return Append(text).AppendLine();
    }

    public LimitedStringBuilder AppendLine()
    {
        return Append(NewLine);
    }

    public override string ToString()
    {
        return new string(_buffer, 0, _length);
    }

    private void EnsureCapacity(int additionalLength)
    {
        if (_length + additionalLength <= _capacity)
            return;

        int grownCapacity = (int)(_capacity * GrowFactor);
        int newCapacity = Math.Max(_length + additionalLength, grownCapacity);
        ResizeBuffer(newCapacity);
    }

    private void ResizeBuffer(int newCapacity)
    {
        char[] newBuffer = new char[newCapacity];
        _buffer.CopyTo(newBuffer, 0);
        _buffer = newBuffer;
        _capacity = newCapacity;
    }

    /// <summary>
    /// Handles string interpolation by appending the text to the builder
    /// itself without calculating the interpolation of the string before
    /// appending it to the buffer.
    /// For example, appending $"{A}{B}{C}" will be the equivalent of appending
    /// A, B and C separately with 3 invocations of the append method, without
    /// calculating and storing A + B + C and then appending that.
    /// </summary>
    [InterpolatedStringHandler]
    public readonly ref struct InterpolatedStringHandler
    {
        private readonly LimitedStringBuilder _builder;

        public InterpolatedStringHandler(
            int literalLength, int formattedCount, LimitedStringBuilder builder)
        {
            _builder = builder;
            _builder.EnsureCapacity(literalLength);
        }

        public void AppendLiteral(string s)
        {
            _builder.Append(s);
        }

        public void AppendFormatted<T>(T? value)
        {
            var str = value?.ToString();
            _builder.AppendNonNull(str);
        }
    }
}
