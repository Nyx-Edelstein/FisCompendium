using System.Linq;
using FisCompendium.Data.Utility;
using FisCompendium.Repository;

namespace FisCompendium.Web.Utilities.SystemData
{
    public class SystemLockRepository : ISystemLockRepository
    {
        public IRepository<SystemLock> Repository { get; }

        public SystemLockRepository(IRepository<SystemLock> repository)
        {
            Repository = repository;
        }

        public bool GetIsSystemLocked()
        {
            return Repository.GetWhere(x => true)?.FirstOrDefault()?.IsLocked == true;
        }

        public void LockSystem()
        {
            var systemLock = Repository.GetWhere(x => true).FirstOrDefault() ?? new SystemLock();
            systemLock.IsLocked = true;
            Repository.Upsert(systemLock);
        }
    }
}
