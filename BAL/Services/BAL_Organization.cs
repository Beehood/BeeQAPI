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


    public class OrganizationBAL : IOrganizationBAL
    {
        private readonly IOrganizationDAL _dal;

        public OrganizationBAL(IOrganizationDAL dal)
        {
            _dal = dal;
        }

        // ========================
        // GET ALL
        // ========================
        public async Task<APIGetResponseModel<List<OrganizationModel>>> GetAll(
            PaginationRequestDto request,
            IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<List<OrganizationModel>>();
            try
            {
                response = await _dal.GetAll(request, transaction);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add(ex.Message);
            }
            return response;
        }

        // ========================
        // GET BY ID
        // ========================
        public async Task<APIGetResponseModel<OrganizationModel>> GetById(
            long id,
            IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<OrganizationModel>();
            try
            {
                response = await _dal.GetById(id, transaction);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add(ex.Message);
            }
            return response;
        }

        // ========================
        // CREATE
        // ========================
        public async Task<APIGetResponseModel<long>> Create(
            OrganizationRequestDto request,
            IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<long>();
            try
            {
                response = await _dal.Insert(request, transaction);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add(ex.Message);
            }
            return response;
        }

        // ========================
        // UPDATE
        // ========================
        public async Task<APIGetResponseModel<long>> Update(
            OrganizationRequestDto request,
            IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<long>();
            try
            {
                response = await _dal.Update(request, transaction);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add(ex.Message);
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
            IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<long>();
            try
            {
                response = await _dal.ChangeStatus(id, status, userId, transaction);
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


