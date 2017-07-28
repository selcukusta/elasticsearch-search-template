using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace netcore_elastic_search_template
{
    class Program
    {
        static void Main(string[] args)
        {
            var paramObject = JObject.FromObject(new
            {
                id = "basic_search_template",
                @params = new { query_string = "kamu" }
            }
            );

            var detailParamObject = JObject.FromObject(new
            {
                file = "sample_template",
                @params = new
                {
                    query_string = "kamu",
                    date_range_filter = new
                    {
                        start = new DateTime(2017, 07, 23).ToString("o")
                    }
                }
            });

            var records = GetRecords(detailParamObject);
            foreach (var item in records)
            {
                Console.WriteLine($"{item.Title} ({item.Created_Date})");
            }
        }

        static List<SampleType> GetRecords(JObject parameters)
        {
            List<SampleType> value = new List<SampleType>();
            var elasticConnection = "http://localhost:9200";
            using (var client = new HttpClient())
            {
                var httpContent = new StringContent(JsonConvert.SerializeObject(parameters), Encoding.UTF8, "application/json");
                var response = client.PostAsync(new Uri(elasticConnection + "/_search/template"), httpContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = response.Content.ReadAsStringAsync().Result;
                    var result = Newtonsoft.Json.JsonConvert.DeserializeObject<JObject>(responseContent);
                    foreach (var hit in result["hits"]["hits"])
                    {
                        SampleType content = hit["_source"].ToObject<SampleType>();
                        value.Add(content);
                    }
                }
                return value;
            }
        }
    }

    enum TemplateType
    {
        File,
        Storage
    }
    class SampleType
    {
        public string Title { get; set; }
        public DateTime Created_Date { get; set; }
    }
}
