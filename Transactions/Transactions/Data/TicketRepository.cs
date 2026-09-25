using Microsoft.EntityFrameworkCore;
using Transactions.Models;

namespace Transactions.Data;

public sealed class TicketRepository
{
    private readonly IDbContextFactory<TransactionsContext> _contextFactory;

    public TicketRepository(IDbContextFactory<TransactionsContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<IReadOnlyList<Ticket>> GetTicketsAsync(CancellationToken cancellationToken = default)
    {
        await using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);

        return await context.Tickets
            .AsNoTracking()
            .OrderBy(ticket => ticket.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task<Ticket> AddTicketsAsync(
        int ticketId,
        Ticket? loadedTicket,
        int amount,
        CancellationToken cancellationToken = default)
    {
        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be greater than zero.");
        }

        await using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);

        if (loadedTicket is null)
        {
            var newTicket = new Ticket
            {
                Id = ticketId,
                Available = amount,
            };
            context.Tickets.Add(newTicket);

            await SaveChangesAsync(context, cancellationToken);
            return newTicket;
        }

        var ticket = new Ticket
        {
            Id = loadedTicket.Id,
            Available = loadedTicket.Available + amount,
            Version = Guid.NewGuid(),
        };

        context.Attach(ticket);
        context.Entry(ticket).Property(t => t.Available).IsModified = true;
        context.Entry(ticket).Property(t => t.Version).IsModified = true;
        context.Entry(ticket).Property(t => t.Version).OriginalValue = loadedTicket.Version;

        await SaveChangesAsync(context, cancellationToken);
        return ticket;
    }

    public async Task<Ticket> RemoveTicketsAsync(
        Ticket loadedTicket,
        int amount,
        CancellationToken cancellationToken = default)
    {
        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be greater than zero.");
        }

        if (loadedTicket.Available < amount)
        {
            throw new InvalidOperationException(
                $"Not enough tickets are available for ticket ID {loadedTicket.Id}.");
        }

        await using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);

        var ticket = new Ticket
        {
            Id = loadedTicket.Id,
            Available = loadedTicket.Available - amount,
            Version = Guid.NewGuid(),
        };

        context.Attach(ticket);
        context.Entry(ticket).Property(t => t.Available).IsModified = true;
        context.Entry(ticket).Property(t => t.Version).IsModified = true;
        context.Entry(ticket).Property(t => t.Version).OriginalValue = loadedTicket.Version;

        await SaveChangesAsync(context, cancellationToken);
        return ticket;
    }

    private static async Task SaveChangesAsync(
        TransactionsContext context,
        CancellationToken cancellationToken)
    {
        try
        {
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new InvalidOperationException(
                "Das Ticket wurde in der Zwischenzeit geändert. Bitte aktualisieren und erneut versuchen.",
                ex);
        }
    }
}
