using System;

namespace MeMeSquad.Identity.Models
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