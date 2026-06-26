using BAL.ContractIF;
using DAL.ContractIF;
using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Services
{
    public class BAL_Customer : IBAL_Customer
    {
        private readonly IDAL_Customer _dal;

        public BAL_Customer(IDAL_Customer dal)
        {
            _dal = dal;
        }

        // ========================
        // GET ALL
        // ========================
        /// <summary>
        /// Customer API - Get All Customers
        /// Author: Swapnlisa
        /// Description:- We use this API to fetch customer list with pagination.
        /// Json Request Format Ex- {"PageNumber":"1","PageSize":"10"}
        /// </summary>
        /// <param name="request">PaginationRequestDto</param>
        /// <param name="roles">User Roles</param>
        /// <param name="email">Logged in User Email</param>
        /// <param name="transaction">DB Transaction</param>
        /// <returns>Returns paginated customer list</returns>
        public async Task<APIGetResponseModel<List<CustomerModel>>> GetAll(PaginationRequestDto request, List<string> roles, string? email, IDbTransaction? transaction = null)
        {
            try
            {
                //  Super Admin → Full access
                if (!roles.Any())
                {
                    return new APIGetResponseModel<List<CustomerModel>>
                    {
                        IsSuccess = false,
                        ErrorMsgs = new List<string> { "Access denied." }
                    };
                }

                //  Org Admin / Branch Admin → SP will filter
                if (roles.Contains("Org Admin") || roles.Contains("Branch Admin"))
                {
                    return await _dal.GetAll(request, email, transaction);
                }

                //  Custom Role → Allow if has permission
                if (roles.Any())
                {
                    return await _dal.GetAll(request, email, transaction);
                }

                return new APIGetResponseModel<List<CustomerModel>>
                {
                    IsSuccess = false,
                    ErrorMsgs = new List<string> { "Access denied." }
                };
            }
            catch (Exception ex)
            {
                throw new Exception("BAL: Error in Customer GetAll", ex);
            }
        }

        // ========================
        // GET BY ID
        // ========================
        /// <summary>
        /// Customer API - Get Customer By Id
        /// Author: Swapnlisa
        /// Description:- We use this API to fetch customer details using CustomerId.
        /// Json Request Format Ex- {"CustomerId":"1"}
        /// </summary>
        /// <param name="id">CustomerId</param>
        /// <param name="roles">User Roles</param>
        /// <param name="email">Logged in User Email</param>
        /// <param name="transaction">DB Transaction</param>
        /// <returns>Returns customer details</returns>
        public async Task<APIGetResponseModel<CustomerModel>> GetById(long id, List<string> roles, string email, IDbTransaction? transaction = null)
        {
            try
            {
                if (!roles.Any())
                {
                    return new APIGetResponseModel<CustomerModel>
                    {
                        IsSuccess = false,
                        ErrorMsgs = new List<string> { "Access denied." }
                    };
                }

                return await _dal.GetById(id, email, transaction);
            }
            catch (Exception ex)
            {
                throw new Exception("BAL: Error in Customer GetById", ex);
            }
        }

        // ========================
        // CREATE
        // ========================
        /// <summary>
        /// Customer API - Create Customer
        /// Author: Swapnlisa
        /// Description:- We use this API to create a new customer.
        /// Json Request Format Ex- {"Name":"John","Phone":"9999999999"}
        /// </summary>
        /// <param name="request">CustomerRequestDto</param>
        /// <param name="roles">User Roles</param>
        /// <param name="email">Logged in User Email</param>
        /// <param name="transaction">DB Transaction</param>
        /// <returns>Returns created CustomerId</returns>
        public async Task<APIGetResponseModel<int>> Create(CustomerRequestDto request, List<string> roles, string email, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<int>();
            IDbTransaction? localtran = null;

            try
            {
                //  Only Super Admin OR Custom Permission
                if (!roles.Any(r => r.Equals("Super Admin", StringComparison.OrdinalIgnoreCase)
  || r.Equals("Org Admin", StringComparison.OrdinalIgnoreCase)
  || r.Equals("Branch Admin", StringComparison.OrdinalIgnoreCase)
  || r.Equals("Counter Admin", StringComparison.OrdinalIgnoreCase)))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Access denied.");
                    return response;
                }

                if (request == null)
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Invalid payload.");
                    return response;
                }

                if (string.IsNullOrWhiteSpace(request.Name))
                    response.ErrorMsgs.Add("Customer Name is required");

                if (string.IsNullOrWhiteSpace(request.Phone))
                    response.ErrorMsgs.Add("Phone is required");

                if (response.ErrorMsgs.Any())
                {
                    response.IsSuccess = false;
                    return response;
                }

                response = await _dal.Insert(request, email, transaction: localtran);

                if (transaction == null && localtran != null)
                    localtran.Commit();
            }
            catch (Exception ex)
            {
                if (transaction == null && localtran != null)
                    localtran.Rollback();

                response.IsSuccess = false;
                response.ErrorMsgs.Add(ex.Message);
            }

            return response;
        }

        // ========================
        // UPDATE
        // ========================
        /// <summary>
        /// Customer API - Update Customer
        /// Author: Swapnlisa
        /// Description:- We use this API to update customer details.
        /// Json Request Format Ex- {"CustomerId":"1","Name":"Updated Name"}
        /// </summary>
        /// <param name="request">CustomerRequestDto</param>
        /// <param name="roles">User Roles</param>
        /// <param name="email">Logged in User Email</param>
        /// <param name="transaction">DB Transaction</param>
        /// <returns>Returns updated CustomerId</returns>

        public async Task<APIGetResponseModel<int>> Update(CustomerRequestDto request, List<string> roles, string email, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<int>();
            IDbTransaction? localtran = null;

            try
            {
                if (!roles.Any(r => r.Equals("Super Admin", StringComparison.OrdinalIgnoreCase)
 || r.Equals("Org Admin", StringComparison.OrdinalIgnoreCase)
 || r.Equals("Branch Admin", StringComparison.OrdinalIgnoreCase)
 || r.Equals("Counter Admin", StringComparison.OrdinalIgnoreCase)))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Access denied.");
                    return response;
                }

                if (request == null || request.CustomerId <= 0)
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Invalid customer data.");
                    return response;
                }

                if (string.IsNullOrWhiteSpace(request.Name))
                    response.ErrorMsgs.Add("Customer Name is required");

                if (string.IsNullOrWhiteSpace(request.Phone))
                    response.ErrorMsgs.Add("Phone is required");

                if (response.ErrorMsgs.Any())
                {
                    response.IsSuccess = false;
                    return response;
                }

                response = await _dal.Update(request, email, transaction: localtran);

                if (transaction == null && localtran != null)
                    localtran.Commit();
            }
            catch (Exception ex)
            {
                if (transaction == null && localtran != null)
                    localtran.Rollback();

                response.IsSuccess = false;
                response.ErrorMsgs.Add(ex.Message);
            }

            return response;
        }

        // ========================
        // STATUS
        // ========================
        /// <summary>
        /// Customer API - Change Customer Status
        /// Author: Swapnlisa
        /// Description:- We use this API to activate or deactivate a customer.
        /// </summary>
        /// <param name="id">CustomerId</param>
        /// <param name="roles">User Roles</param>
        /// <param name="email">Logged in User Email</param>
        /// <param name="transaction">DB Transaction</param>
        /// <returns>Returns status update result</returns>
        public async Task<APIGetResponseModel<int>> ChangeStatus(long id, List<string> roles, string email, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<int>();
            IDbTransaction? localtran = null;

            try
            {
                if (!roles.Any(r => r.Equals("Super Admin", StringComparison.OrdinalIgnoreCase)
  || r.Equals("Org Admin", StringComparison.OrdinalIgnoreCase)
  || r.Equals("Branch Admin", StringComparison.OrdinalIgnoreCase)
  || r.Equals("Counter Admin", StringComparison.OrdinalIgnoreCase)))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Access denied.");
                    return response;
                }

                if (id <= 0)
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Invalid customer ID.");
                    return response;
                }

                response = await _dal.ChangeStatus(id, email, transaction: localtran);

                if (transaction == null && localtran != null)
                    localtran.Commit();
            }
            catch (Exception ex)
            {
                if (transaction == null && localtran != null)
                    localtran.Rollback();

                response.IsSuccess = false;
                response.ErrorMsgs.Add(ex.Message);
            }

            return response;
        }

        // ========================
        // DROPDOWN
        // ========================
        /// <summary>
        /// Customer API - Get Customer Dropdown
        /// Author: Swapnlisa
        /// Description:- Fetches active customer dropdown list.
        /// </summary>
        /// <param name="email">Logged in User Email</param>
        /// <param name="transaction">DB Transaction</param>
        /// <returns>Returns customer dropdown list</returns>
        public async Task<APIGetResponseModel<List<DropdownModel>>> GetDropdown(string email, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<List<DropdownModel>>();

            try
            {
                response = await _dal.GetDropdown(email, transaction);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add(ex.Message);
            }

            return response;
        }
    }

}
