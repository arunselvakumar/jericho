using System;

namespace MeMeSquad.Identity.Models
{
    public class FutureOccurrence : Occurrence
    {
        #region Constructor

        public FutureOccurrence() : base()
        {

        }

        public FutureOccurrence(DateTime willOccurOn) : base(willOccurOn)
        {

        }

        #endregion
    }
}