using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;

public class GameManager : MonoBehaviour
{

    public List<Card> deck = new List<Card>();

    public List<Card>[]  PlayerColumn, EnemyColumn;
    public GameObject PlayerRow0Position, EnemyRow0Position, playerRow, playerDrawnPosition, enemyDrawnPosition, setButton, lastRow, deckButton,
                      CardsStartPosition,  DeckDivider, dropBtn;
    public GameObject[] Row, EnemyRow, Col;

    public GameObject[] playerRowPositions,  enemyRowPositions;
    public int[] enemyFreePlaceArr = {0, 0, 0, 0, 0};
    public bool endGame = false, clickedBtn = false, isPlaying = false;
     public int playerWinings = 0, enemyWinings = 0, playerScore = 0, enemyScore = 0, timesClicked = 0, enemyCurrRow = 1, enemyTimesPlayed = 0,
               playerHighCard = 0, enemyHighCard = 0, freePlaceIndex, playerHigherCard=0, enemyHigherCard=0, secondPlayerPair=0, secondEnemyPair=0;
    public int playerNumOfCards = 5;
    public Text playerScoreText, enemyScoreText;
    public AudioSource drawAudio, startGameAudio;
    public Animator menuAnimator, playerWonAnimator, playerLoseAnimator;
    public static Card currDrawn;
    public static int currRow = 1, currFight = 0, currColumn;
    public static bool cardOnRightPlace = false;


    // Start is called before the first frame update
    void Start()
    {
         menuAnimator.SetBool("StartGamePanel", true);
    }

    void Update()
    {
        if(endGame){
            Invoke("foldAllCards",2f);
            Invoke("EvaluateWInner", 15f);
            deckButton.gameObject.SetActive(false);
            endGame = false;
        }
    }

    public void EvaluateWInner(){
        if (playerWinings > enemyWinings){
            playerWonAnimator.SetBool("PlayerWon", true);
            playerScore++;
            updateScore();
            return;
        }
        playerLoseAnimator.SetBool("PlayerLose", true);
        enemyScore++;
        updateScore();
    }

    public void updateScore(){
        playerScoreText.text = playerScore.ToString();
        enemyScoreText.text = enemyScore.ToString();
    }

    public void onClickYesBtn(){
        if (clickedBtn == false){
            clickedBtn = true;
            RestartGame();
            playerWonAnimator.SetBool("PlayerWon", false);
            playerLoseAnimator.SetBool("PlayerLose", false);
            Invoke("StartGame", 2f);
            startGameAudio.Play();
        }
    }

    public void onClickNoBtn(){
        if (clickedBtn == false){
            clickedBtn = true;
            RestartGame();
            playerWonAnimator.SetBool("PlayerWon", false);
            playerLoseAnimator.SetBool("PlayerLose", false);
            menuAnimator.SetBool("StartGamePanel", true);
            enemyScore = 0;
            playerScore = 0;
            updateScore();
            startGameAudio.Play();
        }
    }  

    public void StartBtn(){
        if (clickedBtn == false){
            clickedBtn = true;
            menuAnimator.SetBool("StartGamePanel", false);
            Invoke("StartGame", 2.5f);
            startGameAudio.Play();
        }     
    }

    public void StartGame(){
        PlayerColumn = new List<Card>[5];
        EnemyColumn = new List<Card>[5];
        deckButton.gameObject.SetActive(true);
        for (int i = 0; i < 5; i++)
        {
            PlayerColumn[i] = new List<Card>();
            EnemyColumn[i] = new List<Card>();
        }
        Card tmpCard;
        int index;
        for(int i=0; i<5 ;i++){
            drawAudio.Play();
            index = Random.Range(0, deck.Count);
            tmpCard = deck[index];
            deck.RemoveAt(index);
            setChildAsRowByNum(tmpCard, 0, 1);
            tmpCard.setOnPlace(playerRowPositions[i].transform.position + new Vector3(0, 0.6f, 0), 20f);
            AddcardToColumn(i, tmpCard, 1);
        }
        for(int i=0; i<5 ;i++){
            drawAudio.Play();
            index = Random.Range(0, deck.Count);
            tmpCard = deck[index];
            deck.RemoveAt(index);
            setChildAsRowByNum(tmpCard, 0, 0);
            tmpCard.setOnPlace(enemyRowPositions[i].transform.position, 20f);
            AddcardToColumn(i, tmpCard, 0);
        }
    }

    public void RestartGame(){
        for (int i=0; i<5; i++){
            foreach (Card card in PlayerColumn[i])
            {
                Card tmpCard = card;
                tmpCard.gameObject.transform.SetParent(DeckDivider.transform, false);
                card.setOnPlace(CardsStartPosition.transform.position, 20f);
                deck.Add(tmpCard);
            }
            foreach (Card card in EnemyColumn[i])
            {
                Card tmpCard = card;
                tmpCard.gameObject.transform.SetParent(DeckDivider.transform, false);
                card.setOnPlace(CardsStartPosition.transform.position, 20f);
                deck.Add(tmpCard);
            }
        }
        isPlaying = false;
        PlayerColumn = new List<Card>[5];
        EnemyColumn = new List<Card>[5];
        timesClicked = 0;
        enemyCurrRow = 1;
        enemyTimesPlayed = 0;
        currRow = 1;
        cardOnRightPlace = false;
        clickedBtn = false;
        playerHighCard = 0;
        enemyHighCard = 0;
        currFight = 0;
        playerWinings = 0;
        enemyWinings = 0;
        playerNumOfCards = 5;
        playerHigherCard = 0;
        enemyHigherCard = 0;
        secondEnemyPair = 0;
        secondPlayerPair = 0;
        for(int i=0; i<5; i++){
            enemyFreePlaceArr[i] = 0;
            Col[i].transform.position += new Vector3(0, .65f*4, 0);
        }
    }

    public void PlayerDraw(){
        if (!isPlaying){
            playerNumOfCards++;
            isPlaying = true;
            clickedBtn = false;
            setButton.gameObject.SetActive(true);
            int index = Random.Range(0, deck.Count);
            currDrawn = deck[index];
            deck.RemoveAt(index);
            setChildAsRowByNum(currDrawn, currRow, 1);
            currDrawn.setOnPlace(playerDrawnPosition.transform.position, 20f);
            if (currRow <= 4){
                playerRow.gameObject.SetActive(true);
            }
            else{
                lastRow.gameObject.SetActive(true);
                lastRow.transform.position = playerRow.transform.position + new Vector3(0,-1.95f,0);
                if (playerNumOfCards == 26){
                    dropBtn.gameObject.SetActive(true);
                }
            }
        }
    }

    public void dropCard (){
        lastRow.gameObject.SetActive(false);
        setButton.gameObject.SetActive(false);
        dropBtn.gameObject.SetActive(false);
        deck.Add(currDrawn);
        currDrawn.gameObject.transform.SetParent(DeckDivider.transform, false);
        currDrawn.setOnPlace(CardsStartPosition.transform.position, 20f);
        Invoke("EnemyLastDraw", 2f);

    }

    public void EnemyDraw(){
        int index = Random.Range(0, deck.Count);
        currDrawn = deck[index];
        deck.RemoveAt(index);
        setChildAsRowByNum(currDrawn, enemyCurrRow, 0);
        freePlaceIndex = enemyColChoose(currDrawn);
        enemyFreePlaceArr[freePlaceIndex] = 1;
        AddcardToColumn(freePlaceIndex, currDrawn, 0);
        Vector3 cardPosition = new Vector3(enemyRowPositions[freePlaceIndex].transform.position.x,
                        enemyRowPositions[freePlaceIndex].transform.position.y + enemyCurrRow*.6f,
                        enemyRowPositions[freePlaceIndex].transform.position.z);
        drawAudio.Play();
        currDrawn.setOnPlace(cardPosition, 30f);
        enemyTimesPlayed ++;
        if (enemyTimesPlayed %5 == 0 && enemyTimesPlayed !=0){
            enemyCurrRow++;
            for (int i=0 ;i<5; i++){
                enemyFreePlaceArr[i]=0;
            }
        }
        isPlaying = false;
    }

    public int enemyColChoose(Card currDrawn){
        bool hasDigit = false;
        bool sameShape = false;
        int hasDigitIdx = -1;
        int sameShapeIdx = -1;
        for(int i=0; i<5; i++){
            if(enemyFreePlaceArr[i] == 1)
                continue;
            else{

                foreach (Card card in EnemyColumn[i] ){
                    if ( card.digit == currDrawn.digit){
                        hasDigitIdx = i;
                        hasDigit = true;
                        break;
                    }
                }
                int counter = 0;
                foreach (Card card in EnemyColumn[i] ){
                    if (card.shape == currDrawn.shape){
                        counter++;
                    }    
                }
                if (counter == EnemyColumn[i].Count){
                    sameShape = true;
                    sameShapeIdx = i;
                }                     
            }
        }
        if (hasDigit && !sameShape){return hasDigitIdx;}
        else if (!hasDigit && sameShape){return sameShapeIdx;}
        else if (hasDigit && sameShape){
            int rnd = Random.Range(0,2);
            return rnd == 1? hasDigitIdx: sameShapeIdx;
        }
        else{
            for(int i=0; i<5; i++)
                if (enemyFreePlaceArr[i] == 0)
                    return i;
        }
        return 0;
    } 

    public int enemyLastChoose(Card currDrawn){
        bool hasDigit = false;
        bool sameShape = false;
        int hasDigitIdx = -1;
        int sameShapeIdx = -1;
        for(int i=0; i<5; i++){
            for (int j=0; j<4; j++){
                if (EnemyColumn[i][j].digit == currDrawn.digit){
                    hasDigitIdx = i;
                    hasDigit = true;
                    break;
                }
            }
            int counter = 0;
            for (int j=0; j<4; j++){
                if (EnemyColumn[i][j].shape == currDrawn.shape){
                    counter++;
                }    
            }
            if (counter == 4){
                sameShape = true;
                sameShapeIdx = i;
            }                     
        }
        if (hasDigit && !sameShape){return hasDigitIdx;}
        else if (!hasDigit && sameShape){return sameShapeIdx;}
        else if (hasDigit && sameShape){return sameShapeIdx;}
        return -1;
        
    } 
    

    public void EnemyLastDraw(){
        currDrawn = deck[0];
        int randomCol = enemyLastChoose(currDrawn);
        if(randomCol != -1){
            deck.RemoveAt(0);
            setChildAsRowByNum(currDrawn, enemyCurrRow, 0);
            Vector3 cardPosition = EnemyColumn[randomCol][4].transform.position;
            currDrawn.setOnPlace(cardPosition, 30f);
            Card tmpCard = EnemyColumn[randomCol][4];
            EnemyColumn[randomCol].RemoveAt(4);
            EnemyColumn[randomCol].Add(currDrawn);
            deck.Add(tmpCard);
            tmpCard.gameObject.transform.SetParent(DeckDivider.transform, false);
            tmpCard.setOnPlace(CardsStartPosition.transform.position, 20f);
                    
        }
        endGame = true;
    }
              

    public void SetCardOnCol(){
        if (cardOnRightPlace){
            if (currRow == 5){
                lastRow.gameObject.SetActive(false);
                setButton.gameObject.SetActive(false);
                dropBtn.gameObject.SetActive(false);
                Card tmpCard = PlayerColumn[currColumn][4];
                PlayerColumn[currColumn].RemoveAt(4);
                deck.Add(tmpCard);
                tmpCard.gameObject.transform.SetParent(DeckDivider.transform, false);
                tmpCard.setOnPlace(CardsStartPosition.transform.position, 20f);
                AddcardToColumn(currColumn, currDrawn, 1);
                Invoke("EnemyLastDraw", 2f);
            }
            else{
            AddcardToColumn(currColumn, currDrawn, 1);
            setButton.gameObject.SetActive(false);
            playerRow.gameObject.SetActive(false);
            SetFalseCol();
            Invoke("EnemyDraw", 1f);
            }
        }
    }

    public void SetFalseCol(){
        cardOnRightPlace = false;
        timesClicked++;
        Col[currColumn].SetActive(false);
        if (timesClicked % 5 == 0 && timesClicked != 0){
            UpdateRow();
        }
    }
    
    // Add card to column(col), if n = 1 -> Add to player, else, Add to enemy
    public void AddcardToColumn(int col, Card card, int n){
        if(n==1){
            PlayerColumn[col].Add(card);
        }
        if(n==0){
            EnemyColumn[col].Add(card);
        }         
    }

    public void setChildAsRowByNum(Card card, int row, int n){
        if (n==1){
            card.gameObject.transform.SetParent(Row[row].transform, false);
        }
        else{
            card.gameObject.transform.SetParent(EnemyRow[row].transform, false);   
        }  
    }

    public void UpdateRow(){
        currRow++;
        for(int i=0 ; i<5; i++){
        Col[i].transform.position += new Vector3(0, -.65f, 0);
        Col[i].gameObject.SetActive(true);
        }
        
    }

    public bool OneFight(int n){
        int[] PlayerEvaluation = EvaluateHand(PlayerColumn[n], out playerHighCard, playerHighCard, out playerHigherCard,playerHigherCard,  out secondPlayerPair);
        int[] EnemyEvaluation = EvaluateHand(EnemyColumn[n], out enemyHighCard, enemyHighCard, out enemyHigherCard, enemyHigherCard,  out secondEnemyPair);
        int[,] playerVector = fromHandToVector(PlayerColumn[n]);
        int[,] enemyVector = fromHandToVector(EnemyColumn[n]);
        for(int i=9; i > 0; i--){
            if (PlayerEvaluation[i] == 1 && EnemyEvaluation[i] == 0){
                return true;
            }
            if (PlayerEvaluation[i] == 0 && EnemyEvaluation[i] == 1){
                return false;
            }
            if (PlayerEvaluation[i] == 1 && EnemyEvaluation[i] == 1){
                if (i == 2){
                    if (playerHighCard > enemyHighCard){
                        return true;
                    }
                    else if (playerHighCard < enemyHighCard){
                        return false;
                    }
                    else{
                        if (secondPlayerPair > secondEnemyPair){
                            return true;
                        }
                        else if(secondPlayerPair < secondEnemyPair){
                            return false;
                        }
                        return playerHigherCard > enemyHigherCard? true : false;
                    }    
                    
                }
                else if (i == 1){
                        if (playerHighCard > enemyHighCard){
                            return true;
                        }
                        else if(playerHighCard < enemyHighCard){
                            return false;
                        }
                        else{
                            return playerHigherCard > enemyHigherCard? true : false;
                        }
                }
                else{
                    return playerHighCard > enemyHighCard? true: false;
                }
                }
        }
        return HighCardFight(playerVector, enemyVector);    
    }

    public void foldAllCards (){
        Invoke("Fight", 2f);
        Invoke("Fight", 4f);
        Invoke("Fight", 6f);
        Invoke("Fight", 8f);
        Invoke("Fight", 10f);
    }

    public void Fight(){
        bool playerWon = false;
        playerWon = OneFight(currFight);
        if (playerWon){
            playerWinings++;
            foldCards(currFight, 0);
        }
        else{
            enemyWinings++;
            foldCards(currFight, 1);
        }
        currFight++;
    }


    public void foldCards(int n, int i){
        if (i == 1){
            for (int j =0 ; j<5; j++){
                PlayerColumn[n][j].setOnPlace(PlayerColumn[n][0].transform.position, 2f);
            }
        }
        else{
            for (int j =0 ; j<5; j++){
                EnemyColumn[n][j].setOnPlace(EnemyColumn[n][0].transform.position, 2f);
            }
        }
    }

    public int[] EvaluateHand(List<Card> hand, out int numOfHighCard, int currhigh, out int higherCard, int pervHighcard, out int secondPair){
        int[] handResult = {0,0,0,0,0,0,0,0,0,0};
        int[,] handVector ;
        handVector = fromHandToVector(hand);
        handResult[9] = isRoyalFlush(handVector, out numOfHighCard, currhigh);
        handResult[8] = isStraightFlush(handVector, out numOfHighCard, currhigh);
        handResult[7] = isFourOfAKind(handVector, out numOfHighCard, currhigh);
        handResult[6] = isFullhouse(handVector, out numOfHighCard, currhigh, pervHighcard);
        handResult[5] = isFlush(handVector, out numOfHighCard, currhigh);
        handResult[4] = isStraight(handVector, out numOfHighCard, currhigh);
        handResult[3] = isThreeOfAkind(handVector, out numOfHighCard, currhigh);
        handResult[2] = isTwoPairs(handVector, out numOfHighCard, currhigh, out higherCard, pervHighcard, out secondPair);
        handResult[1] = isPair(handVector, out numOfHighCard, currhigh,  out higherCard, pervHighcard);
        return handResult;

    }

    public int[,] fromHandToVector(List<Card> hand){
        // handVactor[0] = clubs cards,handVactor[1] = diamonds cards,  handVactor[2] = spades cards, handVactor[3] = heart cards
        int[,] handVector = new int[4,14];
        for (int i=0; i<5; i++){
            char shape = hand[i].shape;
            int digit = hand[i].digit;
            if (shape.Equals('c'))
                handVector[0,digit-1] = 1;

            else if (shape.Equals('d'))
                handVector[1, digit-1] = 1;

            else if (shape.Equals('s'))
                handVector[2, digit-1] = 1;

            else{
                handVector[3, digit-1] = 1;
            }
        }
        // Mark 'A' 
        for (int j= 0 ; j<4; j++){
            if(handVector[j,0] == 1)
                handVector[j,13] = 1;
        }

        return handVector;
    }

    public bool HighCardFight(int[,] playerVector, int[,] enemyVector){
        for (int j= 13 ;j > 0 ;j--){
            if (playerVector[0,j] + playerVector[1,j] + playerVector[2,j] + playerVector[3,j] == 1 &&
                enemyVector[0,j] + enemyVector[1,j] + enemyVector[2,j] + enemyVector[3,j] == 0)
                return true;
            if (playerVector[0,j] + playerVector[1,j] + playerVector[2,j] + playerVector[3,j] == 0 &&
                enemyVector[0,j] + enemyVector[1,j] + enemyVector[2,j] + enemyVector[3,j] == 1)
                return false;
        }
        return false;
    }

    public int isPair(int [,] handVector, out int numOfHighCard, int currhigh, out int highCard , int pervHighcard){
        int isPair = 0;
        int pairCard = 0;
        int higherCard = 0;
        for (int j=13 ;j>0 ;j--){
            if (handVector[0,j] + handVector[1,j] + handVector[2,j] + handVector[3,j] == 2 ){
                isPair = 1;
                pairCard = j+1;
                break;
            }
        }
        if(isPair == 1){
            for (int j=13 ;j>0 ;j--){
                if (handVector[0,j] + handVector[1,j] + handVector[2,j] + handVector[3,j] >= 1 && j+1 != pairCard ){
                    higherCard = j+1; 
                    break;
                }
            }
            highCard = higherCard;
            numOfHighCard = pairCard;
            return isPair;
        }
        else{
            highCard = pervHighcard;
            numOfHighCard = currhigh;
            return isPair;
        }
       
    }

    public int isTwoPairs(int [,] handVector, out int numOfHighCard, int currhigh, out int highCard, int pervHighcard, out int secondPair){
        int isPairOne = 0;
        int isPairTwo = 0;
        int pairOneNum = 0;
        int pairtwoNum = 0;
        for (int j=13 ;j>0 ;j--){
            if (handVector[0,j] + handVector[1,j] + handVector[2,j] + handVector[3,j] >= 2 ){
                if (isPairOne == 0){
                    isPairOne = 1;
                    pairOneNum = j+1;
                }  
                else{
                    isPairTwo = 1;
                    pairtwoNum = j+1;
                    break;
                }
            }
        }
        if (isPairOne+isPairTwo == 2){
            numOfHighCard = pairOneNum>pairtwoNum? pairOneNum : pairtwoNum;
            secondPair = pairOneNum>pairtwoNum? pairtwoNum : pairOneNum;
            for (int j=13 ;j>0 ;j--){
            if (handVector[0,j] + handVector[1,j] + handVector[2,j] + handVector[3,j] >= 1 && j+1 != numOfHighCard && j+1 != secondPair ){
                highCard = j+1;
                return 1;
                }
            }
            highCard = 0;
            return 1;
        }      
        else{
            secondPair = 0;
            highCard = pervHighcard ;
            numOfHighCard = currhigh;
            return 0;
        }
    }

    public int isThreeOfAkind(int [,] handVector,  out int numOfHighCard, int currhigh){
        int isThreeOfAkind = 0;
        int highCard = 0;
        for (int j=13 ;j>=0 ;j--){
            if (handVector[0,j] + handVector[1,j] + handVector[2,j] + handVector[3,j] == 3 ){
                highCard = j+1;
                isThreeOfAkind = 1;
                break;
            }
        }
        if(isThreeOfAkind == 1)
            numOfHighCard = highCard;
        else{
            numOfHighCard = currhigh;
        }
        return isThreeOfAkind;
    }

    public int isStraight(int [,] handVector, out int numOfHighCard, int currhigh){
        int isStraight = 0;
        int straightLen = 0;
        int highCard = 0;
        for (int j=13 ;j>0 ;j--){
            if (handVector[0,j] + handVector[1,j] + handVector[2,j] + handVector[3,j] > 1){
                numOfHighCard = currhigh;
                return isStraight;
            }
            else if(handVector[0,j] + handVector[1,j] + handVector[2,j] + handVector[3,j] == 1){
                highCard = j+1;
                straightLen++;
                for(int k=1 ;k<5; k++){
                    if (j-k < 0){
                        numOfHighCard = currhigh;
                        return isStraight;
                    }
                    if (handVector[0,j-k] + handVector[1,j-k] + handVector[2,j-k] + handVector[3,j-k] > 1 ||
                        handVector[0,j-k] + handVector[1,j-k] + handVector[2,j-k] + handVector[3,j-k] == 0){
                            numOfHighCard = currhigh;
                            return isStraight;
                        }
                    else if(handVector[0,j] + handVector[1,j] + handVector[2,j] + handVector[3,j] == 1){
                        straightLen++;
                        continue;
                    }
                }
                if(straightLen == 5){
                    numOfHighCard = highCard;
                    isStraight = 1;
                    return isStraight;
                }
                else{
                    highCard = 0;
                    straightLen = 0;
                }
            }        
        }
        numOfHighCard = currhigh;
        return isStraight;
    }

    public int isFourOfAKind(int [,] handVector, out int numOfHighCard, int currhigh){
        int isFourOfAkind = 0;
        int highCard = 0;
        for (int j=13 ;j>0 ;j--){
            int n = handVector[0,j] + handVector[1,j] + handVector[2,j] + handVector[3,j];
            if (n == 4){
                highCard = j+1;
                isFourOfAkind = 1;
                break;
            }
        }
        if (isFourOfAkind == 1)
            numOfHighCard = highCard;
        else{
            numOfHighCard = currhigh;
        }
        return isFourOfAkind;
    }

    public int isFlush(int [,] handVector, out int numOfHighCard, int currhigh){
        int sumofClubs=0, sumOfDiamonds=0, sumOfSpades=0, sumOfHeart=0 ;
        int highCard = 0;
        for(int i=0 ;i < 13; i++){
            sumofClubs += handVector[0,i];
            sumOfDiamonds += handVector[1,i];
            sumOfSpades += handVector[2,i];
            sumOfHeart += handVector[3,i];
        }

        if(sumofClubs == 5 || sumOfDiamonds== 5 || sumOfSpades== 5 || sumOfHeart== 5 ){
            for(int j=13 ;j > 0; j--){
                if(handVector[0,j] + handVector[1,j] + handVector[2,j] + handVector[3,j] == 1)
                    highCard = j+1;
            }
            numOfHighCard = highCard;
            return 1;
        }
        numOfHighCard = currhigh;
        return 0;
    }


    
    public int isFullhouse(int [,] handVector, out int numOfHighCard, int currhigh, int pervHighcard){
        int tmpHigh0 = 0;
        int tmpHigh1 = 0;
        int tmp = 0;
        int tmp0 = 0;
        int isTwoPair = isTwoPairs(handVector, out tmpHigh0, currhigh, out tmp, pervHighcard,  out tmp0);
        int isthreeOfAkind = isThreeOfAkind(handVector,  out tmpHigh1, currhigh);
        if (isTwoPair + isthreeOfAkind == 2){
            numOfHighCard = tmpHigh1;
            return 1;
        }
        numOfHighCard = currhigh;
        return 0;
    }

    public int isStraightFlush(int [,] handVector, out int numOfHighCard, int currhigh){
        int tmpHigh0 = 0;
        int tmpHigh1 = 0;
        int isstraight = isStraight(handVector, out tmpHigh0, currhigh);
        int isflush = isFlush(handVector, out tmpHigh1, currhigh);
        if (isstraight + isflush == 2){
            numOfHighCard = tmpHigh0;
            return 1; 
        }
        numOfHighCard = currhigh;
        return 0;
    }

    public int isRoyalFlush(int [,] handVector, out int numOfHighCard, int currhigh){
        int highCard = 0;
        int isstraightFlush = isStraightFlush(handVector, out highCard, currhigh);
        if (handVector[0,13] + handVector[1,13] + handVector[2,13] + handVector[3,13] == 1 &&
            handVector[0,12] + handVector[1,12] + handVector[2,12] + handVector[3,12] == 1 &&
            isstraightFlush == 1){
                numOfHighCard = highCard;
                return 1;
            }
        
        numOfHighCard = currhigh;    
        return 0;
    }
}














