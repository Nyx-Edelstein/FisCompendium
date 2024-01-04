using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using FisCompendium.Repository;

namespace FisCompendium.Data.Game_Data
{
    [HasGuidKey("KnowledgeItemId", isUnique: true)]
    [HasStringKey("Title", isUnique: false)]
    public class KnowledgeItem : DataItem
    {
        public Guid KnowledgeItemId { get; set; }

        [Required]
        [MinLength(2)]
        [RegularExpression("^[a-zA-Z 0-9_\\-\\(\\)\\']*$", ErrorMessage = "Title can contain only alphanumeric characters and \" -_()'\".")]
        public string Title { get; set; }
        
        [Required]
        [MinLength(3)]
        [RegularExpression("^[a-zA-Z 0-9_\\-\\(\\)\\',]*$", ErrorMessage = "Tags can contain only alphanumeric characters and \" -_()'\".")]
        public string TagsRaw { get; set; }
        public List<string> Tags => (TagsRaw ?? string.Empty).Split(',').ToList();

        [Required]
        [MinLength(1)]
        public string Data { get; set; }

        public bool Archived { get; set; }

        public string LastEdited { get; set; }
        public string Editor { get; set; }
    }

    [HasGuidKey("KnowledgeItemId", isUnique: false)]
    [HasStringKey("Title", isUnique: false)]
    public class KnowledgeItemLog : KnowledgeItem
    {
        public string DateReplaced { get; set; }
    }

    [HasGuidKey("KnowledgeItemId", isUnique: false)]
    [HasStringKey("Username", isUnique: false)]
    public class KnowledgeItemComment : DataItem
    {
        public Guid KnowledgeItemId { get; set; }

        public string Username { get; set; }

        [Required]
        [MinLength(5)]
        public string Comment { get; set; }

        public string DateCommented { get; set; }
    }

    public class KnowledgeItemIndex
    {
        public Guid KnowledgeItemId { get; set; }
        public string Title { get; set; }
        public string TagsRaw { get; set; }
        public List<string> Tags => (TagsRaw ?? string.Empty).Split(',').ToList();
        public bool Archived { get; set; }
    }

    public class KnowledgeItemComposite
    {
        public KnowledgeItem Item { get; set; }
        public List<KnowledgeItemComment> Comments { get; set; }
    }
}
