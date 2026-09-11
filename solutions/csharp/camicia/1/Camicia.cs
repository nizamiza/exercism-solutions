using System;

public static class Camicia
{
    public enum GameStatus
    {
        Finished,
        Loop
    }


    public record GameResult(GameStatus Status, int Tricks, int Cards);

    private static List<int[]> RoundHistory = [];
    public static GameResult SimulateGame(string[] playerA, string[] playerB)
    {
        var state = new GameState(playerA, playerB);

        while (true)
        {
            var result = state.DrawCard();
            switch (result)
            {
                case DrawResult.TurnEnded:
                    var deck = DeckToArray(state.PlayerADeck);
                    if (RoundHistory.Exists(d => d.SequenceEqual(deck)))
                    {
                        return new GameResult(GameStatus.Loop, state.TricksCount, state.DrawCount);
                    }
                    RoundHistory.Add(deck);
                    break;
                case DrawResult.GameEnded:
                    return new GameResult(GameStatus.Finished, state.TricksCount, state.DrawCount);
                default:
                    break;
            }
        }
    }

    private record Card(string Face, int PaymentValue);

    private enum DrawResult
    {
        GameEnded,
        TurnContinues,
        TurnEnded
    }

    private sealed class GameState
    {
        public GameState(string[] playerACards, string[] playerBCards)
        {
            PlayerADeck = new Queue<string>(playerACards);
            PlayerBDeck = new Queue<string>(playerBCards);
            CentralDeck = new Queue<string>();
            CurrentPlayerDeck = PlayerADeck;
        }
        public Queue<string> PlayerADeck { get; }
        public Queue<string> PlayerBDeck { get; }
        public Queue<string> CentralDeck { get; }

        public Queue<string> CurrentPlayerDeck { get; set; }
        public void NextPlayer()
        {
            CurrentPlayerDeck = CurrentPlayerDeck == PlayerADeck ? PlayerBDeck : PlayerADeck;
        }
        public int TricksCount { get; private set; }
        public int DrawCount { get; private set; }
        public int PaymentDue { get; private set; }
        public DrawResult DrawCard()
        {
            if (!CurrentPlayerDeck.TryDequeue(out var card))
            {
                while (CentralDeck.TryDequeue(out var c))
                {
                    CurrentPlayerDeck.Enqueue(c);
                }
                TricksCount++;
                return DrawResult.GameEnded;
            }

            DrawCount++;

            var paymentValue = card switch
            {
                "J" => 1,
                "Q" => 2,
                "K" => 3,
                "A" => 4,
                _ => 0,
            };

            CentralDeck.Enqueue(card);

            if (PaymentDue == 0 || paymentValue > 0)
            {
                PaymentDue = paymentValue;
                NextPlayer();
                return DrawResult.TurnContinues;
            }

            PaymentDue--;
            if (PaymentDue == 0)
            {
                NextPlayer();
                while (CentralDeck.TryDequeue(out var c))
                {
                    CurrentPlayerDeck.Enqueue(c);
                }
                TricksCount++;
                return DrawResult.TurnEnded;
            }

            return DrawResult.TurnContinues;
        }
    }

    private static int[] DeckToArray(Queue<string> deck)
    {
        int[] arr = new int[deck.Count];
        int i = 0;
        foreach (var card in deck)
        {
            arr[i] = card switch
            {
                "J" => 1,
                "Q" => 2,
                "K" => 3,
                "A" => 4,
                _ => 0,
            };
            i++;
        }
        return arr;
    }
}
