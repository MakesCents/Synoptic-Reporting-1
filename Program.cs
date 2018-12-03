using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System.Data;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace SynopticReport
{
    public class Meta
    {
        public string versionId { get; set; }
        public DateTime lastUpdated { get; set; }
    }

    public class Text
    {
        public string status { get; set; }
        public string div { get; set; }
    }

    public class Identifier
    {
        public string use { get; set; }
        public string system { get; set; }
        public string value { get; set; }
    }

    public class Coding
    {
        public string system { get; set; }
        public string code { get; set; }
    }

    public class Category
    {
        public List<Coding> coding { get; set; }
    }

    public class Coding2
    {
        public string system { get; set; }
        public string code { get; set; }
    }

    public class Code
    {
        public List<Coding2> coding { get; set; }
        public string text { get; set; }
    }

    public class Subject
    {
        public string reference { get; set; }
    }

    public class RootObject
    {
        public string resourceType { get; set; }
        public string id { get; set; }
        public Meta meta { get; set; }
        public Text text { get; set; }
        public List<Identifier> identifier { get; set; }
        public string status { get; set; }
        public Category category { get; set; }
        public Code code { get; set; }
        public Subject subject { get; set; }
        public string effectiveDateTime { get; set; }
        public DateTime issued { get; set; }
        public string conclusion { get; set; }
    }

    public class User
    {
        public User(string json)
        {
            JObject jObject = JObject.Parse(json);
            JToken jUser = jObject["text"];
            status = (string)jUser["status"];
            //teamname = (string)jUser["teamname"];
            //email = (string)jUser["email"];
            //players = jUser["players"].ToArray();
        }

        public string status { get; set; }
        public string teamname { get; set; }
        public string email { get; set; }
        public Array players { get; set; }
    }

    public class Accession
    {
        public Accession(string json)
        {
            JObject jObject = JObject.Parse(json);
            value = jObject["identifier"][0]["value"].ToString();
            system = jObject["identifier"][0]["system"].ToString();
        }
        public string ans { get; set; }
        public string system { get; set; }
        public string value { get; set; }
    }

    public class CodeSystem
    {
        public CodeSystem(string json)
        {
            JObject jObject = JObject.Parse(json);
            code = jObject["code"]["coding"][0]["code"].ToString();
            system = jObject["code"]["coding"][0]["system"].ToString();
        }
        public string system { get; set; }
        public string code { get; set; }
    }

    public class Program
    {
        private const string URL = "http://hackathon.siim.org/fhir/DiagnosticReport/a819497684894126";
        private const string apikey = "f64cabaa-6baf-48aa-b1ed-7857813c5c07";
        public string responseText;
        private static string text { get; set; }

        public static void Main(string[] args)
        {
            //CreateWebHostBuilder(args).Build().Run();
            //HttpClient client = new HttpClient();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.Headers["apikey" ] = apikey;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();


            WebHeaderCollection header = response.Headers;
            Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
            var encoding = ASCIIEncoding.ASCII;
            using (var reader = new System.IO.StreamReader(response.GetResponseStream(), encoding))
            {
                string responseText = reader.ReadToEnd();
                Trace.WriteLine(responseText);

                //User user = new User(responseText);
                //Trace.WriteLine(user.status);
                Accession id = new Accession(responseText);
                Trace.WriteLine(id.value);
                Trace.WriteLine(id.system);
                CodeSystem loinc = new CodeSystem(responseText);
                Trace.WriteLine(loinc.code);
                Trace.WriteLine(loinc.system);
            }
            //Trace.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            response.Close();
        }
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
