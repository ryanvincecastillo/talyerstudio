namespace TalyerStudio.JobOrder.Application.DTOs;

public class UpdateJobOrderStatusDto
{
    public string Status { get; set; } = string.Empty;
    public string? Notes { get; set; }
}