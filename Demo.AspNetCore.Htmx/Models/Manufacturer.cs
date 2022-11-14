using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Demo.AspNetCore.Htmx.Models
{
    public partial class Manufacturer
    {
        public Manufacturer()
        {
            Models = new HashSet<Model>();
        }

        [Key]
        public int ManufacturerId { get; set; }
        [Required]
        [StringLength(50)]
        [DisplayName("Manufacturer Name")]
        public string ManufacturerName { get; set; }

        [InverseProperty("Manufacturer")]
        public virtual ICollection<Model> Models { get; set; }
    }
}
