using System.IO;
using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System.Data.SqlClient;
using Newtonsoft.Json.Linq;
using MySql.Data.MySqlClient;



namespace LuticaFN
{
    public class SQLstring
    {
        public static string sqlinside = Environment.GetEnvironmentVariable("LuSQL") + ";SslMode = " + MySqlSslMode.Required + ";SslCa= \"./inc/\"";
    }
    public class Function1
    {
        private readonly ILogger<Function1> _logger;

        public Function1(ILogger<Function1> log)
        {
            _logger = log;
        }

        [FunctionName("Function1")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "name" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "name", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **Name** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }
    }
    public class getPosts
    {
        public class Posts
        {
            public string id { get; set; }
            public string name { get; set; }
            public string time { get; set; }
        }
        private readonly ILogger<getPosts> _logger;
        [FunctionName("getPosts")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req, ILogger _logger)
        {
            // Get the connection string from app settings and use it to create a connection.
            var str = Environment.GetEnvironmentVariable("LuSQL");
            List<Posts> postlist = new List<Posts>();
            str += ";SslMode = " + MySqlSslMode.Required;
            str += ";SslCa= \"./inc/\"";
            using (MySqlConnection conn = new MySqlConnection(str))
            {
                conn.Open();
                string size = req.Query["size"];
                int sizeint = int.Parse(size);
                string comstring = "SELECT idtb_posts,post_name,post_time FROM tb_posts ";

                using (MySqlCommand cmd = new MySqlCommand(comstring, conn))
                {
                    // Execute the command and log the # rows affected.
                    var rows = await cmd.ExecuteReaderAsync();

                    while (rows.Read())
                    {
                        var posts = new Posts();
                        posts.id = rows["idtb_posts"].ToString();
                        posts.name = rows["post_name"].ToString();
                        posts.time = rows["post_time"].ToString();
                        postlist.Add(posts);

                    }
                    _logger.LogInformation($"Load_post");

                }
                conn.Close();
            }
            JArray return_val = new JArray();
            foreach (Posts post in postlist)
            {
                JObject return_attr = new JObject();
                return_attr.Add("id", post.id);
                return_attr.Add("name", post.name);
                return_attr.Add("time", post.time);
                return_val.Add(return_attr);
            }
            return new OkObjectResult(return_val);

        }

    }
    public class getPostHtml
    {
        private readonly ILogger<getPosts> _logger;
        [FunctionName("getPostHtml")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req, ILogger _logger)
        {
            JObject ret = new JObject();
            int id = int.Parse(req.Query["id"]);
            string sql = SQLstring.sqlinside;
            using (MySqlConnection con = new MySqlConnection(sql)) 
            {
                con.Open();
                string command = " SELECT post_name, post_time, post_html FROM tb_posts WHERE idtb_posts = "+id.ToString();
                
                using (MySqlCommand cmd = new MySqlCommand(command, con))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    var sqlDataReader = await cmd.ExecuteReaderAsync();
                    while (sqlDataReader.Read())
                    {
                        ret.Add("title", sqlDataReader["post_name"].ToString());
                        ret.Add("time", sqlDataReader["post_time"].ToString());
                        ret.Add("html", sqlDataReader["post_html"].ToString());
                    }
                }
            }
            return new OkObjectResult(ret);
        }
    }
}

