using System.ComponentModel.DataAnnotations;

namespace LittleFish.Core.Dtos
{
    public class EmployeePutDto
    {
        [Required]
        public string LastName { get; set; }
    }
}
