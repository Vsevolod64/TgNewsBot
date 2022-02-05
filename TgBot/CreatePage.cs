using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Createpage
{
    internal class CreatePage
    {
        Dictionary<string, string> data = new Dictionary<string, string>()
        {
            {"access_token", "b991ebacacdc61cdcd637ff313d1f4a846cd2e06a1197015f1c28e4a762d" },
            {"title", ""},
            {"author_name", "Zaporozhye News" },
            {"content","" },
            {"return_content", "true" }
        };


        public string CreatePageFunc(PageNode node, string title)
        {
            CreateJsonFile ConvertData = new();

            try
            {
                string address = @"https://api.telegra.ph/createPage?";

                string DataContent = ConvertData.FormirateString(node);

                data["content"] = DataContent;
                data["title"] = title;

                var Answer = GetRequest(address, data);
                string Content = Answer.Result.Content.ReadAsStringAsync().Result;

                JObject json = JObject.Parse(Content);
                string result = (string)json.SelectToken("result.url");

                return result;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error!" + ex.Message);
                throw;
            }
          
  
        }


        private Task<HttpResponseMessage> GetRequest(string adress, Dictionary<string, string> data)
        {
            HttpClient client = new HttpClient();


            try
            {
                Uri uri = new Uri(adress);
                var content = new FormUrlEncodedContent(data);

                return (client.PostAsync(uri, content));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error!" + ex.ToString());
            }


            client.Dispose();
            return null;
        }
    }
}
