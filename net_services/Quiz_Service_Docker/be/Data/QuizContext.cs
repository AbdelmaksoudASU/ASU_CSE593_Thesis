using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using System.Diagnostics;
using Quiz_Service.Models;

namespace Quiz_Service.Data
{
    public class QuizContext : DbContext
    {
        public QuizContext(DbContextOptions<QuizContext> options) : base(options)
        {
        }
        public DbSet<Program_Matching_Criteria> QuizDB { get; set; }
    }
}


