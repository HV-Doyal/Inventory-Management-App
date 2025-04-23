using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UndergradProject.Data_Access_Layer.Models
{
    public class SellerData
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public float MonthIndex { get; set; }  // e.g. 2025*12 + 4 = 24304
        public float TotalQuantity { get; set; }
        public float TotalRevenue { get; set; }
        public bool IsTopSeller { get; set; }  // THIS is the label (what we’re trying to predict)
    }

}
