using Neo4j.Driver;

var driver = GraphDatabase.Driver(
    "bolt://localhost:7687",
    AuthTokens.Basic("neo4j", "password123")
);

await using var session = driver.AsyncSession();

while (true)
{
    Console.Write("\nMitarbeiter 1 (exit zum Beenden): ");
    string employee1 = Console.ReadLine()!.Trim().ToUpper();

    if (employee1 == "EXIT")
        break;

    Console.Write("Mitarbeiter 2: ");
    string employee2 = Console.ReadLine()!.Trim().ToUpper();

    var result = await session.ExecuteReadAsync(async tx =>
    {
        var cursor = await tx.RunAsync(
            """
            MATCH path = (e:Employee)-[:REPORTS_TO*0..]->(top:Employee)
            WHERE e.ename = $employee1
              AND top.ename = $employee2
            RETURN [n IN nodes(path) | n.ename] AS employees
            ORDER BY length(path)
            LIMIT 1
            """,
            new { employee1, employee2 }
        );

        return await cursor.SingleOrDefaultAsync();
    });

    if (result == null)
    {
        Console.WriteLine("Keine Verbindung gefunden.");
        continue;
    }

    var employees = result["employees"].As<List<string>>();

    Console.WriteLine("\nVerbindung gefunden:\n");
    Console.WriteLine(string.Join(" -[:REPORTS_TO]-> ", employees));
}

await driver.DisposeAsync();