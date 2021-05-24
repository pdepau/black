using UnityEngine;
using UnityEngine.UI;

public class Deck : MonoBehaviour
{
    public Sprite[] faces;
    public GameObject dealer;
    public GameObject player;
    public Button hitButton;
    public Button stickButton;
    public Button playAgainButton;
    public Text finalMessage;
    public Text probMessage;

    public int[] values = new int[52];
    int cardIndex = 0;    
       
    private void Awake()
    {    
        InitCardValues();  
        

    }

    private void Start()
    {
        ShuffleCards();
        StartGame();        
    }

    private void InitCardValues()
    {
        /*TODO:
         * Asignar un valor a cada una de las 52 cartas del atributo "values".
         * En principio, la posición de cada valor se deberá corresponder con la posición de faces. 
         * Por ejemplo, si en faces[1] hay un 2 de corazones, en values[1] debería haber un 2.
         */
        int auxiliar = 1;
        for(int i = 0; i < 52; i++)
        {
            //asignar el valor de la carta
            if (auxiliar < 10)
            {
                values[i] = auxiliar;
            }
            //para 10,j,q,k todas valen 10
            if (auxiliar >= 10)
            {
                values[i] = 10;
            }
            auxiliar++;
            //en caso de ser 14 se asigna el 1 para pasar a la siguiente mano
            if (auxiliar == 14)
            {
                auxiliar = 1;
            }
        }
    }

    private void ShuffleCards()
    {
        /*TODO:
         * Barajar las cartas aleatoriamente.
         * El método Random.Range(0,n), devuelve un valor entre 0 y n-1
         * Si lo necesitas, puedes definir nuevos arrays.
         */
        //creamos dos variables para ir almacenando los valores barajados
        int[] valuesAuxiliar = new int[52];
        Sprite[] facesAuxiliar = new Sprite[52];

        int numero=0;
        int auxiliar=0;
        
        for(int i = 0; i < 52; i++)
        {
            numero = 0;
            //se genera un random entre 0 y 52, si la posicion random de values es !=0 de acaba el bucle dado que hay un numero correcto sino se vuelve a ejecutar
            while (numero == 0)
            {
                auxiliar = Random.Range(0, 52);
                numero = values[auxiliar];

            }
            //ponemos que la posicion de auxiliar sea 0 para luego guardar el valor en el valuesAuxiliar, hacemos lo mismo para faces
            values[auxiliar] = 0;
            valuesAuxiliar[i] = numero;
            facesAuxiliar[i] = faces[auxiliar];
        }
        //guardamos la nueva baraja
        values = valuesAuxiliar;
        faces = facesAuxiliar;
    }

    void StartGame()
    {
        for (int i = 0; i < 2; i++)
        {
            PushPlayer();
            PushDealer();
            /*TODO:
             * Si alguno de los dos obtiene Blackjack, termina el juego y mostramos mensaje
             */

            if (player.GetComponent<CardHand>().points == 21)
            {
                hitButton.interactable = false;
                stickButton.interactable = false;
                finalMessage.text = "HAS GANADO";
            }   
            if (dealer.GetComponent<CardHand>().points == 21)
            {
                hitButton.interactable = false;
                stickButton.interactable = false;
                finalMessage.text = "GANA LA BANCA";
            }
        }
    }

    private void CalculateProbabilities()
    {
        /*TODO:
         * Calcular las probabilidades de:
         * - Teniendo la carta oculta, probabilidad de que el dealer tenga más puntuación que el jugador
         * - Probabilidad de que el jugador obtenga entre un 17 y un 21 si pide una carta
         * - Probabilidad de que el jugador obtenga más de 21 si pide una carta          
         */
    }

    void PushDealer()
    {
        /*TODO:
         * Dependiendo de cómo se implemente ShuffleCards, es posible que haya que cambiar el índice.
         */
        dealer.GetComponent<CardHand>().Push(faces[cardIndex],values[cardIndex]);
        cardIndex++;        
    }

    void PushPlayer()
    {
        /*TODO:
         * Dependiendo de cómo se implemente ShuffleCards, es posible que haya que cambiar el índice.
         */
        player.GetComponent<CardHand>().Push(faces[cardIndex], values[cardIndex]/*,cardCopy*/);
        cardIndex++;
        CalculateProbabilities();
    }       

    //hit=pedir carta
    public void Hit()
    {
        /*TODO: 
         * Si estamos en la mano inicial, debemos voltear la primera carta del dealer.
         */
        if (cardIndex < 5)
        {
            dealer.GetComponent<CardHand>().InitialToggle();
        }

        //Repartimos carta al jugador
        PushPlayer();

        /*TODO:
         * Comprobamos si el jugador ya ha perdido y mostramos mensaje
         */
        if (player.GetComponent<CardHand>().points>21)
        {
            hitButton.interactable = false;
            stickButton.interactable = false;
            finalMessage.text = "GANA LA BANCA";
        }

    }
    //stand = plantarse
    public void Stand()
    {
        /*TODO: 
         * Si estamos en la mano inicial, debemos voltear la primera carta del dealer.
         */
        if (cardIndex < 5)
        {
            dealer.GetComponent<CardHand>().InitialToggle();
        }
        hitButton.interactable = false;
        stickButton.interactable = false;
        /*TODO:
         * Repartimos cartas al dealer si tiene 16 puntos o menos
         * El dealer se planta al obtener 17 puntos o más
         * Mostramos el mensaje del que ha ganado
         */
        //pedimos cartas hasta que el crupier teng mas de 16

        while (dealer.GetComponent<CardHand>().points < 17)
        {
            PushDealer();
        } 

        if(player.GetComponent<CardHand>().points > 21|| dealer.GetComponent<CardHand>().points > 21)
        {
            if (player.GetComponent<CardHand>().points > 21)
            {
                finalMessage.text = "GANA LA BANCA";
            }

            if (dealer.GetComponent<CardHand>().points > 21)
            {
                finalMessage.text = "HAS GANADO";
            }
        }
        else
        {
            if (dealer.GetComponent<CardHand>().points == 21 || dealer.GetComponent<CardHand>().points > player.GetComponent<CardHand>().points)
            {
                finalMessage.text = "GANA LA BANCA";
            }

            if (player.GetComponent<CardHand>().points == 21 || player.GetComponent<CardHand>().points > dealer.GetComponent<CardHand>().points)
            {
                finalMessage.text = "HAS GANADO";
            }

            if (player.GetComponent<CardHand>().points == dealer.GetComponent<CardHand>().points)
            {
                finalMessage.text = "EMPATE";
            }
        }

        
        
    }

    public void PlayAgain()
    {
        hitButton.interactable = true;
        stickButton.interactable = true;
        finalMessage.text = "";
        player.GetComponent<CardHand>().Clear();
        dealer.GetComponent<CardHand>().Clear();          
        cardIndex = 0;
        ShuffleCards();
        StartGame();
    }   
    
}
