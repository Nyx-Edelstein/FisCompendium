using System;
using FisCompendium.Repository;

namespace FisCompendium.Data.User_Data
{
    public class PlayerLogEntry : DataItem
    {
        public string Timestamp { get; set; }
        public string Message { get; set; }

        public PlayerLogEntry()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
