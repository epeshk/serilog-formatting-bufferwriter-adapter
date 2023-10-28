using System.Buffers;
using System.Text;
using Serilog.Events;
using Serilog.Rendering;

[assembly: CLSCompliant(true)]

namespace Serilog.Formatting;

public static class TextFormatterToBufferWriterExtensions
{
  public static IBufferWriterFormatter ToBufferWriterFormatter(this ITextFormatter formatter, IFormatProvider? formatProvider = null, Encoding? encoding=null)
  {
    return new TextFormatterAdapter(formatter, formatProvider, encoding ?? Encoding.UTF8);
  }
}

class TextFormatterAdapter : IBufferWriterFormatter
{
  readonly ITextFormatter formatter;
  readonly IFormatProvider? formatProvider;

  public TextFormatterAdapter(ITextFormatter formatter, IFormatProvider? formatProvider, Encoding encoding)
  {
    this.formatter = formatter;
    this.formatProvider = formatProvider;
    Encoding = encoding;
  }

  public void Format(LogEvent logEvent, IBufferWriter<byte> bufferWriter)
  {
    using var writer = ReusableStringWriter.GetOrCreate(formatProvider);
    formatter.Format(logEvent, writer);
    var str = writer.GetStringBuilder().ToString();
#if NET6_0_OR_GREATER
    var maxByteCount = Encoding.GetMaxByteCount(str.Length);
    var span = bufferWriter.GetSpan(maxByteCount);
    bufferWriter.Advance(Encoding.GetBytes(str, span));
#else
    bufferWriter.Write(Encoding.GetBytes(str));
#endif
  }

  public Encoding Encoding { get; }
}
