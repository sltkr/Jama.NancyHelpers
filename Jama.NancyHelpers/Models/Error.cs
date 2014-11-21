namespace Jama.NancyHelpers.Models
{
    /// <summary>
    /// Represents an error for use with the response builder
    /// </summary>
    public class Error
    {
        public Error(string propertyName, string errorMessage)
        {
            PropertyName = propertyName;
            ErrorMessage = errorMessage;
        }
        
        /// <summary>
        /// Property associated with the error
        /// </summary>
        public string PropertyName { get; private set; }    

        /// <summary>
        /// Error message
        /// </summary>
        public string ErrorMessage { get; private set; }

        public override string ToString()
        {
            return string.Format("PropertyName: {0}, ErrorMessage: {1}", PropertyName, ErrorMessage);
        }
    }
}