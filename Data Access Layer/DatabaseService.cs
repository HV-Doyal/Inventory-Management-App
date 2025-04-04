using SQLite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class DatabaseService
{
    private readonly SQLiteAsyncConnection _database;

    // Constructor that takes the database path
    public DatabaseService(string dbPath)
    {
        // Initialize the SQLite connection
        _database = new SQLiteAsyncConnection(dbPath);
    }

    // Creates a table in the database (Dynamically creates the table for any class)
    public async Task CreateTableAsync<T>() where T : new()
    {
        // Create the table for the given type T (e.g., User, Product, etc.)
        await _database.CreateTableAsync<T>();
        Console.WriteLine("Table created successfully.");
    }

    public async Task InsertDataAsync<T>(T data) where T : new()
    {
        // Insert data into the table corresponding to the type T
        await _database.InsertAsync(data);
        Console.WriteLine("Data inserted successfully.");
    }

    public async Task<List<T>> GetDataAsync<T>() where T : new()
    {
        // Fetch all records from the table corresponding to type T
        var data = await _database.Table<T>().ToListAsync();
        Console.WriteLine("Data retrieved successfully.");
        return data;
    }

}
