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
        public async Task<APIGetResponseModel<List<OrganizationModel>>> GetAll(
            PaginationRequestDto request,
            TokenUserInfo user,
            IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<List<OrganizationModel>>()
            {
                Result = new List<OrganizationModel>()
            };

            try
            {
                // 🔐 Permission Check
                if (user == null || !user.Permissions.Contains("ORG_VIEW"))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Unauthorized access (ORG_VIEW required)");
                    return response;
                }

                response = await _dal.GetAll(request, transaction);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add("Something went wrong");
                Console.WriteLine("GET ALL ERROR: " + ex.Message);
            }

            return response;
        }

        // ========================
        // GET BY ID
        // ========================
        public async Task<APIGetResponseModel<OrganizationModel>> GetById(
            long id,
            TokenUserInfo user,
            IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<OrganizationModel>()
            {
                Result = new OrganizationModel()
            };

            try
            {
                if (user == null || !user.Permissions.Contains("ORG_VIEW"))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Unauthorized access (ORG_VIEW required)");
                    return response;
                }

                response = await _dal.GetById(id, transaction);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add("Something went wrong");
                Console.WriteLine("GET BY ID ERROR: " + ex.Message);
            }

            return response;
        }

        // ========================
        // CREATE
        // ========================
        public async Task<APIGetResponseModel<long>> Create(
            OrganizationRequestDto request,
            string userId,
            TokenUserInfo user,
            IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<long>();

            try
            {
                if (user == null || !user.Permissions.Contains("ORG_CREATE"))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Unauthorized access (ORG_CREATE required)");
                    return response;
                }

                response = await _dal.Insert(request, userId, transaction);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add("Something went wrong");
                Console.WriteLine("CREATE ERROR: " + ex.Message);
            }

            return response;
        }

        // ========================
        // UPDATE
        // ========================
        public async Task<APIGetResponseModel<long>> Update(
            OrganizationRequestDto request,
            string userId,
            TokenUserInfo user,
            IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<long>();

            try
            {
                if (user == null || !user.Permissions.Contains("ORG_UPDATE"))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Unauthorized access (ORG_UPDATE required)");
                    return response;
                }

                response = await _dal.Update(request, userId, transaction);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add("Something went wrong");
                Console.WriteLine("UPDATE ERROR: " + ex.Message);
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
                if (user == null || !user.Permissions.Contains("ORG_STATUS"))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Unauthorized access (ORG_STATUS required)");
                    return response;
                }

                response = await _dal.ChangeStatus(id, status, userId, transaction);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add("Something went wrong");
                Console.WriteLine("STATUS ERROR: " + ex.Message);
            }

            return response;
        }
    }
}