﻿using UnityEngine;
using tabuleiro;
using xadrex;
using UnityEngine.UI;

class GameController : MonoBehaviour {

    public GameObject reiBranco = null;
    public GameObject reiPreto = null;
    public GameObject torreBranca = null;
    public GameObject torrePreta = null;
    public GameObject damaPreta = null;
    public GameObject damaBranca = null;
    public GameObject peaoBranco = null;
    public GameObject peaoPreto = null;
    public GameObject bispoBranco = null;
    public GameObject bispoPreto = null;                                  
    public GameObject cavaloBranco = null;
    public GameObject cavaloPreto = null;




    public Text txtMsg = null;
    public Text txtXeque = null;

    public GameObject pecaEscolhida { get; private set; }

    public Estado estado { get; private set; }



    PartidaDeXadrez partida;
    PosicaoXadrez origem, destino;
    Color corOriginal;

    Vector3 posDescarteBrancas, posDescartePretas;

    void Start() {
        estado = Estado.AguardandoJogada;
        pecaEscolhida = null;
        corOriginal = txtMsg.color;

        posDescarteBrancas = new Vector3(-3f, 0f, -3f);
        posDescartePretas = new Vector3(3f, 0f, 3f);

        partida = new PartidaDeXadrez();

        txtXeque.text = "";
        informarAguardando(); 


        partida = new PartidaDeXadrez();
        Util.instanciarTorre('a', 1, Cor.Branca, partida, torreBranca);
        Util.instanciarCavalo('b', 1, Cor.Branca, partida, cavaloBranco);
        Util.instanciarBispo('c', 1, Cor.Branca, partida, bispoBranco);
        Util.instanciarDama('d', 1, Cor.Branca, partida, damaBranca);
        Util.instanciarRei('e', 1, Cor.Branca, partida, reiBranco);
        Util.instanciarBispo('f', 1, Cor.Branca, partida, bispoBranco);
        Util.instanciarCavalo('g', 1, Cor.Branca, partida, cavaloBranco);
        Util.instanciarTorre('h', 1, Cor.Branca, partida, torreBranca);
        Util.instanciarPeao('a', 2, Cor.Branca, partida, peaoBranco);
        Util.instanciarPeao('b', 2, Cor.Branca, partida, peaoBranco);
        Util.instanciarPeao('c', 2, Cor.Branca, partida, peaoBranco);
        Util.instanciarPeao('d', 2, Cor.Branca, partida, peaoBranco);
        Util.instanciarPeao('e', 2, Cor.Branca, partida, peaoBranco);
        Util.instanciarPeao('f', 2, Cor.Branca, partida, peaoBranco);
        Util.instanciarPeao('g', 2, Cor.Branca, partida, peaoBranco);
        Util.instanciarPeao('h', 2, Cor.Branca, partida, peaoBranco);

        Util.instanciarTorre('a', 8, Cor.Preta, partida, torrePreta);
        Util.instanciarCavalo('b', 8, Cor.Preta, partida, cavaloPreto);
        Util.instanciarBispo('c', 8, Cor.Preta, partida, bispoPreto);
        Util.instanciarDama('d', 8, Cor.Preta, partida, damaPreta);
        Util.instanciarRei('e', 8, Cor.Preta, partida, reiPreto);
        Util.instanciarBispo('f', 8, Cor.Preta, partida, bispoPreto);
        Util.instanciarCavalo('g', 8, Cor.Preta, partida, cavaloPreto);
        Util.instanciarTorre('h', 8, Cor.Preta, partida, torrePreta);
        Util.instanciarPeao('a', 7, Cor.Preta, partida, peaoPreto);
        Util.instanciarPeao('b', 7, Cor.Preta, partida, peaoPreto);
        Util.instanciarPeao('c', 7, Cor.Preta, partida, peaoPreto);
        Util.instanciarPeao('d', 7, Cor.Preta, partida, peaoPreto);
        Util.instanciarPeao('e', 7, Cor.Preta, partida, peaoPreto);
        Util.instanciarPeao('f', 7, Cor.Preta, partida, peaoPreto);
        Util.instanciarPeao('g', 7, Cor.Preta, partida, peaoPreto);
        Util.instanciarPeao('h', 7, Cor.Preta, partida, peaoPreto);




    }

    public void processarMouseDown(GameObject peca, GameObject casa) {
        if (estado == Estado.AguardandoJogada) {
            if (casa != null) {
                try {
                    char coluna = casa.name[0];
                    int linha = casa.name[1] - '0';
                    origem = new PosicaoXadrez(coluna, linha);
                    partida.validarPosicaoDeOrigem(origem.toPosicao());
                    pecaEscolhida = peca;
                    estado = Estado.Arrastando;
                    txtMsg.text = "Solte a peça na casa de destino"; 
                }
                catch (TabuleiroException e) {
                    informarAviso(e.Message);
                }
            }
        }
    }

    public void processarMouseUp(GameObject peca, GameObject casa) {
        if (estado == Estado.Arrastando) {
            if (casa != null) {
                if (pecaEscolhida != null && pecaEscolhida == peca) {
                    try {
                        char coluna = casa.name[0];
                        int linha = casa.name[1] - '0';
                        destino = new PosicaoXadrez(coluna, linha);

                        partida.validarPosicaoDeDestino(origem.toPosicao(), destino.toPosicao());
                        Peca pecaCapturada = partida.realizaJogada(origem.toPosicao(), destino.toPosicao());

                        if (pecaCapturada != null) {
                            removerObjetoCapturado(pecaCapturada); 
                        }

                        peca.transform.position = Util.posicaoNaCena(coluna, linha);

                        pecaEscolhida = null;




                        if (partida.terminada) {
                            estado = Estado.GameOver;
                            txtMsg.text = "Vencedor: " + partida.jogadorAtual;
                            txtXeque.text = "XEQUEMATE"; 
                        }
                        else {
                            estado = Estado.AguardandoJogada;
                            informarAguardando();
                            if (partida.xeque) {
                                txtXeque.text = "XEQUE";
                            }
                            else {
                                txtXeque.text = ""; 
                            }
                        }
                
                    }
                    catch (TabuleiroException e) {
                        peca.transform.position = Util.posicaoNaCena(origem.coluna, origem.linha);
                        estado = Estado.AguardandoJogada;
                        informarAviso(e.Message);  
                    }
                }
            }
        }
    }


    void informarAviso(string msg) {
        txtMsg.color = Color.red;
        txtMsg.text = msg;
        Invoke("InformarAguardando", 1f); 
    }
    void informarAguardando() {
        txtMsg.color = corOriginal;
        txtMsg.text = "Aguardando jogada: " + partida.jogadorAtual;  

    }
    void removerObjetoCapturado(Peca peca) {
        GameObject obj = peca.obj; 
        if (peca.cor == Cor.Branca) {
            obj.transform.position = posDescarteBrancas;
            posDescarteBrancas.z = posDescarteBrancas.z + 0.6f;
        }
        else {
            obj.transform.position = posDescartePretas;
            posDescartePretas.z = posDescartePretas.z - 0.6f;
        }
    }
}
