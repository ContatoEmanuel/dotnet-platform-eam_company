namespace EAM.Web.Portal.Services;

using EAM.Core.Domain.Entities;
using EAM.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

public interface IProductService
{
    Task<List<Project>> GetProductsForSaleAsync();
    Task<Project?> GetProductByIdAsync(int id);
}

public class ProductService : IProductService
{
    private readonly ApplicationDbContext _context;

    public ProductService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Project>> GetProductsForSaleAsync()
    {
        return await _context.Projects
            .Where(p => p.IsActive && p.ForSale)
            .OrderBy(p => p.DisplayOrder)
            .ToListAsync();
    }

    public async Task<Project?> GetProductByIdAsync(int id)
    {
        return await _context.Projects
            .FirstOrDefaultAsync(p => p.Id == id && p.IsActive && p.ForSale);
    }
}
