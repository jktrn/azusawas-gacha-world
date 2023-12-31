namespace RequestClasses
{
    [System.Serializable]
    public class GachaRequest
    {
        public int crystals;
        public int pulls;
        public int numPulls;

        public GachaRequest(int crystals, int pulls, int numPulls)
        {
            this.crystals = crystals;
            this.pulls = pulls;
            this.numPulls = numPulls;
        }
    }

    [System.Serializable]
    public class GachaResponse
    {
        public Character[] characters;
    }

    [System.Serializable]
    public class Character
    {
        public string name;
        public string cardName;
        public string rarity;
        public string attribute;
        public string splashArt;
        public string avatar;
        public string flag;
    }
}
