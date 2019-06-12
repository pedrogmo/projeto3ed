using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apCaminhosMarte
{
    //Felipe Scherer Vicentin (18178)
    //Pedro Gomes Moreira (18174)

    //Classe para exceção de pilha vazia, herda de Exception
    class PilhaVaziaException : Exception
    {
        public PilhaVaziaException(string msg) : base(msg)
        { }
    }
}
