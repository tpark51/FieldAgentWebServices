using System;
using System.ComponentModel.DataAnnotations;

namespace FieldAgent.Web.Models
{
    public class MissionModel
    {
        public int MissionId { get; set; }

        [Required(ErrorMessage = "Agency ID is required")]
        public int AgencyId { get; set; }
        
        
        [Required(ErrorMessage = "Codename is required")]
        [StringLength(50, ErrorMessage = "Codename cannot exceed 50 characters")]
        public string CodeName { get; set; }
        public string Notes { get; set; }
        
        
        [Required(ErrorMessage = "Start date is required")]
        public DateTime? StartDate { get; set; }
        
        [Required(ErrorMessage = "Projected end date is required")]
        public DateTime? ProjectedEndDate { get; set; }
        
        public DateTime? ActualEndDate { get; set; }
        public decimal? OperationalCost { get; set; }
    }
}
