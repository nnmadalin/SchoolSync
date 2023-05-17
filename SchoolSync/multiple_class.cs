using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using FluentFTP;
using System.IO.Compression;

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

        public async Task<string> UploadFileAsync(string path)
        {
            
            try
            {
                using (var ftpClient = new FtpClient())
                {
                    ftpClient.Host = "nnmadalin.me";
                    ftpClient.Credentials = new NetworkCredential("assets@schoolsync.nnmadalin.me", "F8(#-vjTbCmNn52WW9");

                    await Task.Run(() => ftpClient.Connect());
                    string token = generate_token();
                    FileInfo fi = new FileInfo(path);

                    ftpClient.UploadFile(path, "/" + token + fi.Extension);

                    ftpClient.Disconnect();
                    Console.WriteLine("Fișierul a fost încărcat cu succes pe serverul FTP.");
                    return token + fi.Extension;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"A apărut o eroare la încărcarea fișierului pe serverul FTP: {ex.InnerException?.Message ?? ex.Message}");
                return null;
            }
        }

        
    }
}
