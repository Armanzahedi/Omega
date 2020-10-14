using System;
using System.Collections.Generic;
using System.Text;

namespace Omega.Core
{
    public interface IBaseEntity
    {
        int Id { get; set; }
        DateTime AddedDate { get; set; }
        DateTime? ModifiedDate { get; set; }
        bool    Deleted { get; set; }
    }
    public abstract class BaseEntity : IBaseEntity
    {
        public int Id { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool Deleted { get; set; } = false;
    }
}
