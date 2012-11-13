using System.Data.Entity.ModelConfiguration;
using Highway.Data.GettingStarted.Domain.Entities;

namespace Highway.Data.GettingStarted.DataAccess.Mappings
{
    public class AccountMap : EntityTypeConfiguration<Account>
    {
        public AccountMap()
        {
            this.ToTable("Accounts");
            this.HasKey(x => x.AccountId);
            this.Property(x => x.AccountName).HasColumnType("text");
        }
    }
}