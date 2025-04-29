using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.Sqlite;
using System.Globalization;
using System.IO;
using System.Linq;

class SQLiteCsvImporter
{
    static void Main(string[] args)
    {
        if (args.Length < 3)
        {
            Console.WriteLine("Użycie: SQLiteCsvImporter <plik_csv> <nazwa_tabeli> <separator>");
            return;
        }

        string csvPath = args[0];
        string tableName = args[1];
        char separator = args[2][0];

        var (headers, rows) = LoadCsv(csvPath, separator);
        var columnInfo = InferColumnTypes(headers, rows);

        string dbFile = "database.sqlite";
        if (File.Exists(dbFile)) File.Delete(dbFile);
        using var connection = new SqliteConnection($"Data Source={dbFile}");
        connection.Open();

        CreateTable(columnInfo, tableName, connection);
        InsertData(rows, headers, tableName, connection);
        ShowTable(tableName, connection);
    }

    // 1. Wczytanie CSV
    static (List<string> headers, List<List<string>> rows) LoadCsv(string path, char separator)
    {
        var lines = File.ReadAllLines(path);
        if (lines.Length < 1)
            throw new Exception("Plik CSV jest pusty.");

        var headers = lines[0].Split(separator).ToList();
        var rows = new List<List<string>>();

        foreach (var line in lines.Skip(1))
            rows.Add(line.Split(separator).ToList());

        return (headers, rows);
    }

    // 2. Rozpoznawanie typów kolumn
    static List<(string name, string type, bool isNullable)> InferColumnTypes(List<string> headers, List<List<string>> rows)
    {
        var columnInfo = new List<(string name, string type, bool isNullable)>();

        for (int i = 0; i < headers.Count; i++)
        {
            bool isNullable = false;
            bool allInt = true;
            bool allDouble = true;

            foreach (var row in rows)
            {
                if (i >= row.Count || string.IsNullOrWhiteSpace(row[i]))
                {
                    isNullable = true;
                    continue;
                }

                if (!int.TryParse(row[i], out _))
                    allInt = false;

                if (!double.TryParse(row[i], NumberStyles.Any, CultureInfo.InvariantCulture, out _))
                    allDouble = false;
            }

            string type = allInt ? "INTEGER" :
                          allDouble ? "REAL" : "TEXT";

            columnInfo.Add((headers[i], type, isNullable));
        }

        return columnInfo;
    }

    // 3. Tworzenie tabeli
    static void CreateTable(List<(string name, string type, bool isNullable)> columnInfo, string tableName, SqliteConnection connection)
    {
        var cols = columnInfo.Select(c =>
            $"{c.name} {c.type}{(c.isNullable ? "" : " NOT NULL")}"
        );

        string sql = $"CREATE TABLE {tableName} ({string.Join(", ", cols)});";
        using var cmd = new SqliteCommand(sql, connection);
        cmd.ExecuteNonQuery();
        Console.WriteLine($"Tabela '{tableName}' została utworzona.");
    }

    // 4. Wstawianie danych
    static void InsertData(List<List<string>> rows, List<string> headers, string tableName, SqliteConnection connection)
    {
        foreach (var row in rows)
        {
            var colNames = string.Join(", ", headers);
            var paramNames = string.Join(", ", headers.Select(h => "@" + h));

            string sql = $"INSERT INTO {tableName} ({colNames}) VALUES ({paramNames});";

            using var cmd = new SqliteCommand(sql, connection);

            for (int i = 0; i < headers.Count; i++)
            {
                var value = i < row.Count && !string.IsNullOrWhiteSpace(row[i]) ? row[i] : null;
                if (double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out double d))
                    cmd.Parameters.AddWithValue("@" + headers[i], d);
                else if (int.TryParse(value, out int n))
                    cmd.Parameters.AddWithValue("@" + headers[i], n);
                else if (value == null)
                    cmd.Parameters.AddWithValue("@" + headers[i], DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@" + headers[i], value);
            }

            cmd.ExecuteNonQuery();
        }

        Console.WriteLine($"Wstawiono {rows.Count} wierszy.");
    }

    // 5. Wyświetlanie danych
    static void ShowTable(string tableName, SqliteConnection connection)
    {
        string sql = $"SELECT * FROM {tableName}";
        using var cmd = new SqliteCommand(sql, connection);
        using var reader = cmd.ExecuteReader();

        var columnNames = Enumerable.Range(0, reader.FieldCount)
                                    .Select(reader.GetName)
                                    .ToList();

        Console.WriteLine(string.Join(" | ", columnNames));

        while (reader.Read())
        {
            var row = new List<string>();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                row.Add(reader.IsDBNull(i) ? "NULL" : reader.GetValue(i).ToString());
            }
            Console.WriteLine(string.Join(" | ", row));
        }
    }
}
