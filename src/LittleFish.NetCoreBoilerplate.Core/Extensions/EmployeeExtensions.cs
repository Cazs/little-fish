using LittleFish.NetCoreBoilerplate.Core.Dtos;
using LittleFish.NetCoreBoilerplate.Core.Models;

namespace LittleFish.NetCoreBoilerplate.Core.Extensions
{
    internal static class EmployeeExtensions
    {
        public static EmployeeDto MapToDto(this Employee source)
        {
            return new EmployeeDto
            {
                Id = source.EmpNo,
                FirstName = source.FirstName,
                LastName = source.LastName,
                BirthDate = source.BirthDate,
                Gender = source.Gender,
            };
        }
    }
}
