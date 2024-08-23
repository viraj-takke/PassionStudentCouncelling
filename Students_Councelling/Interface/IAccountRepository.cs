using Microsoft.AspNetCore.Mvc;
using Students_Councelling.Models.Viewmodels;

namespace Students_Councelling.Interface
{
    public interface IAccountRepository
    {
        Task<Students> LoginAsync(string email, string password);
        Task<bool> Registration_StudentsAsync(RegisterStudentDto model);
        string GetStudentDetailsByMailId(string mailid);
        long GetStudentIdByMailId(string mailid);
        Task<Students> GetStudentByMailId(string mailid);
        Task<bool> EditProfileAsync(Students model);
    }
}
