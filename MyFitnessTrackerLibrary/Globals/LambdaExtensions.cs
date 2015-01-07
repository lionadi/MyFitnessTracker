using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFitnessTrackerLibrary.Globals
{


    public static class LambdaExtensions
    {
        public static IEnumerable<t> DistinctBy<t>(this IEnumerable<t> list, Func<t, object> propertySelector)
        {
            return list.GroupBy(propertySelector).Select(x => x.First());
        }
    }
}
