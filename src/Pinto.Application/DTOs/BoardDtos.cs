namespace Pinto.Application.DTOs;

public record CreateBoardRequest(string Name, string? Description);

public record UpdateBoardRequest(string Name, string? Description);

public record UpdateCanvasRequest(string? CanvasData);

public record BoardResponse(
    Guid Id,
    string Name,
    string? Description,
    string? CanvasData,
    Guid OwnerId,
    DateTime CreatedAt,
    DateTime UpdatedAt
);
