using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileSharing.Helpers
{
    public class PagedResult<T>
    {
        public int TotalPages { get; }
        public IEnumerable<T> Content { get; set; }
        public PagedResult(int totalResults, int resultsPerPage)
        {
            if (totalResults < 0)
            {
                throw new InvalidDataException($"'{nameof(resultsPerPage)}' must be at least 0, if you see  this, you probably have done something wrong.");
            }

            if (resultsPerPage < 1)
            {
                throw new InvalidDataException($"'{nameof(resultsPerPage)}' must be greater than 0.");
            }

            TotalPages = totalResults / resultsPerPage;

            if (totalResults % resultsPerPage != 0)
            {
                TotalPages++;
            }
        }
    }

    public class PagedResultHelper<T> : PagedResult<T>
    {
        public PagedResultHelper(IEnumerable<T> content, int totalResults, int resultsPerPage, int pageRequested)
                : base(totalResults, resultsPerPage)
        {
            if (pageRequested < 1)
            {
                throw new InvalidDataException($"'{nameof(pageRequested)}' should be greater than 0.");
            }

            pageRequested--;

            content = content.Skip(resultsPerPage * pageRequested);

            content = content.Take(resultsPerPage);

            Content = content;
        }
    }
}
