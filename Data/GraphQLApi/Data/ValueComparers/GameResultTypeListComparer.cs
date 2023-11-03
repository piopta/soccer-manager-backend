using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace GraphQLApi.Data.ValueComparers
{
    public class GameResultTypeListComparer : ValueComparer<IList<GameResultType>>
    {
        public GameResultTypeListComparer()
            : base((c1, c2) => c1.SequenceEqual(c2),
              c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
              c => (IList<GameResultType>)c.ToHashSet())
        {
        }
    }
}
