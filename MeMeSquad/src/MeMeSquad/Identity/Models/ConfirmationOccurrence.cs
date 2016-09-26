namespace MeMeSquad.Identity.Models
{
    using System;

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