using Microsoft.ML;
using Microsoft.ML.Data;
using System.Data.SqlClient;
using testPred2;
using static Microsoft.ML.Data.SchemaDefinition;

string _trainDataPath = Path.Combine(Environment.CurrentDirectory, "Data", "taxi-fare-train.csv");
string _testDataPath = Path.Combine(Environment.CurrentDirectory, "Data", "taxi-fare-test.csv");
string _modelPath = Path.Combine(Environment.CurrentDirectory, "Data", "Model.zip");

//MLContext mlContext = new MLContext(seed: 0);

MLContext mlContext = new MLContext();

DatabaseLoader loader = mlContext.Data.CreateDatabaseLoader<Montant>();

string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Database=data;Integrated Security=True;Connect Timeout=30";

string sqlCommandTrain = "select top 250 CAST(Direction as REAL) as Direction,CAST(NombreCollaborateurs as REAL) as NombreCollaborateurs,CAST(Annee as REAL) as Annee,CAST(Mois as REAL) as Mois,CAST(SommeMontantTotalPaye as REAL) as SommeMontantTotalPaye from test ORDER BY Annee desc";
string sqlCommandTest = "select CAST(Direction as REAL) as Direction,CAST(NombreCollaborateurs as REAL) as NombreCollaborateurs,CAST(Annee as REAL) as Annee,CAST(Mois as REAL) as Mois,CAST(SommeMontantTotalPaye as REAL) as SommeMontantTotalPaye from test ORDER BY Annee desc OFFSET 250 ROWS;";

DatabaseSource dbSourceTrain = new DatabaseSource(SqlClientFactory.Instance, connectionString, sqlCommandTrain);
DatabaseSource dbSourceTest = new DatabaseSource(SqlClientFactory.Instance, connectionString, sqlCommandTest);

var model = Train(mlContext);
Evaluate(mlContext, model);
TestSinglePrediction(mlContext, model);

ITransformer Train(MLContext mlContext)
{
    /*IDataView dataView = mlContext.Data.LoadFromTextFile<TaxiTrip>(dataPath, hasHeader: true, separatorChar: ',');
    var pipeline = mlContext.Transforms.CopyColumns(outputColumnName: "Label", inputColumnName: "FareAmount")
        .Append(mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "VendorIdEncoded", inputColumnName: "VendorId"))
        .Append(mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "RateCodeEncoded", inputColumnName: "RateCode"))
        .Append(mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "PaymentTypeEncoded", inputColumnName: "PaymentType"))
        .Append(mlContext.Transforms.Concatenate("Features", "VendorIdEncoded", "RateCodeEncoded", "PassengerCount", "TripDistance", "PaymentTypeEncoded"))
        .Append(mlContext.Regression.Trainers.FastTree());*/

    IDataView dataView = loader.Load(dbSourceTrain);
    var pipeline = mlContext.Transforms.CopyColumns(outputColumnName: "Label", inputColumnName: "SommeMontantTotalPaye")
       .Append(mlContext.Transforms.Concatenate("Features", "Direction","NombreCollaborateurs","Annee", "Mois"))
       .Append(mlContext.Regression.Trainers.FastTree());
    var model = pipeline.Fit(dataView);
    return model;
}

void Evaluate(MLContext mlContext, ITransformer model)
{
    //IDataView dataView = mlContext.Data.LoadFromTextFile<TaxiTrip>(_testDataPath, hasHeader: true, separatorChar: ',');
    IDataView dataView = loader.Load(dbSourceTest);
    var predictions = model.Transform(dataView);
    Console.WriteLine();
    Console.WriteLine($"*************************************************");
    Console.WriteLine(model.GetOutputSchema(dataView.Schema));
    Console.WriteLine($"*************************************************");
    var metrics = mlContext.Regression.Evaluate(predictions, "Label", "Score");
    Console.WriteLine();
    Console.WriteLine($"*************************************************");
    Console.WriteLine($"*       Model quality metrics evaluation         ");
    Console.WriteLine($"*------------------------------------------------");

    Console.WriteLine($"*       RSquared Score:      {metrics.RSquared:0.##}");

    Console.WriteLine($"*       Root Mean Squared Error:      {metrics.RootMeanSquaredError:#.##}");
}

void TestSinglePrediction(MLContext mlContext, ITransformer model)
{
    var predictionFunction = mlContext.Model.CreatePredictionEngine<Montant, MontantPred>(model);

    var montantSample = new Montant()
    {
        Direction = 2,
        NombreCollaborateurs = 13,
        Annee = 2025,
        Mois = 8,
        SommeMontantTotalPaye = 0
    };

    /*var taxiTripSample = new TaxiTrip()
    {
        VendorId = "VTS",
        RateCode = "1",
        PassengerCount = 1,
        TripTime = 1140,
        TripDistance = 3.75f,
        PaymentType = "CRD",
        FareAmount = 0 // To predict. Actual/Observed = 15.5
    };*/

    var prediction = predictionFunction.Predict(montantSample);
    Console.WriteLine($"**********************************************************************");
    Console.WriteLine($"Predicted fare: {prediction.SommeMontantTotalPaye:0.####}");
    Console.WriteLine($"**********************************************************************");

}