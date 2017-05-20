using System;

namespace Framework.Data.Domain
{
    /// <summary>
    /// 实体基础类
    /// </summary>
    public abstract class BaseEntity
    {
        /// <summary>
        /// 主键标识
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 删除标识
        /// </summary>
        public bool IsDelete { set; get; }

        /// <summary>
        /// 数据创建时间
        /// </summary>
        public virtual DateTime CreateAt { set; get; }

        /// <summary>
        /// 时间戳 并发标识
        /// </summary>
        public byte[] RowVersion { set; get; }

    }

    public abstract class BaseEntity<T> : BaseEntity
    {
    }

}
