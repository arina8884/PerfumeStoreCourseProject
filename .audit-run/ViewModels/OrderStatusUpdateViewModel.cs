using System.ComponentModel.DataAnnotations;

namespace PerfumeStore.MVCC.ViewModels;

public class OrderStatusUpdateViewModel
{
    public int OrderId { get; set; }

    public string CurrentStatus { get; set; } = string.Empty;

    [Required(ErrorMessage = "Выберите статус.")]
    public string Status { get; set; } = string.Empty;

    public IReadOnlyList<string> AvailableStatuses { get; set; } = Array.Empty<string>();
}
