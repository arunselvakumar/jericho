using System;

namespace MeMeSquad.Identity.Models
{
    public class Occurrence
    {
        #region Constructor

        public Occurrence() : this(DateTime.UtcNow)
        {

        }

        public Occurrence(DateTime occuranceInstantUtc)
        {
            Instant = occuranceInstantUtc;
        }

        #endregion

        #region Public Properties

        public DateTime Instant { get; private set; }

        #endregion
    }
}