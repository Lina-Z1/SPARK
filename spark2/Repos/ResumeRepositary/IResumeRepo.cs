using spark2.Models.Entities;

namespace spark2.Repos.ResumeRepositary
{
    public interface IResumeRepo
    {
        
        Task<Resume> CreateResumeBasicAsync(Resume resume);
        Task<List<Resume>> GetAllResumesAsync();
        Task<Resume?> GetResumeByIdAsync(int id);
        Task<Resume?> UpdateResumeBasicAsync(Resume resume);
        Task<bool> DeleteResumeAsync(int id);
        Task<bool> SetPublishStatusForResumeAsync(int resumeId, bool publish);

       
        Task<Education> AddEducationAsync(int resumeId, Education education);
        Task<Education?> UpdateEducationAsync(Education education);
        Task<Education?> GetEducationByIdAsync(int educationId);
        Task<bool> DeleteEducationAsync(int educationId);

        
        Task<Language> AddLanguageAsync(int resumeId, Language language);
        Task<Language?> UpdateLanguageAsync(Language language);
        Task<Language?> GetLanguageByIdAsync(int languageId);
        Task<bool> DeleteLanguageAsync(int languageId);

        
        Task<Experience> AddExperienceAsync(int resumeId, Experience experience);
        Task<Experience?> UpdateExperienceAsync(Experience experience);
        Task<Experience?> GetExperienceByIdAsync(int experienceId);
        Task<bool> DeleteExperienceAsync(int experienceId);

     
        Task<Skill> AddSkillAsync(int resumeId, Skill skill);
        Task<Skill?> UpdateSkillAsync(Skill skill);
        Task<Skill?> GetSkillByIdAsync(int skillId);
        Task<bool> DeleteSkillAsync(int skillId);

     
        Task<Certificate> AddCertificateAsync(int resumeId, Certificate certificate);
        Task<Certificate?> UpdateCertificateAsync(Certificate certificate);
        Task<Certificate?> GetCertificateByIdAsync(int certificateId);
        Task<bool> DeleteCertificateAsync(int certificateId);
    }
}

































//using spark2.Models.DTOs;
//using spark2.Models.Entities;

//namespace spark2.Repos.ResumeRepositary
//{
//    public interface IResumeRepo
//    {
//        Task<Resume> CreateResumeBasicAsync(Resume resume);
//        Task<List<Resume>> GetAllResumesAsync();
//        Task<Resume?> GetResumeByIdAsync(int id);
//        Task<Resume> UpdateResumeBasicAsync(Resume resume);
//        Task<bool> DeleteResumeAsync(int id);
//        Task<bool> SetPublishStatusForResumeAsync(int resumeId, bool publish);



//        Task<Education> AddEducationAsync(int resumeId,Education education);      
//        Task<Education> UpdateEducationAsync(Education education);
//        Task<Education> GetEducationByIdAsync(int educationId);
//        Task<bool> DeleteEducationAsync(int educationId);



//        Task<Language> AddLanguageAsync(int resumeId, Language language);
//        Task<Language> UpdateLanguageAsync(Language language);
//        Task<Language?> GetLanguageByIdAsync(int languageId);
//        Task<bool> DeleteLanguageAsync(int languageId);


//        Task<Experience> AddExperienceAsync(int resumeId, Experience experience);
//        Task<Experience?> UpdateExperienceAsync(Experience experience);
//        Task<Experience?> GetExperienceByIdAsync(int experienceId);
//        Task<bool> DeleteExperienceAsync(int experienceId);



//        Task<Skill> AddSkillAsync(int resumeId, Skill skill);
//        Task<Skill?> UpdateSkillAsync(Skill skill);
//        Task<Skill?> GetSkillByIdAsync(int skillId);
//        Task<bool> DeleteSkillAsync(int skillId);




//        Task<Certificate> AddCertificateAsync(int resumeId, Certificate certificate);
//        Task<Certificate?> UpdateCertificateAsync(Certificate certificate);
//        Task<Certificate?> GetCertificateByIdAsync(int certificateId);
//        Task<bool> DeleteCertificateAsync(int certificateId);

//    }
//}
