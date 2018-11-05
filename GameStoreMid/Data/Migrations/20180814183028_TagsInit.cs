using IgdbAPI;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using unirest_net.http;

namespace GameStoreMid.Migrations
{
    public partial class TagsInit : Migration
    {
        public class RootObject
        {
            public int id { get; set; }
        }

        public class Tags
        {
            public int id { get; set; }
            public string name { get; set; }
        }

        public List<Tags> GetTags()
        {
            List<Tags> tagsList = new List<Tags>();
            string url = "https://api-2445582011268.apicast.io/genres/";
            Task<HttpResponse<string>> jsonResponse = Unirest.get(url + "?limit=50")
                .header("user-key", "de594b8b36d90687b800f09eba174443")
                .header("Accept", "application/json")
                .asJsonAsync<string>();


            var model = JsonConvert.DeserializeObject<List<RootObject>>(jsonResponse.Result.Body);
            StringBuilder genres = new StringBuilder();
            for (int i = 0; i < model.Count; i++)
            {
                genres.Append(model[i].id);
                if (i + 1 != model.Count)
                    genres.Append(",");
            }
            Task<HttpResponse<string>> jsonGenres = Unirest.get(url + genres.ToString())
                .header("user-key", "de594b8b36d90687b800f09eba174443")
                .header("Accept", "application/json")
                .asJsonAsync<string>();
            if (jsonGenres.Result != null)
            {
                tagsList = JsonConvert.DeserializeObject<List<Tags>>(jsonGenres.Result.Body);
            }
            return tagsList;
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var tags = GetTags();
            foreach (Tags tag in tags)
            {
                migrationBuilder.InsertData("Tag", new string[] {"TagID", "name"}, new object[] {tag.id, tag.name});
            }
            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
