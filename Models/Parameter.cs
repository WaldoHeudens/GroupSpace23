using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GroupSpace23.Models
{
    public class Parameter
    {
        [Key]
        public string Name { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        [ForeignKey ("GroupSpace23User")]
        public string UserId { get; set; }
        public DateTime LastChanged { get; set; } = DateTime.Now;
        public DateTime Obsolete { get; set; } = DateTime.MaxValue;
        public string Destination { get; set; }
    }
}
