using spark2.Models.Entities;
using spark2.Models.DTOs;

namespace spark2.Repos
{
    public interface IPortofolioRepo
    {


        Task<Portofolio> CreatePortofolioBasicAsync(Portofolio portofolio); 
        Task<List<Portofolio>> GetAllPortofoliosAsync();
        Task<Portofolio?> GetPortofolioByIdAsync(int id);
        Task<Portofolio?> UpdatePortofolioBasicAsync(int portofolioId, Portofolio portofolio);
        Task<bool> DeletePortofolioAsync(int portofolioId);
        Task<bool> SetPublishStatusForPortofolioAsync(int portofolioId, bool publish);




        Task AddServiceToPortofolioAsync(int portofolioId, Service service);
        Task UpdateServiceAsync(Service updatedServiceInfo);
        Task<Service> GetServiceByIdAsync(int serviceId);
        Task DeleteServiceAsync(int serviceId);



        Task AddProjectToPortofolioAsync(int serviceId, Project project);
        Task UpdateProjectAsync(Project updatedProjectInfo);
        Task<Project> GetProjectByIdAsync(int projectId);
        Task DeleteProjectAsync(int projectId);

    }
}
