
using System.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using MySqlConnector;

var builder = Host.CreateApplicationBuilder(args);

var connectionString = "Server=localhost;Port=3306;Database=Transaktionslog;User=root;Password=insy;";

await using var connection = new MySqlConnection(connectionString);

try
{
    await connection.OpenAsync();
    Console.WriteLine("Mit MySQL verbunden.");
}
catch (Exception ex)
{
    Console.WriteLine($"Verbindungsfehler: {ex.Message}");
    return;
}

while (true)
{
    Console.WriteLine("\n===== TRANSAKTIONSVERWALTUNG =====");
    Console.WriteLine("1 - Konten anzeigen");
    Console.WriteLine("2 - Transaktionslog anzeigen");
    Console.WriteLine("3 - Auf alten Stand zurücksetzen");
    Console.WriteLine("0 - Beenden");
    Console.Write("Auswahl: ");

    var auswahl = Console.ReadLine();

    try
    {
        switch (auswahl)
        {
            case "1":
                await ShowKonten();
                break;

            case "2":
                await ShowLog();
                break;

            case "3":
                await Restore();
                break;

            case "0":
                return;

            default:
                Console.WriteLine("Ungültige Auswahl.");
                break;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Fehler: {ex.Message}");
    }
}

async Task ShowKonten()
{
    const string sql =
        "SELECT konto_id, kontostand FROM konto ORDER BY konto_id";

    await using var cmd = new MySqlCommand(sql, connection);
    await using var reader = await cmd.ExecuteReaderAsync();

    Console.WriteLine("\nKonto-ID | Kontostand");
    Console.WriteLine("--------------------");

    while (await reader.ReadAsync())
    {
        Console.WriteLine(
            $"{reader.GetInt32(0),8} | {reader.GetDecimal(1),10:F2}");
    }
}

async Task ShowLog()
{
    const string sql = """
        SELECT transaktions_id, zeitpunkt, konto_id,
               benutzer, aktion, old_wert, new_wert
        FROM transaktionslog
        ORDER BY transaktions_id
        """;

    await using var cmd = new MySqlCommand(sql, connection);
    await using var reader = await cmd.ExecuteReaderAsync();

    Console.WriteLine("\nID | Zeitpunkt           | Konto | Aktion | Alt | Neu");
    Console.WriteLine("------------------------------------------------------");

    while (await reader.ReadAsync())
    {
        int id = reader.GetInt32(0);
        DateTime zeit = reader.GetDateTime(1);

        string konto = reader.IsDBNull(2)
            ? "-"
            : reader.GetInt32(2).ToString();

        string aktion = reader.GetString(4);

        string alt = reader.IsDBNull(5)
            ? "-"
            : reader.GetDecimal(5).ToString("F2");

        string neu = reader.IsDBNull(6)
            ? "-"
            : reader.GetDecimal(6).ToString("F2");

        Console.WriteLine(
            $"{id,2} | {zeit:yyyy-MM-dd HH:mm:ss} | " +
            $"{konto,5} | {aktion,-6} | {alt,6} | {neu,6}");
    }
}

async Task Restore()
{
    await ShowLog();

    Console.Write("\nBis zu welcher Transaktions-ID wiederherstellen? (0 = Anfang): ");

    if (!int.TryParse(Console.ReadLine(), out int zielId) || zielId < 0)
    {
        Console.WriteLine("Ungültige ID.");
        return;
    }

    Console.Write($"Wirklich auf den Stand nach ID {zielId} zurücksetzen? (ja/nein): ");

    if (!string.Equals(
            Console.ReadLine()?.Trim(),
            "ja",
            StringComparison.OrdinalIgnoreCase))
    {
        Console.WriteLine("Abgebrochen.");
        return;
    }

    await using var transaction =
        await connection.BeginTransactionAsync(IsolationLevel.RepeatableRead);

    try
    {
        if (zielId != 0)
        {
            const string checkSql = """
                SELECT transaktions_id
                FROM transaktionslog
                WHERE transaktions_id = @id
                FOR UPDATE
                """;

            await using var checkCmd =
                new MySqlCommand(checkSql, connection, transaction);

            checkCmd.Parameters.AddWithValue("@id", zielId);

            var result = await checkCmd.ExecuteScalarAsync();

            if (result == null)
                throw new Exception("Diese Transaktions-ID existiert nicht.");
        }
        
        const string sql = """
            SELECT transaktions_id, konto_id, aktion,
                   old_wert, new_wert
            FROM transaktionslog
            WHERE transaktions_id > @id
            ORDER BY transaktions_id DESC
            FOR UPDATE
            """;

        var logs = new List<LogEntry>();

        await using (var cmd = new MySqlCommand(sql, connection, transaction))
        {
            cmd.Parameters.AddWithValue("@id", zielId);

            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                logs.Add(new LogEntry(
                    reader.GetInt32(0),
                    reader.GetInt32(1),
                    reader.GetString(2),
                    reader.IsDBNull(3) ? null : reader.GetDecimal(3),
                    reader.IsDBNull(4) ? null : reader.GetDecimal(4)
                ));
            }
        }

        if (logs.Count == 0)
        {
            Console.WriteLine("Es gibt keine späteren Änderungen.");
            await transaction.CommitAsync();
            return;
        }
        
        foreach (var log in logs)
        {
            await UndoLogEntry(log, transaction);
            Console.WriteLine($"Änderung {log.Id} rückgängig gemacht.");
        }

        await transaction.CommitAsync();

        Console.WriteLine(
            $"\nWiederherstellung erfolgreich! " +
            $"{logs.Count} Änderungen wurden rückgängig gemacht.");
    }
    catch
    {
        await transaction.RollbackAsync();
        Console.WriteLine("Wiederherstellung fehlgeschlagen. Änderungen zurückgerollt.");
        throw;
    }
}

async Task UndoLogEntry(LogEntry log, MySqlTransaction transaction)
{
    string sql;

    switch (log.Aktion)
    {
        case "INSERT":
            if (log.NewWert == null)
                throw new Exception($"Ungültiger INSERT-Logeintrag {log.Id}");

            sql = """
                DELETE FROM konto
                WHERE konto_id = @kontoId
                  AND kontostand = @newWert
                """;
            break;

        case "UPDATE":
            if (log.OldWert == null || log.NewWert == null)
                throw new Exception($"Ungültiger UPDATE-Logeintrag {log.Id}");

            sql = """
                UPDATE konto
                SET kontostand = @oldWert
                WHERE konto_id = @kontoId
                  AND kontostand = @newWert
                """;
            break;

        case "DELETE":
            if (log.OldWert == null)
                throw new Exception($"Ungültiger DELETE-Logeintrag {log.Id}");

            sql = """
                INSERT INTO konto (konto_id, kontostand)
                VALUES (@kontoId, @oldWert)
                """;
            break;

        default:
            throw new Exception($"Unbekannte Aktion: {log.Aktion}");
    }

    await using var cmd = new MySqlCommand(sql, connection, transaction);

    cmd.Parameters.AddWithValue("@kontoId", log.KontoId);

    if (log.OldWert.HasValue)
        cmd.Parameters.AddWithValue("@oldWert", log.OldWert.Value);

    if (log.NewWert.HasValue)
        cmd.Parameters.AddWithValue("@newWert", log.NewWert.Value);

    int affected = await cmd.ExecuteNonQueryAsync();
    
    if (affected != 1 &&
        !(log.Aktion == "UPDATE" && log.OldWert == log.NewWert))
    {
        throw new Exception(
            $"Logeintrag {log.Id} konnte nicht rückgängig gemacht werden.");
    }
}

record LogEntry(
    int Id,
    int KontoId,
    string Aktion,
    decimal? OldWert,
    decimal? NewWert
);
