![alt tag](https://raw.githubusercontent.com/jchristn/ExpressionTree/main/Assets/icon.ico)

# ExpressionTree

Simple class to represent an expression tree.

[![NuGet Version](https://img.shields.io/nuget/v/ExpressionTree.svg?style=flat)](https://www.nuget.org/packages/ExpressionTree/) [![NuGet](https://img.shields.io/nuget/dt/ExpressionTree.svg)](https://www.nuget.org/packages/ExpressionTree) 

## Help, Feedback, Contribute

If you have any issues or feedback, please file an issue here in Github. We'd love to have you help by contributing code for new features, optimization to the existing codebase, ideas for future releases, or fixes!

## Overview

This project was built to provide a simple class to represent an expression tree (term-operator-term) with support for nested expressions, literals, arrays, and lists.

## Deserialization from JSON

Refer to the custom serializers in the ```Test.NewtonsoftJson``` and ```Test.SystemTextJson``` projects.

## New in v1.0.0

- Initial release

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

## Version History

Refer to CHANGELOG.md for version history.
