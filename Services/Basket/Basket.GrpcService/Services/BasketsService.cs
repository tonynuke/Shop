using System.Threading.Tasks;
using Basket.Domain;
using Basket.GrpcClient;
using Basket.Services.Dto;
using Grpc.Core;
using Mapster;
using Microsoft.Extensions.Logging;

namespace Basket.GrpcService.Services
{
    public class BasketsService : GrpcClient.Basket.BasketBase
    {
        private readonly Basket.Services.BasketsService _basketService;
        private readonly ILogger<BasketsService> _logger;

        public BasketsService(Basket.Services.BasketsService basketService, ILogger<BasketsService> logger)
        {
            _basketService = basketService;
            _logger = logger;
        }

        public override async Task<BasketReply> GetOrCreateBasket(GetOrCreateBasketRequest request, ServerCallContext context)
        {
            var userId = context.GetUserId();
            var basket = await _basketService.GetOrCreateBasket(userId);
            return basket.Adapt<BasketReply>();
        }

        public override async Task<BasketReply> UpdateBasket(UpdateBasketRequest request, ServerCallContext context)
        {
            var userId = context.GetUserId();
            var dto = new UpdateBasketDto(userId, request.Items.Adapt<BasketItem[]>());
            var basket = await _basketService.UpdateBasket(dto);
            return basket.Adapt<BasketReply>();
        }

        public override async Task<BasketReply> ClearBasket(ClearBasketRequest request, ServerCallContext context)
        {
            var userId = context.GetUserId();
            var basket = await _basketService.ClearBasket(userId);
            return basket.Adapt<BasketReply>();
        }
    }
}
