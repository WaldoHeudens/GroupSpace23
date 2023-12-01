using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GroupSpace23.Models
{
    public class Parameter
    {
        [Key]
        public string Name { get; set; }
        public string Value { get; set; }

        [ForeignKey ("GroupSpace23User")]
        public string UserId { get; set; }
        public DateTime LastChanged { get; set; }

    }
}
