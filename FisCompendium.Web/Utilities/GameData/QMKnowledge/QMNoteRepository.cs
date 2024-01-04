using System;
using System.Collections.Generic;
using System.Linq;
using FisCompendium.Data.Game_Data;
using FisCompendium.Repository;
using Ganss.XSS;

namespace FisCompendium.Web.Utilities.GameData.Knowledge
{
    public class QMNotesRepository : IQMNoteRepository
    {
        public IRepository<QMNote> QMNoteRepository { get; }
        public IRepository<QMNoteLog> LogRepository { get; }

        public QMNotesRepository(IRepository<QMNote> qmNoteRepository, IRepository<QMNoteLog> logRepository)
        {
            QMNoteRepository = qmNoteRepository;
            LogRepository = logRepository;
        }

        public List<QMNoteIndex> GetIndex(bool includeHidden) => QMNoteRepository.GetWhere(x => !x.Archived || includeHidden)
            .Select(x => new QMNoteIndex
            {
                QMNoteId = x.QMNoteId,
                Title = x.Title,
                TagsRaw = x.TagsRaw,
                Archived = x.Archived,
            }).OrderBy(x => x.Title).ToList();

        public QMNote GetById(Guid id)
        {
            var item = QMNoteRepository.GetWhere(x => x.QMNoteId == id).FirstOrDefault();
            return item;
        }

        private static bool Matches(string a, string b)
        {
            return string.Equals(a, b, StringComparison.InvariantCultureIgnoreCase);
        }

        public Guid GetItemId(string title)
        {
            return QMNoteRepository
                .GetWhere(x => Matches(x.Title, title))
                .FirstOrDefault()?.QMNoteId ?? Guid.Empty;
        }

        public Guid Create(QMNote model)
        {
            //Check for existing items with the same title first
            var existing = QMNoteRepository
                .GetWhere(x => Matches(x.Title, model.Title))
                .FirstOrDefault();
            if (existing != null) return Guid.Empty;

            model.Notes = Sanitize(model.Notes);
            model.QMNoteId = Guid.NewGuid();
            model.LastEdited = $"{DateTime.UtcNow} (UTC)";

            return QMNoteRepository.Upsert(model) ? model.QMNoteId : Guid.Empty;
        }

        public bool Edit(QMNote model)
        {
            var existing = QMNoteRepository.GetWhere(x => x.QMNoteId == model.QMNoteId).FirstOrDefault();
            if (existing == null) return false;

            LogOldItem(existing);

            existing.Title = model.Title;
            existing.TagsRaw = model.TagsRaw;
            existing.Notes = Sanitize(model.Notes);
            existing.Archived = model.Archived;
            existing.LastEdited = $"{DateTime.UtcNow} (UTC)";

            return QMNoteRepository.Upsert(existing);
        }

        private void LogOldItem(QMNote existing)
        {
            var log = new QMNoteLog
            {
                QMNoteId = existing.QMNoteId,
                Title = existing.Title,
                TagsRaw = existing.TagsRaw,
                Notes = existing.Notes,
                Archived = existing.Archived,
                LastEdited = existing.LastEdited,
                DateReplaced = DateTime.UtcNow.ToString()
            };

            LogRepository.Upsert(log);
        }

        private string Sanitize(string notes)
        {
            var sanitizer = new HtmlSanitizer();
            return sanitizer.Sanitize(notes);
        }
    }
}
