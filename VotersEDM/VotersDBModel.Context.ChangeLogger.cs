namespace VotersEDM
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Threading.Tasks;

    using ChangeLoggers;

    public partial class VotersDBEntities : DbContext
    {

        private ChangeLogger changeLogger = new ChangeLogger();

        public override int SaveChanges()
        {
            ChangeLogs = changeLogger.SaveChangesAsync(this, ChangeLogs);
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync()
        {
            ChangeLogs = changeLogger.SaveChangesAsync(this, ChangeLogs);
            return await base.SaveChangesAsync();
        }
    }
}
