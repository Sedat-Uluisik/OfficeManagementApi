using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OfficeManagementApi.Repository;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace OfficeManagementApi.Service
{
    public class MyService : IMyService
    {
        private readonly RepositoryOffice repo;
        public MyService(IConfiguration configuration)
        {
            repo = new RepositoryOffice(configuration.GetConnectionString("DefaultConnection"));
        }
        public async Task MyFun()
        {
            var work = await repo.CheckWorkStatusIsDone();

            if(work != null && work.Status == 0)
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
                        headings = new { en = work.DepartmentName },
                        contents = new { en = work.WorkName + "  - raporlandı." },
                        included_segments = new string[] { "Hizmetli" },
                        isAndroid = "true",

                    };
                    var json = JsonConvert.SerializeObject(obj);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(url, content);

                }
            }

            
        }
    }
}
