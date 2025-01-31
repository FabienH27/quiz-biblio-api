using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace QuizBiblio.Models.Quiz.Utils;

public static class QuizMapper
{

    public static ProjectionDefinition<TSource, TDestination> ProjectTo<TSource, TDestination>()
    {
        var parameter = Expression.Parameter(typeof(TSource), "e");
        var bindings = typeof(TDestination)
            .GetProperties()
            .Where(destProp => destProp.CanWrite)
            .Select(destProp =>
            {
                var sourceProp = typeof(TSource).GetProperty(destProp.Name);
                if (sourceProp == null) return null;

                var sourcePropAccess = Expression.Property(parameter, sourceProp);
                return Expression.Bind(destProp, sourcePropAccess);
            })
            .Where(binding => binding != null)
            .ToArray();

        var memberInit = Expression.MemberInit(Expression.New(typeof(TDestination)), bindings!);
        var lambda = Expression.Lambda<Func<TSource, TDestination>>(memberInit, parameter);

        return Builders<TSource>.Projection.Expression(lambda);
    }

}
