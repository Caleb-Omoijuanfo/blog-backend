using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pempo_backend.Model
{
    public class PasswordHashModel
    {
        public byte[] Salt { get; set; }
        public string Hash { get; set; }
    }
}
