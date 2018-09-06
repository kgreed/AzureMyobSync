using System;
using System.Linq;
using System.Linq.Expressions;

namespace SBD.AMS.MYOB
{
    //http://stackoverflow.com/questions/13045303/linq-function-to-handle-both-fields-equal-or-both-null-or-empty
    public static class CompareExtensions
    {
        public static IQueryable<T> FieldsAreEqualOrBothNullOrEmpty<T>(
            this IQueryable<T> source,
            Expression<Func<T, string>> member,
            string value)
        {
            Expression body;
            if (string.IsNullOrEmpty(value))
            {
                body = Expression.Call(typeof(string), "IsNullOrEmpty", null, member.Body);
            }
            else
            {
                try
                {

                    body = Expression.Equal(
                Expression.Call(member.Body, "ToLower", null),
                Expression.Constant(value.ToLower(), typeof(string)));
                }
                catch (Exception )
                {
                    //var s = ex.ToString();
                    //throw;
                    // https://social.msdn.microsoft.com/Forums/en-US/d260f631-cf76-4c7a-8305-903faf7f373b/nullable-types-in-expression-build?forum=linqprojectgeneral
                    body = Expression.Constant(false);
                }
            }
            var result = source.Where(Expression.Lambda<Func<T, bool>>(body, member.Parameters));
            return result;
        }
    }
}