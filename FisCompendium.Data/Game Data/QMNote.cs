using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using FisCompendium.Repository;

namespace FisCompendium.Data.Game_Data
{
    [HasGuidKey("QMNoteId", isUnique: true)]
    [HasStringKey("Title", isUnique: true)]
    public class QMNote : DataItem
    {
        public Guid QMNoteId { get; set; }

        [Required]
        [MinLength(3)]
        [RegularExpression("^[a-zA-Z 0-9_\\-\\(\\)\\']*$", ErrorMessage = "Title can contain only alphanumeric characters and \" -_()'\".")]
        public string Title { get; set; }
        
        [Required]
        [MinLength(3)]
        [RegularExpression("^[a-zA-Z 0-9_\\-\\(\\)\\',]*$", ErrorMessage = "Tags can contain only alphanumeric characters and \" -_()'\".")]
        public string TagsRaw { get; set; }
        public List<string> Tags => (TagsRaw ?? string.Empty).Split(',').ToList();

        [Required]
        [MinLength(1)]
        public string Notes { get; set; }

        public bool Archived { get; set; }

        public string LastEdited { get; set; }
    }

    [HasGuidKey("QMNoteId", isUnique: false)]
    [HasStringKey("Title", isUnique: false)]
    public class QMNoteLog : QMNote
    {
        public string DateReplaced { get; set; }
    }

    public class QMNoteIndex
    {
        public Guid QMNoteId { get; set; }
        public string Title { get; set; }
        public string TagsRaw { get; set; }
        public List<string> Tags => (TagsRaw ?? string.Empty).Split(',').ToList();
        public bool Archived { get; set; }
    }
}
