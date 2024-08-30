using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minimal_Api.Domain.ModelsView
{
    public struct ValidationsError
    {
        public ValidationsError(){
            this.Message = new List<string>();
        }
        public List<string> Message{get;set; }
    
    }
}