

using UnityEngine;

public class SpawnerView : View
{

    
    void OnDrawGizmos() {
       
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 5);
    }

}


