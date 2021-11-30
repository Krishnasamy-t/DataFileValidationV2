using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DataFileValidationV2.Models;
using System.Text.RegularExpressions;

namespace DataFileValidationV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountInformationController : ControllerBase
    {

        //post .txt file in request body form-data
        [HttpPost, DisableRequestSizeLimit]
        public IActionResult ValidateUploadAccountDetails()
        {
            try
            {
                var accountErrorList = new System.Collections.ArrayList();

                if (Request.Form.Files.Count > 0)
                {
                    var accountDetailsfile = Request.Form.Files[0];
                    if (accountDetailsfile.Length > 0)
                    {
                        using (var streamReader = new StreamReader(accountDetailsfile.OpenReadStream()))
                        {
                            int count = 0;
                            while (streamReader.Peek() >= 0)
                            {
                                count++;
                                string? account = streamReader.ReadLine();
                                if (!String.IsNullOrEmpty(account))
                                {
                                    string[] accountInformation = account.Split(' ');
                                    if (accountInformation.Length > 1)
                                    {
                                        string firstName = accountInformation[0].Trim();
                                        string accountNumber = accountInformation[1].Trim();

                                        if(!(FirstNameValidation(firstName) && AccountNumberValidation(accountNumber)))
                                        {
                                            accountErrorList.Add("Account number - not valid for " + count + " line " + account);
                                        }                                        
                                    }
                                    if(accountInformation.Length == 1)
                                    {
                                        accountErrorList.Add(count + " line account number is not availabe");
                                    }
                                }
                                else
                                {
                                    accountErrorList.Add(count + " line is empty");
                                }
                            }
                        }

                        if (accountErrorList.Count == 0)
                        {
                            AccountInformationFileValidResponse fileValidResponse = new AccountInformationFileValidResponse();
                            fileValidResponse.fileValid = true;
                            return Ok(fileValidResponse);
                        }
                        else
                        {
                            AccountInformationFileInValidResponse fileInValidResponse = new AccountInformationFileInValidResponse();
                            fileInValidResponse.inValidLines = accountErrorList;
                            fileInValidResponse.fileValid = false;
                            return Ok(fileInValidResponse);
                        }
                    }
                    else
                    {
                        return BadRequest((Object)"Account information file is empty");
                    }
                }
                else
                {
                    return BadRequest((Object)"Account information file not provided in request");
                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
       

        public static bool FirstNameValidation(string firstName)
        {
            return Regex.IsMatch(firstName, @"^[A-Z]+[a-zA-Z]*$");            
        }
        public static bool AccountNumberValidation(string accountNumber)
        {            
            return Regex.IsMatch(accountNumber, @"^[34]\d{6}p?$");
        }


    }
}
