using System;
using System.ComponentModel.DataAnnotations;

namespace FieldAgent.Web.Models
{
    public class AliasModel
    {
        public int AliasId { get; set; }

       
        [Required(ErrorMessage = "Agent ID is required")]
        public int? AgentId { get; set; }

        
        [Required(ErrorMessage = "Alias name is required")]
        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters")]
        public string AliasName { get; set; }

        
        public Guid? InterpolId { get; set; }
        public string Persona { get; set; }
    }
}
