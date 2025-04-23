using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using UndergradProject.Data_Access_Layer.Models;

namespace UndergradProject.Data_Access_Layer.Data
{
    public class SalesDatasetBuilderAsync
    {
        private readonly DatabaseService _databaseService;

        public SalesDatasetBuilderAsync(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<List<SellerData>> BuildDatasetAsync()
        {
            // 1. Get all sales
            var allSales = await _databaseService.GetDataAsync<Sale>();

            // 2. Group by (name, category, year, month)
            var monthlyGroups = allSales
                .GroupBy(sale =>
                {
                    var dt = DateTime.ParseExact(sale.date, "M/dd/yyyy", CultureInfo.InvariantCulture);
                    return new { sale.name, sale.category, Year = dt.Year, Month = dt.Month };
                });

            // 3. Summarize each group
            var summaries = monthlyGroups
                .Select(g => new SellerData
                {
                    Name = g.Key.name,
                    Category = g.Key.category,
                    MonthIndex = g.Key.Year * 12 + g.Key.Month,
                    TotalQuantity = g.Sum(x => x.quantity),
                    TotalRevenue = (float)g.Sum(x => x.quantity * x.unitPrice),
                    IsTopSeller = false
                })
                .ToList();

            // 4. Flag top seller(s) for each month
            var byMonth = summaries.GroupBy(s => s.MonthIndex);
            foreach (var monthGroup in byMonth)
            {
                var maxQty = monthGroup.Max(x => x.TotalQuantity);
                foreach (var entry in monthGroup)
                    entry.IsTopSeller = entry.TotalQuantity == maxQty;
            }

            return summaries;
        }
    }
}
