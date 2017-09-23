using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using MySql.Data.MySqlClient;
using Dapper;
using System.Linq;
using System.Collections.Generic;

namespace TodoService.Controllers
{
    public class TodoModel
    {
        public TodoModel()
        {
            
        }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("id")]        
        public string Id { get; set; }

        [JsonProperty("checked")]        
        public bool Checked {get;set;}
    }

    [Route("api/[controller]")]
    public class TodoController
    {
        private string _connectionString;
        private readonly ILogger _logger;

        public TodoController(ILoggerFactory loggerFactory)
        {
            _connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
            _logger = loggerFactory.CreateLogger<TodoController>();
            _logger.LogInformation($"Connection string :{_connectionString}");
        }

        [HttpPost]
        public TodoModel Post([FromBody] TodoModel model)
        {

            using (var connection = new MySqlConnection(_connectionString))
            {
                _logger.LogInformation($"Model Text: {model.Text}");
                var id = connection.Query<string>("INSERT INTO todo (text, checked) values (@Text, @Checked); SELECT LAST_INSERT_ID();", model).Single();
                _logger.LogInformation($"Posted ID: {id}");
                return Get(id);
            }
        }

        [HttpGet("{id}")]
        public TodoModel Get(string id)
        {
            using(var connection = new MySqlConnection(_connectionString))
            {
                return connection.Query<TodoModel>("SELECT id,checked,text FROM todo WHERE id=@Id", new {Id=id}).FirstOrDefault();
            }
        }
    }
}