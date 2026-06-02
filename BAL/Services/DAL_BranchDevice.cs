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
    public class BAL_BranchDevice : IBAL_BranchDevice
    {
        private readonly IDAL_BranchDevice _dal;

        public BAL_BranchDevice(IDAL_BranchDevice dal)
        {
            _dal = dal;
        }

        // ========================
        // GET ALL
        // ========================
        public async Task<APIGetResponseModel<List<DeviceModel>>> GetAll(
            PaginationRequestDto request,
            List<string> roles,
            string? email,
            IDbTransaction? transaction = null)
        {
            try
            {
                if (!(roles.Contains("Super Admin")
                    || roles.Contains("Org Admin")
                    || roles.Contains("Branch Admin")
                    || roles.Contains("Branch User")))
                {
                    return new APIGetResponseModel<List<DeviceModel>>
                    {
                        IsSuccess = false,
                        ErrorMsgs = new List<string> { "Access denied." }
                    };
                }

                return await _dal.GetAll(request, email!, transaction);
            }
            catch (Exception ex)
            {
                throw new Exception("BAL: Error in GetAll", ex);
            }
        }

        // ========================
        // GET BY ID
        // ========================
        public async Task<APIGetResponseModel<DeviceModel>> GetById(
            long id,
            List<string> roles,
            string email,
            IDbTransaction? transaction = null)
        {
            try
            {
                if (!(roles.Contains("Super Admin")
                    || roles.Contains("Org Admin")
                    || roles.Contains("Branch Admin")
                    || roles.Contains("Branch User")))
                {
                    return new APIGetResponseModel<DeviceModel>
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
        public async Task<APIGetResponseModel<int>> Create(
            DeviceRequestDto request,
            List<string> roles,
            string email,
            IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<int>();

            try
            {
                if (!(roles.Contains("Super Admin")
                    || roles.Contains("Org Admin")
                    || roles.Contains("Branch Admin")))
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

                if (string.IsNullOrWhiteSpace(request.DeviceName))
                    response.ErrorMsgs.Add("Device Name is required");

                if (string.IsNullOrWhiteSpace(request.DeviceType))
                    response.ErrorMsgs.Add("Device Type is required");

                if (response.ErrorMsgs.Any())
                {
                    response.IsSuccess = false;
                    return response;
                }

                return await _dal.Insert(request, email, transaction);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add(ex.Message);
                return response;
            }
        }

        // ========================
        // UPDATE
        // ========================
        public async Task<APIGetResponseModel<int>> Update(
            DeviceRequestDto request,
            List<string> roles,
            string email,
            IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<int>();

            try
            {
                if (!(roles.Contains("Super Admin")
                    || roles.Contains("Org Admin")
                    || roles.Contains("Branch Admin")))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Access denied.");
                    return response;
                }

                if (request.DeviceId <= 0)
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Invalid Device Id.");
                    return response;
                }

                return await _dal.Update(request, email, transaction);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add(ex.Message);
                return response;
            }
        }

        // ========================
        // STATUS
        // ========================
        public async Task<APIGetResponseModel<int>> ChangeStatus(
            long id,
            List<string> roles,
            string email,
            IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<int>();

            try
            {
                if (!(roles.Contains("Super Admin")
                    || roles.Contains("Org Admin")
                    || roles.Contains("Branch Admin")))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Access denied.");
                    return response;
                }

                if (id <= 0)
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Invalid Device Id.");
                    return response;
                }

                return await _dal.ChangeStatus(id, email, transaction);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add(ex.Message);
                return response;
            }
        }

        // ========================
        // DROPDOWN
        // ========================
        public async Task<APIGetResponseModel<List<DropdownModel>>> GetDropdown(
            string email,
            IDbTransaction? transaction = null)
        {
            return await _dal.GetDropdown(email, transaction);
        }
    }
}