using System.ComponentModel.DataAnnotations;

namespace Transactions.Models;

public sealed class Ticket
{
    public int Id { get; set; }

    public int Available { get; set; }

    [ConcurrencyCheck]
    public Guid Version { get; set; } = Guid.NewGuid();
}
