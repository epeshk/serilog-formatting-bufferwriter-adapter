using System.Text;
using Serilog.Events;

namespace Serilog.Formatting.BufferWriter.Adapter.Tests;

public class WrappingTests
{
  [Fact]
  public void WrapFormatter()
  {
    var stream = new MemoryStream();
    
    var logger = new LoggerConfiguration()
      .WriteTo.RawStream(stream, new TextFormatter().ToBufferWriterFormatter(), buffered: false)
      .CreateLogger();

    var logEvent = new LogEvent(DateTimeOffset.Now, LogEventLevel.Information, null, MessageTemplate.Empty,
      Array.Empty<LogEventProperty>());

    for (int i = 0; i < 4; i++)
      logger.Write(logEvent);

    var lines = Encoding.UTF8.GetString(stream.ToArray()).Split("\n", StringSplitOptions.TrimEntries|StringSplitOptions.RemoveEmptyEntries);
    
    Assert.Equal(4, lines.Length);
    Assert.All(lines, line => Assert.Equal("{}", line));

  }
}

class TextFormatter : ITextFormatter
{
  public void Format(LogEvent logEvent, TextWriter output)
  {
    output.WriteLine("{}");
  }
}