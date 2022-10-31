using NTS.Model.Categories;
using NTS.Model.CustomerRequirement;
using NTS.Model.Stage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model
{
    public static class SQLHelpper
    {
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string ordering, bool ordertype)
        {
            var type = typeof(T);
            var property = type.GetProperty(ordering);
            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExp = Expression.Lambda(propertyAccess, parameter);
            MethodCallExpression resultExp;
            if (ordertype)
            {
                resultExp = Expression.Call(typeof(Queryable), "OrderBy", new Type[] { type, property.PropertyType }, source.Expression, Expression.Quote(orderByExp));
            }
            else
            {
                resultExp = Expression.Call(typeof(Queryable), "OrderByDescending", new Type[] { type, property.PropertyType }, source.Expression, Expression.Quote(orderByExp));
            }
            return source.Provider.CreateQuery<T>(resultExp);
        }

        public static object OrderBy(object dataQuey, object orderBy, object orderType)
        {
            throw new NotImplementedException();
        }

        public static object OrderBy(IQueryable<StageModel> dataQuery, object orderBy, object orderType)
        {
            throw new NotImplementedException();
        }

        public static object OrderBy(IQueryable<ResultSearchQuoteStepModel> dataQuerys, object orderBy, object orderType)
        {
            throw new NotImplementedException();
        }

        public static object OrderBy(IQueryable<CustomerRequirementSearchResultModel> dataQuery, object orderBy, object orderType)
        {
            throw new NotImplementedException();
        }
    }
}
