//namespace TechStore.Models
//{
//    public class ErrorViewModel
//    {
//        public string? RequestId { get; set; }

//        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
//    }
//}

using System;

namespace TechStore.Models {
    public class ErrorViewModel {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
