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

    public async Task<Ticket> AddTicketsAsync(int ticketId, int amount, CancellationToken cancellationToken = default)
    {
        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be greater than zero.");
        }

        await using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);

        var ticket = await context.Tickets
            .SingleOrDefaultAsync(ticket => ticket.Id == ticketId, cancellationToken);

        if (ticket is null)
        {
            ticket = new Ticket
            {
                Id = ticketId,
                Available = amount,
            };
            context.Tickets.Add(ticket);
        }
        else
        {
            ticket.Available += amount;
            ticket.Version = Guid.NewGuid();
        }

        await SaveChangesAsync(context, cancellationToken);
        return ticket;
    }

    public async Task<Ticket> RemoveTicketsAsync(int ticketId, int amount, CancellationToken cancellationToken = default)
    {
        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be greater than zero.");
        }

        await using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);

        var ticket = await context.Tickets
            .SingleOrDefaultAsync(ticket => ticket.Id == ticketId, cancellationToken)
            ?? throw new InvalidOperationException($"Ticket {ticketId} does not exist.");

        if (ticket.Available < amount)
        {
            throw new InvalidOperationException(
                $"Not enough tickets are available for ticket ID {ticketId}.");
        }

        ticket.Available -= amount;
        ticket.Version = Guid.NewGuid();

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
