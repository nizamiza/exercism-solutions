using System;

public static class Camicia
{
    public enum GameStatus
    {
        Finished,
        Loop
    }

    public record GameResult(GameStatus Status, int Tricks, int Cards);

    public static GameResult SimulateGame(string[] playerA, string[] playerB)
    {
        if (playerA.Length == 0 || playerB.Length == 0)
        {
            return new GameResult(GameStatus.Finished, 0, 0);
        }

        Queue<string> playerADeck = new(playerA);
        Queue<string> playerBDeck = new(playerB);
        Queue<string> centralDeck = new();
        Queue<string> currentDeck = playerADeck;

        List<(int[] PlayerADeck, int[] PlayerBDeck, bool IsPlayerATurn)> roundHistory =
            [(NormalizeDeck(playerADeck), NormalizeDeck(playerBDeck), true)];

        int trickCount = 0;
        int drawCount = 0;
        int paymentDue = 0;

        void SwitchPlayer()
        {
            currentDeck = currentDeck == playerADeck ? playerBDeck : playerADeck;
        }

        void Collect()
        {
            SwitchPlayer();
            while (centralDeck.TryDequeue(out var card))
            {
                currentDeck.Enqueue(card);
            }
            trickCount++;
        }

        while (true)
        {
            if (!currentDeck.TryDequeue(out var card))
            {
                Collect();
                return new GameResult(GameStatus.Finished, trickCount, drawCount);
            }
            drawCount++;

            var paymentValue = GetCardValue(card);
            centralDeck.Enqueue(card);

            if (paymentDue == 0 || paymentValue > 0)
            {
                paymentDue = paymentValue;
                SwitchPlayer();
                continue;
            }

            paymentDue--;
            if (paymentDue == 0)
            {
                Collect();
                if (playerADeck.Count == 0 || playerBDeck.Count == 0)
                {
                    return new GameResult(GameStatus.Finished, trickCount, drawCount);
                }

                var deckA = NormalizeDeck(playerADeck);
                var deckB = NormalizeDeck(playerBDeck);
                var isPlayerTurn = currentDeck == playerADeck;

                if (roundHistory.Exists(item =>
                    item.IsPlayerATurn == isPlayerTurn &&
                    item.PlayerADeck.SequenceEqual(deckA) &&
                    item.PlayerBDeck.SequenceEqual(deckB)))
                {
                    return new GameResult(GameStatus.Loop, trickCount, drawCount);
                }
                roundHistory.Add((deckA, deckB, isPlayerTurn));
            }
        }
    }

    private static int[] NormalizeDeck(Queue<string> deck)
    {
        int[] arr = new int[deck.Count];
        int i = 0;
        foreach (var card in deck)
        {
            arr[i++] = GetCardValue(card);
        }
        return arr;
    }

    private static int GetCardValue(string card) => card switch
    {
        "J" => 1,
        "Q" => 2,
        "K" => 3,
        "A" => 4,
        _ => 0,
    };
}
