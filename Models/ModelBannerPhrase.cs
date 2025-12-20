using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmpireOneRestAPISW1.Models
{
    public class ModelBannerPhraseShort
    {
        public int PhraseId { get; set; }
        public string Phrase { get; set; }
    }

    public class ModelBannerPhraseFull
    {
        public int PhraseId { get; set; }         // Primary key
        public DateTime DateEntered { get; set; } // When phrase was added
        public string Category { get; set; }      // GEN, PBW, STT, LUK
        public string Phrase { get; set; }        // The text itself
        public bool Active { get; set; }          // Whether enabled
        public int Priority { get; set; }         // Priority weighting
        public string Game { get; set; }          // PB, MG, TS, TX
        public int SEQ { get; set; }              // Sequence number
    }
}
