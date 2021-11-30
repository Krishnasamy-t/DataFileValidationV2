using System.Collections;

namespace DataFileValidationV2.Models
{
    public class Account
    {
        public string FirstName { get; set; }
        public string AccountNumber { get; set; }
    }
    public class AccountInformationFileValidResponse
    {
        public bool fileValid { get; set; }
    }
    public class AccountInformationFileInValidResponse
    {
        public bool fileValid { get; set; }
        public ArrayList inValidLines { get; set; }
    }


}
