using System.Collections.Generic;

namespace JobLogic.OAuth.Services
{
    public class RoleResultModel
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public List<RoleItemModel> Roles { get; set; }
    }
    public class RoleItemModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
