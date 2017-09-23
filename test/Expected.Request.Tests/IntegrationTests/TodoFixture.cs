using System;
using Dapper;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace Expected.Request.Tests.IntegrationTests
{
    public class TodoModel 
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("id")]        
        public string Id { get; set; }

        [JsonProperty("checked")]        
        public bool Checked {get;set;}

        public override bool Equals(object obj)
        {
            return obj is TodoModel && Equals(obj as TodoModel);
        }

        public bool Equals(TodoModel model)
        {
            return 
                model.Checked == Checked &&
                model.Id == Id &&
                model.Text == Text;
        }
    }

    public class TodoFixture : TestFixture
    {
        protected string ConnectionString;
        private string _endpoint = "/api/todo";
        protected string ApiUrl;

        public TodoFixture()
        {
            ConnectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
            ApiUrl = Environment.GetEnvironmentVariable("API_URL") + _endpoint;
            Dispose(); //clean the database before each test
        }

        public override void Dispose()
        {
            using (var connection = new MySqlConnection(ConnectionString))
            {
                connection.Execute("truncate todo");
            }
        }
    }
}