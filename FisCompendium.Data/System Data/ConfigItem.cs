using FisCompendium.Repository;

namespace FisCompendium.Data.System_Data
{
    [HasStringKey("Key", isUnique: true)]
    public class ConfigItem : DataItem
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}