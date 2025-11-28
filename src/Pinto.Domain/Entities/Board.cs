namespace Pinto.Domain.Entities;

public class Board
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public string? CanvasData { get; private set; }
    public Guid OwnerId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private Board() { }

    public static Board Create(string name, Guid ownerId, string? description = null)
    {
        return new Board
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description,
            OwnerId = ownerId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    public void Update(string name, string? description)
    {
        Name = name;
        Description = description;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateCanvas(string? canvasData)
    {
        CanvasData = canvasData;
        UpdatedAt = DateTime.UtcNow;
    }
}
