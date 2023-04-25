using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Data;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Stores;
using Nop.Plugin.Payments.RayanWallet.Domain.Data;
using Nop.Plugin.Payments.RayanWallet.Helper;
using Nop.Plugin.Payments.RayanWallet.Models;
using Nop.Services.Orders;
using Nop.Web.Models.Common;

namespace Nop.Plugin.Payments.RayanWallet.Services
{
    public class WalletCustomerHistoryService : IWalletCustomerHistoryService
    {

        #region Fields

        private readonly IRepository<WalletCustomerHistory> _walletCustomerHistoryRepository;
        private readonly ICacheManager _cacheManager;
        private readonly IRepository<WalletCustomer> _walletCustomerRepository;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<Order> _orderRepository;
        private readonly IOrderService _orderService;
        private readonly IWorkContext _workContext;


        #endregion

        public WalletCustomerHistoryService(IRepository<WalletCustomerHistory> walletCustomerHistoryRepository,
            ICacheManager cacheManager, IRepository<WalletCustomer> walletCustomerRepository, IRepository<Customer> customerRepository, IRepository<Order> orderRepository,
            IRepository<Store> storeRepository, IWorkContext workContext, IOrderService orderService)
        {
            _walletCustomerHistoryRepository = walletCustomerHistoryRepository;
            _cacheManager = cacheManager;
            _walletCustomerRepository = walletCustomerRepository;
            _customerRepository = customerRepository;
            _orderRepository = orderRepository;
            _orderService = orderService;
            _workContext = workContext;
        }

        public void Log(WalletCustomerHistory record)
        {
            if (record == null)
                throw new ArgumentNullException(nameof(record));
            _walletCustomerHistoryRepository.Insert(record);
        }

        public virtual void InsertWalletCustomerHistory(WalletCustomerHistory walletCustomerHistory)
        {
            if (walletCustomerHistory == null)
                throw new ArgumentNullException(nameof(walletCustomerHistory));

            _walletCustomerHistoryRepository.Insert(walletCustomerHistory);

        }

        public virtual IPagedList<WalletCustomerHistory> GetAll(int page = 0, int pageSize = int.MaxValue)
        {

            var currentCustomer = _workContext.CurrentCustomer;
            var query = from wus in _walletCustomerHistoryRepository.Table
                        join wcus in _walletCustomerRepository.Table on wus.WalletCustomerId equals wcus.Id
                        where wcus.Username == currentCustomer.Username
                        select wus;
            var records = new PagedList<WalletCustomerHistory>(query, page, pageSize = 10);
            return records;
        }
        public virtual WalletCustomerHistory GetById(int walletCustomerHistoryId)
        {
            if (walletCustomerHistoryId == 0)
                return null;

            return _walletCustomerHistoryRepository.GetById(walletCustomerHistoryId);
        }
        public virtual void UpdateWalletCustomerHistory(WalletCustomerHistory walletCustomerHistory)
        {
            if (walletCustomerHistory == null)
                throw new ArgumentNullException(nameof(walletCustomerHistory));

            _walletCustomerHistoryRepository.Update(walletCustomerHistory);

            //_cacheManager.RemoveByPrefix(PAYMENTBYWALLET_PATTERN_KEY);
        }
        public virtual void DeleteWalletCustomerHistory(WalletCustomerHistory walletCustomerHistory)
        {
            if (walletCustomerHistory == null)
                throw new ArgumentNullException(nameof(walletCustomerHistory));

            _walletCustomerHistoryRepository.Delete(walletCustomerHistory);

            //_cacheManager.RemoveByPrefix(PAYMENTBYWALLET_PATTERN_KEY);
        }
        public string GetCustomerWalletRefNo(Order order)
        {
            var walletCustomerRepository = _walletCustomerHistoryRepository.Table.FirstOrDefault(p => p.OrderId == order.Id && !string.IsNullOrEmpty
            (p.RefNo));
            return walletCustomerRepository.RefNo;
        }

        public WalletCustomerHistoryModel PrepareWalletList(int page = 0)
        {
            var walletCustomerHistories = GetAll(page, pageSize: 10);
            //prepare model
            var model = new WalletCustomerHistoryModel
            {
                WalletHistory = walletCustomerHistories.Select(historyEntry =>
                {
                    return new WalletCustomerHistoryModel.WalletHistoryModel
                    {
                        CreateDate = DateTimeExtentions.ToPersianDateTime(historyEntry.CreateDate),
                        Amount = (decimal)(historyEntry.Amount.HasValue ? historyEntry.Amount : 0),
                        OrderNo = historyEntry.RefNo,
                        TransferTypeWallet = historyEntry.TransactionType,
                    };
                }).ToList(),

                PagerModel = new PagerModel
                {
                    PageSize = walletCustomerHistories.PageSize,
                    TotalRecords = walletCustomerHistories.TotalCount,
                    PageIndex = walletCustomerHistories.PageIndex,
                    ShowTotalSummary = true,
                    RouteActionName = "CustomerWalletCustomerHistoryPaged",
                    UseRouteLinks = true,
                    RouteValues = new WalletCustomerHistoryRouteValues { pageNumber = page }
                }
            };
            return model;
        }
        public WalletCustomerHistory GetById(int? id)
        {
            throw new NotImplementedException();
        }
    }
}
