using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class APIGetResponseModel<T>
    {
        public APIGetResponseModel()
        {
            ErrorMsgs = new List<string>();
        }
        public bool IsSuccess { get; set; }
        public List<string> ErrorMsgs { get; set; }
        public T? Result { get; set; } = default(T?);
        public int TotalRecords { get; set; }
    }
    public class PaginationRequestDto
    {
        public string? SearchKey { get; set; }
        public int PageNo { get; set; }
        public int PageSize { get; set; }
    }

}
