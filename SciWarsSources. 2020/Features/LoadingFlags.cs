using System;
[Flags]
public enum LoadingFlags {
	Login = 0,
	InventoryReceived = 1,
	All = Login | InventoryReceived
}
