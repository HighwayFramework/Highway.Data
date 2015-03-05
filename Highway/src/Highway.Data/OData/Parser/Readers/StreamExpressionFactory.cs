using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq.Expressions;

namespace Highway.Data.OData.Parser.Readers
{
    internal class StreamExpressionFactory : ByteArrayExpressionFactoryBase<Stream>
    {
        public override ConstantExpression Convert(string token)
        {
            var baseResult = base.Convert(token);
            if (baseResult.Value != null)
            {
                var stream = new MemoryStream((byte[]) baseResult.Value);

                return Expression.Constant(stream);
            }

            return baseResult;
        }
    }
}