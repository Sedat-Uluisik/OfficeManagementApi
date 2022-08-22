using System;

namespace OfficeManagementApi.Models
{
    public class WorksAndStatus
    {
        public int IdDepartment { get; set; }
        public int IdWork { get; set; }
        public int Status { get; set; }
        public String CreatedDate { get; set; }
        public String UpdatedDate { get; set; }
        public String DepartmentName { get; set; }
        public String WorkName { get; set; }
    }
}
