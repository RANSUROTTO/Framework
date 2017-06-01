using System;

namespace Framework.Data.Domain
{
    /// <summary>
    /// Base Entity
    /// </summary>
    public abstract class BaseEntity
    {
        /// <summary>
        /// Identity
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Delete the Tag
        /// </summary>
        public bool IsDelete { set; get; }

        /// <summary>
        /// Data creation time
        /// </summary>
        public virtual DateTime CreateAt { set; get; }

        /// <summary>
        /// Timestamp, concurrent identification.
        /// </summary>
        public byte[] RowVersion { set; get; }

    }

    public abstract class BaseEntity<T> : BaseEntity
    {
    }

}
