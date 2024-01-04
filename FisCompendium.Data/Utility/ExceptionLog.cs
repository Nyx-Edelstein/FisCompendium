using System;
using FisCompendium.Repository;

namespace FisCompendium.Data.Utility
{
    public class ExceptionLog : DataItem
    {
        public string Message { get; set; }
        public string ExceptionType { get; set; }

        public ExceptionLog()
        {
            Id = Guid.NewGuid();
        }
    }
}
