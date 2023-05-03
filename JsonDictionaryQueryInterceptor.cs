using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Linq.Expressions;

namespace EFCoreIssue30818
{
    internal class JsonDictionaryQueryInterceptor : IQueryExpressionInterceptor
    {
        public Expression QueryCompilationStarting(Expression queryExpression, QueryExpressionEventData eventData)
        {
            var newExpr = new Visitor().Visit(queryExpression);
            return newExpr;
        }

        private class Visitor : ExpressionVisitor
        {
            protected override Expression VisitIndex(IndexExpression node)
            {
                // Replace dictionary indexer Item[] with JsonValue
                if (node.Object?.Type == typeof(JsonDictionary))
                    return GetJsonValueExpression(Visit(node.Object), node.Arguments[0]);

                return base.VisitIndex(node);
            }

            protected override Expression VisitMethodCall(MethodCallExpression node)
            {
                // Replace dictionary Item() call with JsonValue
                if (node.Object?.Type == typeof(JsonDictionary) &&
                    node.Method.Name == "get_Item")
                    return GetJsonValueExpression(Visit(node.Object), Visit(node.Arguments[0]));

                return base.VisitMethodCall(node);
            }

            private static Expression GetJsonValueExpression(Expression property, Expression key)
            {
                var constant = (key as ConstantExpression).Value;
                return Expression.Call(
                    DbFunctionsExtensions.JsonValueMethod,
                    property,
                    Expression.Constant($"$.\"{constant}\""));
            }
        }
    }
}