using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CatFactLogger.Shared.Data
{
    public sealed record CatFact(
        [property: JsonPropertyName("fact")] string Fact,
        [property: JsonPropertyName("length")] int Length);
}
