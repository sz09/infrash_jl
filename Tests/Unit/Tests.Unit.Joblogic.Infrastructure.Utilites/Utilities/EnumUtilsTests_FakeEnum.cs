using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tests.Unit.Joblogic.Infrastructure.Utilites
{
    public enum EnumUtilsTests_FakeEnum
    {
        NoDescription,

        [Description("Item 1 - Description")]
        [Display(Name = "Item 1 - Display Name")]
        Item1,

        [Description("Item 2 - Description")]
        [Display(Name = "Item 2 - Display Name")]
        Item2,

        [Description("Item 3 - Description")]
        [Display(Name = "Item 3 - Display Name")]
        Item3
    }
}
