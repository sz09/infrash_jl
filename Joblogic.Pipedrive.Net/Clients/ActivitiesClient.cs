﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pipedrive.Helpers;

namespace Pipedrive
{
    /// <summary>
    /// A client for Pipedrive's Activity API.
    /// </summary>
    /// <remarks>
    /// See the <a href="https://developers.pipedrive.com/docs/api/v1/Activities">Activity API documentation</a> for more information.
    public class ActivitiesClient : ApiClient, IActivitiesClient
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ActivitiesClient"/> class.
        /// </summary>
        /// <param name="apiConnection">An API connection</param>
        public ActivitiesClient(IApiConnection apiConnection) : base(apiConnection)
        {
        }

        public Task<IReadOnlyList<Activity>> GetAll(ActivityFilters filters)
        {
            Ensure.ArgumentNotNull(filters, nameof(filters));

            var parameters = filters.Parameters;
            parameters.Add("user_id", "0");
            var options = new ApiOptions
            {
                StartPage = filters.StartPage,
                PageCount = filters.PageCount,
                PageSize = filters.PageSize
            };

            return ApiConnection.GetAll<Activity>(ApiUrls.Activities(), parameters, options);
        }

        public Task<IReadOnlyList<Activity>> GetAllForCurrent(ActivityFilters filters)
        {
            Ensure.ArgumentNotNull(filters, nameof(filters));

            var parameters = filters.Parameters;
            var options = new ApiOptions
            {
                StartPage = filters.StartPage,
                PageCount = filters.PageCount,
                PageSize = filters.PageSize
            };

            return ApiConnection.GetAll<Activity>(ApiUrls.Activities(), parameters, options);
        }

        public Task<IReadOnlyList<Activity>> GetAllForUserId(long userId, ActivityFilters filters)
        {
            Ensure.ArgumentNotNull(filters, nameof(filters));

            var parameters = filters.Parameters;
            parameters.Add("user_id", userId.ToString());
            var options = new ApiOptions
            {
                StartPage = filters.StartPage,
                PageCount = filters.PageCount,
                PageSize = filters.PageSize
            };

            return ApiConnection.GetAll<Activity>(ApiUrls.Activities(), parameters, options);
        }

        public Task<Activity> Get(long id)
        {
            return ApiConnection.Get<Activity>(ApiUrls.Activity(id));
        }

        public Task<Activity> Create(NewActivity data)
        {
            Ensure.ArgumentNotNull(data, nameof(data));

            return ApiConnection.Post<Activity>(ApiUrls.Activities(), data);
        }

        public Task<Activity> Edit(long id, ActivityUpdate data)
        {
            Ensure.ArgumentNotNull(data, nameof(data));

            return ApiConnection.Put<Activity>(ApiUrls.Activity(id), data);
        }

        public Task Delete(long id)
        {
            return ApiConnection.Delete(ApiUrls.Activity(id));
        }

        public Task Delete(List<long> ids)
        {
            Ensure.ArgumentNotNull(ids, nameof(ids));
            Ensure.GreaterThanZero(ids.Count, nameof(ids));

            return ApiConnection.Delete(new Uri($"{ApiUrls.Activities()}?ids={string.Join(",", ids)}", UriKind.Relative));
        }
    }
}
