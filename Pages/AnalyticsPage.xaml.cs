using Microsoft.ML;
using UndergradProject.Data_Access_Layer.Data;
using UndergradProject.Data_Access_Layer.Models;

namespace UndergradProject.Pages;

public partial class AnalyticsPage : ContentPage
{
    private readonly DatabaseService _dbService;
    private readonly string _modelPath;

    public AnalyticsPage()
    {
        InitializeComponent();
        //string dbPath = Path.Combine(FileSystem.AppDataDirectory, "appdata.db3");
        string dbPath = Constants.salesDatabasePath;
        _dbService = new DatabaseService(dbPath);

        _modelPath = Path.Combine(FileSystem.AppDataDirectory, "salesModel.zip");
    }

    private async void trainModelButton_Clicked(object sender, EventArgs e)
    {
        //await _dbService.DropTableAsync<Sale>();
        try
        {
            statusLabel.Text = "Training model...";

            var builder = new SalesDatasetBuilderAsync(_dbService);
            var dataset = await builder.BuildDatasetAsync();

            if (dataset.Count == 0)
            {
                statusLabel.Text = "Not enough sales data to train.";
                return;
            }

            var trainer = new SalesModelTrainer();
            var model = trainer.TrainModel(dataset);

            var mlContext = new MLContext();
            var schema = mlContext.Data.LoadFromEnumerable(dataset).Schema;
            mlContext.Model.Save(model, schema, _modelPath);

            statusLabel.Text = "Model trained and saved successfully!";
        }
        catch (Exception ex)
        {
            statusLabel.Text = $"Error: {ex.Message}";
        }
    }

    private async void predictButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            statusLabel.Text = "Testing model...";

            var builder = new SalesDatasetBuilderAsync(_dbService);
            var dataset = await builder.BuildDatasetAsync();

            if (dataset.Count < 5)
            {
                statusLabel.Text = "Not enough data to test.";
                return;
            }

            var mlContext = new MLContext();
            var data = mlContext.Data.LoadFromEnumerable(dataset);
            var split = mlContext.Data.TrainTestSplit(data, testFraction: 0.2);

            if (!File.Exists(_modelPath))
            {
                statusLabel.Text = "Trained model not found.";
                return;
            }

            ITransformer trainedModel;
            using (var stream = File.OpenRead(_modelPath))
            {
                trainedModel = mlContext.Model.Load(stream, out _);
            }

            var predictions = trainedModel.Transform(split.TestSet);
            var metrics = mlContext.BinaryClassification.Evaluate(predictions);

            statusLabel.Text =
                $"Test Results:\n" +
                $"Accuracy: {metrics.Accuracy:P2}\n" +
                $"AUC: {metrics.AreaUnderRocCurve:P2}\n" +
                $"F1 Score: {metrics.F1Score:P2}";
        }
        catch (Exception ex)
        {
            statusLabel.Text = $"Test failed: {ex.Message}";
        }
    }
}
