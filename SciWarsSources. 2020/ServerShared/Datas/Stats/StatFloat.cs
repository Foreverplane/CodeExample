using MessagePack;

[MessagePackObject()]
[Union(0, typeof(Power))]
[Union(1, typeof(Speed))]
[Union(2, typeof(Power))]
[Union(3, typeof(Temp))]
[Union(4, typeof(AGEEffect))]
[Union(5, typeof(HeatCapacity))]
[Union(6, typeof(Turn))]
[Union(7, typeof(Durability))]
[Union(8, typeof(EneryCapacity))]
[Union(9, typeof(Health))]
[Union(10, typeof(Shield))]
[Union(11, typeof(EneryCapacity))]
[Union(12, typeof(EneryCapacity))]
public abstract class StatFloat : Stat<float>
{

}
