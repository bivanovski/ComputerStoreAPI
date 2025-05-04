using ComputerStore.Service.DTOs;

namespace ComputerStore.Service.Interfaces
{
    public interface IDiscountService
    {
        Task<BasketDiscountResultDto> CalculateDiscountAsync(List<BasketItemDto> basket);
    }
}
