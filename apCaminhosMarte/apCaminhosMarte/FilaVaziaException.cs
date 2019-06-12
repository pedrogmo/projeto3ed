using System;

namespace apCaminhosMarte
{
    //Felipe Scherer Vicentin (18178)
    //Pedro Gomes Moreira (18174)

    //Classe para exceção de fila vazia, herda de Exception
    class FilaVaziaException : Exception
    {
        public FilaVaziaException(String msg) : base(msg)
        { }
    }
}