﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using FluentFTP;
using System.Drawing;
using System.Net.Http.Headers;

namespace SchoolSync
{
    class multiple_class
    {
        public async Task<dynamic> PostRequestAsync(string url, Dictionary<string, string> data)
        {            
            var client = new HttpClient();
            var content = new FormUrlEncodedContent(data);
            var response = await client.PostAsync(url, content);
            var responseString = await response.Content.ReadAsStringAsync();
            dynamic json = JsonConvert.DeserializeObject(responseString);
            return json;
        }       

        public async Task<dynamic> getstring(string url)
        {
            var client = new HttpClient();
            var response = await client.GetStringAsync(url);
            dynamic json = JsonConvert.DeserializeObject(response);
            return json;
        }       

        public string generate_token()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[128];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var finalString = new String(stringChars);
            return finalString.ToString();
        }

        public string generate_token_250()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[250];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var finalString = new String(stringChars);
            return finalString.ToString();
        }       

        public async Task<dynamic> UploadFileAsync( Dictionary<string, string> data, string filePath = null)
        {
            string url = "https://schoolsync.nnmadalin.me/api/upload_file.php";

            var client = new HttpClient();

            var multipartContent = new MultipartFormDataContent();
            foreach (var keyValuePair in data)
            {
                multipartContent.Add(new StringContent(keyValuePair.Value), keyValuePair.Key);
            }

            if (!string.IsNullOrEmpty(filePath))
            {
                var fileContent = new StreamContent(File.OpenRead(filePath));
                var filePart = new ByteArrayContent(await fileContent.ReadAsByteArrayAsync());
                filePart.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    Name = "file",
                    FileName = Path.GetFileName(filePath)
                };
                multipartContent.Add(filePart);
            }

            var response = await client.PostAsync(url, multipartContent);
            var responseString = await response.Content.ReadAsStringAsync();
            dynamic json = JsonConvert.DeserializeObject(responseString);
            return json;
        }        

        public async Task<Image> IncarcaImagineBackgroundAsync(string url)
        {
            try
            {
                using (var webClient = new WebClient())
                {
                    byte[] imageData = await webClient.DownloadDataTaskAsync(url);

                    using (var ms = new System.IO.MemoryStream(imageData))
                    {
                        return Image.FromStream(ms);
                    }
                }
            }
            catch (Exception ex)
            {
                return SchoolSync.Properties.Resources.abstract_pyrimid_upsplash;    
            }
        }
        public async Task<Image> IncarcaImagineAsync(string url)
        {
            try
            {
                using (var webClient = new WebClient())
                {
                    byte[] imageData = await webClient.DownloadDataTaskAsync(url);

                    using (var ms = new System.IO.MemoryStream(imageData))
                    {
                        return Image.FromStream(ms);
                    }
                }
            }
            catch (Exception ex)
            {
                return SchoolSync.Properties.Resources.standard_avatar;    
            }
        }

        public async Task<Image> IncarcaAvatar(string token)
        {
            string url = "https://schoolsync.nnmadalin.me/api/get.php";
            var data = new Dictionary<string, string>();
            data.Add("token", schoolsync.token);
            data.Add("command", "select * from files where token_user = ? and token = ?");

            var param = new Dictionary<string, string>()
                {
                    { "token_user", token},
                    { "token", "user_foto"}
                };
            data.Add("params", JsonConvert.SerializeObject(param));

            dynamic task = await PostRequestAsync(url, data);
            if (task["message"] == "success")
            {
                return  await IncarcaImagineAsync("https://schoolsync.nnmadalin.me/attachments/" + token + "/user_foto/" + task["0"]["name"]);
            }
            else
            {
                return SchoolSync.Properties.Resources.standard_avatar;
            }
        }
    }
}
