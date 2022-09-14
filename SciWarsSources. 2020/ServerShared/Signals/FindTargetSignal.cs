public class FindTargetSignal : ISignal {
    public readonly IdData id;

    public FindTargetSignal(IdData id) {
        this.id = id;
      
    }

    public FindTargetSignal() {
    }
}