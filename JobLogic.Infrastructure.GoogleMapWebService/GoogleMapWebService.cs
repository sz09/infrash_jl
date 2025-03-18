using Geocoding.Google;
using JobLogic.Infrastructure.ServiceResponders;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace JobLogic.Infrastructure.GoogleMapWebService
{ 

    public interface IGoogleMapWebService
    {
        Task<Response<string>> GetFullAddressByLongLatAsync(double latitude, double longitude, CancellationToken cancellationToken = default);

        Task<Response<GoogleMapWebLocationResponse>> GetLongLatByAddressAsync(string address, string postcode, CancellationToken cancellationToken = default);
    }
   
    public class GoogleMapWebService : IGoogleMapWebService
    {
        private readonly string _apiKey;

        public GoogleMapWebService(string apiKey)
        {
            _apiKey = apiKey;
        }

        public async Task<Response<string>> GetFullAddressByLongLatAsync(double latitude, double longitude, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                var geocoder = new GoogleGeocoder(_apiKey);
                var response = await geocoder.ReverseGeocodeAsync(new Geocoding.Location(latitude, longitude));

                if (response.Any())
                {
                    string fullAddress = response.FirstOrDefault().FormattedAddress;
                    return ResponseFactory.Return(fullAddress);
                }
                else
                {
                    return ResponseFactory.ReturnWithError<string>("Address is empty");
                }
                    
            }
            catch (Exception ex)
            {
                return ResponseFactory.ReturnWithException<string>(ex);
            }
        }

        public async Task<Response<GoogleMapWebLocationResponse>> GetLongLatByAddressAsync(string address, string postcode, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                string fullAddress = AsAddressString(address, postcode);
                var geocoder = new GoogleGeocoder(_apiKey);

                var response = await geocoder.GeocodeAsync($"{fullAddress}");
                var googleMapWebLocationResponse = new GoogleMapWebLocationResponse();
               
                if (response != null && response.Any())
                {
                    googleMapWebLocationResponse.PostCode = response.Select(a => a[GoogleAddressType.PostalCode]).First()?.LongName;
                    googleMapWebLocationResponse.Latitude = response.FirstOrDefault().Coordinates.Latitude;
                    googleMapWebLocationResponse.Longitude = response.FirstOrDefault().Coordinates.Longitude;
                }
                else
                {
                    return ResponseFactory.ReturnWithError<GoogleMapWebLocationResponse>("Location is empty");
                }

                return ResponseFactory.Return(googleMapWebLocationResponse);
            }
            catch (Exception ex)
            {
                return ResponseFactory.ReturnWithException<GoogleMapWebLocationResponse>(ex);
            }
        }

        private static string AsAddressString(params string[] strs)
        {
            if (strs == null || strs.Length == 0) return string.Empty;

            string separator = ", ";
            return string.Join(separator, strs.Where(m => !string.IsNullOrWhiteSpace(m)));
        }
    }
}