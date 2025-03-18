using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace JobLogic.Infrastructure.DefaultHttpClient
{
    public interface IHttpClient
    {
        //
        // Summary:
        //     Send a DELETE request to the specified Uri with a cancellation token as an asynchronous
        //     operation.
        //
        // Parameters:
        //   requestUri:
        //     The Uri the request is sent to.
        //
        //   cancellationToken:
        //     A cancellation token that can be used by other objects or threads to receive
        //     notice of cancellation.
        //
        // Returns:
        //     The task object representing the asynchronous operation.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     The requestUri was null.
        //
        //   T:System.InvalidOperationException:
        //     The request message was already sent by the System.Net.Http.HttpClient instance.
        //
        //   T:System.Net.Http.HttpRequestException:
        //     The request failed due to an underlying issue such as network connectivity, DNS
        //     failure, server certificate validation or timeout.
        Task<HttpResponseMessage> DeleteAsync(Uri requestUri, CancellationToken cancellationToken);
        //
        // Summary:
        //     Send a DELETE request to the specified Uri with a cancellation token as an asynchronous
        //     operation.
        //
        // Parameters:
        //   requestUri:
        //     The Uri the request is sent to.
        //
        //   cancellationToken:
        //     A cancellation token that can be used by other objects or threads to receive
        //     notice of cancellation.
        //
        // Returns:
        //     The task object representing the asynchronous operation.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     The requestUri was null.
        //
        //   T:System.InvalidOperationException:
        //     The request message was already sent by the System.Net.Http.HttpClient instance.
        //
        //   T:System.Net.Http.HttpRequestException:
        //     The request failed due to an underlying issue such as network connectivity, DNS
        //     failure, server certificate validation or timeout.
        Task<HttpResponseMessage> DeleteAsync(string requestUri, CancellationToken cancellationToken);
        //
        // Summary:
        //     Send a DELETE request to the specified Uri as an asynchronous operation.
        //
        // Parameters:
        //   requestUri:
        //     The Uri the request is sent to.
        //
        // Returns:
        //     The task object representing the asynchronous operation.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     The requestUri was null.
        //
        //   T:System.InvalidOperationException:
        //     The request message was already sent by the System.Net.Http.HttpClient instance.
        //
        //   T:System.Net.Http.HttpRequestException:
        //     The request failed due to an underlying issue such as network connectivity, DNS
        //     failure, server certificate validation or timeout.
        Task<HttpResponseMessage> DeleteAsync(string requestUri);
        //
        // Summary:
        //     Send a DELETE request to the specified Uri as an asynchronous operation.
        //
        // Parameters:
        //   requestUri:
        //     The Uri the request is sent to.
        //
        // Returns:
        //     The task object representing the asynchronous operation.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     The requestUri was null.
        //
        //   T:System.InvalidOperationException:
        //     The request message was already sent by the System.Net.Http.HttpClient instance.
        //
        //   T:System.Net.Http.HttpRequestException:
        //     The request failed due to an underlying issue such as network connectivity, DNS
        //     failure, server certificate validation or timeout.
        Task<HttpResponseMessage> DeleteAsync(Uri requestUri);
        //
        // Summary:
        //     Send a GET request to the specified Uri as an asynchronous operation.
        //
        // Parameters:
        //   requestUri:
        //     The Uri the request is sent to.
        //
        // Returns:
        //     The task object representing the asynchronous operation.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     The requestUri was null.
        //
        //   T:System.Net.Http.HttpRequestException:
        //     The request failed due to an underlying issue such as network connectivity, DNS
        //     failure, server certificate validation or timeout.
        Task<HttpResponseMessage> GetAsync(string requestUri);
        //
        // Summary:
        //     Send a GET request to the specified Uri with an HTTP completion option as an
        //     asynchronous operation.
        //
        // Parameters:
        //   requestUri:
        //     The Uri the request is sent to.
        //
        //   completionOption:
        //     An HTTP completion option value that indicates when the operation should be considered
        //     completed.
        //
        // Returns:
        //     The task object representing the asynchronous operation.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     The requestUri was null.
        //
        //   T:System.Net.Http.HttpRequestException:
        //     The request failed due to an underlying issue such as network connectivity, DNS
        //     failure, server certificate validation or timeout.
        Task<HttpResponseMessage> GetAsync(string requestUri, HttpCompletionOption completionOption);
        //
        // Summary:
        //     Send a GET request to the specified Uri with an HTTP completion option and a
        //     cancellation token as an asynchronous operation.
        //
        // Parameters:
        //   requestUri:
        //     The Uri the request is sent to.
        //
        //   completionOption:
        //     An HTTP completion option value that indicates when the operation should be considered
        //     completed.
        //
        //   cancellationToken:
        //     A cancellation token that can be used by other objects or threads to receive
        //     notice of cancellation.
        //
        // Returns:
        //     The task object representing the asynchronous operation.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     The requestUri was null.
        //
        //   T:System.Net.Http.HttpRequestException:
        //     The request failed due to an underlying issue such as network connectivity, DNS
        //     failure, server certificate validation or timeout.
        Task<HttpResponseMessage> GetAsync(string requestUri, HttpCompletionOption completionOption, CancellationToken cancellationToken);
        //
        // Summary:
        //     Send a GET request to the specified Uri with a cancellation token as an asynchronous
        //     operation.
        //
        // Parameters:
        //   requestUri:
        //     The Uri the request is sent to.
        //
        //   cancellationToken:
        //     A cancellation token that can be used by other objects or threads to receive
        //     notice of cancellation.
        //
        // Returns:
        //     The task object representing the asynchronous operation.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     The requestUri was null.
        //
        //   T:System.Net.Http.HttpRequestException:
        //     The request failed due to an underlying issue such as network connectivity, DNS
        //     failure, server certificate validation or timeout.
        Task<HttpResponseMessage> GetAsync(string requestUri, CancellationToken cancellationToken);
        //
        // Summary:
        //     Send a GET request to the specified Uri as an asynchronous operation.
        //
        // Parameters:
        //   requestUri:
        //     The Uri the request is sent to.
        //
        // Returns:
        //     The task object representing the asynchronous operation.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     The requestUri was null.
        //
        //   T:System.Net.Http.HttpRequestException:
        //     The request failed due to an underlying issue such as network connectivity, DNS
        //     failure, server certificate validation or timeout.
        Task<HttpResponseMessage> GetAsync(Uri requestUri);
        //
        // Summary:
        //     Send a GET request to the specified Uri with an HTTP completion option as an
        //     asynchronous operation.
        //
        // Parameters:
        //   requestUri:
        //     The Uri the request is sent to.
        //
        //   completionOption:
        //     An HTTP completion option value that indicates when the operation should be considered
        //     completed.
        //
        // Returns:
        //     The task object representing the asynchronous operation.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     The requestUri was null.
        //
        //   T:System.Net.Http.HttpRequestException:
        //     The request failed due to an underlying issue such as network connectivity, DNS
        //     failure, server certificate validation or timeout.
        Task<HttpResponseMessage> GetAsync(Uri requestUri, HttpCompletionOption completionOption);
        //
        // Summary:
        //     Send a GET request to the specified Uri with an HTTP completion option and a
        //     cancellation token as an asynchronous operation.
        //
        // Parameters:
        //   requestUri:
        //     The Uri the request is sent to.
        //
        //   completionOption:
        //     An HTTP completion option value that indicates when the operation should be considered
        //     completed.
        //
        //   cancellationToken:
        //     A cancellation token that can be used by other objects or threads to receive
        //     notice of cancellation.
        //
        // Returns:
        //     The task object representing the asynchronous operation.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     The requestUri was null.
        //
        //   T:System.Net.Http.HttpRequestException:
        //     The request failed due to an underlying issue such as network connectivity, DNS
        //     failure, server certificate validation or timeout.
        Task<HttpResponseMessage> GetAsync(Uri requestUri, HttpCompletionOption completionOption, CancellationToken cancellationToken);
        //
        // Summary:
        //     Send a GET request to the specified Uri with a cancellation token as an asynchronous
        //     operation.
        //
        // Parameters:
        //   requestUri:
        //     The Uri the request is sent to.
        //
        //   cancellationToken:
        //     A cancellation token that can be used by other objects or threads to receive
        //     notice of cancellation.
        //
        // Returns:
        //     The task object representing the asynchronous operation.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     The requestUri was null.
        //
        //   T:System.Net.Http.HttpRequestException:
        //     The request failed due to an underlying issue such as network connectivity, DNS
        //     failure, server certificate validation or timeout.
        Task<HttpResponseMessage> GetAsync(Uri requestUri, CancellationToken cancellationToken);
        //
        // Summary:
        //     Send a GET request to the specified Uri and return the response body as a byte
        //     array in an asynchronous operation.
        //
        // Parameters:
        //   requestUri:
        //     The Uri the request is sent to.
        //
        // Returns:
        //     The task object representing the asynchronous operation.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     The requestUri was null.
        //
        //   T:System.Net.Http.HttpRequestException:
        //     The request failed due to an underlying issue such as network connectivity, DNS
        //     failure, server certificate validation or timeout.
        Task<byte[]> GetByteArrayAsync(string requestUri);
        //
        // Summary:
        //     Send a GET request to the specified Uri and return the response body as a byte
        //     array in an asynchronous operation.
        //
        // Parameters:
        //   requestUri:
        //     The Uri the request is sent to.
        //
        // Returns:
        //     The task object representing the asynchronous operation.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     The requestUri was null.
        //
        //   T:System.Net.Http.HttpRequestException:
        //     The request failed due to an underlying issue such as network connectivity, DNS
        //     failure, server certificate validation or timeout.
        Task<byte[]> GetByteArrayAsync(Uri requestUri);
        //
        // Summary:
        //     Send a GET request to the specified Uri and return the response body as a stream
        //     in an asynchronous operation.
        //
        // Parameters:
        //   requestUri:
        //     The Uri the request is sent to.
        //
        // Returns:
        //     The task object representing the asynchronous operation.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     The requestUri was null.
        //
        //   T:System.Net.Http.HttpRequestException:
        //     The request failed due to an underlying issue such as network connectivity, DNS
        //     failure, server certificate validation or timeout.
        Task<Stream> GetStreamAsync(string requestUri);
        //
        // Summary:
        //     Send a GET request to the specified Uri and return the response body as a stream
        //     in an asynchronous operation.
        //
        // Parameters:
        //   requestUri:
        //     The Uri the request is sent to.
        //
        // Returns:
        //     The task object representing the asynchronous operation.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     The requestUri was null.
        //
        //   T:System.Net.Http.HttpRequestException:
        //     The request failed due to an underlying issue such as network connectivity, DNS
        //     failure, server certificate validation or timeout.
        Task<Stream> GetStreamAsync(Uri requestUri);
        //
        // Summary:
        //     Send a GET request to the specified Uri and return the response body as a string
        //     in an asynchronous operation.
        //
        // Parameters:
        //   requestUri:
        //     The Uri the request is sent to.
        //
        // Returns:
        //     The task object representing the asynchronous operation.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     The requestUri was null.
        //
        //   T:System.Net.Http.HttpRequestException:
        //     The request failed due to an underlying issue such as network connectivity, DNS
        //     failure, server certificate validation or timeout.
        Task<string> GetStringAsync(string requestUri);
        //
        // Summary:
        //     Send a GET request to the specified Uri and return the response body as a string
        //     in an asynchronous operation.
        //
        // Parameters:
        //   requestUri:
        //     The Uri the request is sent to.
        //
        // Returns:
        //     The task object representing the asynchronous operation.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     The requestUri was null.
        //
        //   T:System.Net.Http.HttpRequestException:
        //     The request failed due to an underlying issue such as network connectivity, DNS
        //     failure, server certificate validation or timeout.
        Task<string> GetStringAsync(Uri requestUri);
        //
        // Summary:
        //     Send a POST request to the specified Uri as an asynchronous operation.
        //
        // Parameters:
        //   requestUri:
        //     The Uri the request is sent to.
        //
        //   content:
        //     The HTTP request content sent to the server.
        //
        // Returns:
        //     The task object representing the asynchronous operation.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     The requestUri was null.
        //
        //   T:System.Net.Http.HttpRequestException:
        //     The request failed due to an underlying issue such as network connectivity, DNS
        //     failure, server certificate validation or timeout.
        Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content);
        //
        // Summary:
        //     Send a POST request with a cancellation token as an asynchronous operation.
        //
        // Parameters:
        //   requestUri:
        //     The Uri the request is sent to.
        //
        //   content:
        //     The HTTP request content sent to the server.
        //
        //   cancellationToken:
        //     A cancellation token that can be used by other objects or threads to receive
        //     notice of cancellation.
        //
        // Returns:
        //     The task object representing the asynchronous operation.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     The requestUri was null.
        //
        //   T:System.Net.Http.HttpRequestException:
        //     The request failed due to an underlying issue such as network connectivity, DNS
        //     failure, server certificate validation or timeout.
        Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content, CancellationToken cancellationToken);
        //
        // Summary:
        //     Send a POST request to the specified Uri as an asynchronous operation.
        //
        // Parameters:
        //   requestUri:
        //     The Uri the request is sent to.
        //
        //   content:
        //     The HTTP request content sent to the server.
        //
        // Returns:
        //     The task object representing the asynchronous operation.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     The requestUri was null.
        //
        //   T:System.Net.Http.HttpRequestException:
        //     The request failed due to an underlying issue such as network connectivity, DNS
        //     failure, server certificate validation or timeout.
        Task<HttpResponseMessage> PostAsync(Uri requestUri, HttpContent content);
        //
        // Summary:
        //     Send a POST request with a cancellation token as an asynchronous operation.
        //
        // Parameters:
        //   requestUri:
        //     The Uri the request is sent to.
        //
        //   content:
        //     The HTTP request content sent to the server.
        //
        //   cancellationToken:
        //     A cancellation token that can be used by other objects or threads to receive
        //     notice of cancellation.
        //
        // Returns:
        //     The task object representing the asynchronous operation.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     The requestUri was null.
        //
        //   T:System.Net.Http.HttpRequestException:
        //     The request failed due to an underlying issue such as network connectivity, DNS
        //     failure, server certificate validation or timeout.
        Task<HttpResponseMessage> PostAsync(Uri requestUri, HttpContent content, CancellationToken cancellationToken);
        //
        // Summary:
        //     Send a PUT request to the specified Uri as an asynchronous operation.
        //
        // Parameters:
        //   requestUri:
        //     The Uri the request is sent to.
        //
        //   content:
        //     The HTTP request content sent to the server.
        //
        // Returns:
        //     The task object representing the asynchronous operation.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     The requestUri was null.
        //
        //   T:System.Net.Http.HttpRequestException:
        //     The request failed due to an underlying issue such as network connectivity, DNS
        //     failure, server certificate validation or timeout.
        Task<HttpResponseMessage> PutAsync(string requestUri, HttpContent content);
        //
        // Summary:
        //     Send a PUT request with a cancellation token as an asynchronous operation.
        //
        // Parameters:
        //   requestUri:
        //     The Uri the request is sent to.
        //
        //   content:
        //     The HTTP request content sent to the server.
        //
        //   cancellationToken:
        //     A cancellation token that can be used by other objects or threads to receive
        //     notice of cancellation.
        //
        // Returns:
        //     The task object representing the asynchronous operation.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     The requestUri was null.
        //
        //   T:System.Net.Http.HttpRequestException:
        //     The request failed due to an underlying issue such as network connectivity, DNS
        //     failure, server certificate validation or timeout.
        Task<HttpResponseMessage> PutAsync(string requestUri, HttpContent content, CancellationToken cancellationToken);
        //
        // Summary:
        //     Send a PUT request to the specified Uri as an asynchronous operation.
        //
        // Parameters:
        //   requestUri:
        //     The Uri the request is sent to.
        //
        //   content:
        //     The HTTP request content sent to the server.
        //
        // Returns:
        //     The task object representing the asynchronous operation.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     The requestUri was null.
        //
        //   T:System.Net.Http.HttpRequestException:
        //     The request failed due to an underlying issue such as network connectivity, DNS
        //     failure, server certificate validation or timeout.
        Task<HttpResponseMessage> PutAsync(Uri requestUri, HttpContent content);
        //
        // Summary:
        //     Send a PUT request with a cancellation token as an asynchronous operation.
        //
        // Parameters:
        //   requestUri:
        //     The Uri the request is sent to.
        //
        //   content:
        //     The HTTP request content sent to the server.
        //
        //   cancellationToken:
        //     A cancellation token that can be used by other objects or threads to receive
        //     notice of cancellation.
        //
        // Returns:
        //     The task object representing the asynchronous operation.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     The requestUri was null.
        //
        //   T:System.Net.Http.HttpRequestException:
        //     The request failed due to an underlying issue such as network connectivity, DNS
        //     failure, server certificate validation or timeout.
        Task<HttpResponseMessage> PutAsync(Uri requestUri, HttpContent content, CancellationToken cancellationToken);
        //
        // Summary:
        //     Send an HTTP request as an asynchronous operation.
        //
        // Parameters:
        //   request:
        //     The HTTP request message to send.
        //
        // Returns:
        //     The task object representing the asynchronous operation.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     The request was null.
        //
        //   T:System.InvalidOperationException:
        //     The request message was already sent by the System.Net.Http.HttpClient instance.
        //
        //   T:System.Net.Http.HttpRequestException:
        //     The request failed due to an underlying issue such as network connectivity, DNS
        //     failure, server certificate validation or timeout.
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request);
        //
        // Summary:
        //     Send an HTTP request as an asynchronous operation.
        //
        // Parameters:
        //   request:
        //     The HTTP request message to send.
        //
        //   completionOption:
        //     When the operation should complete (as soon as a response is available or after
        //     reading the whole response content).
        //
        // Returns:
        //     The task object representing the asynchronous operation.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     The request was null.
        //
        //   T:System.InvalidOperationException:
        //     The request message was already sent by the System.Net.Http.HttpClient instance.
        //
        //   T:System.Net.Http.HttpRequestException:
        //     The request failed due to an underlying issue such as network connectivity, DNS
        //     failure, server certificate validation or timeout.
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption completionOption);
        //
        // Summary:
        //     Send an HTTP request as an asynchronous operation.
        //
        // Parameters:
        //   request:
        //     The HTTP request message to send.
        //
        //   completionOption:
        //     When the operation should complete (as soon as a response is available or after
        //     reading the whole response content).
        //
        //   cancellationToken:
        //     The cancellation token to cancel operation.
        //
        // Returns:
        //     The task object representing the asynchronous operation.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     The request was null.
        //
        //   T:System.InvalidOperationException:
        //     The request message was already sent by the System.Net.Http.HttpClient instance.
        //
        //   T:System.Net.Http.HttpRequestException:
        //     The request failed due to an underlying issue such as network connectivity, DNS
        //     failure, server certificate validation or timeout.
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption completionOption, CancellationToken cancellationToken);
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken);
    }
}
