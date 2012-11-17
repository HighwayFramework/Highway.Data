using System.Data.Entity.ModelConfiguration;

namespace Highway.Data.GettingStarted
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