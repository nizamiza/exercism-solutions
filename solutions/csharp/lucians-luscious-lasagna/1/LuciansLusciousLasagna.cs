class Lasagna
{
    public int ExpectedMinutesInOven()
    {
        return 40;
    }

    public int RemainingMinutesInOven(int actualMinutes)
    {
        return ExpectedMinutesInOven() - actualMinutes;
    }

    public int PreparationTimeInMinutes(int layerCount)
    {
        return layerCount * 2;
    }

    public int ElapsedTimeInMinutes(int layerCount, int actualMinutes)
    {
        return PreparationTimeInMinutes(layerCount) + actualMinutes;
    }
}
