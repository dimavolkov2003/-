using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkLibrary
{
    class SocialNetworkException: Exception
    {
        public SocialNetworkException() : base() { }
        public SocialNetworkException(string str) : base(str) { }
        public SocialNetworkException(string str, Exception inner) : base(str, inner) { }
    }
}
