﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using System.Threading.Tasks;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace roslyn_test
{
    public class ServiceProxyGenerater
    {


        #region Implementation of IServiceProxyGenerater

        /// <summary>
        /// 生成服务代理。
        /// </summary>
        /// <param name="interfacTypes">需要被代理的接口类型。</param>
        /// <returns>服务代理实现。</returns>
        public IEnumerable<Type> GenerateProxys(IEnumerable<Type> interfacTypes)
        {
            var assemblys = AppDomain.CurrentDomain.GetAssemblies();

            assemblys = assemblys.Where(i => i.IsDynamic == false).ToArray();
            var trees = interfacTypes.Select(GenerateProxyTree).ToList();

            CSharpCompilation.Create("test", trees).Emit("tt.dll");
            return null;

            //var stream = CompilationUtilitys.CompileClientProxy(trees,
            //    assemblys
            //        .Select(a => MetadataReference.CreateFromFile(a.Location))
            //        .Concat(new[]
            //        {
            //            MetadataReference.CreateFromFile(typeof(Task).GetTypeInfo().Assembly.Location)
            //        }),
            //    _logger);

            //using (stream)
            //{
            //    var assembly = Assembly.Load(stream.ToArray());

            //    return assembly.GetExportedTypes();
            //}
        }

        /// <summary>
        /// 生成服务代理代码树。
        /// </summary>
        /// <param name="interfaceType">需要被代理的接口类型。</param>
        /// <returns>代码树。</returns>
        public SyntaxTree GenerateProxyTree(Type interfaceType)
        {
            var className = interfaceType.Name.StartsWith("I") ? interfaceType.Name.Substring(1) : interfaceType.Name;
            className += "ClientProxy";

            var members = new List<MemberDeclarationSyntax>
            {
                GetConstructorDeclaration(className)
            };

            members.AddRange(GenerateMethodDeclarations(interfaceType.GetMethods()));
            return CompilationUnit()
                .WithUsings(GetUsings())
                .WithMembers(
                    SingletonList<MemberDeclarationSyntax>(
                        NamespaceDeclaration(
                            QualifiedName(
                                QualifiedName(
                                    IdentifierName("Rabbit"),
                                    IdentifierName("Rpc")),
                                IdentifierName("ClientProxys")))
                .WithMembers(
                    SingletonList<MemberDeclarationSyntax>(
                        ClassDeclaration(className)
                            .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)))
                            .WithBaseList(
                                BaseList(
                                    SeparatedList<BaseTypeSyntax>(
                                        new SyntaxNodeOrToken[]
                                        {
                                            SimpleBaseType(IdentifierName("ServiceProxyBase")),
                                            Token(SyntaxKind.CommaToken),
                                            SimpleBaseType(GetQualifiedNameSyntax(interfaceType))
                                        })))
                            .WithMembers(List(members))))))
                .NormalizeWhitespace().SyntaxTree;
        }

        #endregion Implementation of IServiceProxyGenerater

        #region Private Method

        private static QualifiedNameSyntax GetQualifiedNameSyntax(Type type)
        {
            var fullName = type.Namespace + "." + type.Name;
            return GetQualifiedNameSyntax(fullName);
        }

        private static QualifiedNameSyntax GetQualifiedNameSyntax(string fullName)
        {
            return GetQualifiedNameSyntax(fullName.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries));
        }

        private static QualifiedNameSyntax GetQualifiedNameSyntax(IReadOnlyCollection<string> names)
        {
            var ids = names.Select(IdentifierName).ToArray();

            var index = 0;
            QualifiedNameSyntax left = null;
            while (index + 1 < names.Count)
            {
                left = left == null ? QualifiedName(ids[index], ids[index + 1]) : QualifiedName(left, ids[index + 1]);
                index++;
            }
            return left;
        }

        private static SyntaxList<UsingDirectiveSyntax> GetUsings()
        {
            return List(
                new[]
                {
                    UsingDirective(IdentifierName("System")),
                    UsingDirective(GetQualifiedNameSyntax("System.Threading.Tasks")),
                    UsingDirective(GetQualifiedNameSyntax("System.Collections.Generic"))
                });
        }

        private static ConstructorDeclarationSyntax GetConstructorDeclaration(string className)
        {
            return ConstructorDeclaration(Identifier(className))
                .WithModifiers(
                    TokenList(
                        Token(SyntaxKind.PublicKeyword)))
                .WithParameterList(
                    ParameterList(
                        SeparatedList<ParameterSyntax>(
                            new SyntaxNodeOrToken[]
                            {
                                Parameter(
                                    Identifier("remoteInvokeService"))
                                    .WithType(
                                        IdentifierName("IRemoteInvokeService")),
                                Token(SyntaxKind.CommaToken),
                                Parameter(
                                    Identifier("typeConvertibleService"))
                                    .WithType(
                                        IdentifierName("ITypeConvertibleService"))
                            })))
                .WithInitializer(
                        ConstructorInitializer(
                            SyntaxKind.BaseConstructorInitializer,
                            ArgumentList(
                                SeparatedList<ArgumentSyntax>(
                                    new SyntaxNodeOrToken[]{
                                        Argument(
                                            IdentifierName("remoteInvokeService")),
                                        Token(SyntaxKind.CommaToken),
                                        Argument(
                                            IdentifierName("typeConvertibleService"))}))))
                .WithBody(Block());
        }

        private IEnumerable<MemberDeclarationSyntax> GenerateMethodDeclarations(IEnumerable<MethodInfo> methods)
        {
            var array = methods.ToArray();
            return array.Select(GenerateMethodDeclaration).ToArray();
        }

        private static TypeSyntax GetTypeSyntax(Type type)
        {
            //没有返回值。
            if (type == null)
                return null;

            //非泛型。
            if (!type.GetTypeInfo().IsGenericType)
                return GetQualifiedNameSyntax(type.FullName);

            var list = new List<SyntaxNodeOrToken>();

            foreach (var genericTypeArgument in type.GenericTypeArguments)
            {
                list.Add(genericTypeArgument.GetTypeInfo().IsGenericType
                    ? GetTypeSyntax(genericTypeArgument)
                    : GetQualifiedNameSyntax(genericTypeArgument.FullName));
                list.Add(Token(SyntaxKind.CommaToken));
            }

            var array = list.Take(list.Count - 1).ToArray();
            var typeArgumentListSyntax = TypeArgumentList(SeparatedList<TypeSyntax>(array));
            return GenericName(type.Name.Substring(0, type.Name.IndexOf('`')))
                .WithTypeArgumentList(typeArgumentListSyntax);
        }

        private MemberDeclarationSyntax GenerateMethodDeclaration(MethodInfo method)
        {
            var serviceId = "";// _serviceIdGenerator.GenerateServiceId(method);
            var returnDeclaration = GetTypeSyntax(method.ReturnType);

            var parameterList = new List<SyntaxNodeOrToken>();
            var parameterDeclarationList = new List<SyntaxNodeOrToken>();

            foreach (var parameter in method.GetParameters())
            {
                parameterDeclarationList.Add(Parameter(
                                    Identifier(parameter.Name))
                                    .WithType(GetQualifiedNameSyntax(parameter.ParameterType)));
                parameterDeclarationList.Add(Token(SyntaxKind.CommaToken));

                parameterList.Add(InitializerExpression(
                    SyntaxKind.ComplexElementInitializerExpression,
                    SeparatedList<ExpressionSyntax>(
                        new SyntaxNodeOrToken[]{
                            LiteralExpression(
                                SyntaxKind.StringLiteralExpression,
                                Literal(parameter.Name)),
                            Token(SyntaxKind.CommaToken),
                            IdentifierName(parameter.Name)})));
                parameterList.Add(Token(SyntaxKind.CommaToken));
            }
            if (parameterList.Any())
            {
                parameterList.RemoveAt(parameterList.Count - 1);
                parameterDeclarationList.RemoveAt(parameterDeclarationList.Count - 1);
            }

            var declaration = MethodDeclaration(
                returnDeclaration,
                Identifier(method.Name))
                .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.AsyncKeyword)))
                .WithParameterList(ParameterList(SeparatedList<ParameterSyntax>(parameterDeclarationList)));

            ExpressionSyntax expressionSyntax;
            StatementSyntax statementSyntax;

            if (method.ReturnType != typeof(Task))
            {
                expressionSyntax = GenericName(
                    Identifier("Invoke")).WithTypeArgumentList(((GenericNameSyntax)returnDeclaration).TypeArgumentList);
            }
            else
            {
                expressionSyntax = IdentifierName("Invoke");
            }
            expressionSyntax = AwaitExpression(
                InvocationExpression(expressionSyntax)
                    .WithArgumentList(
                        ArgumentList(
                            SeparatedList<ArgumentSyntax>(
                                new SyntaxNodeOrToken[]
                                {
                                        Argument(
                                            ObjectCreationExpression(
                                                GenericName(
                                                    Identifier("Dictionary"))
                                                    .WithTypeArgumentList(
                                                        TypeArgumentList(
                                                            SeparatedList<TypeSyntax>(
                                                                new SyntaxNodeOrToken[]
                                                                {
                                                                    PredefinedType(
                                                                        Token(SyntaxKind.StringKeyword)),
                                                                    Token(SyntaxKind.CommaToken),
                                                                    PredefinedType(
                                                                        Token(SyntaxKind.ObjectKeyword))
                                                                }))))
                                                .WithInitializer(
                                                    InitializerExpression(
                                                        SyntaxKind.CollectionInitializerExpression,
                                                        SeparatedList<ExpressionSyntax>(
                                                            parameterList)))),
                                        Token(SyntaxKind.CommaToken),
                                        Argument(
                                            LiteralExpression(
                                                SyntaxKind.StringLiteralExpression,
                                                Literal(serviceId)))
                                }))));

            if (method.ReturnType != typeof(Task))
            {
                statementSyntax = ReturnStatement(expressionSyntax);
            }
            else
            {
                statementSyntax = ExpressionStatement(expressionSyntax);
            }

            declaration = declaration.WithBody(
                        Block(
                            SingletonList(statementSyntax)));

            return declaration;
        }

        #endregion Private Method
    }
}