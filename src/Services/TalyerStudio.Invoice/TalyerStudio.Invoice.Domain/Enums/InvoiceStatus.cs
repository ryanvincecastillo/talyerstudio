namespace TalyerStudio.Invoice.Domain.Enums;

public enum InvoiceStatus
{
    DRAFT = 0,
    PENDING = 1,
    PAID = 2,
    PARTIALLY_PAID = 3,
    OVERDUE = 4,
    CANCELLED = 5,
    VOID = 6
}