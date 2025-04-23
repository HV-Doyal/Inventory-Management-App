using System;
using System.Collections.Generic;
using Microsoft.ML;
using Microsoft.ML.Data;
using UndergradProject.Data_Access_Layer.Models;

public class SalesModelTrainer
{
    private readonly MLContext mlContext;

    public SalesModelTrainer()
    {
        mlContext = new MLContext(seed: 0);
    }

    public ITransformer TrainModel(List<SellerData> trainingData)
    {
        // 1. Load training data into ML.NET IDataView
        IDataView data = mlContext.Data.LoadFromEnumerable(trainingData);

        // 2. Split into train/test (optional but recommended)
        var split = mlContext.Data.TrainTestSplit(data, testFraction: 0.2);
        var trainData = split.TrainSet;
        var testData = split.TestSet;

        // 3. Build the ML pipeline
        var pipeline = mlContext.Transforms.Categorical
                        .OneHotEncoding(new[] {
                            new InputOutputColumnPair("NameEncoded", "Name"),
                            new InputOutputColumnPair("CategoryEncoded", "Category")
                        })
                        .Append(mlContext.Transforms.Concatenate("Features",
                            "NameEncoded", "CategoryEncoded", "MonthIndex", "TotalQuantity", "TotalRevenue"))
                        .Append(mlContext.BinaryClassification.Trainers.SdcaLogisticRegression(
                            labelColumnName: nameof(SellerData.IsTopSeller),
                            featureColumnName: "Features"));


        // 4. Train the model
        var model = pipeline.Fit(trainData);

        // 5. Evaluate
        var predictions = model.Transform(testData);
        var metrics = mlContext.BinaryClassification.Evaluate(predictions);

        Console.WriteLine($"Accuracy: {metrics.Accuracy:P2}");
        Console.WriteLine($"AUC:      {metrics.AreaUnderRocCurve:P2}");
        Console.WriteLine($"F1 Score: {metrics.F1Score:P2}");

        return model;
    }
}
