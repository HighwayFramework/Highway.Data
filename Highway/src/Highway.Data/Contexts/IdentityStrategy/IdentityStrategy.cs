
using System;
using System.Linq.Expressions;
using System.Reflection;


namespace Highway.Data.Contexts
{
    public abstract class IdentityStrategy<TType, TIdentity> : IIdentityStrategy<TType>
        where TType : class
    {
        public static TIdentity LastValue = default(TIdentity);
        public static Func<TIdentity> Generator = null;

        private readonly Action<TType> identitySetter;

        public IdentityStrategy(Expression<Func<TType, TIdentity>> property)
        {
            identitySetter = obj => 
            {
                var propertyInfo = GetPropertyFromExpression(property);
                var id = (TIdentity) propertyInfo.GetValue(obj, null);
                if(IsDefaultUnsetValue(id)) propertyInfo.SetValue(obj,  Next(), null);
            };
        }

        protected abstract bool IsDefaultUnsetValue(TIdentity id);

        public void Assign(TType entity)
        {
            identitySetter.Invoke(entity);
        }

        public TIdentity Next()
        {
            if (Generator == null) throw new NotImplementedException();
            return Generator.Invoke();
        }

        private PropertyInfo GetPropertyFromExpression(Expression<Func<TType, TIdentity>> lambda)
        {
            MemberExpression Exp = null;
            Expression Sub;

            // this line is necessary, because sometimes the expression 
            // comes as Convert(originalexpression)
            if (lambda.Body is UnaryExpression)
            {
                UnaryExpression UnExp = (UnaryExpression) lambda.Body;
                if (UnExp.Operand is MemberExpression)
                {
                    Exp = (MemberExpression) UnExp.Operand;
                }
                else
                    throw new ArgumentException();
            }
            else if (lambda.Body is MemberExpression)
            {
                Exp = (MemberExpression) lambda.Body;
            }
            else
            {
                throw new ArgumentException();
            }

            return (PropertyInfo) Exp.Member;
        }
    }
}