namespace FisCompendium.Web.Utilities.SystemData
{
    public interface ISystemLockRepository
    {
        bool GetIsSystemLocked();
        void LockSystem();
    }
}
