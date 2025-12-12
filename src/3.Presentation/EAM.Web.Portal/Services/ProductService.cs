using EAM.Core.Application.DTOs.Project;

namespace EAM.Web.Portal.Services;

public interface IProductService
{
    Task<List<ProjectDto>> GetProductsForSaleAsync();
    Task<ProjectDto?> GetProductByIdAsync(int id);
}

public class ProductService : IProductService
{
    private readonly IProjectApiService _projectApiService;

    public ProductService(IProjectApiService projectApiService)
    {
        _projectApiService = projectApiService;
    }

    public async Task<List<ProjectDto>> GetProductsForSaleAsync()
    {
        var allProjects = await _projectApiService.GetAllProjectsAsync();
        return allProjects
            .Where(p => p.IsActive && p.ForSale)
            .OrderBy(p => p.DisplayOrder)
            .ToList();
    }

    public async Task<ProjectDto?> GetProductByIdAsync(int id)
    {
        return await _projectApiService.GetProjectByIdAsync(id);
    }
}
