namespace OfficeManagementApi.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int DepartmentId { get; set; }
        public int StatusId { get; set; }

    }
}
