using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class DestroyEnemy : MonoBehaviour {

	[SerializeField] protected Transform[] toDestroy;

	List<SafeCoroutine> animations;

	public IEnumerator ExplodeAll(){
		animations = new List<SafeCoroutine>();

		toDestroy.ToList().ForEach(x => {
			var animator  = x.GetComponent<Animator>();
			if (animator != null){
				animator.enabled = false;
			}
		});

		for (int i = 0; i < toDestroy.Length; i++){
			animations.Add(this.StartSafeCoroutine(Explode(i)));
		}

		while (animations.Any(x => x != null && x.IsRunning)){
			yield return null;
		}
	}

	IEnumerator Explode(int index){
		var pos = toDestroy[index].position;
		var endPoint = pos + new Vector3(Random.Range(-.35f, .35f), 0, Random.Range(-.35f, .35f));
		var direction = (endPoint - pos);

		float duration = Random.Range(.1f, 3f);

		for (float i = 0; i < 1; i += Time.deltaTime / duration){
			toDestroy[index].Rotate(Vector3.up * Random.Range(12, 30));
			toDestroy[index].position += direction + (Vector3.up * .3f * Mathf.Cos(i * 3.14f));
			yield return null;
		}

		this.StartSafeCoroutine(Shrink(index));

		for (float i = 0; i < 1; i += Time.deltaTime / duration){
			toDestroy[index].Rotate(Vector3.up * Random.Range(12,309));
			toDestroy[index].position += direction + (Vector3.up * -1);
			yield return null;
		}
	}

	IEnumerator Shrink(int index){
		for (float i = 0; i < 1; i += Time.deltaTime / .2f){
			toDestroy[index].localScale = Vector3.Lerp(Vector3.one, Vector3.zero, i);
			yield return null;
		}
	}
}
