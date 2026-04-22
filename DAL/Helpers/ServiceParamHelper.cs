using Dapper;

namespace Helpers
{
    public static class ServiceParamHelper
    {
        public static DynamicParameters GetBaseParams()
        {
            var param = new DynamicParameters();

            param.Add("p_service_id", 0);
            param.Add("p_organization_id", 1); // 🔥 TEMP (replace with JWT later)

            param.Add("p_service_name", "");
            param.Add("p_service_code", "");
            param.Add("p_estimated_time", 0);
            param.Add("p_description", "");

            param.Add("p_SearchKey", "");
            param.Add("p_PageNo", 0);
            param.Add("p_PageSize", 0);

            param.Add("p_user_id", "");

            return param;
        }
    }
}