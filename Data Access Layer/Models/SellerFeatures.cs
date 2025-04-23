using Microsoft.ML.Data;

namespace UndergradProject.Data_Access_Layer.Models
{
    public class SellerFeatures
    {
        [LoadColumn(0)]
        public string Name { get; set; }

        [LoadColumn(1)]
        public string Category { get; set; }

        [LoadColumn(2)]
        public float MonthIndex { get; set; }

        [LoadColumn(3)]
        public float TotalQuantity { get; set; }

        [LoadColumn(4)]
        public float TotalRevenue { get; set; }
    }

    public class SellerPrediction
    {
        [ColumnName("PredictedLabel")]
        public bool IsTopSeller { get; set; }

        public float[] Score { get; set; } // [0] = probability of false, [1] = probability of true
    }
}
