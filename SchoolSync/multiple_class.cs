using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace SchoolSync
{
    class multiple_class
    {
        public async Task<dynamic> PostRequestAsync(string url, Dictionary<string, string> data)
        {
            schoolsync.show_loading();
            var client = new HttpClient();
            var content = new FormUrlEncodedContent(data);
            var response = await client.PostAsync(url, content);
            var responseString = await response.Content.ReadAsStringAsync();
            dynamic json = JsonConvert.DeserializeObject(responseString);
            schoolsync.hide_loading();
            return json;
        }
    }
}
