using System.ComponentModel.DataAnnotations;

namespace LittleFish.NetCoreBoilerplate.Core.Dtos
{
    public class EmployeePutDto
    {
        [Required]
        public string LastName { get; set; }
    }
}
