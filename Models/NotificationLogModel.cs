using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class NotificationLogModel
    {
        public long NotificationId { get; set; }

        public long? OrganizationId { get; set; }

        public long? BranchId { get; set; }

        public long? TokenId { get; set; }

        public long? CustomerId { get; set; }

        public string? Recipient { get; set; }

        public string? NotificationType { get; set; }

        public string? TemplateCode { get; set; }

        public string? Subject { get; set; }

        public string? MessageBody { get; set; }

        public string? ProviderName { get; set; }

        public string? ProviderResponse { get; set; }

        public string? Status { get; set; }

        public string? ErrorMessage { get; set; }

        public DateTime? SentAt { get; set; }

        public DateTime CreatedAt { get; set; }
    }

    public class NotificationLogRequestDto
    {
        public long NotificationId { get; set; }

        public long? OrganizationId { get; set; }

        public long? BranchId { get; set; }

        public long? TokenId { get; set; }

        public long? CustomerId { get; set; }

        public string? Recipient { get; set; }

        public string? NotificationType { get; set; }

        public string? TemplateCode { get; set; }

        public string? Subject { get; set; }

        public string? MessageBody { get; set; }

        public string? ProviderName { get; set; }

        public string? ProviderResponse { get; set; }

        public string? Status { get; set; }

        public string? ErrorMessage { get; set; }

        public DateTime? SentAt { get; set; }
    }

    public class NotificationLogStatusRequestDto
    {
        public long NotificationId { get; set; }
    }

}
