using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TauberMatching.Models
{
    public class UserError
    {
        public int Id {get;set;}
        public string ErrorType {get;set;}
        public string UserId {get;set;}
        public char UserType {get;set;}
    }
}