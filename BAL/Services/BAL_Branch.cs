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

    public class BAL_Branch : IBAL_Branch
    {
        private readonly IDAL_Branch _dal;

        public BAL_Branch(IDAL_Branch dal)
        {
            _dal = dal;
        }

        // ========================
        // GET ALL
        // ========================
        public async Task<APIGetResponseModel<List<BranchModel>>> GetAll(
            PaginationRequestDto request,
            TokenUserInfo user,
            IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<List<BranchModel>>()
            {
                Result = new List<BranchModel>()
            };

            try
            {
                if (user == null || !user.Permissions.Contains("BRANCH_VIEW"))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Unauthorized (BRANCH_VIEW)");
                    return response;
                }

                response = await _dal.GetAll(request, transaction);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add("Something went wrong");
            }

            return response;
        }

        // ========================
        // GET BY ID
        // ========================
        public async Task<APIGetResponseModel<BranchModel>> GetById(
            long id,
            TokenUserInfo user,
            IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<BranchModel>();

            try
            {
                if (user == null || !user.Permissions.Contains("BRANCH_VIEW"))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Unauthorized (BRANCH_VIEW)");
                    return response;
                }

                response = await _dal.GetById(id, transaction);
            }
            catch
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add("Something went wrong");
            }

            return response;
        }

        // ========================
        // CREATE
        // ========================
        public async Task<APIGetResponseModel<long>> Create(
            BranchRequestDto request,
            string userId,
            TokenUserInfo user,
            IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<long>();

            try
            {
                if (user == null || !user.Permissions.Contains("BRANCH_CREATE"))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Unauthorized (BRANCH_CREATE)");
                    return response;
                }

                response = await _dal.Insert(request, userId, transaction);
            }
            catch
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add("Something went wrong");
            }

            return response;
        }

        // ========================
        // UPDATE
        // ========================
        public async Task<APIGetResponseModel<long>> Update(
            BranchRequestDto request,
            string userId,
            TokenUserInfo user,
            IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<long>();

            try
            {
                if (user == null || !user.Permissions.Contains("BRANCH_UPDATE"))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Unauthorized (BRANCH_UPDATE)");
                    return response;
                }

                response = await _dal.Update(request, userId, transaction);
            }
            catch
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add("Something went wrong");
            }

            return response;
        }

        // ========================
        // CHANGE STATUS
        // ========================
        public async Task<APIGetResponseModel<long>> ChangeStatus(
            long id,
            int status,
            long userId,
            TokenUserInfo user,
            IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<long>();

            try
            {
                if (user == null || !user.Permissions.Contains("BRANCH_STATUS"))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Unauthorized (BRANCH_STATUS)");
                    return response;
                }

                response = await _dal.ChangeStatus(id, status, userId, transaction);
            }
            catch
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add("Something went wrong");
            }

            return response;
        }
    }
}
