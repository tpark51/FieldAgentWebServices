using System;
using System.ComponentModel.DataAnnotations;

namespace FieldAgent.Web.Models
{
    public class AgentModel
    {
        public int AgentId { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters")]
        public string FirstName { get; set; }
        
        
        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters")]
        public string LastName { get; set; }


        [Required(ErrorMessage = "Date of birth is required")] 
        public DateTime? DateOfBirth { get; set; }


        [Required(ErrorMessage = "Height is required")]
        public decimal? Height { get; set; }
    }
}
