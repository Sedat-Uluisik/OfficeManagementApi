using System;

namespace OfficeManagementApi.Models
{
    public class DepartmentAndWork
    {
        public int IdDepartment { get; set; }
        public int IdWork { get; set; }
        public int Status { get; set; }
        public String CreatedDate { get; set; }
        public String UpdatedDate { get; set; }
    }
}
