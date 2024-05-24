using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

/// <summary>
/// Main brains of the pair game, dealing with spawning, shuffling, receiving card selections, matching and gameover 
/// you can easily add additional code to CardsMatched(), GameStarted(), GameOver() which are called at key points of the game
/// Vector(x,y) x= rows y=columns
/// </summary>
public class GameManager : Singleton<GameManager>
{
    [SerializeField] GridScaler m_GridScaler;
    [SerializeField] GameObject m_Prefab;
    [SerializeField] Transform m_Parent;

    [SerializeField] TimerManager m_Timer;
    [SerializeField] TriesManager m_TriesManager;

    private List<Sprite> m_Items;

    [SerializeField] Game_TextShowDisplayCardName m_DisplayCardName;

    private List<Card> m_Cards;
    private Dictionary<int, Card> m_SelectedCards;
    private Dictionary<int, Card> m_MatchedCards;

    public bool IsPaused {
        get
        {
            return m_IsPaused;
        }
        set
        {
            m_Timer.IsPaused = value;

            m_IsPaused = value;
        }
    }
    private bool m_IsPaused = false;

    /// <summary>
    /// Used in conjunction with pause dialog, to dermine whether
    /// a user has even interacted with a card
    /// </summary>
    public bool UserInteractedWithCard { get; private set; }

    private void Start()
    {
        Vector2 grid = GridConfigManager.Instance.SelectedGridSize;
        Setup((int)grid.x, (int)grid.y);

        UserInteractedWithCard = false;
    }

    public void Setup(int _rowCount, int _columnCount)
    {
        // added this so it's easier to manage
        m_Items = CardSpriteManager.Instance.Sprites;

        m_GridScaler.Setup(_rowCount, _columnCount);

        int total = _rowCount * _columnCount;

        int pairCount = total / 2;
        
        if(total % 2 != 0)
        {
            Debug.LogError("There is an odd card, totalCards=" + total + " - reconsider changing rowCount or column count so total == even number");
            return;
        }

        if(m_Items.Count < pairCount)
        {
            Debug.LogError("Sprite limitation: GameManager Items --> only has: " + m_Items.Count + "different items/sprites in list, requires items count= " + pairCount + "  sprites");
            return;
        }

        // list of ids that will be used
        List<int> ids = new List<int>();

        for (int i = 0; i < pairCount; i++)
        {
            while(true)
            {
                int id = Random.Range(0, m_Items.Count);

                if(!ids.Contains(id))
                {
                    ids.Add(id);    // draw two pairs and store in ids
                    ids.Add(id);
                    break;
                }
            }
        }

        // Shuffle list
        ids = Shuffle(ids);

        // spawn objects
        m_Cards = new List<Card>();

        m_SelectedCards = new Dictionary<int, Card>();
        m_MatchedCards = new Dictionary<int, Card>();

        for (int i = 0; i < total; i++)
        {
            //creamos un objeto el objeto poseera un prefab base colocando como lo hijo del objeto m_parent, y unas coordenadas transform del grid padre
            GameObject obj = Instantiate(m_Prefab, m_Parent, false);
            //si bien es cierto se crea el objeto, el objeto no se muestra en pantalla va a ser utilizado en la siguiente linea
            //almacenaremos en una variable local card el componente local obj 
            Card card = obj.GetComponent<Card>();
            card.Setup(i, ids[i], m_Items[ids[i]]);
            //usamos el metodo card setup, en la primera vuelta 
            //setup ingresara el numero 0 como su childId,
            // i = posicion en la lista m_cards, en total seran 6
            // ids[i] = id o codigo de la imagen o del objeto, en total seran 3
            // m_items = usa ese codigo para tomar su imagen 
            m_Cards.Add(card);
            // m_cards es la lista que se representara en la grid,
            // es una lista de objetos que tendran su id y tendran su posicion en la grilla
            
        }

        GameStarted();
    }

    /// <summary>
    /// User selected a card
    /// </summary>
    /// <param name="childid"></param>
    public void SelectedCard(int childid)
    {
        // If the game is paused, then prevent the cards from doing anything e.g animating 
        if(m_IsPaused)
        {
            Debug.Log("Game is paused, user cannot do anything, until the game is unpaused");
            return;
        }

        // Used in conjunction with gamescreen.cs to determine whether not the game should show, are sure popup
        if(!UserInteractedWithCard)
        {
            UserInteractedWithCard = true;
        }

        Card currentCard = m_Cards[childid];


        if (m_SelectedCards.Count == 2)
        {

            foreach(KeyValuePair<int, Card> card in m_SelectedCards)
            {
                if(!m_MatchedCards.ContainsKey(card.Key))
                {
                    card.Value.HideCard();
                }
            }

            m_SelectedCards = new Dictionary<int, Card>();
        }

        if(m_MatchedCards.Count > 0)
        {
            foreach(KeyValuePair<int, Card> card in m_MatchedCards)
            {
                if(childid == card.Key)
                {
                    Debug.Log("Card already exists in 'matched' stack - user has selected card that is already showing and 'matched'");
                    return;
                }
            }
        }

        currentCard.ShowCard();
        m_DisplayCardName.Show(currentCard.Sprite.name);
        SoundManager.Instance.Play_CardPlaced();

        // first card, show no need to do any matching algorithm
        if (m_SelectedCards.Count == 0)
        {
            m_SelectedCards.Add(childid, currentCard);
            return;
        }

     
        if(m_SelectedCards.ContainsKey(childid))
        {
            Debug.Log("Card already exists in selected stack - user has selected card that is already showing");
            return;
        }


        // add to selected cards
        if(m_SelectedCards.Count < 2)
        {
            m_SelectedCards.Add(childid, currentCard);
        }


        // check for a match
        if(m_SelectedCards.Count == 2)
        {

            bool matched = false;
            int? pairId = null;

            foreach (KeyValuePair<int, Card> card in m_SelectedCards)
            {
                if (pairId == null)
                    pairId = card.Value.PairId;
                else if (pairId == card.Value.PairId)
                    matched = true;
                else
                {
                    Debug.Log("More than on item in list did not match");
                    matched = false;
                }
            }

            if(matched)
            {
                foreach(KeyValuePair<int, Card> card in m_SelectedCards)
                {
                    m_MatchedCards.Add(card.Key, card.Value);
                }

                CardsMatched();

                CheckGameOver();
            }
            else
            {
                CardsDidNotMatch();
            }

            m_TriesManager.UserTried();

            return;
        }
    }

    /// <summary>
    /// Restarts level to defaultgrid
    /// </summary>
    public void Restart()
    {
        for (int i = 0; i < m_Cards.Count; i++)
        {
            Destroy(m_Cards[i].gameObject);
        }

        m_Cards = new List<Card>();
        m_SelectedCards = new Dictionary<int, Card>();
        m_MatchedCards = new Dictionary<int, Card>();

        Vector2 grid = GridConfigManager.Instance.SelectedGridSize;
        Setup((int)grid.x, (int)grid.y);
    }

    private void CheckGameOver()
    {
        if(m_MatchedCards.Count == m_Cards.Count)
        {
            GameOver();
        }
    }

    private void GameStarted()
    {
        Debug.Log("Game STARTED");

        m_Timer.ResetTimer();
        m_Timer.StartTimer();

        m_TriesManager.Reset();
    }

    private void GameOver()
    {
        Debug.Log("GAME OVER");

        GridUnlockManager.Instance.CompletedCurrentLevel();
      
        m_Timer.StopTimer();

        ScoreManager.Instance.UserScored(GridConfigManager.Instance.SelectedGridSize, m_TriesManager.Tries, m_Timer.TotalTimeInSeconds);

        StartCoroutine(Delay(.5f, () =>
        {
            ScreenManager.Instance.ChangeScreen(Screen.EndGame);
        }));
    }

    private System.Collections.IEnumerator Delay(float time, UnityAction complete)
    {
        yield return new WaitForSeconds(time);

        if(complete != null)
        {
            complete();
        }

    }

    private void CardsMatched()
    {
        Debug.Log("Two cards matched!");
        SoundManager.Instance.Play_CardsMatched();
    }

    private void CardsDidNotMatch()
    {
        Debug.Log("Cards did not match");
    }

    private List<int> Shuffle(List<int> ts)
    {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }

        return ts;
    }
}
