using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quadrado{

	private int x,y,f,h,g;
	private bool caminhavel, descoberto, inicio, destino, caminho;
	private Quadrado pai;

	public Quadrado (int x, int y, bool caminhavel)
	{
		this.x = x;
		this.y = y;
		this.caminhavel = caminhavel;
		descoberto = false;
		caminho = false;
	}

	public Quadrado Pai {
		get {
			return this.pai;
		}
		set {
			pai = value;
		}
	}

	public bool Inicio {
		get {
			return this.inicio;
		}
		set {
			inicio = value;
		}
	}

	public bool Destino {
		get {
			return this.destino;
		}
		set {
			destino = value;
		}
	}

	public int X {
		get {
			return this.x;
		}
		set {
			x = value;
		}
	}

	public int Y {
		get {
			return this.y;
		}
		set {
			y = value;
		}
	}

	public int F {
		get {
			return this.f;
		}
		set {
			f = value;
		}
	}

	public int H {
		get {
			return this.h;
		}
		set {
			h = value;
		}
	}

	public int G {
		get {
			return this.g;
		}
		set {
			g = value;
		}
	}

	public bool Caminhavel {
		get {
			return this.caminhavel;
		}
		set {
			caminhavel = value;
		}
	}

	public bool Descoberto {
		get {
			return this.descoberto;
		}
		set {
			descoberto = value;
		}
	}

	public bool Caminho {
		get {
			return this.caminho;
		}
		set {
			caminho = value;
		}
	}
}
