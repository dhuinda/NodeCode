using CodeDesigner.Core.ast;

namespace CodeDesigner.Core;

/// <summary>
/// Adds prototype declarations for functions that need them. Prototypes are needed in order to call functions before
/// they are defined. In the LLVM IR, these prototypes are usually removed (by built-in LLVM optimizations) and the
/// prototypes and functions are just merged, with the functions in the correct order.
/// </summary>
public class PrototypeAnalyzer
{

    // todo: if more analysis steps are needed (i.e., type inference and generic type generation), there should
    // probably be a unified traversal class which then invokes *Analyzer with a specific node. just to reduce how many
    // times the AST is traversed
    private readonly List<ASTNode> _ast;
    private readonly HashSet<string> _functionNames = new();
    private readonly HashSet<string> _neededPrototypes = new();
    private readonly Dictionary<string, ASTFunctionDefinition> _futurePrototypes = new();
    private string _currentNamespace = "default";

    public PrototypeAnalyzer(List<ASTNode> ast)
    {
        _ast = ast;
    }

    private void Analyze(ASTNode node)
    {
        switch (node.GetType().Name)
        {
            case "ASTFunctionDefinition":
            {
                var funcDef = (ASTFunctionDefinition) node;
                var fullName = $"{_currentNamespace}.{funcDef.Name}";
                _functionNames.Add(fullName);
                if (_neededPrototypes.Contains(fullName))
                {
                    _neededPrototypes.Remove(fullName);
                    _futurePrototypes.Add(fullName, funcDef);
                }
                foreach (var b in funcDef.Body)
                {
                    Analyze(b);
                }

                break;
            }
            case "ASTFunctionInvocation":
            {
                var funcInv = (ASTFunctionInvocation) node;
                var fullName = funcInv.Name.Contains('.') ? funcInv.Name : $"{_currentNamespace}.{funcInv.Name}";
                if (!funcInv.Name.StartsWith("extern.") && !_functionNames.Contains(fullName))
                {
                    _neededPrototypes.Add(fullName);
                }

                foreach (var arg in funcInv.Args)
                {
                    Analyze(arg);
                }

                break;
            }
            case "ASTNamespace":
            {
                var nmspc = (ASTNamespace) node;
                _currentNamespace = nmspc.Name;
                foreach (var child in nmspc.Body)
                {
                    Analyze(child);
                }

                _currentNamespace = "default";
                break;
            }
            case "ASTIfStatement":
            {
                var ifStmt = (ASTIfStatement) node;
                Analyze(ifStmt.Condition);
                foreach (var child in ifStmt.IfBody)
                {
                    Analyze(child);
                }

                foreach (var child in ifStmt.ElseBody)
                {
                    Analyze(child);
                }

                break;
            }
            case "ASTBinaryExpression":
            {
                var binExp = (ASTBinaryExpression) node;
                Analyze(binExp.Lhs);
                Analyze(binExp.Rhs);
                break;
            }
            case "ASTReturn":
            {
                var ret = (ASTReturn) node;
                if (ret.Expression != null)
                {
                    Analyze(ret.Expression);
                }
                break;
            }
        }
    }
    
    public void Run()
    {
        foreach (var node in _ast)
        {
            Analyze(node);
        }

        foreach (var (fullName, node) in _futurePrototypes)
        {
            Console.WriteLine("generating prototype for " + fullName);
            var paramTypes = node.Params.Select(param => param.VariableType).ToList();
            _ast.Insert(0, new ASTPrototypeDeclaration(fullName, paramTypes, node.ReturnType, false));
        }
    }
    
}