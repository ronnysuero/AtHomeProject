using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace AtHomeProject.Domain.Models.Pagination
{
    [ExcludeFromCodeCoverage]
    public class PagedResult<T> : PagedResultBase where T : class
    {
        public IList<T> Results { get; set; } = new List<T>();
    }
}
