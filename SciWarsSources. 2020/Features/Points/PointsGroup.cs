namespace Assets.Scripts.Core.Services
{
    public class PointsDataGroup : IEntityDataGroup
    {
        public IdData idData;
        public NameData nameData;
        public KillsData killsData;
        public DeathData deadData;
        public ResourceData resourceData;
        public ProfileData profileData;

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = idData.GetHashCode();
                hashCode = (hashCode * 397) ^ killsData.GetHashCode();
                hashCode = (hashCode * 397) ^ deadData.GetHashCode();
                return hashCode;
            }
        }
    }
}