using System.Collections.Generic;
using System.Linq;

namespace Jama.NancyHelpers.Models
{
    public class DomainResponse
    {
        private List<Error> _errors = new List<Error>();
        private Dictionary<string, string> _dataBag = new Dictionary<string, string>(); 

        public bool HasErrors { get { return Errors.Any() || !string.IsNullOrEmpty(ErrorType); } }

        public void AddError(string errorMessage)
        {
            _errors.Add(new Error("Custom Error", errorMessage));
        }
        public void AddDataBagEntry(string key, string value)
        {
            _dataBag.Add(key, value);
        }

        public List<Error> Errors
        {
            get { return _errors; }
            set { _errors = value; }
        }

        public string ErrorType { get; set; }

        public Dictionary<string, string> DataBag
        {
            get { return _dataBag; }
            set { _dataBag = value; }
        }
    }

    public class DomainResponse<T> : DomainResponse where T : class
    {
        public DomainResponse()
        {
        }

        public DomainResponse(T item)
        {
            Item = item;
        }

        public T Item { get; set; }
    }
}