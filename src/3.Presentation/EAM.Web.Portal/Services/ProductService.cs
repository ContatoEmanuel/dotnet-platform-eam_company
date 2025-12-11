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
        var project = await _projectApiService.GetProjectByIdAsync(id);
        if (project != null && project.IsActive && project.ForSale)
        {
            return project;
        }
        return null;
    }
}
