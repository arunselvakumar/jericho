using System;

namespace AspNetCore.Identity.MongoDB.Models
{
    public class ConfirmationOccurrence : Occurrence
    {
        #region Constructor
        public ConfirmationOccurrence() : base()
        {

        }

        public ConfirmationOccurrence(DateTime confirmedOn) : base(confirmedOn)
        {

        }
        #endregion
    }
}