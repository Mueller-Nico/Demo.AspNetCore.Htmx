using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Demo.AspNetCore.Htmx.Models
{
    public partial class Model
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ModelId { get; set; }
        public int CategoryId { get; set; }
        public int ManufacturerId { get; set; }
        [Required]
        [StringLength(50)]
        public string ModelName { get; set; }

        // Localization is not part of this project
        // Changed the type to avoid validation errors due to the decimal separator
        //[Column(TypeName = "decimal(10, 1)")]
        //public decimal? Power_KW { get; set; }
        [RegularExpression(@"^\d{1,3}(?:[,.]\d{1})?$",
            ErrorMessage = "Numbers from 0 to 999 with optional one decimal place and a point or comma as the decimal separator are accepted.")] 
        public string Power_KW { get; set; }
        public int? Capacity { get; set; }
        [DisplayName("Category")]
        [ForeignKey("CategoryId")]
        [InverseProperty("Models")]
        public virtual Category Category { get; set; }
        [DisplayName("Manufacturer")]
        [ForeignKey("ManufacturerId")]
        [InverseProperty("Models")]
        public virtual Manufacturer Manufacturer { get; set; }
    }
}
