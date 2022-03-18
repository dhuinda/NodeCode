namespace CodeDesigner.Core;

public interface IAnalyzer
{
    bool ShouldAnalyzeNode(ASTNode astNode);
    
    void Analyze(ASTNode astNode, string currentNamespace);

    void Finalize(List<ASTNode> ast);
}