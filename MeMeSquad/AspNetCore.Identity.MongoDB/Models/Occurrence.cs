namespace AspNetCore.Identity.MongoDB.Models
{
    using System;

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