namespace Assets.Scripts.Core.Services
{
    public struct DriverDescriptor {
        public readonly IdData Idata;
        public readonly DriverData.DriverType DriverType;

        public DriverDescriptor(IdData idata, DriverData.DriverType driverType) {
            Idata = idata;
            DriverType = driverType;
        }

        public bool Equals(DriverDescriptor other) {
            return Equals(Idata, other.Idata) && DriverType == other.DriverType;
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            return obj is DriverDescriptor other && Equals(other);
        }

        public override int GetHashCode() {
            unchecked {
                return ((Idata != null ? Idata.GetHashCode() : 0) * 397) ^ (int)DriverType;
            }
        }
    }
}