using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class WebhookSettingsResponseDto
    {
        public string WebhookUrl { get; set; } = string.Empty;
        public string SecretKey { get; set; } = string.Empty;
    }
}
