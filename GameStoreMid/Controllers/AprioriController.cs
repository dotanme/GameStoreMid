using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Accord.MachineLearning.Rules;
using GameStoreMid.Data;
using GameStoreMid.Models;
using GameStoreMid.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameStoreMid.Controllers
{
    public class AprioriController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly MLApriori _mlApriori;

        public AprioriController(ApplicationDbContext context, MLApriori mlApriori)
        {
            _context = context;
            _mlApriori = mlApriori;
        }

        public class Root
        {
            public Node[] nodes { get; set; }
            public Link[] links { get; set; }
        }

        public class Link :IComparable<Link>
        {
            
            public string source { get; set; }
            public string target { get; set; }
            public int value { get; set; } = 1;

            public int CompareTo(Link other)
            {
                return source.CompareTo(other.source) + target.CompareTo(other.target);
            }
        }

        public class Node
        {
            public string id { get; set; }
            public int group { get; set; }
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public JsonResult GenerateSimilaritiesJson()
        {
            string source, target;
            AssociationRule<int>[] rules = _mlApriori.GetRules();
            Dictionary<int, Product> products = _context.Product.ToDictionary(x=>x.ProductID);
            var nodes = _context.Product.Include(p => p.ProductTags).Select(delegate (Product p)
            {
                var tags = p.ProductTags;
                if (tags.Count > 0)
                    return new Node { id = p.ProductName, group = tags.First().TagID };
                return new Node { id = p.ProductName, group = 0 };
            }).ToArray();

            SortedSet<Link> links = new SortedSet<Link>();

            foreach(AssociationRule<int> rule in rules)
            {
                if(rule.X.Count == 1)
                {
                    source = products[rule.X.First()].ProductName;
                    foreach(int targetId in rule.Y)
                    {
                        target = products[targetId].ProductName;
                        links.Add(new Link { source = source, target = target });
                    }
                }
            }


            
            Root root = new Root { nodes = nodes, links = links.ToArray() };

            return Json(root);
        }
    }
}