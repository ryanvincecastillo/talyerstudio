namespace TalyerStudio.JobOrder.Application.DTOs;

public class AssignMechanicDto
{
    public Guid[] MechanicIds { get; set; } = Array.Empty<Guid>();
}