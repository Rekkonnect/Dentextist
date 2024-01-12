# Dentextist

## Summary

A lightweight package containing string builders supporting custom indentation and enough flexibility for generating code.

This package can be a helpful partner when writing [Roslyn source generators](https://learn.microsoft.com/en-us/dotnet/csharp/roslyn-sdk/source-generators-overview).

View the version history [here](VersionHistory.md).

## Package

The package is found on [NuGet](https://www.nuget.org/packages/Dentextist).

This project is licensed under the MIT license.

## Utilities

### LimitedStringBuilder

A lightweight string builder that only supports appending strings to a growable character buffer. It is intentionally limited to the scope of appending strings, which is the most common tactic in many string building applications.

### IndentedStringBuilder

A string builder that also enables indenting on every new line. Allows customizing the indentation character and its size. Makes use of the `IDisposable` pattern to enable using nesting inside `using` statements, which is convenient for recursive functions.

### CSharpCodeBuilder

A string builder extending `IndentedStringBuilder` with the bonus of supporting bracket blocks, utilizing the same pattern with `using` statements.
