using System;
using System.Collections.Generic;
using System.Linq;
using FisCompendium.Data.Game_Data;
using FisCompendium.Repository;
using Ganss.XSS;

namespace FisCompendium.Web.Utilities.GameData.Knowledge
{
    public class KnowledgeRepository : IKnowledgeRepository
    {
        public IRepository<KnowledgeItem> KnowledgeItemRepository { get; }
        public IRepository<KnowledgeItemComment> CommentsRepository { get;  }
        public IRepository<KnowledgeItemLog> KnowledgeItemLogRepository { get; }

        public KnowledgeRepository(IRepository<KnowledgeItem> knowledgeItemRepository, IRepository<KnowledgeItemComment> commentsRepository, IRepository<KnowledgeItemLog> knowledgeItemLogRepository)
        {
            KnowledgeItemRepository = knowledgeItemRepository;
            CommentsRepository = commentsRepository;
            KnowledgeItemLogRepository = knowledgeItemLogRepository;
        }

        public List<KnowledgeItemIndex> GetIndex(bool includeHidden) => KnowledgeItemRepository.GetWhere(x => !x.Archived || includeHidden)
            .Select(x => new KnowledgeItemIndex
            {
                KnowledgeItemId = x.KnowledgeItemId,
                Title = x.Title,
                TagsRaw = x.TagsRaw,
                Archived = x.Archived,
            }).OrderBy(x => x.Title).ToList();

        public KnowledgeItemComposite GetById(Guid id)
        {
            var item = KnowledgeItemRepository.GetWhere(x => x.KnowledgeItemId == id).FirstOrDefault();
            if (item == null) return null;

            var comments = CommentsRepository.GetWhere(x => x.KnowledgeItemId == item.KnowledgeItemId)
                .OrderByDescending(x => DateTime.Parse(x.DateCommented.Replace(" (UTC)", "")))
                .ToList();

            var composite = new KnowledgeItemComposite
            {
                Item = item,
                Comments = comments
            };

            return composite;
        }

        private static bool Matches(string a, string b)
        {
            return string.Equals(a, b, StringComparison.InvariantCultureIgnoreCase);
        }

        public Guid GetItemId(string title)
        {
            return KnowledgeItemRepository
                .GetWhere(x => Matches(x.Title, title))
                .FirstOrDefault()?.KnowledgeItemId ?? Guid.Empty;
        }

        public Guid Create(KnowledgeItem model)
        {
            //Check for existing items with the same title first
            var existing = KnowledgeItemRepository
                .GetWhere(x => Matches(x.Title, model.Title))
                .FirstOrDefault();
            if (existing != null) return Guid.Empty;

            model.Data = Sanitize(model.Data);
            model.KnowledgeItemId = Guid.NewGuid();
            model.LastEdited = $"{DateTime.UtcNow} (UTC)";

            return KnowledgeItemRepository.Upsert(model) ? model.KnowledgeItemId : Guid.Empty;
        }

        public bool Edit(KnowledgeItem model)
        {
            var existing = KnowledgeItemRepository.GetWhere(x => x.KnowledgeItemId == model.KnowledgeItemId).FirstOrDefault();
            if (existing == null) return false;

            LogOldItem(existing);

            existing.Title = model.Title;
            existing.TagsRaw = model.TagsRaw;
            existing.Data = Sanitize(model.Data);
            existing.Archived = model.Archived;
            existing.LastEdited = $"{DateTime.UtcNow} (UTC)";
            existing.Editor = model.Editor;

            return KnowledgeItemRepository.Upsert(existing);
        }

        private void LogOldItem(KnowledgeItem existing)
        {
            var log = new KnowledgeItemLog
            {
                KnowledgeItemId = existing.KnowledgeItemId,
                Title = existing.Title,
                TagsRaw = existing.TagsRaw,
                Data = existing.Data,
                Archived = existing.Archived,
                LastEdited = existing.LastEdited,
                DateReplaced = DateTime.UtcNow.ToString()
            };

            KnowledgeItemLogRepository.Upsert(log);
        }

        public bool SubmitComment(KnowledgeItemComment comment)
        {
            var knowledgeItemExists = KnowledgeItemRepository.GetWhere(x => x.KnowledgeItemId == comment.KnowledgeItemId).Any();
            if (!knowledgeItemExists) return false;

            comment.Comment = Sanitize(comment.Comment);
            comment.DateCommented = $"{DateTime.UtcNow} (UTC)";

            return CommentsRepository.Upsert(comment);
        }

        private string Sanitize(string notes)
        {
            var sanitizer = new HtmlSanitizer();
            return sanitizer.Sanitize(notes);
        }
    }
}
