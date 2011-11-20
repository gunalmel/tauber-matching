using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TauberMatching.Models
{
    public class UserError
    {
        public int Id {get;set;}
        [MaxLength(128, ErrorMessage = "User error type can be at most 128 characters long")]
        public string ErrorType {get;set;}
        public int UserId {get;set;}
        [MaxLength(32, ErrorMessage = "User type can be at most 32 characters long. See ContactType enum for possible values.")]
        public string UserType {get;set;}
    }
}