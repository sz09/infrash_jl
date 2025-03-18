using IdentityModel;
using IdentityModel.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace JobLogic.OAuth.Services
{
    public interface IISAuthentication
    {
        string GenerateBasicAuthorizationKey(string apiName, string apiSecret);
        Task<TokenResponse> RequestClientTokenAsync();
        Task<TokenResponse> RequestTokenAsync(string username, string password);
        Task<TokenResponse> RequestTokenAsync(string username, string password, string scope);
        Task<LoginTrackingResult> LoginTrackingAsync(Guid userId);

        Task<RegisterResultModel> RegisterUserAsync(string firstName, string lastName, string email, string password, string[] roles);
        Task<RegisterResultModel> EditUserAsync(string id, string firstName, string lastName, string[] roles);
        Task<PasswordResultModel> ResetPasswordAsync(string email, string password, string code);
        Task<PasswordResultModel> ChangePasswordAsync(string email, string newPassword, string oldPassword);
        Task<PasswordResultModel> ForgotPasswordAsync(string email);

        Task<RoleResultModel> GetRolesAsync();
        Task<UserResultModel> GetUsersAsync(string keySearch,string role, bool isActiveUserOnly = true);
        /// <summary>
        /// Get User detail by : 
        /// 1. ID
        /// 2. Email
        /// </summary>
        /// <param name="key">Id or Email</param>
        /// <returns>UserItemModel</returns>
        Task<UserItemModel> GetUserAsync(string key);

        Task<UserLockingStatusResultModel> UpdateUserLockingStatusAsync(string userId,bool isLock = true);

    }
    public class ISAuthentication : IISAuthentication
    {
        private static DiscoveryDocumentResponse _disco;
        private string _baseUrl;
        private string _clientId;
        private string _clientSecrect;
        private string _scope;
        private string _apiName;
        private string _apiSecrect;

        private const string RegisterApi = "/api/users/Register";
        private const string EditUserApi = "/api/users/EditUser";
        private const string ForgotPasswordApi = "/api/users/ForgotPassword";
        private const string ResetPasswordApi = "/api/users/ResetPassword";
        private const string ChangePasswordApi = "/api/users/ChangePassword";
        private const string LoginTrackingApi = "/api/users/LoginTracking";
        private const string GetUsersApi = "/api/users/GetUsers";
        private const string GetUserApi = "/api/users/GetUser";
        private const string GetRolesApi = "/api/roles/GetRoles";
        private const string UpdateUserLockingStatusApi = "/api/users/UpdateUserLockingStatus";

        private readonly IHttpClientFactory _httpClientFactory;

        public ISAuthentication(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public ISAuthentication(ISAuthenticationOptions option, IHttpClientFactory httpClientFactory)
        {
            _baseUrl = option.BaseUrl;
            _clientId = option.ClientId;
            _clientSecrect = option.ClientSecrect;
            _scope = option.Scope;
            _apiName = option.ApiName;
            _apiSecrect = option.ApiSecrect;
            _httpClientFactory = httpClientFactory;
        }

        public string GenerateBasicAuthorizationKey(string apiName, string apiSecret)
        {
            string keyFormat = string.Format("{0}:{1}", apiName, apiSecret);
            return Base64Encode(keyFormat);
        }

        public async Task<TokenResponse> RequestClientTokenAsync()
        {
            var client = _httpClientFactory.CreateClient();

            var disco = await GetDiscoveryAsync();

            var response = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = _clientId,
                ClientSecret = _clientSecrect,
                Scope = _scope
            });

            return response;
        }
        public Task<TokenResponse> RequestTokenAsync(string username, string password)
        {
            return RequestTokenAsync(username, password, _scope);
        }
        public async Task<TokenResponse> RequestTokenAsync(string username, string password, string scope)
        {
            var client = _httpClientFactory.CreateClient();

            var disco = await GetDiscoveryAsync();
            var response = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = _clientId,
                ClientSecret = _clientSecrect,
                UserName = username,
                Password = password,
                GrantType = "password",
                Scope = $"{scope} {OidcConstants.StandardScopes.OpenId} {OidcConstants.StandardScopes.Profile} {OidcConstants.StandardScopes.Email}"
            });
            return response;
        }


        public async Task<LoginTrackingResult> LoginTrackingAsync(Guid userId)
        {
            var result = new LoginTrackingResult() { IsSuccess = true, IsExpiredPassword = false };
            var jsonContent = JsonConvert.SerializeObject(new
            {
                UserId = userId.ToString(),
            });
            var token = GenerateBasicAuthorizationKey(_apiName, _apiSecrect);
            var url = _baseUrl + LoginTrackingApi;
            var httpResponse = await PostJsonToApiAsync(token, url, jsonContent);
            if (!httpResponse.IsSuccessStatusCode)
            {
                if (httpResponse.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    result.IsExpiredPassword = true;
                }
                else
                {
                    result.IsSuccess = false;
                    result.ErrorMessage = await httpResponse.Content.ReadAsStringAsync();
                }
            }
            return result;
        }

        public async Task<PasswordResultModel> ForgotPasswordAsync(string email)
        {
            var result = new PasswordResultModel() { IsSuccess = true };
            string url = _baseUrl + ForgotPasswordApi;
            string jsonContent = JsonConvert.SerializeObject(new { Email = email });
            var token = GenerateBasicAuthorizationKey(_apiName, _apiSecrect);
            var httpResponse = await PostJsonToApiAsync(token, url, jsonContent);
            if (!httpResponse.IsSuccessStatusCode)
            {
                result.IsSuccess = false;
                result.ErrorMessage = await httpResponse.Content.ReadAsStringAsync();
                return result;
            }
            result.Token = JsonConvert.DeserializeObject<string>(await httpResponse.Content.ReadAsStringAsync());
            return result;
        }

        public async Task<PasswordResultModel> ResetPasswordAsync(string email, string password, string code)
        {
            var result = new PasswordResultModel() { IsSuccess = true };
            var url = _baseUrl + ResetPasswordApi;
            var token = await RequestClientTokenAsync();
            var jsonContent = JsonConvert.SerializeObject(new
            {
                Email = email,
                Code = code,
                Password = password
            });
            var resetResult = await PostJsonToApiAsyncWithAuthorizeAsync(token.AccessToken, url, jsonContent);
            if (!resetResult.IsSuccessStatusCode)
            {
                result.IsSuccess = false;
                result.ErrorMessage = await resetResult.Content.ReadAsStringAsync();
            }
            return result;
        }

        public async Task<PasswordResultModel> ChangePasswordAsync(string email, string newPassword, string oldPassword)
        {
            var result = new PasswordResultModel() { IsSuccess = true };
            var url = _baseUrl + ChangePasswordApi;
            var jsonContent = JsonConvert.SerializeObject(new
            {
                Email = email,
                OldPassword = oldPassword,
                NewPassword = newPassword
            });
            var token = await RequestClientTokenAsync();
            var changeResult = await PostJsonToApiAsyncWithAuthorizeAsync(token.AccessToken, url, jsonContent);
            if (!changeResult.IsSuccessStatusCode)
            {
                result.IsSuccess = false;
                result.ErrorMessage = await changeResult.Content.ReadAsStringAsync();
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="roles"></param>
        /// <returns></returns>

        public async Task<RegisterResultModel> RegisterUserAsync(string firstName, string lastName, string email, string password, string[] roles)
        {
            var result = new RegisterResultModel() { IsSuccess = true };
            var registerUrl = _baseUrl + RegisterApi;
            var jsonContent = JsonConvert.SerializeObject(new
            {
                FirstName = firstName,
                Lastname = lastName,
                Email = email,
                Password = password,
                UserRoles = roles
            });
            var token = GenerateBasicAuthorizationKey(_apiName, _apiSecrect);
            var registrationResult = await PostJsonToApiAsync(token, registerUrl, jsonContent);
            if (!registrationResult.IsSuccessStatusCode)
            {
                result.IsSuccess = false;
                result.ErrorMessage = await registrationResult.Content.ReadAsStringAsync();
            }
            return result;
        }
        public async Task<RegisterResultModel> EditUserAsync(string id, string firstName, string lastName, string[] roles)
        {
            var result = new RegisterResultModel() { IsSuccess = true };
            var registerUrl = _baseUrl + EditUserApi;
            var jsonContent = JsonConvert.SerializeObject(new
            {
                Id = id,
                FirstName = firstName,
                Lastname = lastName,
                UserRoles = roles
            });
            var token = GenerateBasicAuthorizationKey(_apiName, _apiSecrect);
            var registrationResult = await PostJsonToApiAsync(token, registerUrl, jsonContent);
            if (!registrationResult.IsSuccessStatusCode)
            {
                result.IsSuccess = false;
                result.ErrorMessage = await registrationResult.Content.ReadAsStringAsync();
            }
            return result;
        }
        public async Task<RoleResultModel> GetRolesAsync()
        {
            var result = new RoleResultModel() { IsSuccess = true };
            var url = _baseUrl + GetRolesApi;

            var token = GenerateBasicAuthorizationKey(_apiName, _apiSecrect);
            var getRoleResult = await PostJsonToApiAsync(token, url, string.Empty);
            if (!getRoleResult.IsSuccessStatusCode)
            {
                result.IsSuccess = false;
                result.ErrorMessage = await getRoleResult.Content.ReadAsStringAsync();
            }
            else
            {
                result.Roles = JsonConvert.DeserializeObject<List<RoleItemModel>>(await getRoleResult.Content.ReadAsStringAsync());
            }
            return result;
        }

        public async Task<UserResultModel> GetUsersAsync(bool isActiveUserOnly = true)
        {
            var result = new UserResultModel() { IsSuccess = true };
            var url = _baseUrl + GetUsersApi;
            var jsonContent = JsonConvert.SerializeObject(new
            {
                IsActiveUserOnly = isActiveUserOnly
            });

            var token = await RequestClientTokenAsync();
            var usersResult = await PostJsonToApiAsyncWithAuthorizeAsync(token.AccessToken, url, jsonContent);
            if (!usersResult.IsSuccessStatusCode)
            {
                result.IsSuccess = false;
                result.ErrorMessage = await usersResult.Content.ReadAsStringAsync();
            }
            else
            {
                result.Users = JsonConvert.DeserializeObject<List<UserItemModel>>(await usersResult.Content.ReadAsStringAsync());
            }
            return result;
        }

        public async Task<UserResultModel> GetUsersAsync(string keySearch, string role, bool isActiveUserOnly = true)
        {
            var result = new UserResultModel() { IsSuccess = true };
            var url = _baseUrl + GetUsersApi;
            var jsonContent = JsonConvert.SerializeObject(new
            {
                IsActiveUserOnly = isActiveUserOnly,
                Role = role,
                KeySearch = keySearch
            });

            var token = await RequestClientTokenAsync();
            var usersResult = await PostJsonToApiAsyncWithAuthorizeAsync(token.AccessToken, url, jsonContent);
            if (!usersResult.IsSuccessStatusCode)
            {
                result.IsSuccess = false;
                result.ErrorMessage = await usersResult.Content.ReadAsStringAsync();
            }
            else
            {
                result.Users = JsonConvert.DeserializeObject<List<UserItemModel>>(await usersResult.Content.ReadAsStringAsync());
            }
            return result;
        }
        public async Task<UserItemModel> GetUserAsync(string key)
        {
            var result = new UserItemModel() { IsSuccess = true };
            var url = _baseUrl + GetUserApi;
            var jsonContent = JsonConvert.SerializeObject(new
            {
                Key = key
            });

            var token = await RequestClientTokenAsync();
            var usersResult = await PostJsonToApiAsyncWithAuthorizeAsync(token.AccessToken, url, jsonContent);
            if (!usersResult.IsSuccessStatusCode)
            {
                result.IsSuccess = false;
                result.ErrorMessage = await usersResult.Content.ReadAsStringAsync();
            }
            else
            {
                result = JsonConvert.DeserializeObject<UserItemModel>(await usersResult.Content.ReadAsStringAsync());
                result.IsSuccess = true;
            }
            return result;
        }

        public async Task<UserLockingStatusResultModel> UpdateUserLockingStatusAsync(string userId, bool isLock = true)
        {
            var result = new UserLockingStatusResultModel() { IsSuccess = true ,UserId=userId};
            var url = _baseUrl + UpdateUserLockingStatusApi;
            var jsonContent = JsonConvert.SerializeObject(new
            {
                UserId = userId,
                Lockout = isLock
            });
            var token = GenerateBasicAuthorizationKey(_apiName, _apiSecrect);
            var updateResult = await PostJsonToApiAsync(token, url, jsonContent);
            if (!updateResult.IsSuccessStatusCode)
            {
                result.IsSuccess = false;
                result.ErrorMessage = await updateResult.Content.ReadAsStringAsync();
            }
            return result;
        }

        #region PRIVATE

        private string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }
        private async Task<HttpResponseMessage> PostJsonToApiAsync(string token, string requestUri, string jsonContent, string authorizeType = "Basic")
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Add("Authorization", string.Format("{0} {1}", authorizeType, token));
            }
            var stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            return await client.PostAsync(requestUri, stringContent);
        }
        private Task<HttpResponseMessage> PostJsonToApiAsyncWithAuthorizeAsync(string token, string requestUri, string jsonContent)
        {
            return PostJsonToApiAsync(token, requestUri, jsonContent, "Bearer");
        }

        private async Task<DiscoveryDocumentResponse> GetDiscoveryAsync()
        {
            var client = _httpClientFactory.CreateClient();
            _disco = await client.GetDiscoveryDocumentAsync(_baseUrl);
            if (_disco.IsError) throw new Exception(_disco.Error);

            return _disco;
        }

      







        #endregion
    }
    public class ISAuthenticationOptions
    {
        public string BaseUrl { get; set; }
        public string ClientId { get; set; }
        public string ClientSecrect { get; set; }
        public string ApiName { get; set; }
        public string ApiSecrect { get; set; }
        public string Scope { get; set; }
    }
}

