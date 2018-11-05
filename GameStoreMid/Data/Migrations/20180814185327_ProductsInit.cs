using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IgdbAPI;
using unirest_net.http;
using Newtonsoft.Json;
using System.Text;

namespace GameStoreMid.Migrations
{
    public partial class ProductsInit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            Random rand = new Random();
            var games = GetProducts();
            StringBuilder pic = new StringBuilder();
            
            foreach (Product product in games)
            {
                pic.Clear();
                var unixDate = product.first_release_date != 0 ? product.first_release_date : product.updated_at;
                DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                DateTime date = start.AddMilliseconds(unixDate).ToLocalTime();
                if(product.cover != null)
                {
                    pic.Append(product.cover.url);
                }
                if (product.screenshots != null)
                {
                    foreach (Cover screenshot in product.screenshots)
                    {
                        pic.Append("," + screenshot.url);
                    }
                }
                migrationBuilder.InsertData("Product", new string[] { "ProductID", "Cost", "DealID", "ImageUrl", "ProductDescription", "ProductName", "ReleaseDate", "TotalQuantity" }, new object[] {product.id, rand.Next(30, 300), null, pic.ToString(), product.summary, product.name,  date, rand.Next(30, 300) });
                if (product.genres != null)
                {
                    foreach (long tag in product.genres)
                    {
                        migrationBuilder.InsertData("ProductTag", new string[] { "ProductID", "TagID" }, new object[] { product.id, tag });
                    }
                }
            }
        }
        public class Product
        {
            public int id { get; set; }
            public string name { get; set; }
            public long updated_at { get; set; }
            public string summary { get; set; }
            public int[] genres { get; set; }
            public long first_release_date { get; set; }
            public Cover cover { get; set; }
            public Cover[] screenshots { get; set; }

        }



        private List<Product> GetProducts()
        {
            string endpoint = Startup.SettingFactory()["IGDB:endpoint"];
            string userkey = Startup.SettingFactory()["IGDB:user-key"];

            string url = "https://api-"+ endpoint +".apicast.io/games/";
            Task<HttpResponse<string>> jsonResponse = Unirest.get(url + "?fields=id,name,cover,summary,first_release_date,updated_at,genres,rating,screenshots,release_dates,esrb&limit=30&order=rating:desc&filter[release_dates.platform][any]=6,48,49&filter[cover][exists]&filter[esrb.rating][eq]=6&filter[first_release_date][gt]=2013-01-01&filter[category][eq]=0")
                .header("user-key", userkey)
                .header("Accept", "application/json")
                .asJsonAsync<string>();
            var x = jsonResponse.Result.Body;
            var model = JsonConvert.DeserializeObject<List<Product>>(jsonResponse.Result.Body);
            return model;

        }


        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
