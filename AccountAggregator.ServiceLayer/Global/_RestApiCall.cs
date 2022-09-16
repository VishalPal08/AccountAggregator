using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace AccountAggregator.ServiceLayer.Global
{
    public class _RestApiCall
    {

        public static string CallingPOSTApi<T>(string Url, T RequestBody, string _Token) where T : class
        {
            try
            {
                string _responseJson = string.Empty;
                using (HttpClientHandler Handler = new HttpClientHandler())
                using (HttpClient client = new HttpClient(Handler))
                {
                    var content = new StringContent(_JsonConvert.SerializeObject<T>(RequestBody), Encoding.UTF8, "application/json");

                    if(!string.IsNullOrEmpty(_Token))
                    {
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _Token);
                    }
                    
                    var APIResponse = client.PostAsync(Url, content).Result;
                    //if (APIResponse.IsSuccessStatusCode)
                    //{
                    //    _responseJson = APIResponse.Content.ReadAsStringAsync().Result;
                    //}
                    //else
                    //{
                    //    _responseJson = APIResponse.Content.ReadAsStringAsync().Result;

                    //    //Console.WriteLine("APIResponse, Error : " + APIResponse.StatusCode);
                    //}

                    _responseJson = APIResponse.Content.ReadAsStringAsync().Result;
                }

                return _responseJson;

            }
            catch(Exception ex)
            {
                throw;
            }
        }

        

    }
}
