using System;
using System.Threading;
using System.Threading.Tasks;
using JobLogic.Infrastructure.RedisCache;
using JobLogic.Infrastructure.ServiceResponders; 
using Newtonsoft.Json;

namespace JobLogic.Infrastructure.GoogleMapWebService
{
    public class GoogleMapWebServiceWithCache : IGoogleMapWebService
    {
        private readonly IGoogleMapWebService _mapWebService;
        private readonly ICache _cache;
        private readonly string _emptyAddress = "Address is empty";
        private readonly string _emptyLocation = "Location is empty";
        public GoogleMapWebServiceWithCache(IGoogleMapWebService mapWebService, ICache cache)
        {
            _mapWebService = mapWebService;
            _cache = cache;
        }

        public async Task<Response<string>> GetFullAddressByLongLatAsync(double latitude, double longitude, CancellationToken cancellationToken = default)
        {
            var key = $"FullAddress_{latitude}_{longitude}";
            var addressFromCache = await _cache.GetValueAsync(key);
            if (string.IsNullOrWhiteSpace(addressFromCache))
            {
                var responseAddressFromService =
                    await _mapWebService.GetFullAddressByLongLatAsync(latitude, longitude, cancellationToken);

                await _cache.SetValueAsync(key,
                    responseAddressFromService.Success ? responseAddressFromService.Data : _emptyAddress);

                return responseAddressFromService;
            }
            if (_emptyAddress.Equals(addressFromCache))
                return ResponseFactory.ReturnWithError<string>(_emptyAddress);

            return ResponseFactory.Return(addressFromCache);
        }

        public async Task<Response<GoogleMapWebLocationResponse>> GetLongLatByAddressAsync(string address, string postcode, CancellationToken cancellationToken = default)
        {
            // handle the address only case
            if (string.IsNullOrWhiteSpace(postcode))
            {
                var responseLongLatFromService =
                    await _mapWebService.GetLongLatByAddressAsync(address, postcode, cancellationToken);
                postcode = responseLongLatFromService.Data?.PostCode;

                await _cache.SetValueAsync($"LongLat_{postcode}",
                    responseLongLatFromService.Success ? JsonConvert.SerializeObject(responseLongLatFromService.Data) : _emptyLocation);

                return responseLongLatFromService;
            }

            var key = $"LongLat_{postcode}";
            var longlatFromCache = await _cache.GetValueAsync(key);

            if (string.IsNullOrWhiteSpace(longlatFromCache))
            {
                var responseLongLatFromService =
                    await _mapWebService.GetLongLatByAddressAsync(address, postcode, cancellationToken);

                await _cache.SetValueAsync(key,
                    responseLongLatFromService.Success ? JsonConvert.SerializeObject(responseLongLatFromService.Data) : _emptyLocation);

                return responseLongLatFromService;
            }
            if (_emptyLocation.Equals(longlatFromCache))
                return ResponseFactory.ReturnWithError<GoogleMapWebLocationResponse>(_emptyLocation);

            return ResponseFactory.Return(JsonConvert.DeserializeObject<GoogleMapWebLocationResponse>(longlatFromCache));
        }
    }
}
