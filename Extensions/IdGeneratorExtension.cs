using AnjeerMarket.Models.Commons;

namespace AnjeerMarket.Extensions;

public static class IdGeneratorExtension
{
    public static long GenerateId<T>(this IEnumerable<T> values) where T : Auditable
        => values.Any() ? values.Last().Id + 1 : 1;
}
