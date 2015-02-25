using Newtonsoft.Json;

namespace Sample
{
    public class AuthenticationResult
    {
        public bool IsSuccess { get; set; }
        public string UserAgent { get; set; }
        public string iProoveUserId { get; set; }
        public string Token { get; set; }
        public string MachineIP { get; set; }

        public AuthenticationResult()
        {
            IsSuccess = true;
        }

        public AuthenticationResult(bool success)
        {
            IsSuccess = success;
            Token = "";
            iProoveUserId = "";
        }

        public string ToJsonString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}