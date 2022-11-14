using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Demo.AspNetCore.Htmx.Models
{
    public partial class Category
    {
        public Category()
        {
            Models = new HashSet<Model>();
        }

        [Key]
        public int CategoryId { get; set; }
        [Required]
        [StringLength(50)]
        [DisplayName("Category Name")]
        public string CategoryName { get; set; }

        [InverseProperty("Category")]
        public virtual ICollection<Model> Models { get; set; }
    }
}
