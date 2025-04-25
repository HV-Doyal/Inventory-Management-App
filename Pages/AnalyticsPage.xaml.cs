using Microsoft.ML;
using Microsoft.ML.Data;
using System.IO;
using UndergradProject.Data_Access_Layer.Data;
using UndergradProject.Data_Access_Layer.Models;
using System.Globalization;

namespace UndergradProject.Pages;

public partial class AnalyticsPage : ContentPage
{
    private readonly DatabaseService _dbService;
    private readonly string _modelPath;
    private readonly string _trainFilePath = Path.Combine(FileSystem.AppDataDirectory, "train.csv");    // Path to training data file

    // Class representing the input data for the model
    public class MonthlySalesData
    {
        public string Month { get; set; }      // "05" for May (month of the sale)
        public string ProductCategory { get; set; }  // Category of the product sold (e.g., Electronics, Furniture)
        public float TotalRevenue { get; set; } // Total revenue generated for the product in that month
    }

    // Class representing the output predictions of the model
    public class MonthlySalesPrediction
    {
        [ColumnName("Score")]  // The predicted value column from the model, labeled as "Score"
        public float PredictedRevenue { get; set; } // Predicted revenue based on the features
    }

    // Constructor
    public AnalyticsPage()
    {
        InitializeComponent();

        // Get the model file path
        _modelPath = Path.Combine(FileSystem.AppDataDirectory, "trained_model.zip");
        // Log the path of the model file
        Console.WriteLine($"Model file path: {_modelPath}");

        // Check if the file exists
        if (File.Exists(_modelPath))
        {
            Console.WriteLine("✅ Model file exists at the specified location.");
        }
        else
        {
            Console.WriteLine("❌ Model file does NOT exist at the specified location.");
        }


        // Ensure the model is copied from the resources to the app's local storage
        CopyModelFileIfNeeded();
        CopyCsvToAppData();
    }

    // Method to copy the model file from resources to app's local storage
    private void CopyModelFileIfNeeded()
    {
        string resourcePath = "UndergradProject.Resources.trained_model.zip"; // Set the correct resource path for the model file in your project

        if (!File.Exists(_modelPath))
        {
            var assembly = typeof(AnalyticsPage).Assembly;
            using (var stream = assembly.GetManifestResourceStream(resourcePath))
            using (var fileStream = new FileStream(_modelPath, FileMode.Create))
            {
                stream.CopyTo(fileStream);
            }
        }
    }

    // Button click handler to trigger prediction
    private async void predictButton_Clicked(object sender, EventArgs e)
    {
        await Predict();
    }

    // Method to make predictions using the trained model
    public void CopyCsvToAppData()
    {
        string appDataPath = "UndergradProject.Resources.train.csv";  // Path to your CSV resource in the project

        if (!File.Exists(_trainFilePath))  // Check if the train.csv doesn't exist
        {
            var assembly = typeof(AnalyticsPage).Assembly;
            using (var stream = assembly.GetManifestResourceStream(appDataPath))
            using (var fileStream = new FileStream(_trainFilePath, FileMode.Create))  // Correct the path to _trainFilePath
            {
                stream.CopyTo(fileStream);
            }
        }
    }

    // Method to make predictions using the trained model
    public async Task Predict()
    {
        var mlContext = new MLContext();
        string predictionResults = "";  // String to store all the predictions

        try
        {
            // Load the trained model from the local file path
            ITransformer model = mlContext.Model.Load(_modelPath, out var modelInputSchema);
            Console.WriteLine("✅ Model loaded successfully.");

            // Load the training data
            var rawData = File.ReadAllLines(_trainFilePath)
                .Skip(1) // Skip the header
                .Select(line =>
                {
                    var parts = line.Split(',');

                    // Ensure that we parse only valid rows and log any issues
                    if (parts.Length < 5)
                    {
                        Console.WriteLine($"❌ Malformed row: {line}");
                        return null; // Skip invalid rows
                    }

                    // Correct the formatting as per console app data preprocessing
                    var month = DateTime.ParseExact(parts[0], "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM");
                    var productCategory = parts[1];  // Product Category
                    var totalRevenue = float.Parse(parts[4], CultureInfo.InvariantCulture);  // Total revenue

                    return new MonthlySalesData
                    {
                        Month = month,
                        ProductCategory = productCategory,
                        TotalRevenue = totalRevenue
                    };
                })
                .Where(data => data != null) // Skip any null data
                .ToList();

            Console.WriteLine($"Loaded {rawData.Count} rows of data.");

            // Create a prediction engine from the loaded model
            var predictionEngine = mlContext.Model.CreatePredictionEngine<MonthlySalesData, MonthlySalesPrediction>(model);
            Console.WriteLine("✅ Prediction engine created.");

            // Calculate the next month
            DateTime currentDate = DateTime.Now;
            DateTime nextMonthDate = currentDate.AddMonths(1);
            string monthToPredict = nextMonthDate.ToString("MM"); // Get the month in MM format

            // Prepare the results string
            predictionResults += $"📅 Predicting revenue for all categories in {monthToPredict}...\n\n";

            // One-hot encode the ProductCategory and Month columns for prediction
            var productCategories = rawData.Select(r => r.ProductCategory).Distinct().ToList();

            // Loop through each product category to predict the revenue for that category
            foreach (var category in productCategories)
            {
                var input = new MonthlySalesData
                {
                    ProductCategory = category,
                    Month = monthToPredict // Next month
                };

                try
                {
                    var prediction = predictionEngine.Predict(input);
                    var resultText = $"📊 Predicted revenue for '{category}' in {monthToPredict}: ${prediction.PredictedRevenue:F2}\n";
                    Console.WriteLine(resultText);

                    // Add the result to the string to be shown on the label
                    predictionResults += resultText;
                }
                catch (Exception ex)
                {
                    var errorText = $"❌ Error predicting revenue for '{category}': {ex.Message}\n";
                    Console.WriteLine(errorText);

                    // Add the error to the results string
                    predictionResults += errorText;
                }
            }

            // Update the label with the prediction results
            statusLabel.Text = predictionResults;
        }
        catch (Exception ex)
        {
            string errorMessage = $"❌ Error in prediction process: {ex.Message}\nStackTrace: {ex.StackTrace}";
            Console.WriteLine(errorMessage);

            // Display the error message on the label
            statusLabel.Text = errorMessage;
        }
    }

}
