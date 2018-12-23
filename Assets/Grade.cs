using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grade : MonoBehaviour {

	public int w,h;
	public Mesh mesh;
	public Material material, matAberta, matFechada;

	private Quadrado[,] grade;
	private List<Quadrado> listaAberta = new List<Quadrado>();
	private List<Quadrado> listaFechada = new List<Quadrado>();

	void Start () {
		grade = new Quadrado[h, w];

		GameObject quadradoGO;

		#region Criacao do Quadrado
		for(int i=0; i<h; i++){
			for (int j = 0; j < w; j++) {
				grade[i,j] = new Quadrado(i,j,true);
			}
		}
		#endregion

		grade [4, 9].Caminhavel = false;
		grade [4, 8].Caminhavel = false;
		grade [4, 7].Caminhavel = false;
		grade [4, 6].Caminhavel = false;
		grade [4, 5].Caminhavel = false;
		grade [4, 4].Caminhavel = false;
		grade [4, 3].Caminhavel = false;
		grade [4, 2].Caminhavel = true;
		grade [4, 1].Caminhavel = false;
		grade [4, 0].Caminhavel = false;

		grade [5, 4].Caminhavel = false;
		grade [5, 5].Caminhavel = false;
		grade [6, 5].Caminhavel = false;
		grade [6, 4].Caminhavel = false;
		grade [6, 3].Caminhavel = false;
		grade [6, 2].Caminhavel = false;
		grade [7, 5].Caminhavel = false;
		grade [8, 5].Caminhavel = false;

		grade [2, 6].Inicio = true;

		grade [6, 6].Destino = true;

		PathFinding (grade [2, 6], grade [6, 6]);

		MeshRenderer[,] renderers = new MeshRenderer[h,w];

		#region Criacao do GameObject
		for(int i=0; i<h; i++){
			for (int j = 0; j < w; j++) {
				
				quadradoGO = new GameObject ();

				MeshRenderer renderer = quadradoGO.AddComponent<MeshRenderer> ();
				MeshFilter meshFilter = quadradoGO.AddComponent<MeshFilter> ();

				if(grade[i,j].F != 0){
					GameObject textGO = new GameObject("Texto");
					textGO.transform.SetParent(quadradoGO.transform);
					textGO.transform.position = Vector3.zero;

					TextMesh text = textGO.AddComponent<TextMesh>();
					text.text = "F="+grade[i,j].F+"\nG="+grade[i,j].G+"\nH="+grade[i,j].H+"\nSeq="+grade[i,j].Seq;
					text.anchor = TextAnchor.MiddleCenter;
					textGO.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
				}

				if(grade[i,j].Descoberto)
					renderer.material = new Material(matAberta);
				else
					renderer.material = new Material(material);

				meshFilter.mesh = mesh;
				quadradoGO.transform.position = new Vector3 (i, j, 0);

				if(!grade[i,j].Caminhavel)
					renderer.material.color = Color.blue;

				if(grade[i,j].Inicio)
					renderer.material.color = Color.green;

				if(grade[i,j].Destino)
					renderer.material.color = Color.red;

				renderers[i,j] = renderer;
			}
		}
		#endregion

		Quadrado atual = grade [6, 6];
		while (atual != grade [2, 6]) {
			atual = atual.Pai;
			atual.Caminho = true;
		}

		for (int i = 0; i < h; i++) {
			for (int j = 0; j < w; j++) {
				if (grade [i, j].Caminho && !grade [i, j].Inicio && !grade [i, j].Destino)
					renderers [i, j].material.color = Color.yellow;
			}
		}

	}

	private int DistanciaManhattan (Quadrado inicio, Quadrado destino){
		int x1, x2, y1, y2;

		x1 = inicio.X;
		y1 = inicio.Y;
		x2 = destino.X;
		y2 = destino.Y;

		int distancia = 0;

		while (x1 != x2) {
			if (x1 > x2)
				x2++;
			if (x1 < x2)
				x2--;
			distancia++;
		}
		while (y1 != y2) {
			if (y1 > y2)
				y2++;
			if (y1 < y2)
				y2--;
			distancia++;
		}

		return distancia*10;
	}

	private bool PathFinding(Quadrado inicio, Quadrado destino){
		Quadrado quadradoAtual = inicio;
		int cont = 0;

		while (quadradoAtual != destino && cont < 5000) {
			if (!listaFechada.Contains (quadradoAtual)) {
				listaFechada.Add (quadradoAtual);
				quadradoAtual.Seq = cont;
				if (listaAberta.Contains (quadradoAtual))
					listaAberta.Remove (quadradoAtual);
			}
			
			quadradoAtual = VerificarPerimetro (quadradoAtual, destino);

			print (quadradoAtual.X+" , "+quadradoAtual.Y);
			cont++;
		}
		print (cont);
		return true;
	}

	private Quadrado VerificarPerimetro(Quadrado quadradoAtual, Quadrado quadradoDestino){
		int x, y;

		x = quadradoAtual.X - 1;
		y = quadradoAtual.Y - 1;

		Quadrado menorCusto = null;

		for(int i=0; i<3; i++){
			for (int j=0; j<3; j++) {
				if (i + x < w && j + y < h && j+y >= 0 && i + x >= 0) {
					
					if (grade [i + x, j + y].Caminhavel && !(i + x == 1 && j + y == 1) && !listaFechada.Contains (grade [i + x, j + y])) {

						if (!listaAberta.Contains (grade [i + x, j + y])) {
							listaAberta.Add (grade [i + x, j + y]);
							grade [i + x, j + y].Pai = quadradoAtual;

							grade [i + x, j + y].H = DistanciaManhattan (grade [i + x, j + y], quadradoDestino);
							grade [i + x, j + y].G = CalculaCusto (i, j);
							grade [i + x, j + y].F = grade [i + x, j + y].G + grade [i + x, j + y].H;
						} else {
							if (CalculaCusto (i, j) < grade [i + x, j + y].G) {
								grade [i + x, j + y].Pai = quadradoAtual;

								grade [i + x, j + y].H = DistanciaManhattan (grade [i + x, j + y], quadradoDestino);
								grade [i + x, j + y].G = CalculaCusto (i, j);
								grade [i + x, j + y].F = grade [i + x, j + y].G + grade [i + x, j + y].H;
							}
						}

						if (menorCusto == null)
							menorCusto = grade [i + x, j + y];

						if (grade [i + x, j + y].F < menorCusto.F)
							menorCusto = grade [i + x, j + y];
					}
				}
			}
		}

		return (menorCusto == null) ? quadradoAtual : menorCusto;
	}

	public int CalculaCusto(int i, int j){
		if (i == j || (i == 0 && j == 2) || (i == 2 && j == 0))
			return 14;
		else
			return 10;	
	}
}
