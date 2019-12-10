namespace ARMS.ViewModel
{
    public class ApiCallbackMessage
    {
        public string Body;
        public bool Success;

        public ApiCallbackMessage()
        {
        }

        public ApiCallbackMessage(string body, bool success)
        {
            Body = body;
            Success = success;
        }
    }
}