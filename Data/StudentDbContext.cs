using Microsoft.EntityFrameworkCore;
using StudentManagementService.Models;
using System.Collections.Generic;

namespace StudentManagementService.Data
{
    public class StudentDbContext: DbContext
    {
        public StudentDbContext(DbContextOptions<StudentDbContext> options) : base(options)
        {
        }

        public DbSet<Student> tbl_Students { get; set; }
        public DbSet<Teacher> tbl_Teachers { get; set; }
    }
}
