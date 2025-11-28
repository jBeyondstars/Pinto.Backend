namespace Pinto.Application.DTOs;

public record CreateBoardRequest(string Name, string? Description);

public record UpdateBoardRequest(string Name, string? Description);

public record BoardResponse(
    Guid Id,
    string Name,
    string? Description,
    Guid OwnerId,
    DateTime CreatedAt,
    DateTime UpdatedAt
);
