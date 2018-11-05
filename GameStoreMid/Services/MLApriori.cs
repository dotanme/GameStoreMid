using Accord.MachineLearning.Rules;
using GameStoreMid.Data;
using GameStoreMid.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace GameStoreMid.Services
{
    public class MLApriori
    {
        private readonly ApplicationDbContext _context;
        private readonly double _minSupport;
        private readonly double _minConfidence;
        AssociationRuleMatcher<int> classifier = null;



        public MLApriori(ApplicationDbContext db)
        {
            _context = db;
            _minSupport = .2;
            _minConfidence = .5;
        }

        public async Task UpdateRecommendedProductsAsync()
        {
            await Task.Run(() =>
            {
                UpdateRecommendedProducts();
            });
        }

        internal AssociationRule<int>[] GetRules()
        {
            if (classifier == null)
                UpdateRecommendedProducts();
            return classifier.Rules;
        }

        public void UpdateRecommendedProducts()
        {

            var orders1 = _context.OrderClient.Include(order => order.ProductOrders).ThenInclude(po => po.Product);
            var orders = _context.ApplicationUser.Include(user => user.Orders).ThenInclude(order => order.ProductOrders)
                .ThenInclude(po => po.Product).Select(delegate (ApplicationUser user)
                {
                    List<int> orderList = new List<int>();
                    foreach (ClientOrder co in user.Orders)
                    {
                        foreach (ProductOrder po in co.ProductOrders)
                        {
                            orderList.Add(po.Product.ProductID);
                        }
                    }
                    return orderList;
                }).ToArray();


            SortedSet<int>[] dataset = ToSortedSet(orders);


            // We will use Apriori to determine the frequent item sets of this database.
            // To do this, we will say that an item set is frequent if it appears in at 
            // least 20% transactions of the database: the value _minSupport * dataset.Length is the support threshold.

            // Create a new a-priori learning algorithm with support 3
            Apriori<int> apriori =
                new Apriori<int>(Convert.ToInt32(_minSupport * dataset.Length), _minConfidence);

            // Use the algorithm to learn a set matcher
            classifier = apriori.Learn(dataset);

        }


        public List<Product> GetRecommendedProducts(int id)
        {
            if (classifier == null)
                UpdateRecommendedProducts();
            int[][] matches = classifier.Decide(new[] { _context.Product.FirstOrDefault(x => x.ProductID == id).ProductID });

            List<int> similarItems = new List<int>();
            foreach (int[] match in matches)
            {
                foreach (int item in match)
                {
                    similarItems.Add(item);
                }
            }
            var similarIds = similarItems.ToHashSet().Take(3); // 3 most recommended items
            var test = _context.Product.Where(x => similarIds.Contains(x.ProductID));
            return test.ToList();

        }

        private SortedSet<int>[] ToSortedSet(List<int>[] orders)
        {
            List<SortedSet<int>> retVal = new List<SortedSet<int>>();
            int index = 0;

            foreach (List<int> order in orders)
            {

                retVal.Add(new SortedSet<int>());
                foreach (int item in order)
                {
                    if (!retVal[index].Add(item)) // if wasnt able to add to the set then it means that the item is already existing in this transaction so we will open a new transactoin
                    {
                        index++;
                        retVal.Add(new SortedSet<int>());
                        retVal[index].Add(item);
                    }
                }
                index++;
            }
            return retVal.ToArray();
        }
    }
}
