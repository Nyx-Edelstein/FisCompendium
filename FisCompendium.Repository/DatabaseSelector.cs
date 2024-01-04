using LiteDB;

namespace FisCompendium.Repository
{
    public static class DatabaseSelector
    {
        public static LiteDatabase PlayerData { get; }
        public static LiteDatabase GameData { get; }
        public static LiteDatabase QMGameData { get; }
        public static LiteDatabase System { get; }

        static DatabaseSelector()
        {
            PlayerData = new LiteDatabase(@"..\PlayerData.ldb");
            GameData = new LiteDatabase(@"..\GameData.ldb");
            QMGameData = new LiteDatabase(@"..\QMGameData.ldb");
            System = new LiteDatabase(@"..\System.ldb");
        }
    }
}
