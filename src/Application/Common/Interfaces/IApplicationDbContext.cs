using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Entities.Audit;
using CleanArchitecture.Razor.Domain.Entities.Log;

using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Razor.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Serilog> Serilogs { get; set; }
        DbSet<AuditTrail> AuditTrails { get; set; }
        DbSet<Customer> Customers { get; set; }
        DbSet<Product> Products { get; set; }
        //DbSet<DocumentType> DocumentTypes { get; set; }
        //DbSet<Document> Documents { get; set; }
        DbSet<KeyValue> KeyValues { get; set; }
        //DbSet<ApprovalData> ApprovalDatas { get; set; }
        DbSet<Category> Categories { get; set; }
        DbSet<Direction> Directions { get; set; }
        DbSet<Contragent> Contragents { get; set; }
        DbSet<ContragentCategory> ContragentCategories { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
