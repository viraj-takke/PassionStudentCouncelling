using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Students_Councelling.Interface;
using Students_Councelling.Models.DAL;
using Students_Councelling.Models.DTO;
using Students_Councelling.Models.Viewmodels;
using Microsoft.EntityFrameworkCore;

namespace Students_Councelling.Models.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AccountRepository> _logger;
        public AccountRepository(ApplicationDbContext context, ILogger<AccountRepository> logger)
        {
            _context = context;
            _logger = logger;
        }   
        public async Task<Students> LoginAsync(string email, string password)
        {
            var student = await _context.students
                .Where(s => s.Email == email && s.Password == password)
                .FirstOrDefaultAsync();

            return student;
        }

        public string GetStudentDetailsByMailId(string mailid)
        {
            var fullname = _context.students
                            .Where(student => student.Email == mailid)
                            .Select(student => student.FirstName + " " + student.LastName)
                            .FirstOrDefault();
            return fullname;
        }

        public long GetStudentIdByMailId(string mailid) 
        {
            var UserId = _context.students
                          .Where(student => student.Email == mailid)
                          .Select(student => student.StudentId)
                          .FirstOrDefault();
            return UserId;
        }
        public async Task<Students> GetStudentByMailId(string mailid)
        {
            var student = _context.students
                          .Where(student => student.Email == mailid)
                          .FirstOrDefault();
            return student;
        }

        public async Task<bool> Registration_StudentsAsync(RegisterStudentDto model)
        {
            if(model.FirstName == null)
            {
                return false;
            }
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var student = new Students
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Password = model.Password,
                    Phone = model.Phone,
                    InstituteName = model.InstituteName,
                    Standard = model.Standard,
                    IsActive = true,
                    CreatedBy = null,
                    CreatedOn = DateTime.Now,
                    ModifiedBy = null,
                    ModifiedOn = null
                };

                _context.students.Add(student);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                _logger.LogInformation("Student registration successful for email: {Email}", model.Email);
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error occurred while registering student with email: {Email}", model.Email);
                return false;
            }
        }

        public async Task<bool> EditProfileAsync(Students model)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Retrieve the student from the database
                var student = await _context.students.FindAsync(model.StudentId);

                if (student == null)
                {
                    _logger.LogWarning("Attempted to update profile for a student that doesn't exist. Student ID: {StudentId}", model.StudentId);
                    return false; // Student not found
                }
                long userid = GetStudentIdByMailId(model.Email);
                
                student.FirstName = model.FirstName;
                student.LastName = model.LastName;
                student.Email = model.Email;
                student.Phone = model.Phone;
                student.InstituteName = model.InstituteName;
                student.Standard = model.Standard;
                student.ModifiedBy = userid.ToString();
                student.ModifiedOn = DateTime.Now;

                // Save the changes
                _context.students.Update(student);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                _logger.LogInformation("Student profile updated successfully for Student ID: {StudentId}", model.StudentId);
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error occurred while updating profile for Student ID: {StudentId}", model.StudentId);
                return false;
            }
        }

    }
}
