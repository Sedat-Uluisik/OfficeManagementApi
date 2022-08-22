using Dapper;
using OfficeManagementApi.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Threading.Tasks;

namespace OfficeManagementApi.Repository
{
    public class RepositoryOffice
    {
        private string conString;

        public RepositoryOffice(String constr)
        {

            //conString = "Server=localhost;Database=OfficeManagementDB;User Id=test;Password=Test!!;";   //IP'de yazabilirsin
            conString = constr;   //IP'de yazabilirsin
            //conString = "Server=localhost;Database=OfficeManagementDB;Trusted_Connection=True;"; //local erişim
        }

        private IDbConnection Connection
        {
            get
            {
                return new SqlConnection(conString);
            }
        }

        //Departman ile ilgili fonksiyonlar           

        public async Task<IEnumerable<Department>> GetAllDepartments()
        {
            using(IDbConnection db = Connection){
                if (db.State == ConnectionState.Closed)
                    db.Open();

                var results = await db.QueryAsync<Department>(
                    "get_all_departments",
                    commandType: CommandType.StoredProcedure
                    );
                db.Close();

                return results.AsList();
            }
        }

        public async Task<IEnumerable<DepartmentAccess>> GetAllDepartmentAccess()
        {
            using (IDbConnection db = Connection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                var results = await db.QueryAsync<DepartmentAccess>(
                    "get_all_department_accesses",
                    commandType: CommandType.StoredProcedure
                    );

                db.Close();

                return results.AsList<DepartmentAccess>();
            }
        }

        public async Task<Department> CheckDepartment(string departmentName)
        {
            var parameter = new DynamicParameters();
            parameter.Add("Name", departmentName, DbType.String);


            using (IDbConnection db = Connection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                var department = await db.QueryFirstOrDefaultAsync<Department>(
                    "check_department",
                    parameter,
                    commandType: CommandType.StoredProcedure);

                db.Close();

                return department;
            }
        }

        public async Task InsertDepartment(Department department)
        {
            var parameters = new DynamicParameters();
            parameters.Add("Name", department.Name, DbType.String);

            using (IDbConnection db = Connection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                await db.ExecuteAsync(
                    "insert_department",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                db.Close();
            }
        }

        public async Task DeleteDepartmentWithId(int departmentId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("departmentId", departmentId, DbType.Int32);

            using(IDbConnection db = Connection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                await db.ExecuteAsync(
                    "delete_department_with_id",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                db.Close();
            }
        }

        public async Task UpdateDepartment(Department department)
        {
            var parameters = new DynamicParameters();
            parameters.Add("departmentId", department.Id);
            parameters.Add("departmentName", department.Name);

            using(IDbConnection db = Connection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                await db.ExecuteAsync(
                    "update_department",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                db.Close();
            }
        }

        //İşler ile ilgili fonksiyonlar

        public async Task<IEnumerable<Work>> GetAllWorks()
        {
            using (IDbConnection db = Connection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                var results = await db.QueryAsync<Work>(
                    "get_all_works",
                    commandType: CommandType.StoredProcedure);
                db.Close();

                return results.AsList();
            }
        }

        public async Task<Work> GetWorkWithId(int workId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("workId", workId);
            using (IDbConnection db = Connection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                var results = await db.QueryFirstOrDefaultAsync<Work>(
                    "get_work_with_id",
                    parameters,
                    commandType: CommandType.StoredProcedure);
                db.Close();

                return results;
            }
        }

        public async Task InsertWork(Work work)
        {
            var parameters = new DynamicParameters();
            parameters.Add("WorkName", work.WorkName, DbType.String);

            using (IDbConnection db = Connection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                await db.ExecuteAsync(
                    "insert_work",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                db.Close();
            }
        }

        public async Task DeleteWorkWithId(int workId)
        {
            var parameter = new DynamicParameters();
            parameter.Add("workId", workId);

            using (IDbConnection db = Connection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                await db.ExecuteAsync(
                    "delete_work",
                    parameter,
                    commandType: CommandType.StoredProcedure);

                db.Close();               
            }
        }

        public async Task UpdateWork(Work work)
        {
            var parameters = new DynamicParameters();
            parameters.Add("workId", work.Id);
            parameters.Add("workName", work.WorkName);

            using (IDbConnection db = Connection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                await db.ExecuteAsync(
                    "update_work",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                db.Close();
            }
        }

        public async Task<Work> CheckWorkIsInserted(string workName)
        {
            var parameter = new DynamicParameters();
            parameter.Add("WorkName", workName, DbType.String);

            using (IDbConnection db = Connection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                var result = await db.QueryFirstOrDefaultAsync<Work>(
                    "get_work_with_work_name",
                    parameter,
                    commandType: CommandType.StoredProcedure);

                db.Close();

                return result;
            }
        }

        //kullanıcı ile ilgili fonksiyolar

        public async Task<IEnumerable<Types>> GetUserTypes()
        {
            using(IDbConnection db = Connection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                var results = await db.QueryAsync<Types>(
                    "get_user_types",
                    commandType: CommandType.StoredProcedure);
                db.Close();
                return results.AsList();
            }
        }


        public async Task DeleteUser(string username, string password)
        {
            var parameter = new DynamicParameters();
            parameter.Add("userName", username);
            parameter.Add("password", password);

            using (IDbConnection db = Connection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                await db.ExecuteAsync(
                    "delete_user",
                    parameter,
                    commandType: CommandType.StoredProcedure);

                db.Close();
            }
        }

        public async Task UpdateUser(User user)
        {
            var parameters = new DynamicParameters();
            parameters.Add("id", user.Id);
            parameters.Add("userName", user.UserName);
            parameters.Add("password", user.Password);
            parameters.Add("departmentId", user.DepartmentId);
            parameters.Add("statusId", user.StatusId);

            using (IDbConnection db = Connection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                await db.ExecuteAsync(
                    "update_user",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                db.Close();
            }
        }


        public async Task<IEnumerable<User>> GetAllUsers()
        {
            using (IDbConnection db = Connection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                var results = await db.QueryAsync<User>(
                   "get_all_users",
                   commandType: CommandType.StoredProcedure);
                db.Close();
                return results.AsList();
            }
        }

        public async Task InsertUser(User user)
        {
            var parameters = new DynamicParameters();
            parameters.Add("UserName", user.UserName);
            parameters.Add("Password", user.Password);
            parameters.Add("DepartmentId", user.DepartmentId);
            parameters.Add("StatusId", user.StatusId);

            using(IDbConnection db = Connection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                await db.ExecuteAsync(
                    "insert_user",
                    parameters,
                    commandType: CommandType.StoredProcedure);
                db.Close();

            }
        }

        public async Task<User> Get_User_With_UserName_And_Password(string userName, string password)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("UserName", userName); 
            parameters.Add("Password", password);                

            using(IDbConnection db = Connection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();


              var result = await db.QueryFirstOrDefaultAsync<User>(
                    "get_User_With_UserName_And_Password",
                    parameters,
                    commandType: CommandType.StoredProcedure);
                db.Close();
                return result;

              
            }
        }

        //Departman ve işin birleştirildiği fonksiyonlar

        /*public async Task<IEnumerable<WorksAndStatus>> getWorkStatusWithDepartmentName(String departmentName)
        {
            var parameters = new DynamicParameters();
            parameters.Add("DepartmentName", departmentName);

            using(IDbConnection db = Connection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                var results = await db.QueryAsync<WorksAndStatus>(
                    "get_work_status_with_department_name_for_department",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                db.Close();

                return results.AsList();
            }
        }*/

        public async Task<DepartmentAndWork> CheckDepartmentAndWork(int departmentId, int workId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("IdDepartment", departmentId);
            parameters.Add("IdWork", workId);

            using (IDbConnection db = Connection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                var result = await db.QueryFirstOrDefaultAsync<DepartmentAndWork>(
                    "check_department_and_work",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                db.Close();

                return result;
            }
        }

        public async Task InsertWorkForDepartment(int departmentId, int workId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("IdDepartment", departmentId);
            parameters.Add("IdWork", workId);
            parameters.Add("Status", 1);
            parameters.Add("CreatedDate", "--");//DateTime.Now.ToString("G", DateTimeFormatInfo.InvariantInfo));
            parameters.Add("UpdatedDate", "--");
            using (IDbConnection db = Connection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                await db.ExecuteAsync(
                    "insert_department_and_work",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                db.Close();
            }
        }

        //departments and accesses

        /*public async Task InsertDepartmentAccess(List<DepartmentAccess> departmentList)
        {
            using (IDbConnection db= Connection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                foreach(var item in departmentList)
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("DepartmentId", item.DepartmentId);
                    parameters.Add("DepartmentAccessId", item.DepartmentAccessId);

                    await db.ExecuteAsync(
                        "insert_department_access",
                        parameters,
                        commandType: CommandType.StoredProcedure);
                }

                db.Close();
            }
        }*/

        public async Task InsertDepartmentAccess(int departmentId ,List<AccessId> accessList)
        {
            using (IDbConnection db = Connection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                foreach (var item in accessList)
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("DepartmentId", departmentId);
                    parameters.Add("DepartmentAccessId", item.DepartmentAccessId);

                    await db.ExecuteAsync(
                        "insert_department_access",
                        parameters,
                        commandType: CommandType.StoredProcedure);
                }

                db.Close();
            }
        }

        public async Task<IEnumerable<WorksAndStatus>> GetAllWorkStatus()
        {
            using (IDbConnection db = Connection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                var list = await db.QueryAsync<WorksAndStatus>(
                    "get_all_work_status",
                    commandType: CommandType.StoredProcedure);

                db.Close();

                return list.AsList();
            }
        }

        public async Task<IEnumerable<WorksAndStatus>> GetWorksStatusForDepartmentAndAccesses(int userDepartmentId)
        {
            var parameter = new DynamicParameters();
            parameter.Add("userDepartmentId", userDepartmentId);

            using(IDbConnection db = Connection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                var list = await db.QueryAsync<WorksAndStatus>(
                    "get_works_for_user_department",
                    parameter,
                    commandType: CommandType.StoredProcedure);

                db.Close();

                return list.AsList();
            }
        }

        public async Task<IEnumerable<WorksAndStatus>> DeleteDepartmentAndWork(int departmentId, int workId)
        {
            var parameter = new DynamicParameters();
            parameter.Add("IdDepartment", departmentId);
            parameter.Add("IdWork", workId);
            using(IDbConnection db= Connection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                await db.ExecuteAsync(
                    "delete_department_and_work",
                    parameter,
                    commandType: CommandType.StoredProcedure);

                var newList = await db.QueryAsync<WorksAndStatus>(
                    "get_all_work_status",
                    commandType: CommandType.StoredProcedure);

                db.Close();

                return newList.AsList();

            }
        }

        public async Task ChangeWorkStatus(int departmentId, int workId, int workStatus)
        {
            var parameter = new DynamicParameters();
            parameter.Add("IdDepartment", departmentId);
            parameter.Add("IdWork", workId);
            parameter.Add("WorkStatus", workStatus);
            parameter.Add("CreatedDate", DateTime.Now.ToString("G", DateTimeFormatInfo.InvariantInfo));
            using (IDbConnection db = Connection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                await db.ExecuteAsync(
                    "change_work_status",
                    parameter,
                    commandType: CommandType.StoredProcedure);

                db.Close();           
            }
        }

        public async Task<IEnumerable<WorksAndStatus>> ChangeWorkStatusToDone(int departmentId, int workId, int workStatus)
        {
            var parameter = new DynamicParameters();
            parameter.Add("IdDepartment", departmentId);
            parameter.Add("IdWork", workId);
            parameter.Add("WorkStatus", workStatus);
            parameter.Add("UpdatedDate", DateTime.Now.ToString("G", DateTimeFormatInfo.InvariantInfo));
            using (IDbConnection db = Connection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                await db.ExecuteAsync(
                    "change_work_status_to_done",
                    parameter,
                    commandType: CommandType.StoredProcedure);

                var newList = await db.QueryAsync<WorksAndStatus>(
                    "get_all_work_status",
                    commandType: CommandType.StoredProcedure);

                db.Close();

                return newList;
            }
        }

        public async Task<WorksAndStatus> CheckWorkStatusIsDone()
        {
            Console.WriteLine("fun çalıştı");
            using (IDbConnection db = Connection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                var work = await db.QueryFirstOrDefaultAsync<WorksAndStatus>(
                    "check_work_status_is_done",
                    commandType: CommandType.StoredProcedure);

                db.Close();

               return work;
            }
        }

        /*
         ALTER procedure [dbo].[get_works_for_user_department](
@userDepartmentId int)
as
begin
	select departmentAndWork.IdDepartment, departmentAndWork.Status, departmentAndWork.CreatedDate, departmentAndWork.UpdatedDate, department.Name as DepartmentName, work.WorkName
	from Department_And_Work as departmentAndWork
	left join Department_Access as departmentAccess
	on departmentAndWork.IdDepartment = departmentAccess.DepartmentAccessId
	left join Departments as department
	on departmentAccess.DepartmentAccessId = department.Id
	left join Works as work
	on departmentAndWork.IdWork = work.Id

	where departmentAccess.DepartmentId = @userDepartmentId
end
         */



    }
}
