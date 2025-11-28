using Microsoft.EntityFrameworkCore;
using Pinto.Application.Interfaces;
using Pinto.Domain.Entities;

namespace Pinto.Infrastructure.Persistence;

public class BoardRepository : IBoardRepository
{
    private readonly PintoDbContext _context;

    public BoardRepository(PintoDbContext context)
    {
        _context = context;
    }

    public async Task<Board?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Boards.FindAsync([id], cancellationToken);
    }

    public async Task<IReadOnlyList<Board>> GetByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken = default)
    {
        return await _context.Boards
            .Where(b => b.OwnerId == ownerId)
            .OrderByDescending(b => b.UpdatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Board>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Boards
            .OrderByDescending(b => b.UpdatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<Board> AddAsync(Board board, CancellationToken cancellationToken = default)
    {
        await _context.Boards.AddAsync(board, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return board;
    }

    public async Task UpdateAsync(Board board, CancellationToken cancellationToken = default)
    {
        _context.Boards.Update(board);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var board = await _context.Boards.FindAsync([id], cancellationToken);
        if (board is not null)
        {
            _context.Boards.Remove(board);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
