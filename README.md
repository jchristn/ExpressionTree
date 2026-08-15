![alt tag](https://raw.githubusercontent.com/jchristn/ExpressionTree/main/Assets/icon.ico)

# ExpressionTree

Simple class to represent an expression tree.

[![NuGet Version](https://img.shields.io/nuget/v/ExpressionTree.svg?style=flat)](https://www.nuget.org/packages/ExpressionTree/) [![NuGet](https://img.shields.io/nuget/dt/ExpressionTree.svg)](https://www.nuget.org/packages/ExpressionTree) 

## Help, Feedback, Contribute

If you have any issues or feedback, please file an issue here in Github. We'd love to have you help by contributing code for new features, optimization to the existing codebase, ideas for future releases, or fixes!

## Overview

This project was built to provide a simple class to represent an expression tree (term-operator-term) with support for nested expressions, literals, arrays, and lists.

## Serialization to and Deserialization from JSON

Refer to the custom serializers in the `Test.NewtonsoftJson` and `Test.SystemTextJson` projects.

## Testing

The library is validated by a [Touchstone](https://www.nuget.org/packages/Touchstone)-based test suite. Test cases are defined once as runner-agnostic descriptors and executed through multiple hosts:

- `Test.Shared` &mdash; central source of truth; all test-case descriptors live here (`Touchstone.Core`).
- `Test.Automated` &mdash; Touchstone CLI runner (`Touchstone.Cli`). Run with `dotnet run --project src/Test.Automated`.
- `Test.Xunit` &mdash; xUnit adapter (`Touchstone.XunitAdapter`). Run with `dotnet test src/Test.Xunit`.
- `Test.Nunit` &mdash; NUnit adapter (`Touchstone.NunitAdapter`). Run with `dotnet test src/Test.Nunit`.

The `Test`, `Test.NewtonsoftJson`, and `Test.SystemTextJson` projects remain as interactive console applications for exploring the library by hand.

## New in v1.1.x

- `Copy` API

## Simple Example
```csharp
using ExpressionTree;

Expr e = new Expr(5, OperatorEnum.GreaterThan, 1);
```
## Nested Example
```csharp
Expr e = new Expr(
	new Expr(5, OperatorEnum.GreaterThan, 1),
	OperatorEnum.And,
	new Expr("Name", OperatorEnum.In, new List<string> { "Smith", "Anderson", "Jones" })
	);
```
## Create a Copy
```csharp
Expr eCopy = e.Copy()
```
## Version History

Refer to CHANGELOG.md for version history.
