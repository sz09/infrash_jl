﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pipedrive
{
    /// <summary>
    /// A client for Pipedrive's Activity Type API.
    /// </summary>
    /// <remarks>
    /// See the <a href="https://developers.pipedrive.com/docs/api/v1/ActivityTypes">Activity Type API documentation</a> for more information.
    public interface IActivityTypesClient
    {
        Task<IReadOnlyList<ActivityType>> GetAll();

        Task<ActivityType> Create(NewActivityType data);

        Task<ActivityType> Edit(long id, ActivityTypeUpdate data);

        Task Delete(long id);

        Task Delete(List<long> ids);
    }
}
