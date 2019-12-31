using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TesteFernando.Models;

namespace TesteFernando.Services
{
    public class ApiService
    {
        HttpClient client = new HttpClient();
        string _url = "https://gateway.marvel.com:443/";
        string PublicKey = "ea2240c8a5adc4b2fc207f6a08d6affd";
        string PrivateKey = "12169071375c756281097c7cf31fa49e9debfa60";
        string Ts = DateTime.Now.ToString();

        public async Task<Marvel> GetCharacters(string limit, string offset)
        {
            try
            {
                var hash = GerarHashMd5(Ts + PrivateKey + PublicKey);

                UriBuilder builder = new UriBuilder(_url);
                builder.Path = "v1/public/characters";

                HttpResponseMessage response = await client.GetAsync(builder + "?limit=" + limit + "&offset=" + offset + "&ts=" + Ts + "&apikey=" + PublicKey + "&hash=" + hash);
                                
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string serialized = await response.Content.ReadAsStringAsync();

                    Marvel result = await Task.Run(() => JsonConvert.DeserializeObject<Marvel>(serialized));
                   
                    return result;
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {                    
                    throw new Exception("Sua sessão expirou.");
                }
                else
                {
                    throw new Exception(JsonConvert.DeserializeObject<ExceptionMessage>(response.Content.ReadAsStringAsync().Result).Message);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Marvel> GetCharactersByName(string name, string limit, string offset)
        {
            try
            {
                var hash = GerarHashMd5(Ts + PrivateKey + PublicKey);

                UriBuilder builder = new UriBuilder(_url);
                builder.Path = "v1/public/characters";

                HttpResponseMessage response = await client.GetAsync(builder + "?name=" + name + "&limit=" + limit + "&offset=" + offset + "&ts=" + Ts + "&apikey=" + PublicKey + "&hash=" + hash);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string serialized = await response.Content.ReadAsStringAsync();

                    Marvel result = await Task.Run(() => JsonConvert.DeserializeObject<Marvel>(serialized));

                    return result;
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new Exception("Sua sessão expirou.");
                }
                else
                {
                    throw new Exception(JsonConvert.DeserializeObject<ExceptionMessage>(response.Content.ReadAsStringAsync().Result).Message);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        class ExceptionMessage
        {
            public string Message { get; set; }
        }

        public static string GerarHashMd5(string input)
        {
            MD5 md5Hash = MD5.Create();
            // Converter a String para array de bytes, que é como a biblioteca trabalha.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Cria-se um StringBuilder para recompôr a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop para formatar cada byte como uma String em hexadecimal
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }
    }
}
