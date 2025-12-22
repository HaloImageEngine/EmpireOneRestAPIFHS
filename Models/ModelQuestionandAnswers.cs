using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmpireOneRestAPIFHS.Models
{
    public class ModelQuestionAnswer

    {
        public int Id { get; set; }
        public int TrackID { get; set; }
        public string Category { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public string AnswerValid { get; set; }

    }

    public class ModelQuestionsAnswers
    {
        public int ID { get; set; }
        public int TrackID { get; set; }
        public string Category { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public bool AnswerValid { get; set; }


    }

    public class ModelQuestionsAnswersbyTestId
    {
        //Single Answer
        public int AnswerResponseId { get; set; }
        public int UserID { get; set; }
        public int UserAlias { get; set; }
        public string QuestionID { get; set; }
        public string QuestionFull { get; set; }
        public string AnswerUser { get; set; }
        public string AnswerFull { get; set; }
        public int Score {  get; set; }
        public string ScoreDesc { get; set; }

    }
    public class ModelQuestionsAnswersbyUserId
    {
        //List
        public int AnswerResponseId { get; set; }
        public int UserID { get; set; }
        public string UserAlias { get; set; }
        public int QuestionID { get; set; }
        public string QuestionShort { get; set; }
        public string AnswerShort { get; set; }
        public string Score { get; set; }

    }

  
}