using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Highway.Data.Security.DataEntitlements.Expressions
{
    /// <summary>
    /// Represents a visitor or rewriter for secured expression trees.
    /// </summary>
    public class SecurityExpressionVistor : ExpressionVisitor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityExpressionVistor"/>.
        /// </summary>
        public SecurityExpressionVistor()
        {
            _metadatas = new List<IncludeSecurityExpressionBuilder>();
        }

        /// <summary>
        /// Visits the children of the <see cref="T:System.Linq.Expressions.MethodCallExpression"/> and collects metadata about secured included properties.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>
        /// The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.
        /// </returns>
        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Method.Name == "Include")
            {
                // If this is an "Include" method, we need to check for security.
                if (!IncludePropertyPathWasAlreadyVisited(node))
                {
                    CollectSecuredIncludeMetaDatas(node);
                }
            }
            else
            {
                // If this is not an "Include" method, we need to visit its children.
                foreach (var expression in node.Arguments)
                {
                    Visit(expression);
                }
            }

            return base.VisitMethodCall(node);
        }

        private void AddNestedPropertyDetails(IncludeSecurityExpressionBuilder includeMetadata, string propertyPathSegment, IncludePathMetadataFragment previousIncludePathMetadataFragment)
        {
            var currentProperty = previousIncludePathMetadataFragment.PropertySingularType.GetProperty(propertyPathSegment);
            var parameterExpression = Expression.Parameter(previousIncludePathMetadataFragment.PropertySingularType);
            var propertyAccessMetadata = new IncludePathMetadataFragment(previousIncludePathMetadataFragment.PropertySingularType, currentProperty.PropertyType, Expression.MakeMemberAccess(parameterExpression, currentProperty));
            includeMetadata.IncludedPropertyDetails.Add(propertyAccessMetadata);
        }

        private void AddPropertyDetails(IncludeSecurityExpressionBuilder includeMetadata, Type typeUnderQuery, ParameterExpression parameterForTypeUnderQuery, string propertyPathSegment)
        {
            var previousMetadata = includeMetadata.IncludedPropertyDetails.LastOrDefault();

            if (previousMetadata == null)
            {
                // This is the root property.
                AddRootPropertyDetails(includeMetadata, typeUnderQuery, parameterForTypeUnderQuery, propertyPathSegment);
            }
            else
            {
                // This is a nested property.
                AddNestedPropertyDetails(includeMetadata, propertyPathSegment, previousMetadata);
            }
        }

        private void AddRootPropertyDetails(IncludeSecurityExpressionBuilder includeMetadata, Type typeUnderQuery, ParameterExpression parameterForTypeUnderQuery, string propertyPathSegment)
        {
            var currentProperty = typeUnderQuery.GetProperty(propertyPathSegment);
            var propertyAccessMetadata = new IncludePathMetadataFragment(typeUnderQuery, currentProperty.PropertyType, Expression.MakeMemberAccess(parameterForTypeUnderQuery, currentProperty));
            includeMetadata.IncludedPropertyDetails.Add(propertyAccessMetadata);
        }

        private void CollectSecuredIncludeMetaDatas(MethodCallExpression callNode)
        {
            var includedPropertyPath = GetIncludedPropertyPath(callNode); // Example:  "Entity.Property.Property"
            if (callNode.Object == null)
            {
                return;
            }

            var queryType = callNode.Object.Type;

            var includeMetadata = new IncludeSecurityExpressionBuilder(includedPropertyPath);
            var typeUnderQuery = queryType.ToSingleType();
            var parameterForTypeUnderQuery = Expression.Parameter(typeUnderQuery);

            var includedPropertyPathSegments = includedPropertyPath.Split('.'); // Example:  {"Entity", "Property", "Property"}
            foreach (var propertyPathSegment in includedPropertyPathSegments)
            {
                AddPropertyDetails(includeMetadata, typeUnderQuery, parameterForTypeUnderQuery, propertyPathSegment);
            }

            _metadatas.Add(includeMetadata);
        }

        private string GetIncludedPropertyPath(MethodCallExpression callNode)
        {
            return ((ConstantExpression)callNode.Arguments[0]).Value.ToString();
        }

        private bool IncludePropertyPathWasAlreadyVisited(MethodCallExpression node)
        {
            return _metadatas.Any(x => x.IncludedPropertyPath == node.Arguments[0].ToString());
        }

        /// <summary>
        /// Gets a set of metadata representing secured, included properites.
        /// </summary>
        public IEnumerable<IncludeSecurityExpressionBuilder> Metadatas
        {
            get
            {
                return _metadatas;
            }
        }

        private List<IncludeSecurityExpressionBuilder> _metadatas;
    }
}
