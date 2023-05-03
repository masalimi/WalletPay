using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Plugin.Payments.RayanWallet.Models;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Payments.RayanWallet.Models
{
    public class WalletCustomerModel : BaseNopEntityModel
    {
        public WalletCustomerModel()
        {
            SelectedWalletCustomerIds = new List<int>();
            AvailableWalletCustomer = new List<SelectListItem>();
        }

        [NopResourceDisplayName("Plugins.Payments.RayanWallet.WalletCustomerList")]
        public IList<int> SelectedWalletCustomerIds { get; set; }
        public IList<SelectListItem> AvailableWalletCustomer { get; set; }

        [NopResourceDisplayName("Plugins.Payments.RayanWallet.UserName")]
        public string UserName { get; set; }
        public string UserName_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Payments.RayanWallet.Amount")]
        public decimal Amount { get; set; }
        public bool Amount_OverrideForStore { get; set; }


        [NopResourceDisplayName("Plugins.Payments.RayanWallet.Active")]
        public bool Active { get; set; }
        public bool Active_OverrideForStore { get; set; }


        [NopResourceDisplayName("Plugins.Payments.RayanWallet.SourceId")]
        public int SourceId { get; set; }
        public bool SourceId_OverrideForStore { get; set; }
    }
}
public class WalletCustomerModelList : BasePagedListModel<WalletCustomerModel>
{

}
