using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmpireOneRestAPIITJ.Models
{
    public class ModelQuestion
    {
        public int ID { get; set; }
        public string Category { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }

    }

    public class ModelQuestions
    {
        public int ID { get; set; }
        public string Category { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }

    }

    public class ModelDropDown
    {
        public int ID { get; set; }
        public string Category { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }

    }

    public class ModelDropDownCat
    {
        public int ID { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string Sequence { get; set; }

    }

    public class ModelSupportTicket
    {
        public DateTime DTID { get; set; }
        public string UserID { get; set; }
        public string Subject { get; set; }
        public string Comment { get; set; }
        public string UserAlias { get; set; }
        public string Response { get; set; }
        public string Email { get; set; }

    }

    public class ModelKeyWord    
    {   
        public string Category { get; set; }
        public string QuestionID { get; set; }
        public string Keyword { get; set; }
    }

    public class KeywordUnit
    {
        public string QuestionId { get; set; } = string.Empty;
        public string Keyword { get; set; } = string.Empty;
    }

    public class ModelGradeReturn
    {
        public string QuestionID { get; set; }
      //  public List<KeywordUnit> KeywordUnitList { get; set; }
        public List<string> KeywordList { get; set; }
        public int NumKeyWords { get; set; }
        public int Matches { get; set; }
        public decimal Grade { get; set; }

    }

}