namespace Assets.Scripts.Core.Services {
	public class ProfileLoadedSignal : ISignal {
		public readonly ProfileData profileData;

		public ProfileLoadedSignal(ProfileData profileData) {
			this.profileData = profileData;
		}
	}
}
