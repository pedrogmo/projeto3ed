using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apCaminhosMarte
{
    //Felipe Scherer Vicentin (18178)
    //Pedro Gomes Moreira (18174)
    class Cidade : IComparable<Cidade>
    {
        protected int codigo, x, y;
        protected string nome;

        public const int COMECO_CODIGO = 0;
        public const int TAMANHO_CODIGO = 3;
        public const int COMECO_NOME = COMECO_CODIGO + TAMANHO_CODIGO;
        public const int TAMANHO_NOME = 15;
        public const int COMECO_X = COMECO_NOME + TAMANHO_NOME;
        public const int TAMANHO_X = 5;
        public const int COMECO_Y = COMECO_X + TAMANHO_X;
        public const int TAMANHO_Y = 5;

        public Cidade(int c, string n, int x, int y)
        {
            Codigo = c;
            Nome = n;
            X = x;
            Y = y;
        }

        public Cidade(string linha)
        {
            Codigo = int.Parse(linha.Substring(COMECO_CODIGO, TAMANHO_CODIGO).Trim());
            Nome = linha.Substring(COMECO_NOME, TAMANHO_NOME).Trim();
            X = int.Parse(linha.Substring(COMECO_X, TAMANHO_X).Trim());
            Y = int.Parse(linha.Substring(COMECO_Y, TAMANHO_Y).Trim());
        }

        public Cidade(int codC)
        {
            Codigo = codC;
        }

        public int Codigo
        {
            get => codigo;
            private set
            {
                if (value < 0)
                    throw new Exception("Código inválido");
                codigo = value;
            }
        }

        public string Nome
        {
            get => nome;
            private set
            {
                if (value == null || value == "")
                    throw new Exception("Nome inválido");
                nome = value;
            }
        }

        public int X
        {
            get => x;
            set
            {
                if (value < 0)
                    throw new Exception("Coordenada x inválida");
                x = value;
            }
        }

        public int Y
        {
            get => y;
            set
            {
                if (value < 0)
                    throw new Exception("Coordenada y inválida");
                y = value;
            }
        }   
        
        public int CompareTo(Cidade c)
        {
            return codigo - c.codigo;
        }

        public override string ToString()
        {
            return $"{codigo} - {nome}";
        }
    }
}
