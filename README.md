# Serilog.Formatting.BufferWriter.Adapter

Integration package to use standard `ITextFormatter` [Serilog](https://serilog.net) formatters for writing events in `IBufferWriterFormatter` interface from [Serilog.Formatting.BufferWriter](https://www.nuget.org/packages/Serilog.Formatting.BufferWriter) package.

### Usage

```csharp
var logger = new LoggerConfiguration()
  .WriteTo.RawConsole(new CompactJsonFormatter().ToBufferWriterFormatter())
  .CreateLogger();
```
