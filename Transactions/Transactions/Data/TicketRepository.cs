using System.Data;
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
        await using var transaction = await context.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken);

        await context.Database.ExecuteSqlInterpolatedAsync(
            $"""
             insert into tickets (Id, available)
             values ({ticketId}, {amount})
             on duplicate key update available = available + values(available);
             """,
            cancellationToken);

        var ticket = await ReadTicketAsync(context, ticketId, cancellationToken)
            ?? throw new InvalidOperationException($"Ticket {ticketId} could not be loaded after update.");

        await transaction.CommitAsync(cancellationToken);
        return ticket;
    }

    public async Task<Ticket> RemoveTicketsAsync(int ticketId, int amount, CancellationToken cancellationToken = default)
    {
        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be greater than zero.");
        }

        await using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
        await using var transaction = await context.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken);

        var updatedRows = await context.Tickets
            .Where(ticket => ticket.Id == ticketId && ticket.Available >= amount)
            .ExecuteUpdateAsync(
                setters => setters.SetProperty(ticket => ticket.Available, ticket => ticket.Available - amount),
                cancellationToken);

        var ticket = await ReadTicketAsync(context, ticketId, cancellationToken)
            ?? throw new InvalidOperationException($"Ticket {ticketId} does not exist.");

        if (updatedRows == 0)
        {
            throw new InvalidOperationException(
                $"Not enough tickets are available for ticket ID {ticketId}.");
        }

        await transaction.CommitAsync(cancellationToken);
        return ticket;
    }

    private static Task<Ticket?> ReadTicketAsync(
        TransactionsContext context,
        int ticketId,
        CancellationToken cancellationToken)
    {
        return context.Tickets
            .AsNoTracking()
            .SingleOrDefaultAsync(ticket => ticket.Id == ticketId, cancellationToken);
    }
}
