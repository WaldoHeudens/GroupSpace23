using System.ComponentModel.DataAnnotations;

namespace GroupSpace23.Models
{
    public class Group
    {
        public int Id { get; set; }

        [Display (Name="Naam")]
        public string Name { get; set; }

        [Display(Name = "Omschrijving")]
        public string Description { get; set; }

        [Display(Name = "Groep aangemaakt")]
        [DataType(DataType.Date)]
        public DateTime Started { get; set; } = DateTime.Now;

        [Display(Name = "Groep gestopt")]
        [DataType(DataType.Date)]
        public DateTime Ended { get; set; } = DateTime.MaxValue;
    }
}
