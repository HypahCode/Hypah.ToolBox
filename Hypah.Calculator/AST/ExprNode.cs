using Hypah.Calculator.Units;

namespace Hypah.Calculator.AST;

public abstract record ExprNode;

public sealed record NumberNode(double Value, UnitDefinition? Unit) : ExprNode;

public sealed record BinaryOpNode(string Operator, ExprNode Left, ExprNode Right) : ExprNode;

public sealed record UnaryOpNode(string Operator, ExprNode Operand) : ExprNode;
