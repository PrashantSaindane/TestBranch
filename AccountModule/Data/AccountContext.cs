using AccountModule.Model;
using Microsoft.EntityFrameworkCore;

namespace AccountModule.Data
{
    public class AccountContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public AccountContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(Configuration.GetConnectionString("BankServiceDB"));
        }
        public DbSet<AccountTypeMaster> AccountTypeMaster { get; set; }
        public DbSet<AccountDetails> AccountDetails { get; set; }
        public DbSet<TransactionMode> TransactionMode { get; set; }
        public DbSet<TransactionType> TransactionType { get; set; }
        public DbSet<Transaction> Transactions{ get; set; }
        public DbSet<Users> Users { get; set; }
    }
}
