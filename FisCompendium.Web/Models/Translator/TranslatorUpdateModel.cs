using System.ComponentModel.DataAnnotations;

namespace FisCompendium.Web.Models.Translator
{
    public class TranslatorUpdateModel
    {
        [Required] [MinLength(1)]
        public string SelectedLanguage { get; set; }

        [Required]
        public int Limit { get; set; }

        [Required] [MinLength(1)]
        public string InputText { get; set; }
    }
}
