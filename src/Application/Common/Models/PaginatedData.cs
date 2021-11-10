using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Razor.Application.Models
{
    public class PaginatedData<T>
  {
    public int total { get; set; }
    public IEnumerable<T> rows { get; set; }
    public PaginatedData(IEnumerable<T> items, int total)
    {
      this.rows = items;
      this.total = total;
    }
    public static async Task<PaginatedData<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
    {
      var count = await source.CountAsync();
      var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
      return new PaginatedData<T>(items, count);
    }
  }
}
