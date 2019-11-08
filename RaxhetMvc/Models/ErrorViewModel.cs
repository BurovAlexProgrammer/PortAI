using System;

namespace RaxhetMvc.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string City { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}