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


    public class BAL_Organization : IBAL_Organization
    {
        private readonly IDAL_Organization _dal;

        public BAL_Organization(IDAL_Organization dal)
        {
            _dal = dal;
        }
        // ========================
        // GET ALL
        // ========================

        /// <summary>
        /// Organization API - Get All Organizations
        /// Author: Swapnlisa
        /// Description:- We use this API to fetch organization list with pagination.
        /// Json Request Format Ex- {"PageNumber":"1","PageSize":"10"}
        /// </summary>
        /// <param name="request">PaginationRequestDto</param>
        /// <param name="user">TokenUserInfo</param>
        /// <param name="transaction">DB Transaction</param>
        /// <returns>Returns paginated organization list</returns>
        public async Task<APIGetResponseModel<List<OrganizationModel>>> GetAll(PaginationRequestDto request,List<string> roles,string? email,IDbTransaction? transaction = null)
        {
            try
            {
                //  Only Super Admin can access Organizations
                if (!roles.Contains("Super Admin"))
                {
                    return new APIGetResponseModel<List<OrganizationModel>>
                    {
                        IsSuccess = false,
                        ErrorMsgs = new List<string> { "Access denied. Only Super Admin allowed." }
                    };
                }

                return await _dal.GetAll(request, email, transaction);
            }
            catch (Exception ex)
            {
                throw new Exception("BAL: Error in GetAll", ex);
            }
        }
        // ========================
        // GET BY ID
        // ========================
        /// <summary>
        /// Organization API - Get Organization By Id
        /// Author: Swapnlisa
        /// Description:- We use this API to fetch organization details using OrganizationId.
        /// Json Request Format Ex- {"OrganizationId":"1"}
        /// </summary>
        /// <param name="id">OrganizationId</param>
        /// <param name="user">TokenUserInfo</param>
        /// <param name="transaction">DB Transaction</param>
        /// <returns>Returns organization details</returns>
        public async Task<APIGetResponseModel<OrganizationModel>> GetById(long id,List<string> roles,string email,IDbTransaction? transaction = null)
        {
            try
            {
                if (!roles.Contains("Super Admin"))
                {
                    return new APIGetResponseModel<OrganizationModel>
                    {
                        IsSuccess = false,
                        ErrorMsgs = new List<string> { "Access denied." }
                    };
                }

                return await _dal.GetById(id, email, transaction);
            }
            catch (Exception ex)
            {
                throw new Exception("BAL: Error in GetById", ex);
            }
        }
        // ========================
        // CREATE
        // ========================
        /// <summary>
        /// Organization API - Create Organization
        /// Author: Swapnlisa
        /// Description:- We use this API to create a new organization.
        /// Json Request Format Ex- {"OrganizationName":"ABC Pvt Ltd","Address":"BBSR"}
        /// </summary>
        /// <param name="request">OrganizationRequestDto</param>
        /// <param name="userId">Logged in UserId</param>
        /// <param name="user">TokenUserInfo</param>
        /// <param name="transaction">DB Transaction</param>
        /// <returns>Returns created OrganizationId</returns>
        public async Task<APIGetResponseModel<int>> Create(OrganizationRequestDto request,List<string> roles,string email,IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<int>();
            IDbTransaction? localtran = null;

            try
            {
                //  ROLE CHECK
                if (!roles.Contains("Super Admin"))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Only Super Admin can create organization.");
                    return response;
                }

                //  VALIDATION
                if (request == null)
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Invalid payload.");
                    return response;
                }

                if (string.IsNullOrWhiteSpace(request.Name))
                    response.ErrorMsgs.Add("Organization Name is required");

                if (string.IsNullOrWhiteSpace(request.Email))
                    response.ErrorMsgs.Add("Email is required");

                if (string.IsNullOrWhiteSpace(request.Phone))
                    response.ErrorMsgs.Add("Phone is required");

                if (response.ErrorMsgs.Any())
                {
                    response.IsSuccess = false;
                    return response;
                }

                // CALL DAL
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
        /// Organization API - Update Organization
        /// Author: Swapnlisa
        /// Description:- We use this API to update organization details.
        /// Json Request Format Ex- {"OrganizationId":"1","OrganizationName":"Updated Name"}
        /// </summary>
        /// <param name="request">OrganizationRequestDto</param>
        /// <param name="userId">Logged in UserId</param>
        /// <param name="user">TokenUserInfo</param>
        /// <param name="transaction">DB Transaction</param>
        /// <returns>Returns updated OrganizationId</returns>
        public async Task<APIGetResponseModel<int>> Update(OrganizationRequestDto request,List<string> roles,string email,IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<int>();
            IDbTransaction? localtran = null;

            try
            {
                if (!roles.Contains("Super Admin"))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Only Super Admin can update organization.");
                    return response;
                }

                if (request == null || request.OrganizationId <= 0)
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Invalid organization data.");
                    return response;
                }

                if (string.IsNullOrWhiteSpace(request.Name))
                    response.ErrorMsgs.Add("Organization Name is required");

                if (string.IsNullOrWhiteSpace(request.Email))
                    response.ErrorMsgs.Add("Email is required");

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
        /// Organization API - Change Organization Status
        /// Author: Swapnlisa
        /// Description:- We use this API to activate or deactivate an organization.
        /// </summary>
        /// <param name="id">OrganizationId</param>
        /// <param name="status">0 = Inactive, 1 = Active</param>
        /// <param name="userId">Logged in UserId</param>
        /// <param name="user">TokenUserInfo</param>
        /// <param name="transaction">DB Transaction</param>
        /// <returns>Returns status update result</returns>
        public async Task<APIGetResponseModel<int>> ChangeStatus(long id,List<string> roles,string email,IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<int>();
            IDbTransaction? localtran = null;

            try
            {

                // 🔍 DEBUG 1: START
                Console.WriteLine("=== BAL START ===");
                Console.WriteLine($"Incoming OrgId: {id}");
                Console.WriteLine("Roles: " + string.Join(",", roles));
                Console.WriteLine("Email: " + email);

                //if (!roles.Contains("Super Admin"))
                //{
                //    response.IsSuccess = false;
                //    response.ErrorMsgs.Add("Only Super Admin can change status.");
                //    return response;
                //}



                //if (id <= 0)
                //{
                //    response.IsSuccess = false;
                //    response.ErrorMsgs.Add("Invalid organization ID.");
                //    return response;
                //}

                //response = await _dal.ChangeStatus(id, email, transaction: localtran);

                //if (transaction == null && localtran != null)
                //    localtran.Commit();

                // 🔍 DEBUG 2: ROLE CHECK
                if (!roles.Any(r => r.Equals("Super Admin", StringComparison.OrdinalIgnoreCase)))
                {
                    Console.WriteLine("❌ ROLE CHECK FAILED");

                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Only Super Admin can change status.");
                    return response;
                }
                else
                {
                    Console.WriteLine("✅ ROLE CHECK PASSED");
                }

                // 🔍 DEBUG 3: ID CHECK
                if (id <= 0)
                {
                    Console.WriteLine("❌ INVALID ORG ID");

                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Invalid organization ID.");
                    return response;
                }

                Console.WriteLine("➡️ CALLING DAL...");

                // 🔍 DEBUG 4: DAL CALL
                response = await _dal.ChangeStatus(id, email, transaction: localtran);

                Console.WriteLine("⬅️ DAL RESPONSE RECEIVED");
                Console.WriteLine("IsSuccess: " + response.IsSuccess);
                Console.WriteLine("Result: " + response.Result);

                // 🔍 DEBUG 5: TRANSACTION
                if (transaction == null && localtran != null)
                {
                    Console.WriteLine("✅ COMMITTING TRANSACTION");
                    localtran.Commit();
                }
            }
            catch (Exception ex)
            {
                //if (transaction == null && localtran != null)
                //    localtran.Rollback();

                //response.IsSuccess = false;
                //response.ErrorMsgs.Add(ex.Message

                Console.WriteLine("❌ BAL ERROR:");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);

                if (transaction == null && localtran != null)
                    localtran.Rollback();

                response.IsSuccess = false;
                response.ErrorMsgs.Add(ex.Message);
            }

            Console.WriteLine("=== BAL END ===");
                
            return response;
        }
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