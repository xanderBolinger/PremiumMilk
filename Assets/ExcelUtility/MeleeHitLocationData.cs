namespace ExcelUtillity
{
    public class MeleeHitLocationData
    {
        public string zoneName;
        public int bloodLossPD;
        public int shockPD;
        public int painPoints;
        public bool knockDown;
        public int knockDownMod;

        public MeleeHitLocationData(string zoneName, int bloodLossPD, int shockPD, int painPoints, bool knockDown, int knockDownMod)
        {
            this.zoneName = zoneName;
            this.bloodLossPD = bloodLossPD;
            this.shockPD = shockPD;
            this.painPoints = painPoints;
            this.knockDown = knockDown;
            this.knockDownMod = knockDownMod;
        }
    }
}