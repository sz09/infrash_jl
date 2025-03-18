using System;
using System.Collections.Generic;

namespace JobLogic.OAuth.Services
{
    public class UserResultBaseModel
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
    }
    public class UserResultModel : UserResultBaseModel
    {
        public List<UserItemModel> Users { get; set; }
    }
    public class UserItemModel : UserResultBaseModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public DateTime RegisterDate { get; set; }
        public bool IsDisabled { get; set; }
        public bool IsDeleted { get; set; }
        public string Role { get; set; }
        public DateTime LastPasswordChangeDate { get; set; }
        public bool IsSetUp => DateTime.Compare(LastPasswordChangeDate, RegisterDate) == 0;
    }
}
