using System.Linq;
using Sitko.ModelSelector.Base;

namespace Sitko.ModelSelector.Extensions
{
    public static class QueryableExtension
    {
        public static IQueryable<T> ApplySelector<T>(this IQueryable<T> model, ModelSelector<T> selector)
        {
            if (selector != null)
                model = selector.ApplyPredicates(model);
            return model;
        }
    }
}