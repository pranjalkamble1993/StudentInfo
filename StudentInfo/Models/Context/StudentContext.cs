using Microsoft.EntityFrameworkCore;
using StudentInfo.Models.Entities;

namespace StudentInfo.Models.Context
{
	public class StudentContext : DbContext
	{
		public StudentContext(DbContextOptions<StudentContext> options) : base(options)
		{
		}

		public DbSet<SchoolClass> SchoolClass { get; set; }
		public DbSet<ParentStudent> ParentStudent { get; set; }
		public DbSet<StudentClass> StudentClass { get; set; }

		public DbSet<Users> Users { get; set; }	
	}
}
