using spark2.Models.Entities;
using Microsoft.EntityFrameworkCore;
using spark2.Data;


namespace spark2.Repos
{
    public class PortofolioRepo : IPortofolioRepo
    {
        private readonly ApplicationDbContext _dbContext;

        public PortofolioRepo(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        #region Basic Portofolio

        public async Task<Portofolio> CreatePortofolioBasicAsync(Portofolio portofolio)
        {
            var newPortofolio = new Portofolio
            {
                FullName = portofolio.FullName,
                Email = portofolio.Email,

                PersonalImage = portofolio.PersonalImage,
                Bio = portofolio.Bio,
                JobTitle = portofolio.JobTitle,
                Address = portofolio.Address,
                PortofolioCreatedDate = DateOnly.FromDateTime(DateTime.UtcNow),
                PortofolioModifiedDate = DateOnly.FromDateTime(DateTime.UtcNow),
                IsDeleted = false,
                IsPublished = false,
                PersonId = portofolio.PersonId,
            };

            _dbContext.Portofolios.Add(newPortofolio);
            await _dbContext.SaveChangesAsync();
            return newPortofolio;
        }


        public async Task<Portofolio?> GetPortofolioByIdAsync(int id)
        {
            return await _dbContext.Portofolios
                .Include(p => p.Services)
                .Include(p => p.Projects)
                .FirstOrDefaultAsync(p => p.PortofolioId == id && !p.IsDeleted);
        }



        public async Task<List<Portofolio>> GetAllPortofoliosAsync()
        {
            return await _dbContext.Portofolios
                .Include(p => p.Services)
                .Include(p => p.Projects)
                .Where(p => !p.IsDeleted)
                .ToListAsync();
        }

        public async Task<Portofolio> UpdatePortofolioBasicAsync(int portofolioId,Portofolio portofolio)
        {
            var existingPortofolio = await _dbContext.Portofolios.FindAsync(portofolioId);
            if (existingPortofolio == null) return null;

            existingPortofolio.FullName = portofolio.FullName;
            existingPortofolio.Email = portofolio.Email;
            existingPortofolio.PersonalImage = portofolio.PersonalImage;
            existingPortofolio.Bio = portofolio.Bio;
            existingPortofolio.JobTitle = portofolio.JobTitle;
            existingPortofolio.Address = portofolio.Address;
            existingPortofolio.PortofolioModifiedDate = DateOnly.FromDateTime(DateTime.UtcNow);
            existingPortofolio.IsPublished = portofolio.IsPublished;

            await _dbContext.SaveChangesAsync();

            return existingPortofolio;
        }



        public async Task<bool> DeletePortofolioAsync(int portofolioId)
        {
            var portofolio = await _dbContext.Portofolios.FindAsync(portofolioId);
            if (portofolio == null)
                return false;

            portofolio.IsDeleted = true;
            portofolio.PortofolioModifiedDate = DateOnly.FromDateTime(DateTime.UtcNow);

            await _dbContext.SaveChangesAsync();
            return true;
        }


        public async Task<bool> SetPublishStatusForPortofolioAsync(int portofolioId, bool publish)
        {
            var portofolio = await _dbContext.Portofolios.FindAsync(portofolioId);
            if (portofolio == null)
                return false;

            portofolio.IsPublished = publish;
            portofolio.PortofolioModifiedDate = DateOnly.FromDateTime(DateTime.UtcNow);

            await _dbContext.SaveChangesAsync();
            return true;
        }

        #endregion




        #region Service CRUD

        public async Task AddServiceToPortofolioAsync(int portofolioId, Service service)
        {
            service.PortofolioId = portofolioId;

            _dbContext.Services.Add(service);
            await _dbContext.SaveChangesAsync();
        }

       
        public async Task<Service> GetServiceByIdAsync(int serviceId)
        {
            return await _dbContext.Services.FirstOrDefaultAsync(s => s.ServiceId == serviceId);
        }

        public async Task UpdateServiceAsync(Service updatedServiceInfo)
        {
            var existingService = await _dbContext.Services.FindAsync(updatedServiceInfo.ServiceId);
            if (existingService == null) return;

            existingService.ServiceName = updatedServiceInfo.ServiceName;
            existingService.ServiceDescription = updatedServiceInfo.ServiceDescription;
            existingService.ServiceImage = updatedServiceInfo.ServiceImage;

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteServiceAsync(int serviceId)
        {
            var service = await _dbContext.Services.FindAsync(serviceId);
            if (service != null)
            {
                _dbContext.Services.Remove(service);
                await _dbContext.SaveChangesAsync();
            }
        }

        #endregion



        #region Project CRUD


        public async Task AddProjectToPortofolioAsync(int portofolioId, Project project)
        {
            project.PortofolioId = portofolioId;

            _dbContext.Projects.Add(project);
            await _dbContext.SaveChangesAsync();
        }



        public async Task<Project> GetProjectByIdAsync(int projectId)
        {
            return await _dbContext.Projects.FirstOrDefaultAsync(p => p.ProjectId == projectId);
        }

   
        public async Task UpdateProjectAsync(Project updatedProjectInfo)
        {
            var existingProject = await _dbContext.Projects.FindAsync(updatedProjectInfo.ProjectId);
            if (existingProject == null) return;

            existingProject.ProjectName = updatedProjectInfo.ProjectName;
            existingProject.ProjectDescription = updatedProjectInfo.ProjectDescription;
            existingProject.ProjectLink = updatedProjectInfo.ProjectLink;
            existingProject.ProjectAttachment = updatedProjectInfo.ProjectAttachment;
            existingProject.ProjectImage = updatedProjectInfo.ProjectImage;

            await _dbContext.SaveChangesAsync();
        }

     

        public async Task DeleteProjectAsync(int projectId)
        {
            var project = await _dbContext.Projects.FindAsync(projectId);
            if (project != null)
            {
                _dbContext.Projects.Remove(project);
                await _dbContext.SaveChangesAsync();
            }
        }

        #endregion





    }
}

