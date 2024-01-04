using System;
using System.Collections.Generic;
using FisCompendium.Data.Game_Data;

namespace FisCompendium.Web.Utilities.GameData.Knowledge
{
    public interface IQMNoteRepository
    {
        List<QMNoteIndex> GetIndex(bool includeHidden);
        QMNote GetById(Guid id);
        Guid GetItemId(string title);
        Guid Create(QMNote model);
        bool Edit(QMNote model);
    }
}
