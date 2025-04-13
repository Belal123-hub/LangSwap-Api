using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Data
{
    public interface IBaseEntity
    {
        public DateTime CreateDateTime { get; set; }
        public DateTime ModifyDateTime { get; set; }
        public DateTime? DeleteDateTime { get; set; }
    }
}
