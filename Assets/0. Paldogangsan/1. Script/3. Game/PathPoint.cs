using UnityEngine;
using System.Collections;

public class PathPoint : MonoBehaviour {
	void OnDrawGizmos(){
		Gizmos.color=Color.blue;
		Gizmos.DrawWireSphere(transform.position,0.25f);
	}
}
