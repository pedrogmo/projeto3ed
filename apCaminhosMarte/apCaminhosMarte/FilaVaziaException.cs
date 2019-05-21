using System;

namespace apCaminhosMarte
{
    class FilaVaziaException : Exception
    {
        public FilaVaziaException(String msg) : base(msg)
        {
        }
    }
}