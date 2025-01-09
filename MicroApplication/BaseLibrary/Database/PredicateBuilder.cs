using System.Linq.Expressions;

namespace BaseLibrary.Database
{
    //http://www.albahari.com/nutshell/predicatebuilder.aspx
    public static class PredicateBuilder
    {
        public static Expression<Func<T, bool>> True<T>() { return f => true; }
        public static Expression<Func<T, bool>> False<T>() { return f => false; }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1,
                                                            Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>
                  (Expression.OrElse(expr1.Body, invokedExpr), expr1.Parameters);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1,
                                                             Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>
                  (Expression.AndAlso(expr1.Body, invokedExpr), expr1.Parameters);
        }
    }

    /*
     var inner = PredicateBuilder.False<Product>();
inner = inner.Or (p => p.Description.Contains ("foo"));
inner = inner.Or (p => p.Description.Contains ("far"));

var outer = PredicateBuilder.True<Product>();
outer = outer.And (p => p.Price > 100);
outer = outer.And (p => p.Price < 1000);
outer = outer.And (inner);

    //private Expression<Func<ApplicationUser, bool>> PreparePredicate() 
        //{
        //    var outer = PredicateBuilder.True<ApplicationUser>();
        //    outer = outer.And(p => p.Price > 100);
        //    outer = outer.And(p => p.Price < 1000);
        //    outer = outer.And(inner);
        //}
     */
}
