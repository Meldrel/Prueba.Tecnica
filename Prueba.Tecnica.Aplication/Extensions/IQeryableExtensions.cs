using System.Linq.Expressions;

namespace Prueba.Tecnica.Aplication.Extensions
{
    public static class IQueryableExtensions
    {
        /// <summary>
        /// Añade el filtro del where si se cumple la condition 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source">IQueryable sobre el que se quiere hacer la consulta</param>
        /// <param name="condition">Condición para que se ejecute el predicado</param>
        /// <param name="predicate">Predicado que se quiere ejecutar</param>
        /// <returns></returns>
        public static IQueryable<TSource> WhereIf<TSource>(this IQueryable<TSource> source, bool condition, Expression<Func<TSource, bool>> predicate)
        {
            if (condition)
                return source.Where(predicate);
            else
                return source;
        }
    }
}
