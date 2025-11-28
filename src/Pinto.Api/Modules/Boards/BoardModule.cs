using Pinto.Application.DTOs;
using Pinto.Application.Interfaces;
using Pinto.Domain.Entities;

namespace Pinto.Api.Modules.Boards;

public static class BoardModule
{
    public static IEndpointRouteBuilder MapBoardEndpoints(this IEndpointRouteBuilder app)
    {
        var boards = app.MapGroup("/api/boards").WithTags("Boards");

        boards.MapGet("/", GetAll).WithName("GetBoards");
        boards.MapGet("/{id:guid}", GetById).WithName("GetBoard");
        boards.MapPost("/", Create).WithName("CreateBoard");
        boards.MapPut("/{id:guid}", Update).WithName("UpdateBoard");
        boards.MapDelete("/{id:guid}", Delete).WithName("DeleteBoard");
        boards.MapPut("/{id:guid}/canvas", UpdateCanvas).WithName("UpdateCanvas");

        return app;
    }

    private static async Task<IResult> GetAll(IBoardRepository repository, CancellationToken ct)
    {
        var boards = await repository.GetAllAsync(ct);
        return Results.Ok(boards.Select(ToResponse));
    }

    private static async Task<IResult> GetById(Guid id, IBoardRepository repository, CancellationToken ct)
    {
        var board = await repository.GetByIdAsync(id, ct);
        return board is null ? Results.NotFound() : Results.Ok(ToResponse(board));
    }

    private static async Task<IResult> Create(CreateBoardRequest request, IBoardRepository repository, CancellationToken ct)
    {
        var ownerId = Guid.NewGuid(); // TODO: Get from auth context
        var board = Board.Create(request.Name, ownerId, request.Description);
        await repository.AddAsync(board, ct);
        return Results.Created($"/api/boards/{board.Id}", ToResponse(board));
    }

    private static async Task<IResult> Update(Guid id, UpdateBoardRequest request, IBoardRepository repository, CancellationToken ct)
    {
        var board = await repository.GetByIdAsync(id, ct);
        if (board is null) return Results.NotFound();

        board.Update(request.Name, request.Description);
        await repository.UpdateAsync(board, ct);
        return Results.Ok(ToResponse(board));
    }

    private static async Task<IResult> Delete(Guid id, IBoardRepository repository, CancellationToken ct)
    {
        var board = await repository.GetByIdAsync(id, ct);
        if (board is null) return Results.NotFound();

        await repository.DeleteAsync(id, ct);
        return Results.NoContent();
    }

    private static async Task<IResult> UpdateCanvas(Guid id, UpdateCanvasRequest request, IBoardRepository repository, CancellationToken ct)
    {
        var board = await repository.GetByIdAsync(id, ct);
        if (board is null) return Results.NotFound();

        board.UpdateCanvas(request.CanvasData);
        await repository.UpdateAsync(board, ct);
        return Results.Ok(ToResponse(board));
    }

    private static BoardResponse ToResponse(Board board) => new(
        board.Id,
        board.Name,
        board.Description,
        board.CanvasData,
        board.OwnerId,
        board.CreatedAt,
        board.UpdatedAt
    );
}
