using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;

// presently, this reference would need
// to change for each different namespace
using VotersEDM;

namespace ChangeLoggers
{
    public class ChangeLogger
    {

        /*
         * There are a couple of significant drawbacks to this particular SaveChanges override:
         * 
         * 1. No primary key value for ADDED entities in the ChangeLog. This is because, in this system,
         * the database is responsible for creating the primary key values (via IDENTITY columns)
         * and therefore the primary keys do not exist before the entity is added to the database.
         * Attempting to use the database-generated primary keys for Added entities would result
         * in two round-trips to the database on every save.
         *   
         * 2. Support for single-column primary keys only. This code makes an explicit assumption
         * a single primary key column per table in your database, which is not true in the real world.
         * 
         */
        public DbSet<ChangeLog> SaveChanges(
            DbContext caller,
            DbSet<ChangeLog> changeLogs)
        {
            var insertedEntities = caller.ChangeTracker.Entries()
                .Where(p => p.State == EntityState.Added).ToList();
            var modifiedEntities = caller.ChangeTracker.Entries()
                .Where(p => p.State == EntityState.Modified).ToList();
            var deletedEntities = caller.ChangeTracker.Entries()
                .Where(p => p.State == EntityState.Deleted).ToList();
            var now = DateTime.UtcNow;

            foreach (var insert in insertedEntities)
            {
                var entityName = insert.Entity.GetType().Name;
                var primaryKey = GetPrimaryKeyValue(caller, insert);
                var primaryKeyName = GetPrimaryKeyName(caller, insert);
                primaryKey = insert.CurrentValues[primaryKeyName];

                foreach (var prop in insert.CurrentValues.PropertyNames)
                {
                    var currentValue = insert.CurrentValues[prop] is null ? DBNull.Value.ToString() : insert.CurrentValues[prop].ToString();

                    ChangeLog log = new ChangeLog()
                    {
                        EntityName = entityName,
                        PrimaryKeyValue = primaryKey.ToString(),
                        PropertyName = prop,
                        OldValue = DBNull.Value.ToString(),
                        NewValue = currentValue.ToString(),
                        DateChanged = now,
                        Action = "Inserted"
                    };
                    changeLogs.Add(log);
                }
            }

            foreach (var change in modifiedEntities)
            {
                var entityName = change.Entity.GetType().Name;
                var primaryKey = GetPrimaryKeyValue(caller, change);

                foreach (var prop in change.OriginalValues.PropertyNames)
                {
                    var originalValue = change.OriginalValues[prop] is null ? DBNull.Value.ToString() : change.OriginalValues[prop].ToString();
                    var currentValue = change.CurrentValues[prop] is null ? DBNull.Value.ToString() : change.CurrentValues[prop].ToString();

                    if (originalValue != currentValue)
                    {
                        ChangeLog log = new ChangeLog()
                        {
                            EntityName = entityName,
                            PrimaryKeyValue = primaryKey.ToString(),
                            PropertyName = prop,
                            OldValue = originalValue.ToString(),
                            NewValue = currentValue.ToString(),
                            DateChanged = now,
                            Action = "Changed"
                        };
                        changeLogs.Add(log);
                    }
                }
            }

            foreach (var delete in deletedEntities)
            {
                var entityName = delete.Entity.GetType().Name;
                var primaryKey = GetPrimaryKeyValue(caller, delete);

                foreach (var prop in delete.OriginalValues.PropertyNames)
                {
                    var originalValue = delete.OriginalValues[prop] is null ? DBNull.Value.ToString() : delete.OriginalValues[prop].ToString();

                    ChangeLog log = new ChangeLog()
                    {
                        EntityName = entityName,
                        PrimaryKeyValue = primaryKey.ToString(),
                        PropertyName = prop,
                        OldValue = originalValue.ToString(),
                        NewValue = DBNull.Value.ToString(),
                        DateChanged = now,
                        Action = "Deleted"
                    };
                    changeLogs.Add(log);
                }
            }

            return changeLogs;
        }

        /*
         * There are a couple of significant drawbacks to this particular SaveChanges override:
         * 
         * 1. No primary key value for ADDED entities in the ChangeLog. This is because, in this system,
         * the database is responsible for creating the primary key values (via IDENTITY columns)
         * and therefore the primary keys do not exist before the entity is added to the database.
         * Attempting to use the database-generated primary keys for Added entities would result
         * in two round-trips to the database on every save.
         *   
         * 2. Support for single-column primary keys only. This code makes an explicit assumption
         * a single primary key column per table in your database, which is not true in the real world.
         * 
         */
        public DbSet<ChangeLog> SaveChangesAsync(
            DbContext caller,
            DbSet<ChangeLog> changeLogs)
        {
            var insertedEntities = caller.ChangeTracker.Entries()
                .Where(p => p.State == EntityState.Added).ToList();
            var modifiedEntities = caller.ChangeTracker.Entries()
                .Where(p => p.State == EntityState.Modified).ToList();
            var deletedEntities = caller.ChangeTracker.Entries()
                .Where(p => p.State == EntityState.Deleted).ToList();
            var now = DateTime.UtcNow;


            foreach (var insert in insertedEntities)
            {
                var entityName = insert.Entity.GetType().Name;
                var primaryKeyName = GetPrimaryKeyName(caller, insert);
                insert.CurrentValues[primaryKeyName] = Guid.NewGuid();
                var primaryKey = GetPrimaryKeyValue(caller, insert);
                primaryKey = insert.CurrentValues[primaryKeyName];

                foreach (var prop in insert.CurrentValues.PropertyNames)
                {
                    var currentValue = insert.CurrentValues[prop] is null ? DBNull.Value.ToString() : insert.CurrentValues[prop].ToString();

                    ChangeLog log = new ChangeLog()
                    {
                        EntityName = entityName,
                        PrimaryKeyValue = primaryKey.ToString(),
                        PropertyName = prop,
                        OldValue = DBNull.Value.ToString(),
                        NewValue = currentValue.ToString(),
                        DateChanged = now,
                        Action = "Inserted"
                    };
                    changeLogs.Add(log);
                }
            }

            foreach (var change in modifiedEntities)
            {
                var entityName = change.Entity.GetType().Name;
                var primaryKey = GetPrimaryKeyValue(caller, change);

                foreach (var prop in change.OriginalValues.PropertyNames)
                {
                    var originalValue = change.OriginalValues[prop] is null ? DBNull.Value.ToString() : change.OriginalValues[prop].ToString();
                    var currentValue = change.CurrentValues[prop] is null ? DBNull.Value.ToString() : change.CurrentValues[prop].ToString();

                    if (originalValue != currentValue)
                    {
                        ChangeLog log = new ChangeLog()
                        {
                            EntityName = entityName,
                            PrimaryKeyValue = primaryKey.ToString(),
                            PropertyName = prop,
                            OldValue = originalValue.ToString(),
                            NewValue = currentValue.ToString(),
                            DateChanged = now,
                            Action = "Changed"
                        };
                        changeLogs.Add(log);
                    }
                }
            }

            foreach (var delete in deletedEntities)
            {
                var entityName = delete.Entity.GetType().Name;
                var primaryKey = GetPrimaryKeyValue(caller, delete);

                foreach (var prop in delete.OriginalValues.PropertyNames)
                {
                    var originalValue = delete.OriginalValues[prop] is null ? DBNull.Value.ToString() : delete.OriginalValues[prop].ToString();

                    ChangeLog log = new ChangeLog()
                    {
                        EntityName = entityName,
                        PrimaryKeyValue = primaryKey.ToString(),
                        PropertyName = prop,
                        OldValue = originalValue.ToString(),
                        NewValue = DBNull.Value.ToString(),
                        DateChanged = now,
                        Action = "Deleted"
                    };
                    changeLogs.Add(log);
                }
            }

            return changeLogs;
        }

        private String GetPrimaryKeyName(
            DbContext caller,
            DbEntityEntry entry,
            String defaultPrimaryKeyName = "ID")
        {

            var objectStateEntry = ((IObjectContextAdapter)caller).ObjectContext.ObjectStateManager.GetObjectStateEntry(entry.Entity);
            var primaryKeyName = String.Empty;
            if (objectStateEntry.EntityKey.EntityKeyValues != null && objectStateEntry.EntityKey.EntityKeyValues.Length > 0)
            {
                primaryKeyName = objectStateEntry.EntityKey.EntityKeyValues[0].Key;
            }
            else
            {
                primaryKeyName = defaultPrimaryKeyName;
            }

            return primaryKeyName;
        }

        private object GetPrimaryKeyValue(
            DbContext caller,
            DbEntityEntry entry)
        {
            var objectStateEntry = ((IObjectContextAdapter)caller).ObjectContext.ObjectStateManager.GetObjectStateEntry(entry.Entity);
            var primaryKeyValue = new Object();
            if (objectStateEntry.EntityKey.EntityKeyValues != null && objectStateEntry.EntityKey.EntityKeyValues.Length > 0)
            {
                primaryKeyValue = objectStateEntry.EntityKey.EntityKeyValues[0].Value;
            }
            else
            {
                primaryKeyValue = DBNull.Value;
            }
            return primaryKeyValue;
        }
    }
}