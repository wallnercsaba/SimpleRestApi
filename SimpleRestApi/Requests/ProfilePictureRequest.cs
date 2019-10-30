using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleRestApi.Requests
{
    public class ProfilePictureRequest
    {
        public IFormFile Image { get; set; }
    }
}
