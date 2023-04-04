using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Lab6.Models
{
    public class Student
    {
        [SwaggerSchema(ReadOnly = true)]
        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "First Name")]
        public String FirstName { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public String LastName { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Program")]
        public String Program { get; set; }
    }
}
