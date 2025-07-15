using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class CriticalNodeAccessibilityAnalyzer : DiagnosticAnalyzer
    {
        private static DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            id: "INJECT001",
            title: "CriticalNodeAttribute can only be applied to public members!",
            messageFormat: "The member '{0}' is not public. [CriticalNodeAttribute] can only be applied to public fields or properties.",
            category: "Usage",
            defaultSeverity: DiagnosticSeverity.Warning,
            isEnabledByDefault: true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.EnableConcurrentExecution();
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.RegisterSyntaxNodeAction(AnalyzeAttribute, SyntaxKind.Attribute);
        }

        private void AnalyzeAttribute(SyntaxNodeAnalysisContext context)
        {
            var attributeSyntax = (AttributeSyntax)context.Node;
            var symbol = context.SemanticModel.GetSymbolInfo(attributeSyntax).Symbol;
            if (symbol == null)
                return;

            if (symbol.ContainingType.Name != "CriticalNodeAttribute")
                return;

            var target = attributeSyntax.Parent?.Parent;
            if (target is PropertyDeclarationSyntax prop)
            {
                var propSymbol = context.SemanticModel.GetDeclaredSymbol(prop);
                if (propSymbol is { DeclaredAccessibility: not Accessibility.Public })
                {
                    var diagnostic = Diagnostic.Create(Rule, prop.Identifier.GetLocation(), prop.Identifier.Text);
                    context.ReportDiagnostic(diagnostic);
                }
            }
            else if (target is FieldDeclarationSyntax field)
            {
                foreach (var variable in field.Declaration.Variables)
                {
                    var fieldSymbol = context.SemanticModel.GetDeclaredSymbol(variable);
                    if (fieldSymbol is { DeclaredAccessibility: not Accessibility.Public })
                    {
                        var diagnostic = Diagnostic.Create(Rule, variable.Identifier.GetLocation(), variable.Identifier.Text);
                        context.ReportDiagnostic(diagnostic);
                    }
                }
            }
        }
    }
}
