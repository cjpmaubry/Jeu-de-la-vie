using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EsilvGui;

namespace Console_Application_jeu_de_la_vie_C.A
{
    class Program
    {
        static void AffichageGrille(int[,] Matrice)
        {//Cette fonction permet d'afficher une matrice d'entier dans la console.           
            for (int indexLigne = 0; indexLigne < Matrice.GetLength(0); indexLigne++)
            {
                for (int indexColonne = 0; indexColonne < Matrice.GetLength(1); indexColonne++)
                {
                    Console.Write(Matrice[indexLigne, indexColonne]);
                }
                Console.WriteLine();
            }
        }

        static int[,] Grille(int ligne, int colonne)
        {// Cette fonction permet la création d'une "Grille de jeu" en fonction du nombre de ligne et de colonne entrer par l'utilisateur.
            int[,] Grilledejeu = new int[ligne, colonne];
            for (int Ligne = 0; Ligne < Grilledejeu.GetLength(0); Ligne++)
            {
                for (int Colonne = 0; Colonne < Grilledejeu.GetLength(1); Colonne++)
                {
                    Grilledejeu[Ligne, Colonne] = 0; // Remplissage de la matrice par des cellules mortes pour initialiser
                }
            }
            return Grilledejeu;
        }

        static void Grainedegénération(int[,] Matrice, double TauxDeRemplissage)
        {// Cette fonction permet de générer une population de facon aléatoire dans une matrice entrée en paramètre. La proportion de la population est également définie en paramètre.
            Random generateur = new Random();
            double nombredecelluleinitialle = Matrice.Length * TauxDeRemplissage;           
            for (int i = 0; i < nombredecelluleinitialle; i++)
            {
                int CoordonneeLigne = generateur.Next(0, Matrice.GetLength(0));
                int CoordonneeColonne = generateur.Next(0, Matrice.GetLength(1));
                if (Matrice[CoordonneeLigne, CoordonneeColonne] == 1)
                    i--;
                else
                    Matrice[CoordonneeLigne, CoordonneeColonne] = 1;
            }
        }

        static int CompteurDeVoisin(int[,] Matrice, int[] Coordonnee)
        {// Cette fonction compte le nombre de cellule vivante autour d'une cellule dont les coordonnées ont été entrer en paramètre. 
            int compteur = 0;
            for (int indexLigne = 0; indexLigne < 3; indexLigne++)
            {
                for (int indexColonne = 0; indexColonne < 3; indexColonne++)
                {
                    int CoordonneeLigne = (((Coordonnee[0]-1  + indexLigne) % Matrice.GetLength(0)) + Matrice.GetLength(0)) % Matrice.GetLength(0);
                    int CoordonneeColonne = (((Coordonnee[1]-1  + indexColonne) % Matrice.GetLength(1)) + Matrice.GetLength(1)) % Matrice.GetLength(1);

                    if ((Coordonnee[0] != CoordonneeLigne) || (Coordonnee[1] != CoordonneeColonne))
                    {

                        if (Matrice[CoordonneeLigne, CoordonneeColonne] == 1)
                            compteur++;
                    }

                }
            }
            return compteur;
        }

        static void Generation(int NombreDeVoisin, int[] Coordonnee, int[,] MatriceLecture, int[,] MatriceEcriture)
        {//Cette fonction change le statue de la cellule dont les coordonnées ont été entré en paramètre, c'est à dire morte ou vivante.
            
                switch (NombreDeVoisin)
                {
                    case 0:
                    case 1:
                        MatriceEcriture[Coordonnee[0], Coordonnee[1]] = 0;
                        break;
                    case 2:
                        if (MatriceLecture[Coordonnee[0], Coordonnee[1]] == 0)
                        {
                            MatriceEcriture[Coordonnee[0], Coordonnee[1]] = 0;
                            break;
                        }
                        else
                        {
                            MatriceEcriture[Coordonnee[0], Coordonnee[1]] = 1;
                            break;
                        }
                    case 3:
                        MatriceEcriture[Coordonnee[0], Coordonnee[1]] = 1;
                        break;
                    case 4:
                    case 5:
                    case 6:
                    case 7:
                    case 8:
                        MatriceEcriture[Coordonnee[0], Coordonnee[1]] = 0;
                        break;

                }
            }
        
        static void Reecriture(int[,] Matrice1, int[,] Matrice2)
        {//Cette fonction change les valeurs presentent dans la Matrice 1 à partir des valeurs de la Matrice 2. 
            for (int indexLigne = 0; indexLigne < Matrice1.GetLength(0); indexLigne++)
            {
                for (int indexColonne = 0; indexColonne < Matrice1.GetLength(1); indexColonne++)
                {
                    Matrice1[indexLigne, indexColonne] = Matrice2[indexLigne, indexColonne];
                }
            }
        }

        static void Tour(int[,] Matrice,bool EtatIntermediaire)
        {//Cette fonction sert à exécuter plusieurs fonctions precedemment définie selon un ordre précis.
            int NombreDeVoisin = 0;
            int[] Coordonnee = new int[2];
            int[,] MatriceTransition = new int[Matrice.GetLength(0), Matrice.GetLength(1)];
            for (int indexLigne = 0; indexLigne < Matrice.GetLength(0); indexLigne++)
            {
                for (int indexColonne = 0; indexColonne < Matrice.GetLength(1); indexColonne++)
                {
                    Coordonnee[0] = indexLigne;
                    Coordonnee[1] = indexColonne;
                    NombreDeVoisin = CompteurDeVoisin(Matrice, Coordonnee);
                    Generation(NombreDeVoisin, Coordonnee, Matrice,MatriceTransition);
                }           
            }
            // Partie relatif à l'affichage des états intermédiares, peut étre activé ou non
            if (EtatIntermediaire==true)
            AffichageEtatIntermédiaire(MatriceTransition,Matrice);
            Reecriture(Matrice, MatriceTransition);
        }

        static int CompteurDeCelluleVivante(int[,] Matrice)
        {// Cette fonction compte le nombre de cellule vivante sur une matrice entrée en paramètre.
            int NombreDeCelluleVivante = 0;
            for (int indexLigne = 0; indexLigne < Matrice.GetLength(0); indexLigne++)
            {
                for (int indexColonne = 0; indexColonne < Matrice.GetLength(1); indexColonne++)
                {
                    if (Matrice[indexLigne, indexColonne] == 1)
                        NombreDeCelluleVivante++;
                }
            }
            Console.WriteLine(" Il y a "+NombreDeCelluleVivante+ " cellule(s) vivante(s)");
            return NombreDeCelluleVivante;
        }

        static void AffichageEtatIntermédiaire(int[,] MatriceTransition, int[,] Matrice)
        {// Cette fonction permet d'afficher l'état intermédiaire entre 2 générations de cellule. Pour cela, elle compare les changement entre 2 matrices fournis.
            int[,] MatriceEtatIntermediaire = new int[Matrice.GetLength(0), Matrice.GetLength(1)];
            for (int indexLigne = 0; indexLigne < Matrice.GetLength(0); indexLigne++)
            {
                for (int indexColonne = 0; indexColonne < Matrice.GetLength(1); indexColonne++)
                {
                    if (Matrice[indexLigne, indexColonne] == MatriceTransition[indexLigne, indexColonne])
                        MatriceEtatIntermediaire[indexLigne, indexColonne] = Matrice[indexLigne, indexColonne];
                    else
                    {
                        if (MatriceTransition[indexLigne, indexColonne] == 1 && Matrice[indexLigne, indexColonne] == 0)
                            MatriceEtatIntermediaire[indexLigne, indexColonne] = 2;
                        else
                        {
                            if (MatriceTransition[indexLigne, indexColonne] == 0 && Matrice[indexLigne, indexColonne] == 1)
                                MatriceEtatIntermediaire[indexLigne, indexColonne] = 3;
                        }
                    }                
                }
            }
            PassageEntierSymbole(MatriceEtatIntermediaire);
            

        }

        static void PassageEntierSymbole(int[,] Matrice)
        {//Cette fonction permet de passer à un affiche par symbole sur la console à partir d'une matrice d'entier.
            string[,] MatriceSymbole = new string[Matrice.GetLength(0), Matrice.GetLength(1)];
            for (int indexLigne = 0; indexLigne < Matrice.GetLength(0); indexLigne++)
            {
                for (int indexColonne = 0; indexColonne < Matrice.GetLength(1); indexColonne++)
                {
                    if (Matrice[indexLigne, indexColonne] == 0)
                        MatriceSymbole[indexLigne, indexColonne] ="." ;
                    else
                    {
                        if ( Matrice[indexLigne, indexColonne] == 1)
                            MatriceSymbole[indexLigne, indexColonne] = "#" ;
                        else
                        {
                            if (Matrice[indexLigne, indexColonne] == 2)
                                MatriceSymbole[indexLigne, indexColonne] ="-" ;
                            else
                            {
                                if (Matrice[indexLigne, indexColonne] == 3)
                                    MatriceSymbole[indexLigne, indexColonne] ="*" ;
                            }
                        }
                    }
                }
            }
            AffichageGrille2(MatriceSymbole);
        }

        static void AffichageGrille2(string[,] Matrice)
        {////Cette fonction permet d'afficher une matrice de caractères dans la console.
            for (int indexLigne = 0; indexLigne < Matrice.GetLength(0); indexLigne++)
            {
                for (int indexColonne = 0; indexColonne < Matrice.GetLength(1); indexColonne++)
                {
                    Console.Write(Matrice[indexLigne, indexColonne]);
                }
                Console.WriteLine();
            }
        }

        [System.STAThreadAttribute()]
        static void Main(string[] args)
        {
            // 1e partie, ici est défini toute les instructions permettants de demander à l'utilisateur les paramètres choisies
            Console.WriteLine("Jeu de la vie");
            Console.WriteLine("Merci de bien vouloir intialiser les paramètres de génération");
            Console.WriteLine("Entrer taux de remplissage de cellules vivantes au départ c'est à dire une valeur réelle comprise entre [0.1,0.9] ");
            double TauxDeRemplissage = Convert.ToDouble(Console.ReadLine());
            while ((TauxDeRemplissage <= 0.1) || (TauxDeRemplissage > 0.9))
            {
            Console.WriteLine("Erreur valeur saisie mauvaise !");
            Console.WriteLine("Merci de bien vouloir entrer taux de remplissage de cellules vivantes au départ c'est à dire une valeur réelle comprise entre [0.1,0.9] ");
            TauxDeRemplissage = Convert.ToDouble(Console.ReadLine());
            }

            Console.WriteLine("Entrer la valeur de la longueur de la grille");
            int Longueur = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Entrer la valeur de la largeur de la grille");
            int Largeur = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Voulez vous afficher les etats intermediaire entre les tours ?");
            Console.WriteLine("Si oui taper 1 sinon sur n'importe quelle autre touche ?");

            string Valeur = Console.ReadLine();
            bool AffichageEtatIntermediaire = false;
            if (Valeur == "1")
                AffichageEtatIntermediaire = true;

            int[,] tableaudetest = Grille(Longueur, Largeur);           
            Grainedegénération(tableaudetest, TauxDeRemplissage);          
            int CaseRemplie = Convert.ToInt32( tableaudetest.Length * TauxDeRemplissage);
            Console.WriteLine("Le taux de remplissage initiale est de " + TauxDeRemplissage + " soit de "+ CaseRemplie+ " cellule sur " +tableaudetest.Length);
            Console.WriteLine();
            AffichageGrille(tableaudetest);         
            Console.WriteLine("Appuyer sur entrer pour passer au tour suivant ");
            Console.WriteLine("Appuyer sur espace puis entrer pour quitter");


            // 2eme zone, ici est paramétré la GUI
            Fenetre gui = new Fenetre(tableaudetest, 15, 0, 0, "Jeu de la vie");
            String Touche="0";
            int NumeroDeLaGeneration = 0;
            Console.WriteLine("Appuyer sur une touche pour lancer la 1e generation.");
            Console.ReadKey();


            // 3eme zone, ici est défini la boucle qui permet de passer d'une génération à une autre
            while (Touche != " ")
            {
                Tour(tableaudetest, AffichageEtatIntermediaire);
                Console.WriteLine();
                AffichageGrille(tableaudetest);
                Console.WriteLine();
                NumeroDeLaGeneration++;
                gui.Rafraichir();
                gui.ChangerMessage("Génération " + NumeroDeLaGeneration + " Il y a " + CompteurDeCelluleVivante(tableaudetest) + " cellules vivantes");
                Touche = Console.ReadLine();
            }
            
        }
    }
}
