using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Drawing2D;
using System.Linq;

namespace apCaminhosMarte
{
    //Felipe Scherer Vicentin (18178)
    //Pedro Gomes Moreira (18174)
    public partial class Form1 : Form
    {
        #region Variáveis e Constantes

        ArvoreBinaria<Cidade> arvore;
        Grafo grafo;
        //Possibilidades de caminhos entre duas cidades
        List<Caminho> possibilidades;
        //caminho escolhido pelo usuário para ser desenhado
        Caminho caminhoAtual;
        //Objeto que representa o gif que aparece quando o usuário seleciona a cidade de destino
        Bitmap gif;
        //Cor usada para desenhar o fundo dos nós no tpArvore
        Color corNo = Color.Blue;
        //Cor usada para desenhar as linhas que unem os nós no tpArvore
        Color corLinhaArvore = Color.Red;
        //Cor usada para escrever o valor do nó correspondente no tpArvore
        Color corCodCidade = Color.Yellow;
        //Cor usada para escrever o nome das Cidades no mapa
        Color corNomeCidade = Color.Black;
        //Cor usada para desenhar as linhas entre as cidades no mapa
        Color corLinhaCidade = Color.FromArgb(51, 77, 201);
        //Cor usada para desenhar o ponto referente às cidades no mapa
        Color corCidade = Color.Black;
        //Cor usada para diferenciar o caminho escolhido pelo usuário dos outros caminhos possíveis
        Color corLinhaCaminhoSelecionado = Color.Red; //para caminho selecionado
        //Grossura da linha usada para unir cidades no mapa
        const int GROSSURA_CANETA = 3;
        //Diâmetro do círculo referente à cada nó no tpArvore
        const int DIAMETRO_NO = 30;
        //Diâmetro do círculo usado para representar onde cada cidade está
        const int DIAMETRO_CIDADE = 10;
        //Largura da imagem do mapa
        const int LARGURA = 4096;
        //Altura da imagem do mapa
        const int ALTURA = 2048;
        //é a distância para cima ou para baixo para fazer as linhas das cidades da borda
        const int DIST_CIDADES = 20;
        #endregion

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //instanciando objeto do gif usando o arquivo salvo na pasta de Imagens
            gif = new Bitmap("../../Imagens/cidadeDestino.gif");
            //anima o gif
            ImageAnimator.Animate(gif, DrawFrame);
            arvore = new ArvoreBinaria<Cidade>(); //instanciação da árvore de cidades
            var leitorCidades = new StreamReader("../../Arquivos/CidadesMarte.txt", Encoding.UTF7); //StreamReader que lê o arquivo de cidades, com código, nome e posição X, Y
            while (!leitorCidades.EndOfStream)
                arvore.Inserir(new Cidade(leitorCidades.ReadLine())); //cria objeto da cidade com base na string e insere na árvore
            leitorCidades.Close();

            arvore.InOrdem((Cidade c) => //visita a árvore do modo in-ordem, ou seja, ordenada
            {
                //adiciona cada cidade ao ListBox de cidades de origem e destino
                lsbOrigem.Items.Add(c);
                lsbDestino.Items.Add(c);
            });
            int[,] matriz = new int[arvore.Quantidade, arvore.Quantidade]; //cria a matriz para o grafo
            var leitorCaminhos = new StreamReader("../../Arquivos/CaminhosEntreCidadesMarte.txt"); //leitor dos caminhos entre as cidades
            while (!leitorCaminhos.EndOfStream)
            {
                //cada linha contém codigo1, codigo2 e distância
                string linha = leitorCaminhos.ReadLine();
                int l = int.Parse(linha.Substring(0, Cidade.TAMANHO_CODIGO).Trim()); //retorna o primeiro código
                //linha da matriz é cidade de origem
                int c = int.Parse(linha.Substring(Cidade.TAMANHO_CODIGO, Cidade.TAMANHO_CODIGO).Trim()); //retorna o segundo código
                //coluna da matriz é cidade de destino
                int d = int.Parse(linha.Substring(2 * Cidade.TAMANHO_CODIGO, 5).Trim()); //distância entre cidades
                matriz[l, c] = d; //coloca a distância relativa às cidades l e c
            }
            //as cidades que não têm um caminho ligando-as permanecerão com 0 na matriz
            grafo = new Grafo(matriz); //grafo é instanciado com essa matriz
            leitorCaminhos.Close();
        }

        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            dgvCaminhos.Rows.Clear(); //limpa os DataGridView para evitar conflito entre a busca atual e a anterior
            dgvMelhorCaminho.Rows.Clear();

            if (lsbOrigem.SelectedIndex == -1) //se uma cidade de origem não foi selecionada
                MessageBox.Show("Selecione uma cidade de origem", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning); //mensagem para o usuário selecionar uma cidade de origem
            else if (lsbDestino.SelectedIndex == -1) //se uma cidade de destino não foi selecionada
                MessageBox.Show("Selecione uma cidade de destino", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning); //mensagem para o usuário selecionar uma cidade de destino
            else if (lsbOrigem.SelectedIndex == lsbDestino.SelectedIndex) //se a cidade de origem for a mesma da de destino
                MessageBox.Show("Selecione uma cidade de destino diferente da origem", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning); //mensagem para o usuário selecionar cidades diferentes
            else //se estiver tudo certo com a escolha de cidades
            {
                possibilidades = grafo.Caminhos(lsbOrigem.SelectedIndex, lsbDestino.SelectedIndex); //grafo retorna uma lista com todas as possibilidades de caminhos entre as duas cidades selecionadas
                //e essa lista é armazenada na lista "possibilidades"
                bool nenhumCaminho = possibilidades.Count == 0; //se o número de elementos da lista de possibilidades for 0, significa que não há nenhum caminho
                if (nenhumCaminho) //se não houver caminhos
                    MessageBox.Show("Nenhum caminho foi encontrado", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information); //mensagem para o usuário avisando-o da ausência de caminhos
                else //se houver caminhos
                {
                    dgvCaminhos.RowCount = possibilidades.Count; //número de linhas do dgvCaminhos é o número de possibilidades que existem --> evita linhas desnecessárias no DataGridView
                    dgvCaminhos.ColumnCount = arvore.Quantidade; //número máximo de cidades pelas quais algum caminho passa é justamente o número de cidades que existem, que é a quantidade de nós
                    //da árvore de cidades
                    int l = 0, c = 0; //inteiros que representam a linha e a coluna do dgv
                    int maiorCaminho = 0; //varíavel representa quantas cidades têm o caminho que passa por mais cidades
                    foreach (Caminho caminho in possibilidades) //para cada caminho nas possibilidades
                    {
                        if (caminho.Rota.Count > maiorCaminho) //se o número de cidades pelas quais o caminho atual passa for maior que a do maiorCaminho até então
                            maiorCaminho = caminho.Rota.Count; //o maiorCaminho é o caminho atual
                        c = 0;
                        foreach (int cidade in caminho.Rota) //independente de ser ou não o maior caminho, suas cidadas são buscadas na árvore e adicionadas ao dgvCaminhos
                            dgvCaminhos.Rows[l].Cells[c++].Value = arvore.Buscar(new Cidade(cidade));
                        ++l;
                    }
                    dgvCaminhos.ColumnCount = maiorCaminho; //número de colunas do dgvCaminhos é o número de cidades do maior caminho, que é a variável maiorCaminho
                    //só tem um caminho que é o melhor, portanto o número de linhas do dgvMelhorCaminho é 1
                    dgvMelhorCaminho.RowCount = 1;                    
                    Caminho melhor = null; //o melhor caminho (mais curto)
                    int menorDist = int.MaxValue; //menor distância que é percorrida pelo melhor caminho começa sendo a maior possível
                    foreach (Caminho cam in possibilidades)
                        if (cam.DistanciaTotal < menorDist) //se o caminho atual tiver uma distância menor que a atual
                        {
                            menorDist = cam.DistanciaTotal;
                            melhor = cam; //o melhor caminho é o atual
                        }
                    dgvMelhorCaminho.ColumnCount = melhor.Rota.Count; //número de colunas do dgvMelhorCaminho é o número de cidades pelas quais ele passa

                    c = 0;
                    foreach (int cidade in melhor.Rota) //percorre-se o melhor caminho
                        dgvMelhorCaminho.Rows[0].Cells[c++].Value = arvore.Buscar(new Cidade(cidade)); //as cidades pelas quais ele passa são adicionadas às colunas do dgvMelhorCaminho
                    dgvCaminhos.Rows[0].Selected = dgvMelhorCaminho.Rows[0].Selected = false; //a princípio, não seleciona nenhum caminho
                    MessageBox.Show("Caminhos foram encontrados, clique em algum deles para visualizar", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information); //mensagem para o usuário
                }
            }
        }        

        private void tpArvore_Paint(object sender, PaintEventArgs e)
        {
            DesenharArvore(e.Graphics, tpArvore, arvore.Raiz); //desenha a árvore de cidades com seus respectivos nós
        }

        private void dgvCaminhos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex; //quando algum caminho é selecionado
            if (index >= 0)
            {
                caminhoAtual = possibilidades[index]; //o caminho atual é o que acabou de ser selecionado pelo usuário
                var listaCidades = new List<int>(); //lista com os códigos das cidades do melhor caminho
                for (int c = 0; c < dgvMelhorCaminho.ColumnCount; ++c) //percorre todas as colunas do dgvMelhorCaminho
                    listaCidades.Add(int.Parse(dgvMelhorCaminho.Rows[0].Cells[c].Value.ToString().Substring(0, 2))); //obtém o código relativo à cidade da coluna atual
                if (caminhoAtual.Rota.SequenceEqual(listaCidades)) //se o caminho selecionado é o melhor, ele também é selecionado no dgvMelhorCaminho
                    dgvMelhorCaminho.Rows[0].Selected = true;
                else
                    dgvMelhorCaminho.Rows[0].Selected = false; //se não, ele não é selecionado
            }
        }

        private void lsb_SelectedIndexChanged(object sender, EventArgs e) //evento para ambos os listboxes
        {
            caminhoAtual = null; //quando a origem muda, tanto o caminho atual quanto as possibilidades não se aplicam mais, pois se referiam à uma rota anterior
            possibilidades = null;
            dgvCaminhos.RowCount = dgvCaminhos.ColumnCount = dgvMelhorCaminho.RowCount = dgvMelhorCaminho.ColumnCount = 0; //número de linhas e colunas dos DataGridViews reseta
        }

        private void DrawFrame(object sender, EventArgs e) //método necessário para animação do gif
        {
            pbMapa.Invalidate();
        }

        private void pbMapa_Paint(object sender, PaintEventArgs e)
        {
            Graphics gfx = e.Graphics; //Graphics do mapa
            for (int l = 0; l < arvore.Quantidade; ++l) //fors para percorrer matriz do grafo
                for (int c = 0; c < arvore.Quantidade; ++c)
                {
                    int dist = grafo.DistanciaEntre(l, c); //distância entre duas cidades l e c
                    if (dist != 0) //se a distância for diferente de 0, ou seja, se houver um caminho entre as cidades l e c
                    {
                        Cidade um = arvore.Buscar(new Cidade(l)); //cidade um é buscada na árvore
                        Cidade dois = arvore.Buscar(new Cidade(c)); //cidade dois é buscada na árvore
                        if (dois.Nome == "Gondor" && um.Nome == "Arrakeen") //caso especial: caminho saí da direita do mapa e continua na esquerda
                        {
                            DesenhaLinha(gfx, 0, um.Y + DIST_CIDADES, um, false); //desenha a linha de Gondor até a borda da direita
                            DesenhaLinha(gfx, LARGURA - 1, um.Y + DIST_CIDADES, dois, true); //desenha a linha da borda da esquerda até Arrakeen
                            var ponto1 = new PointF(pbMapa.Size.Width * dois.X / LARGURA, pbMapa.Size.Height * dois.Y / ALTURA); //pontos que representam, em coordenadas (X,Y), onde a distância
                            //desse caminho deve ser desenhada. No caso, será próximo de Arrakeen
                            var ponto2 = new PointF(pbMapa.Size.Width-1, pbMapa.Size.Height * (um.Y + DIST_CIDADES) / ALTURA);
                            DesenhaDistancia(gfx, ponto1, ponto2, dist); //desenha a distância nos dados pontos
                        }
                        else if (dois.Nome == "Gondor" && um.Nome == "Senzeni Na") //caso especial, como o acima
                        {
                            DesenhaLinha(gfx, 0, um.Y - DIST_CIDADES, um, false); //desenha a linha de Gondor até a borda direita
                            DesenhaLinha(gfx, LARGURA - 1, um.Y - DIST_CIDADES, dois, true); //desenha a linha da borda da esquerda até Senzeni Na
                            var ponto1 = new PointF(0, pbMapa.Size.Height * (um.Y - DIST_CIDADES) / ALTURA); //pontos de onde a distância será escrita. Nesse caso, será próximo de Gondor
                            var ponto2 = new PointF(pbMapa.Size.Width * um.X / LARGURA, pbMapa.Size.Height * um.Y / ALTURA);
                            DesenhaDistancia(gfx, ponto1, ponto2, dist); //distância é escrita nos dados pontos
                        }
                        else //caso padrão
                        {
                            DesenhaLinha(gfx, um, dois, true); //desenha a linha entre as duas cidades atuais
                            var ponto1 = new PointF(pbMapa.Size.Width * um.X / LARGURA, pbMapa.Size.Height * um.Y / ALTURA); //pontos das cidades, calculados com base nas dimensões do mapa
                            var ponto2 = new PointF(pbMapa.Size.Width * dois.X / LARGURA, pbMapa.Size.Height * dois.Y / ALTURA);
                            DesenhaDistancia(gfx, ponto1, ponto2, dist); //Escreve a distância do caminho entre as cidades
                        }
                    }
                }
            if (caminhoAtual != null) //se houver um caminhoAtual para uma possível rota entre duas cidades
            {
                List<Cidade> caminho = new List<Cidade>(); //lista de cidades pelas quais o caminhoAtual passa
                foreach (int i in caminhoAtual.Rota)
                    caminho.Add(arvore.Buscar(new Cidade(i))); //cidades são buscadas na árvore e adicionadas à lista
                DesenhaCaminho(gfx, caminho); //caminhoAtual é desenhado, com base nas cidades pelas quais ele passa
            }
            int origem = lsbOrigem.SelectedIndex;
            int destino = lsbDestino.SelectedIndex;
            arvore.InOrdem((Cidade c) => //para cada cidade na árvore
            {
                DesenhaCidade(gfx, c, c.Codigo == origem, c.Codigo == destino); //desenhar o ponto relativo à esta
            });
        }
        /// <summary>
        /// Método que desenha o ponto relativo à uma Cidade dada
        /// </summary>
        /// <param name="gfx">Objeto Graphics que será usado para desenhar</param>
        /// <param name="c">Cidade a ser desenhada</param>
        /// <param name="origem">Verifica se a cidade passada é a cidade de origem</param>
        /// <param name="destino">Verifica se a cidade passada é a cidade de destino</param>
        private void DesenhaCidade(Graphics gfx, Cidade c, bool origem, bool destino)
        {
            int x = pbMapa.Size.Width * c.X / LARGURA - DIAMETRO_CIDADE / 2; //calcula os pontos onde os pontos serão desenhados
            int y = pbMapa.Size.Height * c.Y / ALTURA - DIAMETRO_CIDADE / 2;
            gfx.FillEllipse(new SolidBrush(corCidade), x, y, DIAMETRO_CIDADE, DIAMETRO_CIDADE); //desenha o ponto no mapa
            gfx.DrawString(c.Nome, new Font("Century Gothic", 10, FontStyle.Bold), new SolidBrush(corCidade), new PointF(x - 10, y + 10)); //Escreve o nome da Cidade passada logo abaixo do ponto
            if (origem) //se a cidade for a origem, um ícone personalizado será desenhado, mostrando para o usuário onde fica a cidade escolhida
                gfx.DrawImage(Image.FromFile("../../Imagens/cidadeOrigem.png"), x - DIAMETRO_CIDADE * 1.5f, y - DIAMETRO_CIDADE * 3.5f, DIAMETRO_CIDADE * 4, DIAMETRO_CIDADE * 4);
            if (destino) //se for a cidade de destino do usuário, o gif representando o destino será desenhado no ponto da cidade
            {
                //Código abaixo é usado para desenhar o gif de modo animado no determinado ponto
                ImageAnimator.UpdateFrames(gif);
                using (Bitmap bmp = new Bitmap(gif.Width, gif.Height))
                {
                    using (Graphics gr = Graphics.FromImage(bmp))
                    {
                        gr.DrawImage(gif, 0, 0);
                    }
                    gfx.DrawImage(bmp, x - DIAMETRO_CIDADE * 2.5f, y - DIAMETRO_CIDADE * 2.5f, DIAMETRO_CIDADE * 6, DIAMETRO_CIDADE * 6);
                }
            }
        }
        /// <summary>
        /// Escreve a distância relativa à duas cidades
        /// </summary>
        /// <param name="gfx">Objeto do Graphics que será usado para desenhar</param>
        /// <param name="ponto1">Ponto que representa onde a primeira cidade está no mapa</param>
        /// <param name="ponto2">Ponto que representa onde a segunda cidade está no mapa</param>
        /// <param name="dist">distância entre as duas cidades</param>
        private void DesenhaDistancia (Graphics gfx, PointF ponto1, PointF ponto2, int dist)
        {            
            float xString = 0, yString = 0; //o meio do caminho é cálculado com base nos pontos e armazenado nas variáveis xString e yString
            if (ponto1.X < ponto2.X)
                xString = ponto1.X + (ponto2.X - ponto1.X) / 2;
            else
                xString = ponto2.X + (ponto1.X - ponto2.X) / 2;
            if (ponto1.Y < ponto2.Y)
                yString = ponto1.Y + (ponto2.Y - ponto1.Y) / 2;
            else
                yString = ponto2.Y + (ponto1.Y - ponto2.Y) / 2;
            //yString terá uma pequena defasagem de posição para não sobrepor a linha desenhada entre as duas cidades
            //essa defasagem, dependendo dos pontos, será positiva ou negativa

            //xString está apontando para o meio do caminho; Se a distância fosse escrito aqui, o começo deste ficaria no meio
            //xString deve ser uma posição tal que o meio da distância coincida com o meio do caminho
            SizeF size = gfx.MeasureString(dist + "", new Font("Century Gothic", 8, FontStyle.Bold)); //tamanho da distância, em string, em px
            xString -= size.Width / 2; //com essa defasagem, xString terá seu meio exatamente na posição central
            gfx.DrawString(dist + "", new Font("Century Gothic", 8, FontStyle.Bold), new SolidBrush(corCidade), xString, yString); //distância é escrita no ponto exato
        }
        /// <summary>
        /// Desenha a linha entre duas cidades
        /// </summary>
        /// <param name="gfx">Objeto do Graphics que será usado para desenhar</param>
        /// <param name="um">A primeira cidade da qual a linha será desenhada</param>
        /// <param name="dois">A segunda cidade para a qual a linha será desenhada</param>
        /// <param name="comSeta">Verifica se, ao final da linha, uma pequena seta indicando o sentido será desenhada</param>
        private void DesenhaLinha(Graphics gfx, Cidade um, Cidade dois, bool comSeta)
        {
            var caneta = new Pen(corLinhaCidade, 2.5f); //objeto da caneta é instanciado
            AdjustableArrowCap bigArrow = new AdjustableArrowCap(DIAMETRO_CIDADE / 2, DIAMETRO_CIDADE / 2); //objeto da seta ao final da linha é instanciado
            PointF p1 = new PointF(pbMapa.Size.Width * um.X / LARGURA, pbMapa.Size.Height * um.Y / ALTURA); //pontos relativos às cidades passadas
            PointF p2 = new PointF(pbMapa.Size.Width * dois.X / LARGURA, pbMapa.Size.Height * dois.Y / ALTURA);
            if (comSeta) //se o a linha desejada contém a seta indicando o sentido
                caneta.CustomEndCap = bigArrow; //seta será adicionada ao final da linha
            gfx.DrawLine(caneta, p1, p2); //linha é desenhada
        }
        /// <summary>
        /// Desenha a linha entre duas cidades com uma caneta específica
        /// </summary>
        /// <param name="gfx">Objeto do Graphics que será usado para desenhar</param>
        /// <param name="caneta">O objeto da caneta específica desejada para o desenho</param>
        /// <param name="um">A primeira cidade da qual a linha será desenhada</param>
        /// <param name="dois">A segunda cidade para a qual a linha será desenhada</param>
        /// <param name="comSeta">Verifica se, ao final da linha, uma pequena seta indicando o sentido será desenhada</param>
        private void DesenhaLinha(Graphics gfx, Pen caneta, Cidade um, Cidade dois, bool comSeta)
        {
            PointF p1 = new PointF(pbMapa.Size.Width * um.X / LARGURA, pbMapa.Size.Height * um.Y / ALTURA); //calculo dos da cidade pontos no mapa
            PointF p2 = new PointF(pbMapa.Size.Width * dois.X / LARGURA, pbMapa.Size.Height * dois.Y / ALTURA);
            if (comSeta) //se a seta for desejada
                caneta.CustomEndCap = new AdjustableArrowCap(DIAMETRO_CIDADE / 2, DIAMETRO_CIDADE / 2); //esta é adicionada ao final da linha
            gfx.DrawLine(caneta, p1, p2); //linha é desenhada
        }
        /// <summary>
        /// Desenha a linha entre uma coordenada com dois pontos e uma cidade
        /// </summary>
        /// <param name="gfx">Objeto do Graphics que será usado para desenhar</param>
        /// <param name="x1">Coordenada x de origem</param>
        /// <param name="y1">Coordenada y de origem</param>
        /// <param name="dois">Cidade de destino</param>
        /// <param name="comSeta">Verifica se, ao final da linha, uma pequena seta indicando o sentido será desenhada</param>
        private void DesenhaLinha(Graphics gfx, float x1, float y1, Cidade dois, bool comSeta)
        {
            var caneta = new Pen(corLinhaCidade, 2.5f); //caneta é instanciada
            AdjustableArrowCap bigArrow = new AdjustableArrowCap(DIAMETRO_CIDADE / 2, DIAMETRO_CIDADE / 2); //seta do final da linha é instanciado
            PointF p1 = new PointF(pbMapa.Size.Width * x1 / LARGURA, pbMapa.Size.Height * y1 / ALTURA); //calcula o ponto de origem com base em x1 e y1
            PointF p2 = new PointF(pbMapa.Size.Width * dois.X / LARGURA, pbMapa.Size.Height * dois.Y / ALTURA); //calcula o ponto de destino com base na cidade passada
            if (comSeta) //se a seta for desejada
                caneta.CustomEndCap = bigArrow; //seta é adicionada ao final da linha
            gfx.DrawLine(caneta, p1, p2); //linha é desenhada
        }
        /// <summary>
        /// Desenha a linha entre uma coordenada com dois pontos e uma cidade, com uma caneta específica
        /// </summary>
        /// <param name="gfx">Objeto do Graphics que será usado para desenhar</param>
        /// <param name="caneta">O objeto da caneta específica desejada para o desenho</param>
        /// <param name="x1">Coordenada x de origem</param>
        /// <param name="y1">Coordenada y de origem</param>
        /// <param name="dois">Cidade de destino</param>
        /// <param name="comSeta">Verifica se, ao final da linha, uma pequena seta indicando o sentido será desenhada</param>
        private void DesenhaLinha(Graphics gfx, Pen caneta, float x1, float y1, Cidade dois, bool comSeta)
        {
            AdjustableArrowCap bigArrow = new AdjustableArrowCap(DIAMETRO_CIDADE / 2, DIAMETRO_CIDADE / 2); //seta do final da linha é instanciado
            PointF p1 = new PointF(pbMapa.Size.Width * x1 / LARGURA, pbMapa.Size.Height * y1 / ALTURA); //calculo dos pontos no pbMapa
            PointF p2 = new PointF(pbMapa.Size.Width * dois.X / LARGURA, pbMapa.Size.Height * dois.Y / ALTURA);
            if (comSeta) //se a seta foir desejada
                caneta.CustomEndCap = bigArrow; //seta é adicionada
            gfx.DrawLine(caneta, p1, p2); //linha é desenhada com a caneta específica
        }
        /// <summary>
        /// Desenha um caminho entre cidades no mapa
        /// </summary>
        /// <param name="gfx">Objeto do Graphics que será usado para desenhar</param>
        /// <param name="caminhoAtual">Lista com todas as cidades do caminho</param>
        private void DesenhaCaminho(Graphics gfx, List<Cidade> caminhoAtual)
        {
            Cidade anterior = caminhoAtual[0]; //anterior é a cidade de origem, a princípio
            foreach (Cidade atual in caminhoAtual) //percorre-se todas as cidades da lista passada
            {                
                Pen caneta = new Pen(corLinhaCaminhoSelecionado, GROSSURA_CANETA); //caneta é instanciada
                if (!atual.Equals(anterior)) //se a cidade atual não for igual à cidade anterior
                {
                    PointF p1 = new PointF(pbMapa.Size.Width * anterior.X / LARGURA, pbMapa.Size.Height * anterior.Y / ALTURA); //calcula os pontos relativos entre a cidade atual e a anterior
                    PointF p2 = new PointF(pbMapa.Size.Width * atual.X / LARGURA, pbMapa.Size.Height * atual.Y / ALTURA);

                    if (atual.Nome == "Gondor" && anterior.Nome == "Arrakeen") //caso especial: caminho saí da direita do mapa e continua na esquerda
                    {
                        DesenhaLinha(gfx, caneta, 0, anterior.Y + DIST_CIDADES, anterior, false); //desenha a linha de Gondo até a borda da direita
                        DesenhaLinha(gfx, caneta, LARGURA - 1, anterior.Y + DIST_CIDADES, atual, true); //desenha a linha da borda da esquerda até Arrakeen
                    }
                    else if (atual.Nome == "Gondor" && anterior.Nome == "Senzeni Na") //caso especial: caminho saí da direita do mapa e continua na esquerda
                    {
                        DesenhaLinha(gfx, caneta, 0, anterior.Y - DIST_CIDADES, anterior, false); //desenha a linha de Gondo até a borda da direita
                        DesenhaLinha(gfx, caneta, LARGURA - 1, anterior.Y - DIST_CIDADES, atual, true); //desenha a linha da borda da esquerda até Senzeni Na
                    }
                    else //caso padrão
                        DesenhaLinha(gfx, caneta, anterior, atual, true); //desenha a linha entre os dois pontos

                    anterior = atual; //anterior é atualizado
                }
            }
        }

        private void DesenharArvore(Graphics gfx, TabPage tpArvore, NoArvore<Cidade> raiz)
        {
            DesenhaNoArvore(true, raiz, (int)tpArvore.Width / 2, 0, Math.PI / 2,
                                 Math.PI / 2.5, 450, gfx);
        }

        private void DesenhaNoArvore(bool primeiraVez, NoArvore<Cidade> noAtual,
                   float x, float y, double angulo, double incremento,
                   double comprimento, Graphics gfx)
        {
            float xf, yf;
            if (noAtual != null)
            {
                Pen caneta = new Pen(corLinhaArvore);
                xf = (float) Math.Round(x + Math.Cos(angulo) * comprimento);
                yf = primeiraVez? 25 : (float) Math.Round(y + Math.Sin(angulo) * comprimento);
                gfx.DrawLine(caneta, x, y, xf, yf);
                var esq = noAtual.Esquerdo;
                DesenhaNoArvore(false, esq, xf, yf, Math.PI / 2 + incremento, incremento * 0.60, comprimento * 0.8, gfx);
                var dir = noAtual.Direito;
                DesenhaNoArvore(false, dir, xf, yf, Math.PI / 2 - incremento, incremento * 0.60, comprimento * 0.8, gfx);
                SolidBrush preenchimento = new SolidBrush(corNo);
                gfx.FillEllipse(preenchimento, xf - DIAMETRO_NO/2, yf - DIAMETRO_NO / 2, DIAMETRO_NO, DIAMETRO_NO);
                gfx.DrawString(noAtual.Info.Codigo + "", new Font("Century Gothic", 12), new SolidBrush(corCodCidade), xf - (DIAMETRO_NO / 2 - 5), yf - DIAMETRO_NO / 2);
                gfx.DrawString(noAtual.Info.Nome, new Font("Century Gothic", 12), new SolidBrush(corNomeCidade), xf - DIAMETRO_NO, yf + DIAMETRO_NO / 2);
                //Desenha nome da cidade embaixo do código
            }
        }

        private void dgvMelhorCaminho_CellClick(object sender, DataGridViewCellEventArgs e) //quando o melhor caminho é escolhido como caminhoAtual
        {
            var listaCidades = new List<int>(); //lista com os códigos das cidades do melhor caminho
            for (int c = 0; c < dgvMelhorCaminho.ColumnCount; ++c) //percorre todas as colunas do dgvMelhorCaminho
                listaCidades.Add(int.Parse(dgvMelhorCaminho.Rows[0].Cells[c].Value.ToString().Substring(0,2))); //obtém o código relativo à cidade da coluna atual
            int ind = 0; //índice usado para saber linha do dgvCaminhos
            foreach (Caminho c in possibilidades) //percorre todos os caminhos nas possibilidades
            {
                if (c.Rota.SequenceEqual(listaCidades)) //se o caminho atual tiver a mesma rota da melhor rota, este é o melhor caminho
                {
                    dgvCaminhos.Rows[ind].Selected = true; //seleciona a linha correspondente no dgvCaminhos
                    caminhoAtual = c; //caminhoAtual é atualizado
                    break; //loop termina para evitar repetições desnecessárias
                }
                dgvCaminhos.Rows[ind].Selected = false; //não seleciona linha do dgvCaminhos
                ++ind; //próxima linha
            }
        }
    }
}