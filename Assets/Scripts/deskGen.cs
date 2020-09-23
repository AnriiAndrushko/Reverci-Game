using UnityEngine;
using UnityEngine.UI;

public class deskGen : MonoBehaviour
{
    [SerializeField] GameObject cell;
    [SerializeField] Transform canvas;
    [SerializeField] private Text whoseTurnText;
    [SerializeField] private Sprite[] TexturesOfCell;
    [SerializeField] private Text ScoreText;
    const float rastoianie =62f;
    const int descSise = 8;
    private GameObject[] cellBufer;
    private Image[] cellBuferImages;
    private Image[] PlayerBuferImages;
    private bool whoseTurn = true;
    private int[] stateMass;
    private int moveCount = 0;
    private int blackCount = 0;
    private int whiteCount = 0;
    private bool isGameEnd = false;
    private bool wasSkiped = false;

    void Start()
    {
        cellBufer = new GameObject[descSise * descSise];
        cellBuferImages = new Image[descSise * descSise];
        PlayerBuferImages = new Image[descSise * descSise];
        stateMass = new int[descSise * descSise];
        whoseTurn = true;
        for (int i = 0; i < descSise; i++)
        {
            for (int j = 0; j < descSise; j++)
            {
                stateMass[j + i * descSise] = 0;
                cellBufer[j + i * descSise] = Instantiate(cell, new Vector3(transform.position.x + j * rastoianie, transform.position.y + i * rastoianie, 0), Quaternion.identity, canvas);
                cellBufer[j + i * descSise].name = "cell " + (i * descSise + j);
                cellBuferImages[j + i * descSise] = cellBufer[j + i * descSise].GetComponent<Image>();
                PlayerBuferImages[j + i * descSise] = cellBufer[j + i * descSise].GetComponentsInChildren<Image>()[1];
            }
        }

        stateMass[28] = 2;
        PlayerBuferImages[28].color = Color.white;
        stateMass[27] = 1;
        PlayerBuferImages[27].color = Color.black;
        stateMass[35] = 2;
        PlayerBuferImages[35].color = Color.white;
        stateMass[36] = 1;
        PlayerBuferImages[36].color = Color.black;

        CheckAvaliableMoves();
    }

    public void CheckAvaliableMoves()
    {

        blackCount = 0;
        whiteCount = 0;
        int player = 0;
        int enemy = 0;
        if (!whoseTurn)
        {
            player = 1;
            enemy = 2;
        }
        else
        {
            player = 2;
            enemy = 1;
        }
        bool haveAvaliableTurns = false;
        for (int i = 0; i < descSise; i++)
        {
            for (int j = 0; j < descSise; j++)
            {
                if (PlayerBuferImages[i*descSise+j].color == Color.black)
                {
                    blackCount++;
                }
                else if (PlayerBuferImages[i * descSise + j].color == Color.white)
                {
                    whiteCount++;
                }

                cellBuferImages[i * descSise + j].sprite = TexturesOfCell[0];
                int curnetCell = i * descSise + j;
                if (stateMass[curnetCell] == 0)
                {
                    if (Check(curnetCell, player, enemy, curnetCell % descSise + 1, -1, 0) ||
                    Check(curnetCell, player, enemy, descSise - curnetCell % descSise + 1, 1, 0) ||
                    Check(curnetCell, player, enemy, curnetCell / descSise + 1, 0, -1) ||
                    Check(curnetCell, player, enemy, descSise - curnetCell / descSise, 0, 1) ||
                    Check(curnetCell, player, enemy, curnetCell / descSise + 1, 1, -1) ||
                    Check(curnetCell, player, enemy, curnetCell / descSise + 1, -1, -1) ||
                    Check(curnetCell, player, enemy, descSise - curnetCell / descSise, -1, 1) ||
                    Check(curnetCell, player, enemy, descSise - curnetCell / descSise, 1, 1))
                    {
                        cellBuferImages[i * descSise + j].sprite = TexturesOfCell[1];
                        haveAvaliableTurns = true;
                    }
                }

            }
        }

        if (!haveAvaliableTurns && moveCount < 60&&wasSkiped)
        {
            if (blackCount > whiteCount)
            {
                whoseTurnText.text = "Черные выиграли";
            }
            else if (blackCount == whiteCount)
            {
                whoseTurnText.text = "Ничья";
            }
            else
            {
                whoseTurnText.text = "Белые выиграли";
            }
            Debug.Log("коне игры");
            isGameEnd = true;
            wasSkiped = false;
        }


        if (!haveAvaliableTurns&&moveCount<60)
        {
            whoseTurn = !whoseTurn;
            wasSkiped = true;
            CheckAvaliableMoves();
            Debug.Log("скипай ход");
        }
        else if (moveCount == 60)
        {
            if (blackCount > whiteCount)
            {
                whoseTurnText.text = "Черные выиграли";
            }
            else if (blackCount == whiteCount)
            {
                whoseTurnText.text = "Ничья";
            }
            else
            {
                whoseTurnText.text = "Белые выиграли";
            }
            Debug.Log("коне игры");
            isGameEnd = true;
        }
        else
        {
            moveCount++;
        }

        if (whoseTurn&&!isGameEnd)
        {
            whoseTurnText.text = "Сейчас ходит: черный";
        }
        else if(!isGameEnd)
        {
            whoseTurnText.text = "Сейчас ходит: белый";
        }


        ScoreText.text = "Черные " + blackCount + ":" + whiteCount + " Белые";
    }

    public void DoTurn(int numberOfCell)
    {
        if(cellBuferImages[numberOfCell].sprite == TexturesOfCell[1])
        {
            int player = 0;
            int enemy = 0;
            if (!whoseTurn)
            {
                player = 1;
                enemy = 2;
            }
            else
            {
                player = 2;
                enemy = 1;
            }

            if (whoseTurn)
            {
                stateMass[numberOfCell] = 1;
                whoseTurn = false;
                PlayerBuferImages[numberOfCell].color = Color.black;
            }
            else
            {
                stateMass[numberOfCell] = 2;
                whoseTurn = true;
                PlayerBuferImages[numberOfCell].color = Color.white;
            }
            Check(numberOfCell, player, enemy, numberOfCell % descSise + 1, -1, 0, true);
            Check(numberOfCell, player, enemy, descSise - numberOfCell % descSise + 1, 1, 0, true);
            Check(numberOfCell, player, enemy, numberOfCell / descSise + 1, 0, -1, true);
            Check(numberOfCell, player, enemy, descSise - numberOfCell / descSise, 0, 1, true);
            Check(numberOfCell, player, enemy, numberOfCell / descSise + 1, 1, -1, true);
            Check(numberOfCell, player, enemy, numberOfCell / descSise + 1, -1, -1, true);
            Check(numberOfCell, player, enemy, descSise - numberOfCell / descSise, -1, 1, true);
            Check(numberOfCell, player, enemy, descSise - numberOfCell / descSise, 1, 1, true);

            CheckAvaliableMoves();
        }
    }

    bool Check(int numberOfCell, int whoseTurn, int enemy, int loopLim, int indexI, int indexDI, bool chekOrTurn = false)
    {
        int enemyCount = 0;
        for (int i = 1; i < loopLim; i++)
        {
            if ((numberOfCell + i * indexI + descSise * i * indexDI) / descSise != (numberOfCell + descSise * i * indexDI) / descSise || numberOfCell + descSise * i*indexDI + i*indexI < 0 || numberOfCell + descSise * i * indexDI + i * indexI > descSise*descSise-1)
            {
              break;
            }
            if (stateMass[numberOfCell + indexI * i + indexDI * i * descSise] != 0)
            {
                if (enemyCount > 0 && stateMass[numberOfCell + indexI * i + indexDI * i * descSise] == whoseTurn)
                {
                if (chekOrTurn) { 
                    for (int c = 0; c <= enemyCount + 1; c++)
                    {
                        stateMass[numberOfCell + c * indexI + c * indexDI * descSise] = whoseTurn;
                        if (whoseTurn == 1)
                        {
                            PlayerBuferImages[numberOfCell + c * indexI + c * indexDI * descSise].color = Color.black;
                        }
                        else
                        {
                            PlayerBuferImages[numberOfCell + c * indexI + c * indexDI * descSise].color = Color.white;
                        }
                    }
                }
                    return true;
                }
                else if (i >= 1 && stateMass[numberOfCell + indexI * i + indexDI * i * descSise] == whoseTurn)
                {
                    break;
                }
                if (i >= 1 && stateMass[numberOfCell + indexI * i + indexDI * i * descSise] == enemy)
                {
                    enemyCount++;
                }
            }
            else
            {
                break;
            }
        }
        return false;
    }

    public void Restart()
    {
        wasSkiped = false;
        isGameEnd = false;
        moveCount = 0;
        whoseTurn = true;
        for (int i = 0; i < descSise; i++)
        {
            for (int j = 0; j < descSise; j++)
            {
                Destroy(cellBufer[j + i * descSise]);
                stateMass[j + i * descSise] = 0;
                cellBufer[j + i * descSise] = Instantiate(cell, new Vector3(transform.position.x + j * rastoianie, transform.position.y+ i * rastoianie, 0), Quaternion.identity, canvas);
                cellBufer[j + i * descSise].name = "cell " + (i * descSise + j);
                cellBuferImages[j + i * descSise].sprite = TexturesOfCell[0];
                cellBuferImages[j + i * descSise] = cellBufer[j + i * descSise].GetComponent<Image>();
                PlayerBuferImages[j + i * descSise] = cellBufer[j + i * descSise].GetComponentsInChildren<Image>()[1];
            }
        }
        stateMass[28] = 2;
        PlayerBuferImages[28].color = Color.white;
        stateMass[27] = 1;
        PlayerBuferImages[27].color = Color.black;
        stateMass[35] = 2;
        PlayerBuferImages[35].color = Color.white;
        stateMass[36] = 1;
        PlayerBuferImages[36].color = Color.black;

        CheckAvaliableMoves();
    }
}