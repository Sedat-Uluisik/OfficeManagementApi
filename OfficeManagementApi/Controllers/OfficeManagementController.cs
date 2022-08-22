using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OfficeManagementApi.Models;
using OfficeManagementApi.Repository;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace OfficeManagementApi.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/")]
    [ApiController]
    public class OfficeManagementController : ControllerBase
    {
        private readonly RepositoryOffice repository;

        public OfficeManagementController(IConfiguration configuration)
        {
            repository = new RepositoryOffice(configuration.GetConnectionString("DefaultConnection"));
        }

        [HttpPost]
        [Route("hangfire")]
        public IActionResult Hangfire()
        {
            var jobId = BackgroundJob.Schedule(() =>
                repository.GetAllDepartments(),
                TimeSpan.FromSeconds(10)
            );

            return Ok();
        }


        [HttpGet]
        [Route("getAllDepartments")]
        public async Task<ActionResult<IEnumerable<Department>>> GetAllDepartments()
        {
            try
            {
                var results = await repository.GetAllDepartments();

                if (results == null)
                    return NotFound();

                return Ok(results);

            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("insertDepartment")]
        public async Task<ActionResult> InsertDepartment(Department department)
        {
            try
            {
                var currentDepartment = await repository.CheckDepartment(department.Name);

                if (currentDepartment != null)
                {
                    return Conflict();
                }
                else
                {
                    await repository.InsertDepartment(department);
                    var check = await repository.CheckDepartment(department.Name);
                    if (check != null)
                        return Ok(check);
                    else
                        return NotFound();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("deleteDepartment")]
        public async Task<ActionResult> DeleteDepartmentWithId(Department department)
        {
            try
            {
                var currentDepartment = await repository.CheckDepartment(department.Name);

                if (currentDepartment != null)
                {
                    await repository.DeleteDepartmentWithId(department.Id);
                    var isDelete = await repository.CheckDepartment(department.Name);

                    if (isDelete != null)
                        return Conflict();
                    else
                        return Ok(department);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("updateDepartment")]
        public async Task<ActionResult> UpdateDepartment(Department department)
        {
            try
            {
                await repository.UpdateDepartment(department);
                var list = await repository.GetAllDepartments();
                if (list != null)
                    return Ok(list);
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("getAllDepartmentAccess")]
        public async Task<ActionResult<IEnumerable<DepartmentAccess>>> GetAllDepartmentAccess()
        {
            try
            {
                var results = await repository.GetAllDepartmentAccess();

                if (results == null)
                    return NotFound();

                return Ok(results);

            } catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("allWorks")]
        public async Task<ActionResult<IEnumerable<Work>>> GetAllWorks()
        {
            try
            {
                var results = await repository.GetAllWorks();

                if (results == null)
                    return NotFound();

                return Ok(results);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Route("insertWork")]
        public async Task<ActionResult> InsertWork(Work work)
        {
            try
            {
                var currentWork = await repository.CheckWorkIsInserted(work.WorkName);

                if (currentWork != null)
                {
                    return Conflict(); //iş zaten mevcut
                }
                else
                {
                    await repository.InsertWork(work);
                    var newWork = await repository.CheckWorkIsInserted(work.WorkName);

                    if (newWork != null)
                    {
                        return Ok(newWork);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("deleteWork")]
        public async Task<ActionResult> DeleteWorkWithId(Work work)
        {
            try
            {
                var currentWork = await repository.CheckWorkIsInserted(work.WorkName);

                if (currentWork != null)
                {
                    await repository.DeleteWorkWithId(work.Id);
                    var isDelete = await repository.CheckWorkIsInserted(work.WorkName);

                    if (isDelete != null)
                    {
                        return Conflict();
                    }
                    else
                    {
                        return Ok(work);
                    }
                }
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("updateWork")]
        public async Task<ActionResult> UpdateWork(Work work)
        {
            try
            {
                await repository.UpdateWork(work);
                var newList = await repository.GetAllWorks();
                if (newList != null)
                {
                    return Ok(newList);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /*[HttpGet]
        [Route("getWorkStatusWithDepartmentName/{departmentName}")]
        public async Task<ActionResult<IEnumerable<WorksAndStatus>>> GetWorkStatusWithDepartmentName(String departmentName)
        {
            try
            {
                var results = await repository.getWorkStatusWithDepartmentName(departmentName);

                if (results == null)
                    return NotFound();

                return Ok(results);
            }
            catch(Exception ex)
            {
                return StatusCode(500);
            }
        }*/

        [HttpGet]
        [Route("UserTypes")]
        public async Task<ActionResult<IEnumerable<Types>>> GetUserTypes()
        {
            try
            {
                var results = await repository.GetUserTypes();

                if (results == null)
                    return NotFound();
                return Ok(results);
            }
            catch (Exception error)
            {
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Route("allUsers")]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            try
            {
                var results = await repository.GetAllUsers();

                if (results == null)
                    return NotFound();
                return Ok(results);

            }
            catch (Exception error)
            {
                return StatusCode(500, error.Message);
            }

        }

        [HttpPost]
        [Route("insertUser")]

        public async Task<ActionResult> InsertUser(User user)
        {
            try
            {
                var currentUser = await repository.Get_User_With_UserName_And_Password(user.UserName, user.Password);

                if (currentUser != null)
                {
                    return Conflict();
                }
                else
                {
                    await repository.InsertUser(user);
                    var newUser = await repository.Get_User_With_UserName_And_Password(user.UserName, user.Password);
                    if (newUser != null)
                        return Ok(newUser);
                    else
                        return NotFound();
                }
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        [HttpGet]
        [Route("getUserWithUserNameAndPassword/{userName}/{password}")]
        public async Task<ActionResult<User>> Get_User_With_UserName_And_Password(string userName, string password)
        {

            try
            {
                var result = await repository.Get_User_With_UserName_And_Password(userName, password);
                if (result == null)
                    return NotFound();
                else
                    return Ok(result);
            }
            catch (Exception error)
            {
                return StatusCode(500, error.Message);
            }
        }

        [HttpDelete]
        [Route("deleteUser/{username}/{password}")]
        public async Task<ActionResult> DeleteUser(string username, string password)
        {
            try
            {
                var currentUser = await repository.Get_User_With_UserName_And_Password(username, password);

                if (currentUser != null)
                {
                    await repository.DeleteUser(username, password);
                    var userList = await repository.GetAllUsers();
                    return Ok(userList);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("updateUser")]
        public async Task<ActionResult> UpdateUser(User user)
        {
            try
            {
                await repository.UpdateUser(user);
                var userList = await repository.GetAllUsers();
                return Ok(userList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("insertWorkForDepartment/{departmentId}")]
        public async Task<ActionResult> InsertWorkForDepartment(int departmentId, List<WorkId> workIdList)
        {
            try
            {
                foreach (var workId in workIdList)
                {
                    var currentData = await repository.CheckDepartmentAndWork(departmentId, workId.IdWork);

                    if (currentData == null)
                    {
                        await repository.InsertWorkForDepartment(departmentId, workId.IdWork);
                    }
                }
                var workStatusList = await repository.GetAllWorkStatus();
                return Ok(workStatusList);
            }
            catch
            {
                return BadRequest();
            }
        }

        /*[HttpPost]
        [Route("insertDepartmentAccessList")]
        public async Task<ActionResult> InsertList(List<DepartmentAccess> list)
        {
            try
            {
                await repository.InsertDepartmentAccess(list);
                return StatusCode(200);
            }catch(Exception ex)
            {
                return StatusCode(500);
            }
        }*/

        [HttpPost]
        [Route("insertDepartmentAccessList/{departmentId}")]
        public async Task<ActionResult> InsertDepartmentAccessList(int departmentId, List<AccessId> accessList)
        {
            try
            {
                await repository.InsertDepartmentAccess(departmentId, accessList);
                return Ok(accessList);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("getAllWorkStatus")]
        public async Task<ActionResult<IEnumerable<WorksAndStatus>>> GetAllWorkStatus()
        {
            try
            {
                var list = await repository.GetAllWorkStatus();

                if (list == null)
                    return NotFound();

                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("getWorkStatusForDepartment/{departmentId}")]
        public async Task<ActionResult<IEnumerable<WorksAndStatus>>> GetWorkStatusForDepartment(int departmentId) {
            try
            {
                var list = await repository.GetWorksStatusForDepartmentAndAccesses(departmentId);

                if (list == null)
                    return NotFound();

                return Ok(list);                                  
            }
            catch (Exception Ex)
            {
                return BadRequest(Ex.Message);
            }
        }

        [HttpDelete]
        [Route("deleteDepartmentAndWork/{departmentId}/{workId}")]
        public async Task<ActionResult<IEnumerable<WorksAndStatus>>> DeleteDepartmentAndWork(int departmentId, int workId)
        {
            try
            {
                var newList = await repository.DeleteDepartmentAndWork(departmentId, workId);
                if(newList == null)
                {
                    var refresh = await repository.GetAllWorkStatus();
                    if (refresh == null)
                        return NotFound();
                    else
                        return Ok(refresh);
                }else
                    return Ok(newList);

            }catch(Exception Ex)
            {
                return BadRequest(Ex.Message);
            }
        }

        [HttpPut]
        [Route("changeWorkStatus/{departmentId}/{workId}/{workStatus}/{userStatus}")]
        public async Task<ActionResult<IEnumerable<WorksAndStatus>>> ChangeWorkStatus(int departmentId, int workId, int workStatus, int userStatus)
        {
            try
            {
                await repository.ChangeWorkStatus(departmentId, workId, workStatus);

                if (userStatus != 1)
                {
                    var list = await repository.GetAllWorkStatus();
                    if (list == null)
                        return NotFound();
                    else
                        return Ok(list);
                }
                else
                {
                    var list = await repository.GetWorksStatusForDepartmentAndAccesses(departmentId);

                    if (list == null)
                        return NotFound();
                    else 
                        return Ok(list);
                }
            }
            catch (Exception Ex)
            {
                return BadRequest(Ex.Message);
            }
        }

        [HttpPut]
        [Route("changeWorkStatusToDone/{departmentId}/{workId}/{workStatus}")]
        public async Task<ActionResult<IEnumerable<WorksAndStatus>>> ChangeWorkStatusToDone(int departmentId, int workId, int workStatus)
        {
            try
            {
                var newList = await repository.ChangeWorkStatusToDone(departmentId, workId, workStatus);

                if (newList == null)
                    return NotFound();
                else
                {
                    SendNotificationForDone(departmentId);
                    return Ok(newList);
                }
            }
            catch(Exception Ex)
            {
                return BadRequest(Ex.Message);
            }
        }

        private async void SendNotificationForDone(int departmentId)
        {
            using (var client = new HttpClient())
            {
                var url = new Uri("https://onesignal.com/api/v1/notifications");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", "MmUxNWY5MDMtYzc1Mi00NmJlLWExYmEtMDA0NWM2MjkxNTdj");
                var obj = new
                {
                    app_id = "da2ae273-1399-42b7-96e4-03567d24795f",
                    headings = new { en = "Düzeltme" },
                    contents = new { en = "  Raporlanan iş düzeltildi." },
                    included_segments = new string[] { "Kullanıcı" },
                    isAndroid = "true",
                    filters = new object[]
                    {
                        new
                        {
                            field = "tag",
                            key = "department",
                            value = departmentId.ToString()
                        }
                    }

                };
                var json = JsonConvert.SerializeObject(obj);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, content);

            }
        }
    }
}

//https://stackoverflow.com/questions/47701244/send-post-request-from-webapi-c-sharp-to-onesignal-url
