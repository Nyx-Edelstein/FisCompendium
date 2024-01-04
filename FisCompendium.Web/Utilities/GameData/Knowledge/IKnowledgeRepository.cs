using System;
using System.Collections.Generic;
using FisCompendium.Data.Game_Data;

namespace FisCompendium.Web.Utilities.GameData.Knowledge
{
    public interface IKnowledgeRepository
    {
        List<KnowledgeItemIndex> GetIndex(bool includeHidden);
        KnowledgeItemComposite GetById(Guid id);
        Guid GetItemId(string title);
        Guid Create(KnowledgeItem model);
        bool Edit(KnowledgeItem model);
        bool SubmitComment(KnowledgeItemComment comment);
    }
}
