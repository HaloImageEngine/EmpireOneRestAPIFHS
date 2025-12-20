using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmpireOneRestAPISW1.Models
{
    public class ModelCardCount
    {
        public int UserID { get; set; }
        public string UserAlias { get; set; }
        public DateTime CDate { get; set; }
        public string CardGame { get; set; }
        public int CardGameCount { get; set; }
    }
}