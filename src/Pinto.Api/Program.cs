using Pinto.Application.DTOs;
using Pinto.Application.Interfaces;
using Pinto.Domain.Entities;
using Pinto.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddSignalR()
    .AddMessagePackProtocol();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors();

// Health check
app.MapGet("/health", () => Results.Ok(new { status = "healthy" }))
    .WithName("HealthCheck")
    .WithTags("Health");

// Board endpoints
var boards = app.MapGroup("/api/boards").WithTags("Boards");

boards.MapGet("/", async (IBoardRepository repository, CancellationToken ct) =>
{
    var allBoards = await repository.GetAllAsync(ct);
    return Results.Ok(allBoards.Select(ToResponse));
})
.WithName("GetBoards");

boards.MapGet("/{id:guid}", async (Guid id, IBoardRepository repository, CancellationToken ct) =>
{
    var board = await repository.GetByIdAsync(id, ct);
    return board is null ? Results.NotFound() : Results.Ok(ToResponse(board));
})
.WithName("GetBoard");

boards.MapPost("/", async (CreateBoardRequest request, IBoardRepository repository, CancellationToken ct) =>
{
    // TODO: Get actual owner ID from auth context
    var ownerId = Guid.NewGuid();
    var board = Board.Create(request.Name, ownerId, request.Description);
    await repository.AddAsync(board, ct);
    return Results.Created($"/api/boards/{board.Id}", ToResponse(board));
})
.WithName("CreateBoard");

boards.MapPut("/{id:guid}", async (Guid id, UpdateBoardRequest request, IBoardRepository repository, CancellationToken ct) =>
{
    var board = await repository.GetByIdAsync(id, ct);
    if (board is null) return Results.NotFound();

    board.Update(request.Name, request.Description);
    await repository.UpdateAsync(board, ct);
    return Results.Ok(ToResponse(board));
})
.WithName("UpdateBoard");

boards.MapDelete("/{id:guid}", async (Guid id, IBoardRepository repository, CancellationToken ct) =>
{
    var board = await repository.GetByIdAsync(id, ct);
    if (board is null) return Results.NotFound();

    await repository.DeleteAsync(id, ct);
    return Results.NoContent();
})
.WithName("DeleteBoard");

app.Run();

static BoardResponse ToResponse(Board board) => new(
    board.Id,
    board.Name,
    board.Description,
    board.OwnerId,
    board.CreatedAt,
    board.UpdatedAt
);

public partial class Program;
