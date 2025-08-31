using System;

namespace TDP.BLL.DTOs
{
    public class UpdateResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public UserDto User { get; set; }
    }
} 