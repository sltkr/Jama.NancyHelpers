namespace Jama.NancyHelpers.Models
{
    public class Error
    {
        public Error(string propertyName, string errorMessage)
        {
            PropertyName = propertyName;
            ErrorMessage = errorMessage;
        }

        public string PropertyName { get; private set; }    
        public string ErrorMessage { get; private set; }

        public override string ToString()
        {
            return string.Format("PropertyName: {0}, ErrorMessage: {1}", PropertyName, ErrorMessage);
        }
    }
}