using ComputerStore.Service.DTOs;

namespace ComputerStore.Service.Interfaces
{
    public interface IStockImportService
    {
        Task ImportAsync(List<ImportStockDto> stockItems);
    }
}